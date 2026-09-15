using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using USWsSync.Core.History;

namespace USWsSync_UI.Pages
{
    public class SyncRunViewModel
    {
        public long Id { get; set; }
        public string RunId { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int DurationMs { get; set; }
        public SyncDirection Direction { get; set; }
        public SyncTriggerSource TriggerSource { get; set; }
        public SyncRunStatus Status { get; set; }
        public int TotalTables { get; set; }
        public int TablesWithChanges { get; set; }
        public int TablesWithErrors { get; set; }
        public int TotalRecords { get; set; }
        public DateTime DataFromDate { get; set; }
        public DateTime DataToDate { get; set; }
        public string? ErrorSummary { get; set; }

        public string FormattedDate => StartTime > DateTime.MinValue ? StartTime.ToString("yyyy-MM-dd") : "-";
        public string FormattedTime => StartTime > DateTime.MinValue ? StartTime.ToString("HH:mm:ss") : "-";
        public string DurationText => DurationMs >= 1000 ? $"{(DurationMs / 1000.0):F1} s" : $"{DurationMs} ms";

        public string DirectionText => Direction == SyncDirection.Download ? "Descarga" : "Subida";
        public string DirectionGlyph => Direction == SyncDirection.Download ? "\uE896" : "\uE898";

        public string TriggerText => TriggerSource == SyncTriggerSource.Scheduled ? "Auto (5m)" : "Manual UI";
        public string TriggerGlyph => TriggerSource == SyncTriggerSource.Scheduled ? "\uE823" : "\uE779";

        public string StatusText { get; set; } = "Exitosa";
        public string StatusGlyph { get; set; } = "\uE73E";
        public Brush StatusForeground { get; set; } = null!;
        public Brush StatusBackground { get; set; } = null!;
        public Brush StatusBorderBrush { get; set; } = null!;

        public Brush CardBackground { get; set; } = null!;
        public Brush CardBorderBrush { get; set; } = null!;

        public string ChangesSummary => TablesWithChanges > 0
            ? $"{TablesWithChanges} tablas cambiaron · {TotalRecords:N0} reg."
            : "Sin cambios (0 reg.)";

        public string ErrorTablesText => TablesWithErrors > 0 ? $"{TablesWithErrors} con error" : "";
        public Visibility ErrorVisibility => TablesWithErrors > 0 ? Visibility.Visible : Visibility.Collapsed;

        public string DataWindowSummary
        {
            get
            {
                if (DataFromDate <= DateTime.MinValue || DataToDate <= DateTime.MinValue)
                    return "Corte: No especificado";

                var span = DataToDate - DataFromDate;
                string spanStr = FormatCompactSpan(span);
                return $"Corte: {DataFromDate:dd-MMM HH:mm} → {DataToDate:dd-MMM HH:mm} ({spanStr})";
            }
        }

        public string CoverageText
        {
            get
            {
                if (DataFromDate <= DateTime.MinValue || DataToDate <= DateTime.MinValue)
                    return "Sin corte definido";

                var span = DataToDate - DataFromDate;
                if (span.TotalDays >= 1)
                {
                    int days = (int)span.TotalDays;
                    int hours = span.Hours;
                    return $"{days} {(days == 1 ? "día" : "días")}{(hours > 0 ? $" y {hours} h" : "")} de información";
                }
                if (span.TotalHours >= 1)
                {
                    return $"{(int)span.TotalHours} h y {span.Minutes} m de información";
                }
                return $"{Math.Max(1, (int)span.TotalMinutes)} min de información";
            }
        }

        private static string FormatCompactSpan(TimeSpan span)
        {
            if (span.TotalDays >= 1)
            {
                int days = (int)span.TotalDays;
                int hours = span.Hours;
                return hours > 0 ? $"{days}d {hours}h" : $"{days}d";
            }
            if (span.TotalHours >= 1)
            {
                return $"{(int)span.TotalHours}h {span.Minutes}m";
            }
            return $"{Math.Max(1, (int)span.TotalMinutes)}m";
        }

        public SyncRunRecord OriginalRecord { get; set; } = null!;
    }

