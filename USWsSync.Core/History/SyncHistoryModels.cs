using System;
using System.Collections.Generic;

namespace USWsSync.Core.History
{
    public enum SyncDirection
    {
        Download,
        Upload
    }

    public enum SyncTriggerSource
    {
        Scheduled,
        ManualUI
    }

    public enum SyncRunStatus
    {
        Success,
        Warning,
        Error
    }

    public class SyncRunRecord
    {
        public long Id { get; set; }
        public string RunId { get; set; } = Guid.NewGuid().ToString("N");
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now;
        public int DurationMs { get; set; }
        public SyncDirection Direction { get; set; }
        public SyncTriggerSource TriggerSource { get; set; }
        public SyncRunStatus Status { get; set; }
        public int TotalTables { get; set; }
        public int TablesWithChanges { get; set; }
        public int TablesWithErrors { get; set; }
        public int TotalRecords { get; set; }
        public DateTime DataFromDate { get; set; } = DateTime.MinValue;
        public DateTime DataToDate { get; set; } = DateTime.MinValue;
        public string? ErrorSummary { get; set; }

        public List<SyncRunItemRecord> Items { get; set; } = new();
    }

    public class SyncRunItemRecord
    {
        public long Id { get; set; }
        public long RunId { get; set; }
        public string TableName { get; set; } = string.Empty;
        public int RecordsCount { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string? ErrorMessage { get; set; }
        public int DurationMs { get; set; }
    }

    public class SyncHistoryFilter
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public SyncDirection? Direction { get; set; }
        public SyncRunStatus? Status { get; set; }
        public bool OnlyWithChangesOrErrors { get; set; }
        public string? TableSearch { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }

    public class SyncHistoryStats
    {
        public int TotalRuns { get; set; }
        public int SuccessRuns { get; set; }
        public int WarningRuns { get; set; }
        public int ErrorRuns { get; set; }
        public long TotalRecordsTransferred { get; set; }
    }
}
