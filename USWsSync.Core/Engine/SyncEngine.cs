using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using USWsLibrary.Models;
using USWsSync.Core.Configuration;
using USWsSync.Core.Registry;

namespace USWsSync.Core.Engine
{
    public class SyncEngine : ISyncEngine
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SyncEngine> _logger;
        private readonly Func<SyncConfig>? _configProvider;
        private readonly JsonSerializerOptions _jsonOptions;

        public SyncEngine(
            IHttpClientFactory httpClientFactory,
            Func<SyncConfig>? configProvider = null,
            ILogger<SyncEngine>? logger = null)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _configProvider = configProvider;
            _logger = logger ?? NullLogger<SyncEngine>.Instance;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = null, // Match C# model property names
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = false
            };
        }

        private SyncConfig GetConfig() => _configProvider?.Invoke() ?? ConfigManager.LoadConfig();

        private HttpClient CreateClient(int timeoutSeconds = 120)
        {
            var client = _httpClientFactory.CreateClient("SyncEngineClient");
            client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
            return client;
        }

        public async Task<bool> CheckConnectionAsync(string ipOrUrl, CancellationToken ct = default)
        {
            try
            {
                var cleanBase = SyncConfig.CleanIp(ipOrUrl);
                var testUrl = $"{cleanBase}/Clientes/listSisParametros?lastUpdate={DateTime.Today:yyyy-MM-ddTHH:mm:ss}&lastUpdate2={DateTime.Now:yyyy-MM-ddTHH:mm:ss}";

                using var client = CreateClient(timeoutSeconds: 8);
                using var request = new HttpRequestMessage(HttpMethod.Post, testUrl);
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);

                _logger.LogInformation("Verificando conexión con {Url}: Status {StatusCode}", testUrl, response.StatusCode);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Fallo al verificar conexión con {Target}", ipOrUrl);
                return false;
            }
        }

        public async Task<PagedList<T>?> DownloadAsync<T>(
            string baseUrl,
            string methodName,
            DateTime f1,
            DateTime f2,
            CancellationToken ct = default) where T : class, new()
        {
            var cleanBase = SyncConfig.CleanIp(baseUrl);
            var f1Str = f1.ToString("yyyy-MM-ddTHH:mm:ss");
            var f2Str = f2.ToString("yyyy-MM-ddTHH:mm:ss");
            var url = $"{cleanBase}/Clientes/{methodName}?lastUpdate={f1Str}&lastUpdate2={f2Str}";

            var config = GetConfig();
            var tableName = typeof(T).Name;
            var isLocalSource = cleanBase.Contains(config.CleanIpLocal, StringComparison.OrdinalIgnoreCase);
            var opFolder = isLocalSource ? "Upload_DesdeLocal" : "Download_DesdeNube";
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            try
            {
                using var client = CreateClient(timeoutSeconds: 180);
                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Accept.ParseAdd("application/json");

                using var response = await client.SendAsync(request, ct);
                var rawBody = await response.Content.ReadAsStringAsync(ct);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error HTTP al descargar {Table} desde {Url}: {StatusCode} - {Body}",
                        tableName, url, response.StatusCode, rawBody);
                    SaveErrorLog(opFolder, "Errores_Red", $"ErrorHTTP_{tableName}_{timestamp}.txt",
                        $"URL: {url}\nStatusCode: {response.StatusCode}\nBody: {rawBody}");
                    return null;
                }

                try
                {
                    var result = JsonSerializer.Deserialize<PagedList<T>>(rawBody, _jsonOptions);
                    return result;
                }
                catch (Exception parseEx)
                {
                    _logger.LogError(parseEx, "Error al deserializar JSON de {Table} desde {Url}", tableName, url);
                    SaveErrorLog(opFolder, "Errores_Parseo", $"ErrorParseo_{tableName}_{timestamp}.txt",
                        $"URL: {url}\nException: {parseEx}\nRawResponse:\n{rawBody}");
                    return null;
                }
            }
            catch (Exception netEx)
            {
                _logger.LogError(netEx, "Excepción de red al descargar {Table} desde {Url}", tableName, url);
                SaveErrorLog(opFolder, "Errores_Red", $"ErrorRed_{tableName}_{timestamp}.txt",
                    $"URL: {url}\nException: {netEx}");
                return null;
            }
        }

        public async Task<ErrorSave> UploadAsync<T>(
            string baseUrl,
            string methodName,
            PagedList<T> payload,
            CancellationToken ct = default) where T : class, new()
        {
            var cleanBase = SyncConfig.CleanIp(baseUrl);
            var url = $"{cleanBase}/Clientes/{methodName}";

            var config = GetConfig();
            var tableName = typeof(T).Name;
            var isLocalTarget = cleanBase.Contains(config.CleanIpLocal, StringComparison.OrdinalIgnoreCase);
            var opFolder = isLocalTarget ? "Download_HaciaLocal" : "Upload_HaciaNube";
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            string jsonPayload;
            try
            {
                jsonPayload = JsonSerializer.Serialize(payload, _jsonOptions);
                if (config.GuardarLogsJson)
                {
                    SaveDebugJson(opFolder, timestamp, tableName, jsonPayload);
                }
            }
            catch (Exception encEx)
            {
                _logger.LogError(encEx, "Error al serializar JSON para {Table}", tableName);
                SaveErrorLog(opFolder, "Errores_Parseo", $"ErrorEncode_{tableName}_{timestamp}.txt",
                    $"URL: {url}\nException: {encEx}");
                return new ErrorSave
                {
                    Tabla = tableName,
                    errorExit = true,
                    errorMessage = $"Excepción Crítica (Encode JSON): {encEx.Message}"
                };
            }

            try
            {
                using var client = CreateClient(timeoutSeconds: 180);
                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Accept.ParseAdd("application/json");
                request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                using var response = await client.SendAsync(request, ct);
                var rawBody = await response.Content.ReadAsStringAsync(ct);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error HTTP al subir {Table} hacia {Url}: {StatusCode} - {Body}",
                        tableName, url, response.StatusCode, rawBody);
                    SaveErrorLog(opFolder, "Errores_Red", $"ErrorHTTP_{tableName}_{timestamp}.txt",
                        $"URL: {url}\nStatusCode: {response.StatusCode}\nBody: {rawBody}");
                    return new ErrorSave
                    {
                        Tabla = tableName,
                        errorExit = true,
                        errorMessage = $"Error HTTP {(int)response.StatusCode} al subir {tableName}: {rawBody}"
                    };
                }

                try
                {
                    var errorSave = JsonSerializer.Deserialize<ErrorSave>(rawBody, _jsonOptions);
                    if (errorSave == null)
                    {
                        return new ErrorSave
                        {
                            Tabla = tableName,
                            errorExit = true,
                            errorMessage = "Respuesta vacía o nula del servidor al guardar " + tableName
                        };
                    }
                    return errorSave;
                }
                catch (Exception decEx)
                {
                    _logger.LogError(decEx, "Error al deserializar ErrorSave de {Table}", tableName);
                    SaveErrorLog(opFolder, "Errores_Parseo", $"ErrorDecode_{tableName}_{timestamp}.txt",
                        $"URL: {url}\nException: {decEx}\nRawResponse:\n{rawBody}");
                    return new ErrorSave
                    {
                        Tabla = tableName,
                        errorExit = true,
                        errorMessage = $"Error de parseo al leer respuesta de guardado: {decEx.Message}"
                    };
                }
            }
            catch (Exception netEx)
            {
                _logger.LogError(netEx, "Excepción de red al subir {Table} hacia {Url}", tableName, url);
                SaveErrorLog(opFolder, "Errores_Red", $"ErrorRed_{tableName}_{timestamp}.txt",
                    $"URL: {url}\nException: {netEx}");
                return new ErrorSave
                {
                    Tabla = tableName,
                    errorExit = true,
                    errorMessage = $"Excepción Crítica (HTTP Subida): {netEx.Message}"
                };
            }
        }

        public async Task<bool> ExecuteDownloadBatchAsync(
            string sourceApiBase,
            string targetApiBase,
            DateTime f1,
            DateTime f2,
            IProgress<SyncProgressInfo>? progress,
            CancellationToken ct = default)
        {
            var tables = TableRegistry.DownloadTables;
            var total = tables.Count;
            var allSuccess = true;

            _logger.LogInformation("Iniciando lote de Descarga (Nube -> Local). Tablas: {Total}. Rango: {F1} a {F2}", total, f1, f2);

            for (int i = 0; i < total; i++)
            {
                ct.ThrowIfCancellationRequested();

                var item = tables[i];
                var currentIndex = i + 1;
                var pct = (double)currentIndex / total;

                progress?.Report(new SyncProgressInfo(
                    item.TableName, currentIndex, total, pct, true,
                    $"[Descarga {currentIndex}/{total}] Descargando {item.TableName}..."
                ));

                try
                {
                    var data = await item.DownloadFunc(this, sourceApiBase, f1, f2, ct);
                    if (data == null)
                    {
                        allSuccess = false;
                        var errorMsg = $"Error o respuesta nula al descargar {item.TableName}";
                        _logger.LogWarning("{Msg}", errorMsg);
                        progress?.Report(new SyncProgressInfo(
                            item.TableName, currentIndex, total, pct, false, errorMsg
                        ));
                        continue;
                    }

                    // Guardar en destino local
                    progress?.Report(new SyncProgressInfo(
                        item.TableName, currentIndex, total, pct, true,
                        $"[Descarga {currentIndex}/{total}] Guardando {item.TableName} en base local..."
                    ));

                    var saveResult = await item.UploadFunc(this, targetApiBase, data, ct);
                    if (saveResult.errorExit)
                    {
                        allSuccess = false;
                        var errorMsg = $"Error guardando {item.TableName}: {saveResult.errorMessage}";
                        _logger.LogError("{Msg}", errorMsg);
                        progress?.Report(new SyncProgressInfo(
                            item.TableName, currentIndex, total, pct, false, errorMsg
                        ));
                    }
                    else
                    {
                        var okMsg = $"[OK {currentIndex}/{total}] {item.TableName} sincronizada con éxito.";
                        _logger.LogInformation("{Msg}", okMsg);
                        progress?.Report(new SyncProgressInfo(
                            item.TableName, currentIndex, total, pct, true, okMsg
                        ));
                    }
                }
                catch (Exception ex)
                {
                    allSuccess = false;
                    var errorMsg = $"Excepción no controlada procesando {item.TableName}: {ex.Message}";
                    _logger.LogError(ex, "{Msg}", errorMsg);
                    progress?.Report(new SyncProgressInfo(
                        item.TableName, currentIndex, total, pct, false, errorMsg
                    ));
                }
            }

            _logger.LogInformation("Fin del lote de Descarga. Resultado global: {Result}", allSuccess ? "EXITOSO" : "CON ERRORES");
            return allSuccess;
        }

        public async Task<bool> ExecuteUploadBatchAsync(
            string sourceApiBase,
            string targetApiBase,
            DateTime f1,
            DateTime f2,
            IProgress<SyncProgressInfo>? progress,
            CancellationToken ct = default)
        {
            var tables = TableRegistry.UploadTables;
            var total = tables.Count;
            var allSuccess = true;

            _logger.LogInformation("Iniciando lote de Subida (Local -> Nube). Tablas: {Total}. Rango: {F1} a {F2}", total, f1, f2);

            for (int i = 0; i < total; i++)
            {
                ct.ThrowIfCancellationRequested();

                var item = tables[i];
                var currentIndex = i + 1;
                var pct = (double)currentIndex / total;

                progress?.Report(new SyncProgressInfo(
                    item.TableName, currentIndex, total, pct, true,
                    $"[Subida {currentIndex}/{total}] Consultando {item.TableName} en base local..."
                ));

                try
                {
                    var data = await item.DownloadFunc(this, sourceApiBase, f1, f2, ct);
                    if (data == null)
                    {
                        allSuccess = false;
                        var errorMsg = $"Error o respuesta nula al obtener {item.TableName} desde local";
                        _logger.LogWarning("{Msg}", errorMsg);
                        progress?.Report(new SyncProgressInfo(
                            item.TableName, currentIndex, total, pct, false, errorMsg
                        ));
                        continue;
                    }

                    progress?.Report(new SyncProgressInfo(
                        item.TableName, currentIndex, total, pct, true,
                        $"[Subida {currentIndex}/{total}] Subiendo {item.TableName} a la nube..."
                    ));

                    var saveResult = await item.UploadFunc(this, targetApiBase, data, ct);
                    if (saveResult.errorExit)
                    {
                        allSuccess = false;
                        var errorMsg = $"Error guardando {item.TableName} en nube: {saveResult.errorMessage}";
                        _logger.LogError("{Msg}", errorMsg);
                        progress?.Report(new SyncProgressInfo(
                            item.TableName, currentIndex, total, pct, false, errorMsg
                        ));
                    }
                    else
                    {
                        var okMsg = $"[OK {currentIndex}/{total}] {item.TableName} subida con éxito.";
                        _logger.LogInformation("{Msg}", okMsg);
                        progress?.Report(new SyncProgressInfo(
                            item.TableName, currentIndex, total, pct, true, okMsg
                        ));
                    }
                }
                catch (Exception ex)
                {
                    allSuccess = false;
                    var errorMsg = $"Excepción no controlada procesando {item.TableName}: {ex.Message}";
                    _logger.LogError(ex, "{Msg}", errorMsg);
                    progress?.Report(new SyncProgressInfo(
                        item.TableName, currentIndex, total, pct, false, errorMsg
                    ));
                }
            }

            _logger.LogInformation("Fin del lote de Subida. Resultado global: {Result}", allSuccess ? "EXITOSO" : "CON ERRORES");
            return allSuccess;
        }

        private void SaveDebugJson(string opFolder, string timestamp, string tableName, string content)
        {
            try
            {
                var dir = Path.Combine(@"C:\logsJSON", opFolder, timestamp);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var filePath = Path.Combine(dir, $"{tableName}.json");
                File.WriteAllText(filePath, content, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo escribir JSON de depuración para {Table}", tableName);
            }
        }

        private void SaveErrorLog(string opFolder, string subFolder, string fileName, string content)
        {
            try
            {
                var dir = Path.Combine(@"C:\logsJSON", opFolder, subFolder);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var filePath = Path.Combine(dir, fileName);
                File.WriteAllText(filePath, content, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo escribir log de error {FileName}", fileName);
            }
        }
    }
}
