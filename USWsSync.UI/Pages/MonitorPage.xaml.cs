using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.ApplicationModel.DataTransfer;
using USWsSync.Core.Registry;
using USWsSync.Core.State;

namespace USWsSync_UI.Pages
{
    public class TableMonitorItem
    {
        public string TableName { get; set; } = "";
        public string ModuleName { get; set; } = "";
        public SyncModule Module { get; set; }
        public DateTime LastSuccessDate { get; set; }
        public string LastSuccessText { get; set; } = "-";
        public DateTime LastAttemptDate { get; set; }
        public string LastAttemptText { get; set; } = "-";
        public int RecordsCount { get; set; }
        public string RecordsCountText { get; set; } = "0";
        public string StatusKey { get; set; } = "Pending";
        public string StatusText { get; set; } = "Pendiente";
        public string StatusGlyph { get; set; } = "\uE823";
        public Brush StatusForeground { get; set; } = null!;
        public Brush StatusBackground { get; set; } = null!;
        public Brush StatusBorderBrush { get; set; } = null!;
        public bool HasError { get; set; }
        public string? LastError { get; set; }
        public Visibility ErrorButtonVisibility => HasError ? Visibility.Visible : Visibility.Collapsed;
        public Visibility OkTextVisibility => HasError ? Visibility.Collapsed : Visibility.Visible;
    }

    public sealed partial class MonitorPage : Page
    {
        private readonly List<TableMonitorItem> _allItems = new();
        private bool _isInitialized;

        private static readonly SolidColorBrush SuccessForegroundBrush = new(Windows.UI.Color.FromArgb(255, 16, 124, 65));
        private static readonly SolidColorBrush SuccessBackgroundBrush = new(Windows.UI.Color.FromArgb(25, 16, 124, 65));
        private static readonly SolidColorBrush SuccessBorderBrush = new(Windows.UI.Color.FromArgb(80, 16, 124, 65));

        private static readonly SolidColorBrush ErrorForegroundBrush = new(Windows.UI.Color.FromArgb(255, 196, 43, 28));
        private static readonly SolidColorBrush ErrorBackgroundBrush = new(Windows.UI.Color.FromArgb(25, 196, 43, 28));
        private static readonly SolidColorBrush ErrorBorderBrush = new(Windows.UI.Color.FromArgb(80, 196, 43, 28));

        private static readonly SolidColorBrush PendingForegroundBrush = new(Windows.UI.Color.FromArgb(255, 115, 115, 115));
        private static readonly SolidColorBrush PendingBackgroundBrush = new(Windows.UI.Color.FromArgb(20, 128, 128, 128));
        private static readonly SolidColorBrush PendingBorderBrush = new(Windows.UI.Color.FromArgb(60, 128, 128, 128));

        public MonitorPage()
        {
            InitializeComponent();
            Loaded += MonitorPage_Loaded;
        }

