using System;
using System.Threading;
using System.Threading.Tasks;
using USWsLibrary.Models;

using System.Collections.Generic;
using USWsSync.Core.Registry;
using USWsSync.Core.History;

namespace USWsSync.Core.Engine
{
    public record SyncProgressInfo(
        string TableName,
        int CurrentIndex,
        int TotalTables,
        double Percentage,
        bool IsSuccess,
        string Message,
        int RecordsCount = 0
    );

    public interface ISyncEngine
    {
        Task<bool> CheckConnectionAsync(string ipOrUrl, CancellationToken ct = default);
        Task<PagedList<T>?> DownloadAsync<T>(string baseUrl, string methodName, DateTime f1, DateTime f2, CancellationToken ct = default) where T : class, new();
        Task<ErrorSave> UploadAsync<T>(string baseUrl, string methodName, PagedList<T> payload, int batchSize = 1000, CancellationToken ct = default) where T : class, new();
        Task<bool> ExecuteDownloadBatchAsync(
            string sourceApiBase, 
            string targetApiBase, 
            DateTime f1, 
            DateTime f2, 
            IProgress<SyncProgressInfo>? progress, 
            IEnumerable<string>? tableFilter = null,
            SyncModule? moduleFilter = null,
            bool updateWatermark = false,
            bool useIndividualTableDates = false,
            SyncTriggerSource trigger = SyncTriggerSource.Scheduled,
            CancellationToken ct = default);
        Task<bool> ExecuteUploadBatchAsync(
            string sourceApiBase, 
            string targetApiBase, 
            DateTime f1, 
            DateTime f2, 
            IProgress<SyncProgressInfo>? progress, 
            IEnumerable<string>? tableFilter = null,
            SyncModule? moduleFilter = null,
            bool updateWatermark = false,
            bool useIndividualTableDates = false,
            SyncTriggerSource trigger = SyncTriggerSource.Scheduled,
            CancellationToken ct = default);
    }
}
