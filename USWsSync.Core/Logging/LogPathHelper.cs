using System;
using System.IO;

namespace USWsSync.Core.Logging
{
    public static class LogPathHelper
    {
        public const string BaseLogsDirectory = @"C:\logs";

        public static string GetLogDirectory(string component, DateTime date)
        {
            return Path.Combine(
                BaseLogsDirectory,
                component,
                date.ToString("yyyy"),
                date.ToString("MM"),
                date.ToString("dd")
            );
        }

        public static string GetLogFilePath(string component, DateTime timestamp)
        {
            var dir = GetLogDirectory(component, timestamp);
            var fileName = $"{component}_{timestamp:yyyyMMdd_HHmmss}.log";
            return Path.Combine(dir, fileName);
        }

        public static string EnsureDirectoryExists(string component, DateTime date)
        {
            var dir = GetLogDirectory(component, date);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }
    }
}
