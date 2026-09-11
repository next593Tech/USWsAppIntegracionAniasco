using System;
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
    public sealed partial class DownloadPage : Page
    {
        private readonly ISyncEngine _syncEngine;
        private CancellationTokenSource? _cts;
        private readonly StringBuilder _logBuilder = new();

        public DownloadPage()
        {
            InitializeComponent();
            _syncEngine = App.Services.GetRequiredService<ISyncEngine>();
            Loaded += DownloadPage_Loaded;
        }

        private void DownloadPage_Loaded(object sender, RoutedEventArgs e)
        {
            var config = ConfigManager.LoadConfig();
            DpFechaInicio.Date = DateTimeOffset.Now.Date;
            DpFechaFinal.Date = DateTimeOffset.Now.Date;
            TxtLastDate.Text = config.LastDateDownload.ToString("yyyy-MM-dd HH:mm:ss");
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
            TxtProgressPct.Text = "0%";
            TxtCurrentTable.Text = "Iniciando verificación de conexión...";
            _logBuilder.Clear();
            TxtLogs.Text = "";

            _cts = new CancellationTokenSource();
            var ct = _cts.Token;

            AppendLog($"[{DateTime.Now:HH:mm:ss}] Iniciando descarga de datos desde {config.IpPublica} hacia {config.IpLocal}");
            AppendLog($"[{DateTime.Now:HH:mm:ss}] Rango seleccionado: {f1:yyyy-MM-dd HH:mm:ss} - {f2:yyyy-MM-dd HH:mm:ss}");

            try
            {
                var connOk = await _syncEngine.CheckConnectionAsync(config.IpPublica, ct);
                if (!connOk)
                {
                    AppendLog($"[{DateTime.Now:HH:mm:ss}] ERROR: No hay conexión con el servidor público ({config.IpPublica}). Verifique red.");
                    TxtStatus.Text = "Fallo de conexión.";
                    BtnStart.IsEnabled = true;
                    BtnCancel.IsEnabled = false;
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

                        AppendLog($"[{DateTime.Now:HH:mm:ss}] {info.Message}");
                    });
                });

                var success = await Task.Run(() => _syncEngine.ExecuteDownloadBatchAsync(
                    config.GetPublicApiBase(),
                    config.GetLocalApiBase(),
                    f1,
                    f2,
                    progress,
                    ct
                ), ct);

                if (success)
                {
                    config.LastDateDownload = f2;
                    ConfigManager.SaveConfig(config);
                    TxtLastDate.Text = config.LastDateDownload.ToString("yyyy-MM-dd HH:mm:ss");

                    AppendLog($"[{DateTime.Now:HH:mm:ss}] DESCARGA FINALIZADA CON ÉXITO AL 100%.");
                    TxtStatus.Text = "Proceso completado exitosamente.";
                }
                else
                {
                    AppendLog($"[{DateTime.Now:HH:mm:ss}] DESCARGA FINALIZADA CON ADVERTENCIAS/ERRORES. Revise el registro.");
                    TxtStatus.Text = "Proceso completado con errores en algunas tablas.";
                }
            }
            catch (OperationCanceledException)
            {
                AppendLog($"[{DateTime.Now:HH:mm:ss}] Operación cancelada por el usuario.");
                TxtStatus.Text = "Cancelado.";
            }
            catch (Exception ex)
            {
                AppendLog($"[{DateTime.Now:HH:mm:ss}] ERROR INESPERADO: {ex.Message}");
                TxtStatus.Text = "Error crítico durante la sincronización.";
            }
            finally
            {
                BtnStart.IsEnabled = true;
                BtnCancel.IsEnabled = false;
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
    }
}