    public class SyncRunItemViewModel
    {
        public long Id { get; set; }
        public long RunId { get; set; }
        public string TableName { get; set; } = string.Empty;
        public int RecordsCount { get; set; }
        public string RecordsText => RecordsCount.ToString("N0");
        public int DurationMs { get; set; }
        public string DurationText => DurationMs >= 1000 ? $"{(DurationMs / 1000.0):F1} s" : $"{DurationMs} ms";
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public string StatusText => IsSuccess ? "Correcto" : "Falló";
        public string StatusGlyph => IsSuccess ? "\uE73E" : "\uEA39";
        public Brush StatusForeground { get; set; } = null!;
        public Brush StatusBackground { get; set; } = null!;
        public Brush StatusBorderBrush { get; set; } = null!;

        public bool HasError => !IsSuccess || !string.IsNullOrEmpty(ErrorMessage);
        public Visibility ErrorButtonVisibility => HasError ? Visibility.Visible : Visibility.Collapsed;
        public Visibility OkTextVisibility => HasError ? Visibility.Collapsed : Visibility.Visible;
    }

    public sealed partial class HistoryPage : Page
    {
        public ObservableCollection<SyncRunViewModel> Runs { get; } = new();
        public ObservableCollection<SyncRunItemViewModel> SelectedRunItems { get; } = new();

        private SyncRunViewModel? _currentSelectedRun;
        private bool _isLoaded;

        private static readonly SolidColorBrush SuccessForegroundBrush = new(Windows.UI.Color.FromArgb(255, 16, 124, 65));
        private static readonly SolidColorBrush SuccessBackgroundBrush = new(Windows.UI.Color.FromArgb(25, 16, 124, 65));
        private static readonly SolidColorBrush SuccessBorderBrush = new(Windows.UI.Color.FromArgb(80, 16, 124, 65));

        private static readonly SolidColorBrush WarningForegroundBrush = new(Windows.UI.Color.FromArgb(255, 157, 93, 0));
        private static readonly SolidColorBrush WarningBackgroundBrush = new(Windows.UI.Color.FromArgb(25, 157, 93, 0));
        private static readonly SolidColorBrush WarningBorderBrush = new(Windows.UI.Color.FromArgb(80, 157, 93, 0));

        private static readonly SolidColorBrush ErrorForegroundBrush = new(Windows.UI.Color.FromArgb(255, 196, 43, 28));
        private static readonly SolidColorBrush ErrorBackgroundBrush = new(Windows.UI.Color.FromArgb(25, 196, 43, 28));
        private static readonly SolidColorBrush ErrorBorderBrush = new(Windows.UI.Color.FromArgb(80, 196, 43, 28));

        private static readonly SolidColorBrush CardDefaultBackground = new(Windows.UI.Color.FromArgb(10, 255, 255, 255));
        private static readonly SolidColorBrush CardDefaultBorder = new(Windows.UI.Color.FromArgb(25, 128, 128, 128));
        private static readonly SolidColorBrush CardErrorBorder = new(Windows.UI.Color.FromArgb(90, 196, 43, 28));

        public HistoryPage()
        {
            InitializeComponent();
            LvRuns.ItemsSource = Runs;
            LvRunItems.ItemsSource = SelectedRunItems;

            // Rango por defecto: hoy completo
            DpFromDate.Date = DateTimeOffset.Now.Date;
            DpToDate.Date = DateTimeOffset.Now.Date;
            TpFromTime.Time = new TimeSpan(0, 0, 0);
            TpToTime.Time = new TimeSpan(23, 59, 59);

            Loaded += HistoryPage_Loaded;
        }

        private void HistoryPage_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Low, async () =>
            {
                if (!_isLoaded)
                {
                    _isLoaded = true;
                    await LoadDataAsync();
                }
            });
        }

        private async void BtnToday_Click(object sender, RoutedEventArgs e)
        {
            DpFromDate.Date = DateTimeOffset.Now.Date;
            DpToDate.Date = DateTimeOffset.Now.Date;
            ChkFilterTime.IsChecked = false;
            TxtSearchTable.Text = string.Empty;
            CmbDirection.SelectedIndex = 0;
            CmbStatus.SelectedIndex = 0;
            ChkOnlyChanges.IsChecked = true;
            await LoadDataAsync();
        }

