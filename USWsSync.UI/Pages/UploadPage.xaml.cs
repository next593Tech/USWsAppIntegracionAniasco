using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using USWsSync.Core.Configuration;
using USWsSync.Core.Engine;

namespace USWsSync_UI.Pages
{
    public sealed partial class UploadPage : Page
    {
        private readonly ISyncEngine _syncEngine;
        private CancellationTokenSource? _cts;
        private readonly StringBuilder _logBuilder = new();

        public UploadPage()
        {
            InitializeComponent();
            _syncEngine = App.Services.GetRequiredService<ISyncEngine>();
            Loaded += UploadPage_Loaded;
        }

        private void UploadPage_Loaded(object sender, RoutedEventArgs e)
        {
            var config = ConfigManager.LoadConfig();
            DpFechaInicio.Date = DateTimeOffset.Now.Date;
            DpFechaFinal.Date = DateTimeOffset.Now.Date;
            TxtLastDate.Text = config.LastDateUpdate.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            var f1Offset = DpFechaInicio.Date ?? DateTimeOffset.Now.Date;
            var f2Offset = DpFechaFinal.Date ?? DateTimeOffset.Now.Date;

            var f1 = new DateTime(f1Offset.Year, f1Offset.Month, f1Offset.Day, 0, 0, 0);
            var f2 = new DateTime(f2Offset.Year, f2Offset.Month, f2Offset.Day, 23, 59, 59);

            var config = ConfigManager.LoadConfig();

            BtnStart.IsEnabled = false;
            BtnCancel.IsEnabled = true;
            PbSync.Value = 0;
            PbSync.ShowError = false;
            TxtProgressPct.Text = "0%";
            TxtStatus.Text = "Verificando conectividad...";

            _logBuilder.Clear();
            TxtLogs.Text = "";

            _cts = new CancellationTokenSource();
            var ct = _cts.Token;

            AppendLog($"[{DateTime.Now:HH:mm:ss}] Iniciando subida de datos desde {config.IpLocal} hacia {config.IpPublica}");
            AppendLog($"[{DateTime.Now:HH:mm:ss}] Rango seleccionado: {f1:yyyy-MM-dd HH:mm:ss} - {f2:yyyy-MM-dd HH:mm:ss}");

            var errorCount = 0;
            var successCount = 0;
            var failedTables = new List<string>();

            try
            {
                var connOk = await _syncEngine.CheckConnectionAsync(config.IpLocal, ct);
                if (!connOk)
                {
                    PbSync.ShowError = true;
                    AppendLog($"[{DateTime.Now:HH:mm:ss}] ERROR: No hay conexión con el servidor local ERP ({config.IpLocal}). Verifique red o servicio.");
                    TxtStatus.Text = "Fallo de conexión local.";
                    BtnStart.IsEnabled = true;
                    BtnCancel.IsEnabled = false;
                    SaveSessionLog("Subida", _logBuilder.ToString());
                    return;
                }

                var pubOk = await _syncEngine.CheckConnectionAsync(config.IpPublica, ct);
                if (!pubOk)
                {
                    PbSync.ShowError = true;
                    AppendLog($"[{DateTime.Now:HH:mm:ss}] ERROR: No hay conexión con el servidor en la nube ({config.IpPublica}). Verifique red.");
                    TxtStatus.Text = "Fallo de conexión pública.";
                    BtnStart.IsEnabled = true;
                    BtnCancel.IsEnabled = false;
                    SaveSessionLog("Subida", _logBuilder.ToString());
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
                            if (!failedTables.Contains(info.TableName))
                            {
                                failedTables.Add(info.TableName);
                                errorCount++;
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

                var success = await Task.Run(() => _syncEngine.ExecuteUploadBatchAsync(
                    config.GetLocalApiBase(),
                    config.GetPublicApiBase(),
                    f1,
                    f2,
                    progress,
                    ct
                ), ct);

                if (success && errorCount == 0)
                {
                    PbSync.ShowError = false;
                    PbSync.Value = 100;
                    TxtProgressPct.Text = "100%";
                    TxtStatus.Text = $"Subida exitosa al 100% ({successCount} tablas procesadas sin errores).";

                    AppendLog($"[{DateTime.Now:HH:mm:ss}] SUBIDA FINALIZADA CON ÉXITO AL 100% ({successCount} tablas).");
                    AppendLog($"[{DateTime.Now:HH:mm:ss}] NOTA: La fecha de corte se mantiene intacta para la tarea programada.");
                }
                else
                {
                    PbSync.ShowError = true;
                    TxtStatus.Text = $"Subida con errores en {failedTables.Count} tabla(s): {string.Join(", ", failedTables)}. (Exitosas: {successCount})";

                    AppendLog($"[{DateTime.Now:HH:mm:ss}] SUBIDA FINALIZADA CON ERRORES: {failedTables.Count} tablas fallaron ({string.Join(", ", failedTables)}).");
                    AppendLog($"[{DateTime.Now:HH:mm:ss}] Tablas exitosas: {successCount}.");
                    AppendLog($"[{DateTime.Now:HH:mm:ss}] REGLA DE ORO: La fecha de corte NO se actualiza.");
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
                SaveSessionLog("Subida", _logBuilder.ToString());
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

        private void AppendLog(string message)
        {
            _logBuilder.AppendLine(message);
            TxtLogs.Text = _logBuilder.ToString();
            LogScrollViewer.ChangeView(null, LogScrollViewer.ScrollableHeight, null);
        }

        private void SaveSessionLog(string operation, string fullLog)
        {
            try
            {
                var logsDir = @"C:\logs";
                if (!Directory.Exists(logsDir))
                {
                    Directory.CreateDirectory(logsDir);
                }
                var now = DateTime.Now;
                var fileName = $"Sync_Manual_{operation}_{now:yyyyMMdd_HHmmss}.log";
                var generalFile = $"Sync_General_{now:yyyyMMdd}.log";

                File.WriteAllText(Path.Combine(logsDir, fileName), fullLog, Encoding.UTF8);

                var separator = $"\n========================================================================\n=== REGISTRO MANUAL {operation.ToUpper()} - {now:yyyy-MM-dd HH:mm:ss} ===\n========================================================================\n";
                File.AppendAllText(Path.Combine(logsDir, generalFile), separator + fullLog + "\n", Encoding.UTF8);
            }
            catch { }
        }
    }
}
