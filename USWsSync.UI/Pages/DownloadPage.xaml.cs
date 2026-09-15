using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using USWsSync.Core.Configuration;
using USWsSync.Core.Engine;
using USWsSync.Core.Logging;
using USWsSync.Core.Registry;
using USWsSync.Core.State;
using USWsSync.Core.History;

namespace USWsSync_UI.Pages
{
    public sealed partial class DownloadPage : Page
    {
        private readonly ISyncEngine _syncEngine;
        private CancellationTokenSource? _cts;
        private readonly StringBuilder _logBuilder = new();
        private readonly List<string> _failedTablesList = new();
        private bool _isInitialized;

        public DownloadPage()
        {
            InitializeComponent();
            NavigationCacheMode = Microsoft.UI.Xaml.Navigation.NavigationCacheMode.Required;
            _syncEngine = App.Services.GetRequiredService<ISyncEngine>();
            Loaded += DownloadPage_Loaded;
        }

        private void DownloadPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized)
            {
                var config = ConfigManager.LoadConfig();
                DpFechaInicio.Date = DateTimeOffset.Now.Date;
                DpFechaFinal.Date = DateTimeOffset.Now.Date;

                var dlState = SyncStateManager.LoadState(isUpload: false);
                TxtLastDate.Text = dlState.LastGlobalSuccess > DateTime.MinValue
                    ? dlState.LastGlobalSuccess.ToString("yyyy-MM-dd HH:mm:ss")
                    : config.LastDateDownload.ToString("yyyy-MM-dd HH:mm:ss");

                var priorFailed = SyncStateManager.GetFailedTables(isUpload: false);
                if (priorFailed.Count > 0)
                {
                    _failedTablesList.Clear();
                    _failedTablesList.AddRange(priorFailed);
                    BtnRetryFailed.Visibility = Visibility.Visible;
                    TxtRetryFailed.Text = $"Reintentar tablas fallidas previas ({_failedTablesList.Count})";
                }

                _isInitialized = true;
            }
        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            await RunSyncAsync(tableFilter: null);
        }

        private async void BtnRetryFailed_Click(object sender, RoutedEventArgs e)
        {
            if (_failedTablesList.Count == 0) return;
            await RunSyncAsync(tableFilter: new List<string>(_failedTablesList));
        }

        private async Task RunSyncAsync(IEnumerable<string>? tableFilter)
        {
            var f1Offset = DpFechaInicio.Date ?? DateTimeOffset.Now.Date;
            var f2Offset = DpFechaFinal.Date ?? DateTimeOffset.Now.Date;

            var f1 = new DateTime(f1Offset.Year, f1Offset.Month, f1Offset.Day, 0, 0, 0);
            var f2 = new DateTime(f2Offset.Year, f2Offset.Month, f2Offset.Day, 23, 59, 59);

            var config = ConfigManager.LoadConfig();

            SyncModule? moduleFilter = null;
            if (CmbModule.SelectedItem is ComboBoxItem item && item.Tag is string tag && Enum.TryParse<SyncModule>(tag, out var parsedModule) && parsedModule != SyncModule.Todos)
            {
                moduleFilter = parsedModule;
            }

            var updateWatermark = ChkUpdateWatermark.IsChecked == true;

            BtnStart.IsEnabled = false;
            BtnCancel.IsEnabled = true;
            BtnRetryFailed.IsEnabled = false;
            PbSync.Value = 0;
            PbSync.ShowError = false;
            TxtProgressPct.Text = "0%";
            TxtStatus.Text = "Verificando conectividad...";

            _logBuilder.Clear();
            TxtLogs.Text = "";

            _cts = new CancellationTokenSource();
            var ct = _cts.Token;

            var isRetry = tableFilter != null && tableFilter.Any();
            var targetDesc = isRetry 
                ? $"Reintento de {tableFilter!.Count()} tablas" 
                : (moduleFilter.HasValue ? $"Módulo {moduleFilter.Value}" : "Todos los módulos");

            AppendLog($"[{DateTime.Now:HH:mm:ss}] Iniciando descarga de datos ({targetDesc}) desde {config.IpPublica} hacia {config.IpLocal}");
            AppendLog($"[{DateTime.Now:HH:mm:ss}] Rango seleccionado: {f1:yyyy-MM-dd HH:mm:ss} - {f2:yyyy-MM-dd HH:mm:ss}");
            if (updateWatermark)
            {
                AppendLog($"[{DateTime.Now:HH:mm:ss}] MODO CHECKPOINT: Las marcas de agua se actualizarán para las tablas exitosas.");
            }

            var currentFailedTables = new List<string>();
            var successCount = 0;

            try
            {
                var connOk = await _syncEngine.CheckConnectionAsync(config.IpPublica, ct);
                if (!connOk)
                {
                    PbSync.ShowError = true;
                    AppendLog($"[{DateTime.Now:HH:mm:ss}] ERROR: No hay conexión con el servidor público ({config.IpPublica}). Verifique red.");
                    TxtStatus.Text = "Fallo de conexión con el servidor remoto.";
                    return;
                }

                var progress = new Progress<SyncProgressInfo>(info =>
                {
                    DispatcherQueue.TryEnqueue(() =>
                    {
                        TxtCurrentTable.Text = $"Tabla: {info.TableName}";
                        var pctValue = Math.Round(info.Percentage * 100, 1);
                        PbSync.Value = pctValue;
                        TxtProgressPct.Text = $"{pctValue}%";
                        TxtStatus.Text = info.Message;

                        if (!info.IsSuccess)
                        {
                            if (!currentFailedTables.Contains(info.TableName))
                            {
                                currentFailedTables.Add(info.TableName);
                            }
                            PbSync.ShowError = true;
                        }
                        else if (info.Message.StartsWith("[OK"))
                        {
                            successCount++;
                        }

                        AppendLog($"[{DateTime.Now:HH:mm:ss}] {info.Message}");
                    });
                });

                var success = await Task.Run(() => _syncEngine.ExecuteDownloadBatchAsync(
                    config.GetPublicApiBase(),
                    config.GetLocalApiBase(),
                    f1,
                    f2,
                    progress,
                    tableFilter: tableFilter,
                    moduleFilter: moduleFilter,
                    updateWatermark: updateWatermark,
                    useIndividualTableDates: false,
                    trigger: SyncTriggerSource.ManualUI,
                    ct: ct
                ), ct);

                if (success && currentFailedTables.Count == 0)
                {
                    PbSync.ShowError = false;
                    PbSync.Value = 100;
                    TxtProgressPct.Text = "100%";
                    TxtStatus.Text = $"Descarga exitosa al 100% ({successCount} tablas procesadas sin errores).";

                    AppendLog($"[{DateTime.Now:HH:mm:ss}] DESCARGA FINALIZADA CON ÉXITO AL 100% ({successCount} tablas).");
                    
                    _failedTablesList.Clear();
                    BtnRetryFailed.Visibility = Visibility.Collapsed;

                    if (updateWatermark)
                    {
                        var dlState = SyncStateManager.LoadState(isUpload: false);
                        TxtLastDate.Text = dlState.LastGlobalSuccess.ToString("yyyy-MM-dd HH:mm:ss");
                        AppendLog($"[{DateTime.Now:HH:mm:ss}] Marca de agua de descarga actualizada a {dlState.LastGlobalSuccess:yyyy-MM-dd HH:mm:ss}.");
                    }
                    else
                    {
                        AppendLog($"[{DateTime.Now:HH:mm:ss}] NOTA: La marca de agua se mantuvo intacta por solicitud del usuario.");
                    }
                }
                else
                {
                    PbSync.ShowError = true;
                    TxtStatus.Text = $"Descarga con errores en {currentFailedTables.Count} tabla(s): {string.Join(", ", currentFailedTables)}. (Exitosas: {successCount})";

                    AppendLog($"[{DateTime.Now:HH:mm:ss}] DESCARGA FINALIZADA CON ERRORES: {currentFailedTables.Count} tablas fallaron ({string.Join(", ", currentFailedTables)}).");
                    AppendLog($"[{DateTime.Now:HH:mm:ss}] Tablas exitosas: {successCount}.");

                    _failedTablesList.Clear();
                    _failedTablesList.AddRange(currentFailedTables);
                    BtnRetryFailed.Visibility = Visibility.Visible;
                    TxtRetryFailed.Text = $"Reintentar solo tablas fallidas ({_failedTablesList.Count})";
                }
            }
            catch (OperationCanceledException)
            {
                PbSync.ShowError = true;
                AppendLog($"[{DateTime.Now:HH:mm:ss}] Operación cancelada por el usuario.");
                TxtStatus.Text = "Cancelado por el usuario.";
            }
            catch (Exception ex)
            {
                PbSync.ShowError = true;
                AppendLog($"[{DateTime.Now:HH:mm:ss}] ERROR INESPERADO: {ex.Message}");
                TxtStatus.Text = "Error crítico durante la sincronización.";
            }
            finally
            {
                BtnStart.IsEnabled = true;
                BtnCancel.IsEnabled = false;
                BtnRetryFailed.IsEnabled = true;
                var savedPath = TraceLogger.WriteTraceLog(LogComponents.UiDescarga, _logBuilder.ToString());
                if (!string.IsNullOrEmpty(savedPath))
                {
                    AppendLog($"[{DateTime.Now:HH:mm:ss}] Archivo de log generado en: {savedPath}");
                }
                _cts?.Dispose();
                _cts = null;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            _cts?.Cancel();
            BtnCancel.IsEnabled = false;
            TxtStatus.Text = "Cancelando operación...";
        }

        private void BtnCopyLogs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var text = TxtLogs.Text;
                if (string.IsNullOrEmpty(text)) return;

                var package = new Windows.ApplicationModel.DataTransfer.DataPackage();
                package.RequestedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
                package.SetText(text);
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(package);

                TxtCopyBtnLabel.Text = "¡Copiado!";
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
                timer.Tick += (s, args) =>
                {
                    TxtCopyBtnLabel.Text = "Copiar Log";
                    timer.Stop();
                };
                timer.Start();
            }
            catch
            {
                // Fallback silencioso si el portapapeles del SO está bloqueado
            }
        }

        private void BtnClearLogs_Click(object sender, RoutedEventArgs e)
        {
            _logBuilder.Clear();
            TxtLogs.Text = string.Empty;
        }

        private void AppendLog(string message)
        {
            _logBuilder.AppendLine(message);

            var hadSelection = TxtLogs.SelectionLength > 0;
            var selStart = TxtLogs.SelectionStart;
            var selLen = TxtLogs.SelectionLength;

            TxtLogs.Text = _logBuilder.ToString();

            if (hadSelection)
            {
                TxtLogs.Select(selStart, selLen);
            }
            else
            {
                TxtLogs.SelectionStart = TxtLogs.Text.Length;
            }
        }
    }
}
