using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace USWsSync.Core.History
{
    public static class SyncHistoryRepository
    {
        private static readonly SemaphoreSlim _writeLock = new(1, 1);
        private static bool _isInitialized;

        public static string DbPath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sync_history.db");

        private static string ConnectionString => new SqliteConnectionStringBuilder
        {
            DataSource = DbPath,
            Mode = SqliteOpenMode.ReadWriteCreate,
            Cache = SqliteCacheMode.Shared
        }.ToString();

        public static async Task InitializeDatabaseAsync()
        {
            if (_isInitialized) return;

            await _writeLock.WaitAsync();
            try
            {
                if (_isInitialized) return;

                var dbDir = Path.GetDirectoryName(DbPath);
                if (!string.IsNullOrEmpty(dbDir) && !Directory.Exists(dbDir))
                {
                    Directory.CreateDirectory(dbDir);
                }

                await using var conn = new SqliteConnection(ConnectionString);
                await conn.OpenAsync();

                // Pragmas de alto rendimiento y concurrencia para SQLite
                await using (var pragmaCmd = conn.CreateCommand())
                {
                    pragmaCmd.CommandText = @"
                        PRAGMA journal_mode = WAL;
                        PRAGMA synchronous = NORMAL;
                        PRAGMA busy_timeout = 5000;
                        PRAGMA foreign_keys = ON;
                    ";
                    await pragmaCmd.ExecuteNonQueryAsync();
                }

                // Esquema de tablas
                await using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS sync_runs (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            run_id TEXT NOT NULL,
                            start_time TEXT NOT NULL,
                            end_time TEXT NOT NULL,
                            duration_ms INTEGER NOT NULL,
                            direction TEXT NOT NULL,
                            trigger_source TEXT NOT NULL,
                            status TEXT NOT NULL,
                            total_tables INTEGER NOT NULL,
                            tables_with_changes INTEGER NOT NULL,
                            tables_with_errors INTEGER NOT NULL,
                            total_records INTEGER NOT NULL,
                            data_from_date TEXT NULL,
                            data_to_date TEXT NULL,
                            error_summary TEXT NULL
                        );

                        CREATE TABLE IF NOT EXISTS sync_run_items (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            run_id INTEGER NOT NULL,
                            table_name TEXT NOT NULL,
                            records_count INTEGER NOT NULL,
                            is_success INTEGER NOT NULL,
                            error_message TEXT NULL,
                            duration_ms INTEGER NOT NULL,
                            FOREIGN KEY (run_id) REFERENCES sync_runs(id) ON DELETE CASCADE
                        );

                        CREATE INDEX IF NOT EXISTS idx_sync_runs_start_time ON sync_runs(start_time DESC);
                        CREATE INDEX IF NOT EXISTS idx_sync_runs_status ON sync_runs(status);
                        CREATE INDEX IF NOT EXISTS idx_sync_run_items_run_id ON sync_run_items(run_id);
                        CREATE INDEX IF NOT EXISTS idx_sync_run_items_table ON sync_run_items(table_name);
                    ";
                    await cmd.ExecuteNonQueryAsync();
                }

                // Migración de columnas para bases de datos existentes
                try
                {
                    await using var colCheckCmd = conn.CreateCommand();
                    colCheckCmd.CommandText = "PRAGMA table_info(sync_runs);";
                    var existingCols = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    await using (var rdr = await colCheckCmd.ExecuteReaderAsync())
                    {
                        while (await rdr.ReadAsync())
                        {
                            existingCols.Add(rdr.GetString(1));
                        }
                    }

                    if (!existingCols.Contains("data_from_date"))
                    {
                        await using var alterCmd = conn.CreateCommand();
                        alterCmd.CommandText = "ALTER TABLE sync_runs ADD COLUMN data_from_date TEXT NULL;";
                        await alterCmd.ExecuteNonQueryAsync();
                    }
                    if (!existingCols.Contains("data_to_date"))
                    {
                        await using var alterCmd = conn.CreateCommand();
                        alterCmd.CommandText = "ALTER TABLE sync_runs ADD COLUMN data_to_date TEXT NULL;";
                        await alterCmd.ExecuteNonQueryAsync();
                    }
                }
                catch { }

                _isInitialized = true;
            }
            finally
            {
                _writeLock.Release();
            }
        }

        public static async Task<long> SaveRunAsync(SyncRunRecord run, IEnumerable<SyncRunItemRecord>? items = null)
        {
            await InitializeDatabaseAsync();
            await _writeLock.WaitAsync();

            try
            {
                await using var conn = new SqliteConnection(ConnectionString);
                await conn.OpenAsync();
                await using var tx = (SqliteTransaction)await conn.BeginTransactionAsync();

                long insertedId;
                await using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
                        INSERT INTO sync_runs (
                            run_id, start_time, end_time, duration_ms, direction, 
                            trigger_source, status, total_tables, tables_with_changes, 
                            tables_with_errors, total_records, data_from_date, data_to_date, error_summary
                        ) VALUES (
                            @runId, @startTime, @endTime, @durationMs, @direction, 
                            @triggerSource, @status, @totalTables, @tablesChanges, 
                            @tablesErrors, @totalRecords, @dataFromDate, @dataToDate, @errorSummary
                        );
                        SELECT last_insert_rowid();
                    ";

                    cmd.Parameters.AddWithValue("@runId", run.RunId);
                    cmd.Parameters.AddWithValue("@startTime", run.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"));
                    cmd.Parameters.AddWithValue("@endTime", run.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"));
                    cmd.Parameters.AddWithValue("@durationMs", run.DurationMs);
                    cmd.Parameters.AddWithValue("@direction", run.Direction.ToString().ToUpperInvariant());
                    cmd.Parameters.AddWithValue("@triggerSource", run.TriggerSource.ToString().ToUpperInvariant());
                    cmd.Parameters.AddWithValue("@status", run.Status.ToString().ToUpperInvariant());
                    cmd.Parameters.AddWithValue("@totalTables", run.TotalTables);
                    cmd.Parameters.AddWithValue("@tablesChanges", run.TablesWithChanges);
                    cmd.Parameters.AddWithValue("@tablesErrors", run.TablesWithErrors);
                    cmd.Parameters.AddWithValue("@totalRecords", run.TotalRecords);
                    cmd.Parameters.AddWithValue("@dataFromDate", run.DataFromDate > DateTime.MinValue ? run.DataFromDate.ToString("yyyy-MM-ddTHH:mm:ss") : DBNull.Value);
                    cmd.Parameters.AddWithValue("@dataToDate", run.DataToDate > DateTime.MinValue ? run.DataToDate.ToString("yyyy-MM-ddTHH:mm:ss") : DBNull.Value);
                    cmd.Parameters.AddWithValue("@errorSummary", (object?)run.ErrorSummary ?? DBNull.Value);

                    var scalarResult = await cmd.ExecuteScalarAsync();
                    insertedId = Convert.ToInt64(scalarResult);
                }

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        await using var itemCmd = conn.CreateCommand();
                        itemCmd.Transaction = tx;
                        itemCmd.CommandText = @"
                            INSERT INTO sync_run_items (
                                run_id, table_name, records_count, is_success, error_message, duration_ms
                            ) VALUES (
                                @runId, @tableName, @recordsCount, @isSuccess, @errorMessage, @durationMs
                            );
                        ";

                        itemCmd.Parameters.AddWithValue("@runId", insertedId);
                        itemCmd.Parameters.AddWithValue("@tableName", item.TableName);
                        itemCmd.Parameters.AddWithValue("@recordsCount", item.RecordsCount);
                        itemCmd.Parameters.AddWithValue("@isSuccess", item.IsSuccess ? 1 : 0);
                        itemCmd.Parameters.AddWithValue("@errorMessage", (object?)item.ErrorMessage ?? DBNull.Value);
                        itemCmd.Parameters.AddWithValue("@durationMs", item.DurationMs);

                        await itemCmd.ExecuteNonQueryAsync();
                    }
                }

                await tx.CommitAsync();
                run.Id = insertedId;
                return insertedId;
            }
            finally
            {
                _writeLock.Release();
            }
        }

        public static async Task<List<SyncRunRecord>> GetRunsAsync(SyncHistoryFilter filter)
        {
            await InitializeDatabaseAsync();
            var list = new List<SyncRunRecord>();

            await using var conn = new SqliteConnection(ConnectionString);
            await conn.OpenAsync();

            var sb = new StringBuilder();
            sb.Append("SELECT id, run_id, start_time, end_time, duration_ms, direction, trigger_source, status, total_tables, tables_with_changes, tables_with_errors, total_records, error_summary, data_from_date, data_to_date FROM sync_runs WHERE 1=1 ");

            using var cmd = conn.CreateCommand();

            if (filter.FromDate.HasValue)
            {
                sb.Append("AND start_time >= @fromDate ");
                cmd.Parameters.AddWithValue("@fromDate", filter.FromDate.Value.ToString("yyyy-MM-ddTHH:mm:ss"));
            }

            if (filter.ToDate.HasValue)
            {
                sb.Append("AND start_time <= @toDate ");
                cmd.Parameters.AddWithValue("@toDate", filter.ToDate.Value.ToString("yyyy-MM-ddTHH:mm:ss"));
            }

            if (filter.Direction.HasValue)
            {
                sb.Append("AND direction = @direction ");
                cmd.Parameters.AddWithValue("@direction", filter.Direction.Value.ToString().ToUpperInvariant());
            }

            if (filter.Status.HasValue)
            {
                sb.Append("AND status = @status ");
                cmd.Parameters.AddWithValue("@status", filter.Status.Value.ToString().ToUpperInvariant());
            }

            if (filter.OnlyWithChangesOrErrors)
            {
                sb.Append("AND (tables_with_changes > 0 OR tables_with_errors > 0) ");
            }

            if (!string.IsNullOrWhiteSpace(filter.TableSearch))
            {
                sb.Append("AND id IN (SELECT run_id FROM sync_run_items WHERE table_name LIKE @tblSearch) ");
                cmd.Parameters.AddWithValue("@tblSearch", "%" + filter.TableSearch.Trim() + "%");
            }

            sb.Append("ORDER BY start_time DESC ");

            if (filter.PageSize > 0)
            {
                var offset = Math.Max(0, (filter.Page - 1) * filter.PageSize);
                sb.Append("LIMIT @pageSize OFFSET @offset ");
                cmd.Parameters.AddWithValue("@pageSize", filter.PageSize);
                cmd.Parameters.AddWithValue("@offset", offset);
            }

            cmd.CommandText = sb.ToString();

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var record = new SyncRunRecord
                {
                    Id = reader.GetInt64(0),
                    RunId = reader.GetString(1),
                    StartTime = DateTime.TryParse(reader.GetString(2), out var st) ? st : DateTime.MinValue,
                    EndTime = DateTime.TryParse(reader.GetString(3), out var et) ? et : DateTime.MinValue,
                    DurationMs = reader.GetInt32(4),
                    Direction = Enum.TryParse<SyncDirection>(reader.GetString(5), true, out var dir) ? dir : SyncDirection.Download,
                    TriggerSource = Enum.TryParse<SyncTriggerSource>(reader.GetString(6), true, out var trg) ? trg : SyncTriggerSource.Scheduled,
                    Status = Enum.TryParse<SyncRunStatus>(reader.GetString(7), true, out var sta) ? sta : SyncRunStatus.Success,
                    TotalTables = reader.GetInt32(8),
                    TablesWithChanges = reader.GetInt32(9),
                    TablesWithErrors = reader.GetInt32(10),
                    TotalRecords = reader.GetInt32(11),
                    ErrorSummary = reader.IsDBNull(12) ? null : reader.GetString(12),
                    DataFromDate = !reader.IsDBNull(13) && DateTime.TryParse(reader.GetString(13), out var dfd) ? dfd : DateTime.MinValue,
                    DataToDate = !reader.IsDBNull(14) && DateTime.TryParse(reader.GetString(14), out var dtd) ? dtd : DateTime.MinValue
                };

                list.Add(record);
            }

            return list;
        }

        public static async Task<List<SyncRunItemRecord>> GetRunItemsAsync(long runId)
        {
            await InitializeDatabaseAsync();
            var list = new List<SyncRunItemRecord>();

            await using var conn = new SqliteConnection(ConnectionString);
            await conn.OpenAsync();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT id, run_id, table_name, records_count, is_success, error_message, duration_ms
                FROM sync_run_items
                WHERE run_id = @runId
                ORDER BY is_success ASC, records_count DESC, table_name ASC
            ";
            cmd.Parameters.AddWithValue("@runId", runId);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var item = new SyncRunItemRecord
                {
                    Id = reader.GetInt64(0),
                    RunId = reader.GetInt64(1),
                    TableName = reader.GetString(2),
                    RecordsCount = reader.GetInt32(3),
                    IsSuccess = reader.GetInt32(4) == 1,
                    ErrorMessage = reader.IsDBNull(5) ? null : reader.GetString(5),
                    DurationMs = reader.GetInt32(6)
                };
                list.Add(item);
            }

            return list;
        }

        public static async Task<SyncHistoryStats> GetStatisticsAsync(DateTime from, DateTime to)
        {
            await InitializeDatabaseAsync();
            var stats = new SyncHistoryStats();

            await using var conn = new SqliteConnection(ConnectionString);
            await conn.OpenAsync();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT 
                    COUNT(*),
                    SUM(CASE WHEN status = 'SUCCESS' THEN 1 ELSE 0 END),
                    SUM(CASE WHEN status = 'WARNING' THEN 1 ELSE 0 END),
                    SUM(CASE WHEN status = 'ERROR' THEN 1 ELSE 0 END),
                    COALESCE(SUM(total_records), 0)
                FROM sync_runs
                WHERE start_time >= @from AND start_time <= @to;
            ";
            cmd.Parameters.AddWithValue("@from", from.ToString("yyyy-MM-ddTHH:mm:ss"));
            cmd.Parameters.AddWithValue("@to", to.ToString("yyyy-MM-ddTHH:mm:ss"));

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                stats.TotalRuns = reader.GetInt32(0);
                stats.SuccessRuns = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                stats.WarningRuns = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                stats.ErrorRuns = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                stats.TotalRecordsTransferred = reader.IsDBNull(4) ? 0 : reader.GetInt64(4);
            }

            return stats;
        }

        public static async Task<int> PruneOldRecordsAsync(int retentionDays = 60)
        {
            if (retentionDays <= 0) return 0;

            await InitializeDatabaseAsync();
            await _writeLock.WaitAsync();

            try
            {
                await using var conn = new SqliteConnection(ConnectionString);
                await conn.OpenAsync();

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    PRAGMA foreign_keys = ON;
                    DELETE FROM sync_runs 
                    WHERE datetime(start_time) < datetime('now', '-' || @days || ' days');
                ";
                cmd.Parameters.AddWithValue("@days", retentionDays);

                var affected = await cmd.ExecuteNonQueryAsync();
                return affected;
            }
            finally
            {
                _writeLock.Release();
            }
        }
    }
}