        private async void BtnApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            await LoadDataAsync();
        }

        private async void DpDate_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            if (_isLoaded)
            {
                await LoadDataAsync();
            }
        }

        private async void TpTime_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            if (_isLoaded)
            {
                await LoadDataAsync();
            }
        }

        private async void Cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isLoaded)
            {
                await LoadDataAsync();
            }
        }

        private async void ChkOnlyChanges_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                await LoadDataAsync();
            }
        }

        private async void ChkFilterTime_CheckChanged(object sender, RoutedEventArgs e)
        {
            bool isTimeEnabled = ChkFilterTime.IsChecked == true;
            TpFromTime.IsEnabled = isTimeEnabled;
            TpToTime.IsEnabled = isTimeEnabled;

            if (_isLoaded)
            {
                await LoadDataAsync();
            }
        }

        private async void TxtSearchTable_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                await LoadDataAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            var filter = BuildCurrentFilter();

            // 1. Cargar Estadísticas / KPIs
            DateTime statsFrom = filter.FromDate ?? DateTime.Today;
            DateTime statsTo = filter.ToDate ?? DateTime.Today.AddDays(1).AddTicks(-1);
            var stats = await SyncHistoryRepository.GetStatisticsAsync(statsFrom, statsTo);

            TxtKpiTotalRuns.Text = stats.TotalRuns.ToString("N0");
            TxtKpiSuccessRuns.Text = stats.SuccessRuns.ToString("N0");
            TxtKpiErrorRuns.Text = (stats.WarningRuns + stats.ErrorRuns).ToString("N0");
            TxtKpiTotalRecords.Text = stats.TotalRecordsTransferred.ToString("N0");

            // 2. Cargar Lista de Sincronizaciones
            var runRecords = await SyncHistoryRepository.GetRunsAsync(filter);

            Runs.Clear();
            foreach (var rec in runRecords)
            {
                Runs.Add(MapToViewModel(rec));
            }

            TxtRunCount.Text = $"{Runs.Count} registros";

            if (Runs.Count == 0)
            {
                PnlNoRuns.Visibility = Visibility.Visible;
                LvRuns.Visibility = Visibility.Collapsed;
                PnlSelectRunPrompt.Visibility = Visibility.Visible;
                PnlRunDetail.Visibility = Visibility.Collapsed;
                _currentSelectedRun = null;
            }
            else
            {
                PnlNoRuns.Visibility = Visibility.Collapsed;
                LvRuns.Visibility = Visibility.Visible;

                // Si no hay selección, seleccionar la primera por conveniencia
                if (_currentSelectedRun == null || !Runs.Any(x => x.Id == _currentSelectedRun.Id))
                {
                    LvRuns.SelectedIndex = 0;
                }
            }
        }

        private SyncHistoryFilter BuildCurrentFilter()
        {
            var filter = new SyncHistoryFilter
            {
                Page = 1,
                PageSize = 100,
                OnlyWithChangesOrErrors = ChkOnlyChanges.IsChecked == true,
                TableSearch = string.IsNullOrWhiteSpace(TxtSearchTable.Text) ? null : TxtSearchTable.Text.Trim()
            };

            // Rango de fechas y horas
            DateTime fromDate = DpFromDate.Date?.Date ?? DateTime.Today;
            DateTime toDate = DpToDate.Date?.Date ?? DateTime.Today;

            if (ChkFilterTime.IsChecked == true)
            {
                filter.FromDate = fromDate.Date + TpFromTime.Time;
                filter.ToDate = toDate.Date + TpToTime.Time;
            }
            else
            {
                filter.FromDate = fromDate.Date;
                filter.ToDate = toDate.Date.AddDays(1).AddTicks(-1); // 23:59:59.999
            }

            // Dirección
            string? dirTag = (CmbDirection.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            if (dirTag == "Download") filter.Direction = SyncDirection.Download;
            else if (dirTag == "Upload") filter.Direction = SyncDirection.Upload;
            else filter.Direction = null;

            // Estado
            string? statusTag = (CmbStatus.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            if (statusTag == "Success") filter.Status = SyncRunStatus.Success;
            else if (statusTag == "Warning") filter.Status = SyncRunStatus.Warning;
            else if (statusTag == "Error") filter.Status = SyncRunStatus.Error;
            else filter.Status = null;

            return filter;
        }

        private SyncRunViewModel MapToViewModel(SyncRunRecord rec)
        {
            var vm = new SyncRunViewModel
            {
                Id = rec.Id,
                RunId = rec.RunId,
                StartTime = rec.StartTime,
                EndTime = rec.EndTime,
                DurationMs = rec.DurationMs,
                Direction = rec.Direction,
                TriggerSource = rec.TriggerSource,
                Status = rec.Status,
                TotalTables = rec.TotalTables,
                TablesWithChanges = rec.TablesWithChanges,
                TablesWithErrors = rec.TablesWithErrors,
                TotalRecords = rec.TotalRecords,
                DataFromDate = rec.DataFromDate,
                DataToDate = rec.DataToDate,
                ErrorSummary = rec.ErrorSummary,
                OriginalRecord = rec
            };

            switch (rec.Status)
            {
                case SyncRunStatus.Success:
                    vm.StatusText = "Exitosa";
                    vm.StatusGlyph = "\uE73E";
                    vm.StatusForeground = SuccessForegroundBrush;
                    vm.StatusBackground = SuccessBackgroundBrush;
                    vm.StatusBorderBrush = SuccessBorderBrush;
                    vm.CardBackground = CardDefaultBackground;
                    vm.CardBorderBrush = CardDefaultBorder;
                    break;
                case SyncRunStatus.Warning:
                    vm.StatusText = "Aviso";
                    vm.StatusGlyph = "\uE7BA";
                    vm.StatusForeground = WarningForegroundBrush;
                    vm.StatusBackground = WarningBackgroundBrush;
                    vm.StatusBorderBrush = WarningBorderBrush;
                    vm.CardBackground = CardDefaultBackground;
                    vm.CardBorderBrush = CardDefaultBorder;
                    break;
                case SyncRunStatus.Error:
                default:
                    vm.StatusText = "Error";
                    vm.StatusGlyph = "\uEA39";
                    vm.StatusForeground = ErrorForegroundBrush;
                    vm.StatusBackground = ErrorBackgroundBrush;
                    vm.StatusBorderBrush = ErrorBorderBrush;
                    vm.CardBackground = CardDefaultBackground;
                    vm.CardBorderBrush = CardErrorBorder;
                    break;
            }

            return vm;
        }

        private async void LvRuns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LvRuns.SelectedItem is SyncRunViewModel selected)
            {
                _currentSelectedRun = selected;
                await DisplayRunDetailAsync(selected);
            }
            else
            {
                _currentSelectedRun = null;
                PnlSelectRunPrompt.Visibility = Visibility.Visible;
                PnlRunDetail.Visibility = Visibility.Collapsed;
            }
        }

        private async Task DisplayRunDetailAsync(SyncRunViewModel run)
        {
            PnlSelectRunPrompt.Visibility = Visibility.Collapsed;
            PnlRunDetail.Visibility = Visibility.Visible;

            // Encabezado
            TxtDetailStatus.Text = run.StatusText.ToUpperInvariant();
            TxtDetailStatus.Foreground = run.StatusForeground;
            BdDetailStatus.Background = run.StatusBackground;
            BdDetailStatus.BorderBrush = run.StatusBorderBrush;
            BdDetailStatus.BorderThickness = new Thickness(1);

            TxtDetailRunId.Text = $"ID: #{run.Id} · GUID: {run.RunId}";

            // Tarjeta A: Proceso Técnico
            TxtDetailStartTime.Text = run.StartTime > DateTime.MinValue ? run.StartTime.ToString("yyyy-MM-dd HH:mm:ss") : "-";
            TxtDetailEndTime.Text = run.EndTime > DateTime.MinValue ? run.EndTime.ToString("yyyy-MM-dd HH:mm:ss") : "-";
            TxtDetailDuration.Text = $"{run.DurationText} ({run.DurationMs:N0} ms)";

            // Tarjeta B: Período de Información Afectada (Corte ERP)
            TxtDetailDataFrom.Text = run.DataFromDate > DateTime.MinValue ? run.DataFromDate.ToString("yyyy-MM-dd HH:mm:ss") : "Sin corte inicial";
            TxtDetailDataTo.Text = run.DataToDate > DateTime.MinValue ? run.DataToDate.ToString("yyyy-MM-dd HH:mm:ss") : "Sin corte final";
            TxtDetailCoverage.Text = run.CoverageText;

            // Panel de Error Global
            if (!string.IsNullOrEmpty(run.ErrorSummary))
            {
                BdGlobalError.Visibility = Visibility.Visible;
                TxtGlobalErrorMessage.Text = run.ErrorSummary;
            }
            else
            {
                BdGlobalError.Visibility = Visibility.Collapsed;
            }

            // Cargar ítems delta de SQLite
            var items = await SyncHistoryRepository.GetRunItemsAsync(run.Id);
            SelectedRunItems.Clear();

            foreach (var item in items)
            {
                var itemVm = new SyncRunItemViewModel
                {
                    Id = item.Id,
                    RunId = item.RunId,
                    TableName = item.TableName,
                    RecordsCount = item.RecordsCount,
                    DurationMs = item.DurationMs,
                    IsSuccess = item.IsSuccess,
                    ErrorMessage = item.ErrorMessage,
                    StatusForeground = item.IsSuccess ? SuccessForegroundBrush : ErrorForegroundBrush,
                    StatusBackground = item.IsSuccess ? SuccessBackgroundBrush : ErrorBackgroundBrush,
                    StatusBorderBrush = item.IsSuccess ? SuccessBorderBrush : ErrorBorderBrush
                };
                SelectedRunItems.Add(itemVm);
            }

            if (SelectedRunItems.Count == 0)
            {
                PnlNoItemChanges.Visibility = Visibility.Visible;
                LvRunItems.Visibility = Visibility.Collapsed;
            }
            else
            {
                PnlNoItemChanges.Visibility = Visibility.Collapsed;
                LvRunItems.Visibility = Visibility.Visible;
            }
        }

        private async void BtnViewGlobalError_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSelectedRun == null || string.IsNullOrEmpty(_currentSelectedRun.ErrorSummary)) return;

            var dialog = new ContentDialog
            {
                Title = "Diagnóstico de Incidencia Global",
                PrimaryButtonText = "Copiar Error",
                CloseButtonText = "Cerrar",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.Content.XamlRoot
            };

            var sp = new StackPanel { Spacing = 10 };
            sp.Children.Add(new TextBlock
            {
                Text = $"Sincronización: #{_currentSelectedRun.Id} ({_currentSelectedRun.RunId}) · Hora: {_currentSelectedRun.FormattedTime}",
                FontSize = 12,
                Foreground = (Brush)Application.Current.Resources["TextFillColorSecondaryBrush"]
            });

            sp.Children.Add(new TextBox
            {
                Text = _currentSelectedRun.ErrorSummary,
                IsReadOnly = true,
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                Height = 200,
                FontFamily = new FontFamily("Consolas")
            });

            dialog.Content = sp;

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    var pkg = new DataPackage();
                    pkg.SetText(_currentSelectedRun.ErrorSummary);
                    Clipboard.SetContent(pkg);
                }
                catch { }
            }
        }

        private async void BtnViewItemDetail_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is SyncRunItemViewModel item)
            {
                var dialog = new ContentDialog
                {
                    Title = $"Diagnóstico de Tabla: {item.TableName}",
                    PrimaryButtonText = "Copiar Diagnóstico",
                    CloseButtonText = "Cerrar",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = this.Content.XamlRoot
                };

                var sp = new StackPanel { Spacing = 10 };
                sp.Children.Add(new TextBlock
                {
                    Text = $"Tabla: {item.TableName} · Registros: {item.RecordsText} · Duración: {item.DurationText}",
                    FontSize = 12,
                    Foreground = (Brush)Application.Current.Resources["TextFillColorSecondaryBrush"]
                });

                sp.Children.Add(new TextBox
                {
                    Text = item.ErrorMessage ?? "No se reportó mensaje detallado.",
                    IsReadOnly = true,
                    AcceptsReturn = true,
                    TextWrapping = TextWrapping.Wrap,
                    Height = 220,
                    FontFamily = new FontFamily("Consolas")
                });

                dialog.Content = sp;

                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    try
                    {
                        var pkg = new DataPackage();
                        pkg.SetText(item.ErrorMessage ?? "");
                        Clipboard.SetContent(pkg);
                    }
                    catch { }
                }
            }
        }

        private void BtnCopyRunSummary_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSelectedRun == null) return;

            var sb = new StringBuilder();
            sb.AppendLine("=== REPORTE DE AUDITORÍA BIZOR SYNC ===");
            sb.AppendLine($"ID Sincronización:  #{_currentSelectedRun.Id}");
            sb.AppendLine($"GUID Trazabilidad:  {_currentSelectedRun.RunId}");
            sb.AppendLine($"Fecha/Hora Proceso: {_currentSelectedRun.StartTime:yyyy-MM-dd HH:mm:ss} a {_currentSelectedRun.EndTime:HH:mm:ss}");
            sb.AppendLine($"Duración Real:      {_currentSelectedRun.DurationText} ({_currentSelectedRun.DurationMs:N0} ms)");
            sb.AppendLine($"Flujo de Datos:     {_currentSelectedRun.DirectionText}");
            sb.AppendLine($"Origen:             {_currentSelectedRun.TriggerText}");
            sb.AppendLine($"Estado Operativo:   {_currentSelectedRun.StatusText}");
            sb.AppendLine();
            sb.AppendLine("--- VENTANA DE DATOS (CORTE DOBRA ERP) ---");
            sb.AppendLine($"Corte Anterior (Desde): {_currentSelectedRun.DataFromDate:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Corte Aplicado (Hasta): {_currentSelectedRun.DataToDate:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Cobertura Contable:     {_currentSelectedRun.CoverageText}");
            sb.AppendLine();
            sb.AppendLine("--- RESUMEN DE REGISTROS ---");
            sb.AppendLine($"Total Tablas Evaluadas: {_currentSelectedRun.TotalTables}");
            sb.AppendLine($"Tablas con Movimiento:  {_currentSelectedRun.TablesWithChanges}");
            sb.AppendLine($"Tablas con Incidencia:  {_currentSelectedRun.TablesWithErrors}");
            sb.AppendLine($"Registros Transferidos: {_currentSelectedRun.TotalRecords:N0}");

            if (!string.IsNullOrEmpty(_currentSelectedRun.ErrorSummary))
            {
                sb.AppendLine();
                sb.AppendLine("--- INCIDENCIA GLOBAL ---");
                sb.AppendLine(_currentSelectedRun.ErrorSummary);
            }

            sb.AppendLine();
            sb.AppendLine("--- TABLAS AFECTADAS (DELTAS) ---");
            if (SelectedRunItems.Count == 0)
            {
                sb.AppendLine("Sincronización limpia: Sin novedades ni deltas transferidos.");
            }
            else
            {
                foreach (var itm in SelectedRunItems)
                {
                    sb.AppendLine($"[{itm.StatusText.ToUpper()}] {itm.TableName} | Registros: {itm.RecordsCount} | Duración: {itm.DurationText}");
                    if (!itm.IsSuccess && !string.IsNullOrEmpty(itm.ErrorMessage))
                    {
                        sb.AppendLine($"   Error: {itm.ErrorMessage}");
                    }
                }
            }

            try
            {
                var pkg = new DataPackage();
                pkg.SetText(sb.ToString());
                Clipboard.SetContent(pkg);
            }
            catch { }
        }
    }
}