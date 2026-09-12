using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace USWsSync.Core.State
{
    public static class SyncStateManager
    {
        private static readonly object _lock = new object();
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never
        };

        public static string GetStateFilePath(bool isUpload)
        {
            var fileName = isUpload ? "sync_state_upload.json" : "sync_state_download.json";
            return Path.Combine(AppContext.BaseDirectory, fileName);
        }

        public static SyncDirectionState LoadState(bool isUpload, DateTime? fallbackDate = null)
        {
            lock (_lock)
            {
                var filePath = GetStateFilePath(isUpload);
                var defaultDate = fallbackDate ?? DateTime.Today.AddDays(-1);

                if (!File.Exists(filePath))
                {
                    var newState = new SyncDirectionState
                    {
                        Direction = isUpload ? "Upload" : "Download",
                        LastGlobalSuccess = defaultDate
                    };
                    SaveState(isUpload, newState);
                    return newState;
                }

                try
                {
                    var json = File.ReadAllText(filePath);
                    var state = JsonSerializer.Deserialize<SyncDirectionState>(json, _jsonOptions);
                    if (state != null)
                    {
                        if (state.Tables == null)
                            state.Tables = new Dictionary<string, SyncTableState>(StringComparer.OrdinalIgnoreCase);
                        return state;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SyncStateManager] Error leyendo estado {(isUpload ? "Upload" : "Download")}: {ex.Message}");
                }

                return new SyncDirectionState
                {
                    Direction = isUpload ? "Upload" : "Download",
                    LastGlobalSuccess = defaultDate
                };
            }
        }

        public static void SaveState(bool isUpload, SyncDirectionState state)
        {
            lock (_lock)
            {
                var filePath = GetStateFilePath(isUpload);
                try
                {
                    var dir = Path.GetDirectoryName(filePath);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    var json = JsonSerializer.Serialize(state, _jsonOptions);
                    File.WriteAllText(filePath, json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SyncStateManager] Error guardando estado {(isUpload ? "Upload" : "Download")}: {ex.Message}");
                }
            }
        }

        public static DateTime GetTableCutDate(bool isUpload, string tableName, DateTime fallbackDate)
        {
            var state = LoadState(isUpload, fallbackDate);
            if (state.Tables.TryGetValue(tableName, out var tableState))
            {
                if (tableState.LastSuccessDate > DateTime.MinValue)
                {
                    return tableState.LastSuccessDate;
                }
            }
            return fallbackDate;
        }

        public static void RecordTableResult(
            bool isUpload,
            string tableName,
            DateTime syncToDate,
            bool isSuccess,
            int recordsCount,
            string? error = null)
        {
            lock (_lock)
            {
                var state = LoadState(isUpload);

                if (!state.Tables.TryGetValue(tableName, out var tableState))
                {
                    tableState = new SyncTableState { TableName = tableName };
                    state.Tables[tableName] = tableState;
                }

                tableState.LastAttemptDate = DateTime.Now;
                tableState.RecordsCount = recordsCount;

                if (isSuccess)
                {
                    tableState.LastSuccessDate = syncToDate;
                    tableState.IsSuccess = true;
                    tableState.LastError = null;
                }
                else
                {
                    tableState.IsSuccess = false;
                    tableState.LastError = error;
                }

                SaveState(isUpload, state);
            }
        }

        public static List<string> GetFailedTables(bool isUpload)
        {
            var state = LoadState(isUpload);
            return state.Tables.Values
                .Where(t => !t.IsSuccess && !string.IsNullOrEmpty(t.LastError))
                .Select(t => t.TableName)
                .ToList();
        }

        public static void ResetAllTablesCutDate(bool isUpload, DateTime newCutDate)
        {
            lock (_lock)
            {
                var state = LoadState(isUpload, newCutDate);
                state.LastGlobalSuccess = newCutDate;
                foreach (var table in state.Tables.Values)
                {
                    table.LastSuccessDate = newCutDate;
                    table.IsSuccess = true;
                    table.LastError = null;
                }
                SaveState(isUpload, state);
            }
        }
    }
}
