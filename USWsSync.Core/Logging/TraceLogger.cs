using System;
using System.IO;
using System.Text;

namespace USWsSync.Core.Logging
{
    public static class TraceLogger
    {
        public static string WriteTraceLog(string component, string traceContent, DateTime? timestamp = null)
        {
            try
            {
                var now = timestamp ?? DateTime.Now;
                var filePath = LogPathHelper.GetLogFilePath(component, now);
                var dir = Path.GetDirectoryName(filePath);

                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(filePath, traceContent, Encoding.UTF8);
                return filePath;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
