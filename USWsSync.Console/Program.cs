using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using USWsSync.Core.Configuration;
using USWsSync.Core.Engine;

namespace USWsSync.Console
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            // Asegurar directorio de logs
            var logsDir = @"C:\logs";
            if (!Directory.Exists(logsDir))
            {
                try { Directory.CreateDirectory(logsDir); } catch { }
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(Path.Combine(logsDir, "Sync_Console_.log"), rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            try
            {
                Log.Information("========================================================================");
                Log.Information("=== Bizor Sync Console - Tarea Programada de Integración Dobra ERP ===");
                Log.Information("========================================================================");

                // Configurar Inyección de Dependencias
                var services = new ServiceCollection();
                services.AddLogging(builder => builder.AddSerilog(dispose: true));
                services.AddHttpClient("SyncEngineClient", client =>
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                });
                services.AddSingleton<ISyncEngine, SyncEngine>();

                using var provider = services.BuildServiceProvider();
                var engine = provider.GetRequiredService<ISyncEngine>();

                // Cargar configuración compartida desde appsettings.json local
                var config = ConfigManager.LoadConfig();
                Log.Information("Configuración cargada desde: {Path}", ConfigManager.GetConfigFilePath());
                Log.Information("IP Local: {IpLocal} | IP Pública: {IpPublica}", config.IpLocal, config.IpPublica);
                Log.Information("Última Fecha de Corte registrada: {LastDate:yyyy-MM-dd HH:mm:ss}", config.LastDateUpdate);

                // 1. Verificación de Conectividad
                Log.Information("Verificando conectividad con ambos extremos...");
                var localOk = await engine.CheckConnectionAsync(config.IpLocal);
                var publicOk = await engine.CheckConnectionAsync(config.IpPublica);

                if (!localOk || !publicOk)
                {
                    Log.Error("ABORTANDO: Conexión fallida. Local (ERP): {LocalOk} | Pública (Nube): {PublicOk}",
                        localOk ? "OK" : "FALLÓ", publicOk ? "OK" : "FALLÓ");
                    return 1;
                }
                Log.Information("Conexión exitosa con ambos extremos.");

                // 2. Establecer fechas del proceso
                var fechaCorte = DateTime.Now;
                var fechaInicio = config.LastDateUpdate;

                Log.Information("Iniciando ciclo de sincronización.");
                Log.Information("Ventana de sincronización: Desde {Desde:yyyy-MM-dd HH:mm:ss} hasta {Corte:yyyy-MM-dd HH:mm:ss} (Corte Actual)",
                    fechaInicio, fechaCorte);

                var progressReporter = new Progress<SyncProgressInfo>(info =>
                {
                    if (!info.IsSuccess)
                    {
                        Log.Warning("[{Table}] {Msg}", info.TableName, info.Message);
                    }
                    else if (info.Message.StartsWith("[OK"))
                    {
                        Log.Information("{Msg}", info.Message);
                    }
                });

                // 3. Fase 1: Descarga (Nube -> Local)
                Log.Information(">>> FASE 1: DESCARGA DE NOVEDADES (Nube -> Local)");
                var downloadOk = await engine.ExecuteDownloadBatchAsync(
                    config.GetPublicApiBase(),
                    config.GetLocalApiBase(),
                    fechaInicio,
                    fechaCorte,
                    progressReporter);

                // 4. Fase 2: Subida (Local -> Nube)
                Log.Information(">>> FASE 2: SUBIDA DE NOVEDADES (Local -> Nube)");
                var uploadOk = await engine.ExecuteUploadBatchAsync(
                    config.GetLocalApiBase(),
                    config.GetPublicApiBase(),
                    fechaInicio,
                    fechaCorte,
                    progressReporter);

                // 5. Evaluación de Regla de Oro
                if (downloadOk && uploadOk)
                {
                    config.LastDateUpdate = fechaCorte;
                    ConfigManager.SaveConfig(config);

                    Log.Information("========================================================================");
                    Log.Information("SINCRONIZACIÓN 100% EXITOSA. 0 errores detectados.");
                    Log.Information("FECHA DE CORTE AVANZADA A: {Corte:yyyy-MM-dd HH:mm:ss}", fechaCorte);
                    Log.Information("========================================================================");
                    return 0;
                }
                else
                {
                    Log.Error("========================================================================");
                    Log.Error("SINCRONIZACIÓN FINALIZÓ CON ERRORES.");
                    Log.Error("Estado Descarga: {DownOk} | Estado Subida: {UpOk}",
                        downloadOk ? "OK" : "FALLARON ALGUNAS TABLAS",
                        uploadOk ? "OK" : "FALLARON ALGUNAS TABLAS");
                    Log.Error("REGLA DE ORO APLICADA: La fecha de corte NO se actualiza.");
                    Log.Error("La próxima sincronización reintentará desde: {LastDate:yyyy-MM-dd HH:mm:ss}", config.LastDateUpdate);
                    Log.Error("========================================================================");
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Excepción fatal durante la ejecución de la tarea programada.");
                return 1;
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }
    }
}
