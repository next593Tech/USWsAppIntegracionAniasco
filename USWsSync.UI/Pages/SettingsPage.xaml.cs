using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using USWsSync.Core.Configuration;
using USWsSync.Core.Engine;

namespace USWsSync_UI.Pages
{
    public sealed partial class SettingsPage : Page
    {
        private readonly ISyncEngine _syncEngine;

        private bool _isInitialized;

        public SettingsPage()
        {
            InitializeComponent();
            NavigationCacheMode = Microsoft.UI.Xaml.Navigation.NavigationCacheMode.Required;
            _syncEngine = App.Services.GetRequiredService<ISyncEngine>();
            Loaded += SettingsPage_Loaded;
        }

        private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized)
            {
                LoadSettings();
                _isInitialized = true;
            }
        }

        private void LoadSettings()
        {
            var config = ConfigManager.LoadConfig();
            TxtIpLocal.Text = config.IpLocal;
            TxtIpPublica.Text = config.IpPublica;
            TxtLastDateUpdate.Text = config.LastDateUpdate.ToString("yyyy-MM-ddTHH:mm:ss");
        }

        private async void BtnTestConnection_Click(object sender, RoutedEventArgs e)
        {
            BtnTestConnection.IsEnabled = false;
            SettingsInfoBar.IsOpen = true;
            SettingsInfoBar.Severity = InfoBarSeverity.Informational;
            SettingsInfoBar.Title = "Probando conexión...";
            SettingsInfoBar.Message = "Enviando solicitud de comprobación a los servidores...";

            try
            {
                var localTarget = TxtIpLocal.Text.Trim();
                var publicTarget = TxtIpPublica.Text.Trim();

                var localOk = await _syncEngine.CheckConnectionAsync(localTarget);
                var publicOk = await _syncEngine.CheckConnectionAsync(publicTarget);

                if (localOk && publicOk)
                {
                    SettingsInfoBar.Severity = InfoBarSeverity.Success;
                    SettingsInfoBar.Title = "Conexión Exitosa";
                    SettingsInfoBar.Message = "Se estableció comunicación correcta con el Servidor Local y la Nube.";
                }
                else
                {
                    SettingsInfoBar.Severity = InfoBarSeverity.Warning;
                    SettingsInfoBar.Title = "Problemas de Conexión";
                    SettingsInfoBar.Message = $"Local (ERP): {(localOk ? "OK" : "NO RESPONDE")} | Nube: {(publicOk ? "OK" : "NO RESPONDE")}";
                }
            }
            catch (Exception ex)
            {
                SettingsInfoBar.Severity = InfoBarSeverity.Error;
                SettingsInfoBar.Title = "Error";
                SettingsInfoBar.Message = $"Fallo al probar conexiones: {ex.Message}";
            }
            finally
            {
                BtnTestConnection.IsEnabled = true;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var config = ConfigManager.LoadConfig();

                config.IpLocal = TxtIpLocal.Text.Trim();
                config.IpPublica = TxtIpPublica.Text.Trim();

                if (DateTime.TryParse(TxtLastDateUpdate.Text.Trim(), out var parsedDate))
                {
                    config.LastDateUpdate = parsedDate;
                }

                ConfigManager.SaveConfig(config);

                SettingsInfoBar.IsOpen = true;
                SettingsInfoBar.Severity = InfoBarSeverity.Success;
                SettingsInfoBar.Title = "Guardado Exitoso";
                SettingsInfoBar.Message = $"Configuración persistida correctamente en {ConfigManager.GetConfigFilePath()}";
            }
            catch (Exception ex)
            {
                SettingsInfoBar.IsOpen = true;
                SettingsInfoBar.Severity = InfoBarSeverity.Error;
                SettingsInfoBar.Title = "Error al Guardar";
                SettingsInfoBar.Message = ex.Message;
            }
        }
    }
}
