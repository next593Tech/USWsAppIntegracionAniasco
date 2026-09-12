using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using USWsLibrary.ModelDobraDatabase;
using USWsLibrary.Models;

namespace USWsLibrary.Services
{
    /// <summary>
    /// Motor de sincronizacion por lotes basado en Entity Framework puro (LINQ Contains + HashSet).
    /// Elimina el problema N+1 sin quemar sentencias SQL.
    /// </summary>
    public static class BatchSyncHelper
    {
        public static ErrorSave ExecuteBatchSave<TEntity, TKey>(
            List<TEntity> items,
            string tableName,
            Func<TEntity, TKey> keySelector,
            Func<DobraConnection, DbSet<TEntity>> dbSetSelector,
            Func<DobraConnection, List<TKey>, HashSet<TKey>> existingKeysFinder,
            int chunkSize = 500,
            Action<TEntity, bool> beforeSave = null)
            where TEntity : class
        {
            var errorSave = new ErrorSave { Tabla = tableName, errorExit = false, errorMessage = null };
            if (items == null || items.Count == 0) return errorSave;

            var totalCount = items.Count;

            for (int offset = 0; offset < totalCount; offset += chunkSize)
            {
                var chunk = items.Skip(offset).Take(chunkSize).ToList();
                var chunkKeys = chunk.Select(keySelector).Where(k => k != null).Distinct().ToList();

                // Deduplicate within chunk to avoid attaching the same primary key twice
                var distinctChunk = chunk
                    .GroupBy(keySelector)
                    .Select(g => g.Key == null ? g.First() : g.Last())
                    .ToList();

                using (var db = new DobraConnection())
                {
                    db.Configuration.AutoDetectChangesEnabled = false;
                    db.Configuration.ValidateOnSaveEnabled = false;

                    using (var tx = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var existingSet = existingKeysFinder(db, chunkKeys);
                            var dbSet = dbSetSelector(db);

                            foreach (var item in distinctChunk)
                            {
                                var key = keySelector(item);
                                var exists = key != null && existingSet.Contains(key);

                                if (beforeSave != null)
                                {
                                    beforeSave(item, exists);
                                }

                                if (exists)
                                {
                                    db.Entry(item).State = EntityState.Modified;
                                }
                                else
                                {
                                    dbSet.Add(item);
                                }
                            }

                            db.Configuration.AutoDetectChangesEnabled = true;
                            db.SaveChanges();
                            tx.Commit();
                        }
                        catch (Exception ex)
                        {
                            try { tx.Rollback(); } catch { }
                            FormatError(ex, tableName, distinctChunk, keySelector, errorSave);
                            return errorSave;
                        }
                    }
                }
            }

            return errorSave;
        }

        public static ErrorSave ExecuteBatchSaveComposite<TEntity>(
            List<TEntity> items,
            string tableName,
            Func<TEntity, string> compositeKeySelector,
            Func<DobraConnection, DbSet<TEntity>> dbSetSelector,
            Func<DobraConnection, List<TEntity>, HashSet<string>> existingKeysFinder,
            int chunkSize = 500,
            Action<TEntity, bool> beforeSave = null)
            where TEntity : class
        {
            var errorSave = new ErrorSave { Tabla = tableName, errorExit = false, errorMessage = null };
            if (items == null || items.Count == 0) return errorSave;

            var totalCount = items.Count;

            for (int offset = 0; offset < totalCount; offset += chunkSize)
            {
                var chunk = items.Skip(offset).Take(chunkSize).ToList();

                var distinctChunk = chunk
                    .GroupBy(compositeKeySelector)
                    .Select(g => g.Key == null ? g.First() : g.Last())
                    .ToList();

                using (var db = new DobraConnection())
                {
                    db.Configuration.AutoDetectChangesEnabled = false;
                    db.Configuration.ValidateOnSaveEnabled = false;

                    using (var tx = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var existingSet = existingKeysFinder(db, distinctChunk);
                            var dbSet = dbSetSelector(db);

                            foreach (var item in distinctChunk)
                            {
                                var key = compositeKeySelector(item);
                                var exists = key != null && existingSet.Contains(key);

                                if (beforeSave != null)
                                {
                                    beforeSave(item, exists);
                                }

                                if (exists)
                                {
                                    db.Entry(item).State = EntityState.Modified;
                                }
                                else
                                {
                                    dbSet.Add(item);
                                }
                            }

                            db.Configuration.AutoDetectChangesEnabled = true;
                            db.SaveChanges();
                            tx.Commit();
                        }
                        catch (Exception ex)
                        {
                            try { tx.Rollback(); } catch { }
                            FormatError(ex, tableName, distinctChunk, compositeKeySelector, errorSave);
                            return errorSave;
                        }
                    }
                }
            }

            return errorSave;
        }

        public static void FormatError<TEntity, TKey>(
            Exception ex,
            string tableName,
            List<TEntity> chunk,
            Func<TEntity, TKey> keySelector,
            ErrorSave errorSave)
        {
            errorSave.errorExit = true;
            var sb = new StringBuilder();

            if (ex is DbEntityValidationException valEx)
            {
                sb.AppendLine("[Error Validacion EF en " + tableName + "]:");
                foreach (var err in valEx.EntityValidationErrors)
                {
                    foreach (var ve in err.ValidationErrors)
                    {
                        sb.AppendLine(" - Campo '" + ve.PropertyName + "': " + ve.ErrorMessage);
                    }
                }
            }
            else
            {
                var sqlEx = FindSqlException(ex);
                if (sqlEx != null)
                {
                    sb.AppendLine("[Error SQL Server #" + sqlEx.Number + " en " + tableName + "]: " + sqlEx.Message + " (Linea " + sqlEx.LineNumber + ")");
                }
                else
                {
                    var root = GetRootException(ex);
                    sb.AppendLine("[Excepcion en " + tableName + "]: " + root.Message);
                }
            }

            if (chunk != null && chunk.Count > 0 && keySelector != null)
            {
                var sampleKeys = chunk.Select(keySelector).Where(k => k != null).Take(5).ToList();
                if (sampleKeys.Count > 0)
                {
                    sb.Append(" (Lote fallido IDs muestra: " + string.Join(", ", sampleKeys));
                    if (chunk.Count > 5) sb.Append(" ... y " + (chunk.Count - 5) + " mas)");
                    else sb.Append(")");
                }
            }

            errorSave.errorMessage = (errorSave.errorMessage != null ? errorSave.errorMessage + "\n" : "") + sb.ToString();
        }

        public static string FormatGeneralException(Exception ex)
        {
            if (ex is DbEntityValidationException valEx)
            {
                var sb = new StringBuilder("[Error Validacion EF]: ");
                foreach (var err in valEx.EntityValidationErrors)
                {
                    foreach (var ve in err.ValidationErrors)
                    {
                        sb.Append(string.Format("'{0}': {1}; ", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                return sb.ToString();
            }

            var sqlEx = FindSqlException(ex);
            if (sqlEx != null)
            {
                return string.Format("[Error SQL Server #{0}]: {1} (Linea {2})", sqlEx.Number, sqlEx.Message, sqlEx.LineNumber);
            }

            var root = GetRootException(ex);
            return "[Error de Excepcion]: " + root.Message;
        }

        private static SqlException FindSqlException(Exception ex)
        {
            var cur = ex;
            while (cur != null)
            {
                if (cur is SqlException sqlEx) return sqlEx;
                cur = cur.InnerException;
            }
            return null;
        }

        private static Exception GetRootException(Exception ex)
        {
            var cur = ex;
            while (cur.InnerException != null) cur = cur.InnerException;
            return cur;
        }
    }
}