        private void MonitorPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                LoadData();
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void RbDirection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isInitialized)
            {
                LoadData();
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void CmbFilterModule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void CmbFilterStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void LoadData()
        {
            bool isUpload = RbDirection.SelectedIndex == 1;
            var state = SyncStateManager.LoadState(isUpload);
            var tableDefs = isUpload ? TableRegistry.UploadTables : TableRegistry.DownloadTables;

            _allItems.Clear();

            foreach (var def in tableDefs)
            {
                var item = new TableMonitorItem
                {
                    TableName = def.TableName,
                    ModuleName = def.Module.ToString(),
                    Module = def.Module
                };

                if (state.Tables.TryGetValue(def.TableName, out var tableState))
                {
                    item.LastSuccessDate = tableState.LastSuccessDate;
                    item.LastSuccessText = tableState.LastSuccessDate > DateTime.MinValue
                        ? tableState.LastSuccessDate.ToString("yyyy-MM-dd HH:mm:ss")
                        : "Sin corte";

                    item.LastAttemptDate = tableState.LastAttemptDate;
                    item.LastAttemptText = tableState.LastAttemptDate > DateTime.MinValue
                        ? tableState.LastAttemptDate.ToString("yyyy-MM-dd HH:mm:ss")
                        : "Sin intento";

                    item.RecordsCount = tableState.RecordsCount;
                    item.RecordsCountText = tableState.RecordsCount.ToString("N0");

                    if (tableState.IsSuccess)
                    {
                        item.StatusKey = "Success";
                        item.StatusText = "Correcto";
                        item.StatusGlyph = "\uE73E"; // CheckMark
                        item.StatusForeground = SuccessForegroundBrush;
                        item.StatusBackground = SuccessBackgroundBrush;
                        item.StatusBorderBrush = SuccessBorderBrush;
                        item.HasError = false;
                    }
                    else if (!string.IsNullOrEmpty(tableState.LastError))
                    {
                        item.StatusKey = "Error";
                        item.StatusText = "Con error";
                        item.StatusGlyph = "\uEA39"; // ErrorBadge
                        item.StatusForeground = ErrorForegroundBrush;
                        item.StatusBackground = ErrorBackgroundBrush;
                        item.StatusBorderBrush = ErrorBorderBrush;
                        item.HasError = true;
                        item.LastError = tableState.LastError;
                    }
                    else
                    {
                        item.StatusKey = "Pending";
                        item.StatusText = "Pendiente";
                        item.StatusGlyph = "\uE823"; // Clock
                        item.StatusForeground = PendingForegroundBrush;
                        item.StatusBackground = PendingBackgroundBrush;
                        item.StatusBorderBrush = PendingBorderBrush;
                        item.HasError = false;
                    }
                }
                else
                {
                    item.LastSuccessText = "Sin corte";
                    item.LastAttemptText = "Sin intento";
                    item.RecordsCount = 0;
                    item.RecordsCountText = "0";
                    item.StatusKey = "Pending";
                    item.StatusText = "Pendiente";
                    item.StatusGlyph = "\uE823";
                    item.StatusForeground = PendingForegroundBrush;
                    item.StatusBackground = PendingBackgroundBrush;
                    item.StatusBorderBrush = PendingBorderBrush;
                    item.HasError = false;
                }

                _allItems.Add(item);
            }

            // KPIs
            int total = _allItems.Count;
            int successCount = _allItems.Count(x => x.StatusKey == "Success");
            int errorCount = _allItems.Count(x => x.StatusKey == "Error");
            int pendingCount = _allItems.Count(x => x.StatusKey == "Pending");
            long totalRecords = _allItems.Sum(x => (long)x.RecordsCount);

            TxtTotalTables.Text = total.ToString();
            TxtTotalTablesSub.Text = isUpload ? "72 tablas en catálogo Dobra" : "32 tablas en catálogo Dobra";

            TxtHealthStatus.Text = $"{successCount} OK · {errorCount} Error";
            TxtHealthStatusSub.Text = $"{pendingCount} pendientes de ejecución";

            TxtLastRun.Text = state.LastGlobalSuccess > DateTime.MinValue
                ? state.LastGlobalSuccess.ToString("yyyy-MM-dd HH:mm")
                : "Sin registro global";
            TxtLastRunSub.Text = isUpload ? "Corte global subida" : "Corte global descarga";

            TxtTotalRecords.Text = totalRecords.ToString("N0");
            TxtTotalRecordsSub.Text = "Filas en última ejecución";

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (!_isInitialized) return;

            string search = TxtSearch.Text?.Trim().ToUpperInvariant() ?? "";
            string moduleTag = (CmbFilterModule.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "Todos";
            string statusTag = (CmbFilterStatus.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "Todos";

            var filtered = _allItems.AsEnumerable();

            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(x => x.TableName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                               x.ModuleName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.Equals(moduleTag, "Todos", StringComparison.OrdinalIgnoreCase))
            {
                filtered = filtered.Where(x => string.Equals(x.ModuleName, moduleTag, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.Equals(statusTag, "Todos", StringComparison.OrdinalIgnoreCase))
            {
                filtered = filtered.Where(x => string.Equals(x.StatusKey, statusTag, StringComparison.OrdinalIgnoreCase));
            }

            LvTables.ItemsSource = filtered.ToList();
        }

        private async void BtnViewDetail_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is TableMonitorItem item)
            {
                var dialog = new ContentDialog
                {
                    Title = $"Diagnóstico de Incidencia: {item.TableName}",
                    PrimaryButtonText = "Copiar Diagnóstico",
                    CloseButtonText = "Cerrar",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = this.Content.XamlRoot
                };

                var contentPanel = new StackPanel { Spacing = 12 };

                contentPanel.Children.Add(new TextBlock
                {
                    Text = $"Módulo: {item.ModuleName}  ·  Último intento: {item.LastAttemptText}",
                    Foreground = (Brush)Application.Current.Resources["TextFillColorSecondaryBrush"],
                    FontSize = 12
                });

                var tbError = new TextBox
                {
                    Text = item.LastError ?? "No se reportó mensaje detallado de error.",
                    IsReadOnly = true,
                    AcceptsReturn = true,
                    TextWrapping = TextWrapping.Wrap,
                    Height = 220,
                    FontFamily = new FontFamily("Consolas")
                };
                contentPanel.Children.Add(tbError);

                dialog.Content = contentPanel;

                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    try
                    {
                        var dataPackage = new DataPackage();
                        dataPackage.SetText(item.LastError ?? "");
                        Clipboard.SetContent(dataPackage);
                    }
                    catch
                    {
                        // Fallback silencioso si el portapapeles del sistema está bloqueado
                    }
                }
            }
        }
    }
}
