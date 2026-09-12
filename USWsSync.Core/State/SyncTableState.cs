using System;
using System.Collections.Generic;

namespace USWsSync.Core.State
{
    public class SyncTableState
    {
        public string TableName { get; set; } = "";
        public DateTime LastSuccessDate { get; set; }
        public DateTime LastAttemptDate { get; set; }
        public bool IsSuccess { get; set; }
        public int RecordsCount { get; set; }
        public string? LastError { get; set; }
    }

    public class SyncDirectionState
    {
        public string Direction { get; set; } = ""; // "Download" o "Upload"
        public DateTime LastGlobalSuccess { get; set; } = DateTime.Today.AddDays(-1);
        public Dictionary<string, SyncTableState> Tables { get; set; } = new Dictionary<string, SyncTableState>(StringComparer.OrdinalIgnoreCase);
    }
}
