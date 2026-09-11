using System;

namespace USWsSync.Core.Configuration
{
    public class SyncConfig
    {
        public string IpLocal { get; set; } = "192.168.10.39:8484";
        public string IpPublica { get; set; } = "192.168.10.39:8484";
        public DateTime LastDateUpdate { get; set; } = DateTime.Today.AddDays(-1);
        public DateTime LastDateDownload { get; set; } = DateTime.Today.AddDays(-1);
        public bool GuardarLogsJson { get; set; } = true;
        public string DirectorioLogs { get; set; } = @"C:\logsJSON";
        public int TimeoutSegundos { get; set; } = 300;

        public string CleanIpLocal => CleanHost(IpLocal);
        public string CleanIpPublica => CleanHost(IpPublica);

        public string GetLocalApiBase() => CleanIp(IpLocal);
        public string GetPublicApiBase() => CleanIp(IpPublica);

        public static string CleanHost(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            var trimmed = input.Trim();
            if (trimmed.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                trimmed = trimmed.Substring(7);
            if (trimmed.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                trimmed = trimmed.Substring(8);
            return trimmed.TrimEnd('/');
        }

        public static string CleanIp(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            var trimmed = input.Trim();
            string protocol = "http://";
            if (trimmed.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                protocol = "https://";
                trimmed = trimmed.Substring(8);
            }
            else if (trimmed.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                trimmed = trimmed.Substring(7);
            }
            trimmed = trimmed.TrimEnd('/');
            if (trimmed.EndsWith("/api", StringComparison.OrdinalIgnoreCase))
            {
                return $"{protocol}{trimmed}";
            }
            return $"{protocol}{trimmed}/api";
        }
    }
}
