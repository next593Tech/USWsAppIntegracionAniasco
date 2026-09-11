using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace USWsSync.Core.Configuration
{
    public static class ConfigManager
    {
        private static readonly object _lock = new object();
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never
        };

        public static string GetConfigFilePath()
        {
            return Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        }

        public static SyncConfig LoadConfig(string? customPath = null)
        {
            lock (_lock)
            {
                var filePath = customPath ?? GetConfigFilePath();
                if (!File.Exists(filePath))
                {
                    var defaultConfig = new SyncConfig();
                    SaveConfig(defaultConfig, filePath);
                    return defaultConfig;
                }

                try
                {
                    var json = File.ReadAllText(filePath);
                    using var doc = JsonDocument.Parse(json);

                    // Revisa si existe la seccion "SyncConfig", de lo contrario lee el root
                    if (doc.RootElement.TryGetProperty("syncConfig", out var section) ||
                        doc.RootElement.TryGetProperty("SyncConfig", out section))
                    {
                        return JsonSerializer.Deserialize<SyncConfig>(section.GetRawText(), _jsonOptions) ?? new SyncConfig();
                    }

                    return JsonSerializer.Deserialize<SyncConfig>(json, _jsonOptions) ?? new SyncConfig();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ConfigManager] Error leyendo config, usando defaults: {ex.Message}");
                    return new SyncConfig();
                }
            }
        }

        public static void SaveConfig(SyncConfig config, string? customPath = null)
        {
            lock (_lock)
            {
                var filePath = customPath ?? GetConfigFilePath();
                try
                {
                    // Formato compatible con appsettings.json estructurado
                    var wrapper = new
                    {
                        SyncConfig = config
                    };

                    var json = JsonSerializer.Serialize(wrapper, _jsonOptions);
                    var dir = Path.GetDirectoryName(filePath);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    File.WriteAllText(filePath, json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ConfigManager] Error guardando config: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
