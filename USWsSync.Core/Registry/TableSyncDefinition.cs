using System;
using System.Threading;
using System.Threading.Tasks;
using USWsLibrary.Models;
using USWsSync.Core.Engine;

namespace USWsSync.Core.Registry
{
    public class TableSyncDefinition
    {
        public string TableName { get; set; } = "";
        public string DownloadMethod { get; set; } = "";
        public string UploadMethod { get; set; } = "";
        public SyncModule Module { get; set; } = SyncModule.Todos;
        public int RecommendedBatchSize { get; set; } = 1000;
        public Func<object?, int> GetRecordCount { get; set; } = _ => 0;
        public Func<ISyncEngine, string, DateTime, DateTime, CancellationToken, Task<object?>> DownloadFunc { get; set; } = null!;
        public Func<ISyncEngine, string, object, CancellationToken, Task<ErrorSave>> UploadFunc { get; set; } = null!;

        public static TableSyncDefinition Create<T>(
            string tableName, 
            string downloadMethod, 
            string uploadMethod, 
            SyncModule module = SyncModule.Todos, 
            int batchSize = 1000) where T : class, new()
        {
            return new TableSyncDefinition
            {
                TableName = tableName,
                DownloadMethod = downloadMethod,
                UploadMethod = uploadMethod,
                Module = module,
                RecommendedBatchSize = batchSize,
                GetRecordCount = (data) => (data as PagedList<T>)?.Results?.Count ?? 0,
                DownloadFunc = async (engine, url, f1, f2, ct) =>
                {
                    var result = await engine.DownloadAsync<T>(url, downloadMethod, f1, f2, ct);
                    return result;
                },
                UploadFunc = async (engine, url, data, ct) =>
                {
                    if (data is PagedList<T> pagedList)
                    {
                        return await engine.UploadAsync<T>(url, uploadMethod, pagedList, batchSize, ct);
                    }
                    return new ErrorSave { errorExit = true, errorMessage = "Tipo de dato incompatible para subida: " + typeof(T).Name };
                }
            };
        }
    }
}
