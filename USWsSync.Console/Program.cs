using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using USWsSync.Core.Configuration;
using USWsSync.Core.Engine;
using USWsSync.Core.Logging;
using USWsSync.Core.History;

namespace USWsSync.Console
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var now = DateTime.Now;
            var consoleLogFile = LogPathHelper.GetLogFilePath(LogComponents.Consola, now);
            var consoleLogDir = Path.GetDirectoryName(consoleLogFile);
            if (!string.IsNullOrEmpty(consoleLogDir) && !Directory.Exists(consoleLogDir))
            {
                try { Directory.CreateDirectory(consoleLogDir); } catch { }
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(consoleLogFile,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            try
            {
                Log.Information("========================================================================");
                Log.Information("=== Bizor Sync Console - Tarea Programada de Integración Dobra ERP ===");
                Log.Information("========================================================================");

                if (args.Length > 0 && args[0].Equals("--test-history", StringComparison.OrdinalIgnoreCase))
                {
                    Log.Information(">>> EJECUTANDO AUTO-DIAGNÓSTICO DE AUDITORÍA SQLITE (--test-history)...");
                    await SyncHistoryRepository.InitializeDatabaseAsync();
                    Log.Information("Base de datos SQLite inicializada en: {DbPath}", SyncHistoryRepository.DbPath);

                    var testRun = new SyncRunRecord
                    {
                        RunId = Guid.NewGuid().ToString("N"),
                        StartTime = DateTime.Now.AddMinutes(-5),
                        EndTime = DateTime.Now,
                        DurationMs = 300000,
                        Direction = SyncDirection.Download,
                        TriggerSource = SyncTriggerSource.Scheduled,
                        Status = SyncRunStatus.Warning,
                        TotalTables = 90,
                        TablesWithChanges = 2,
                        TablesWithErrors = 1,
                        TotalRecords = 125,
                        DataFromDate = DateTime.Now.AddDays(-2),
                        DataToDate = DateTime.Now.AddMinutes(-5),
                        ErrorSummary = "Error simulado para verificación de diagnósticos."
                    };

                    var testItems = new List<SyncRunItemRecord>
                    {
                        new() { TableName = "CXC_CLIENTES", RecordsCount = 125, IsSuccess = true, DurationMs = 850 },
                        new() { TableName = "VEN_FACTURAS", RecordsCount = 0, IsSuccess = false, ErrorMessage = "Timeout simulado al consultar endpoint de facturación.", DurationMs = 5000 }
                    };

                    var savedId = await SyncHistoryRepository.SaveRunAsync(testRun, testItems);
                    Log.Information("Sincronización de prueba guardada exitosamente con ID SQLite: {Id}", savedId);

                    var runs = await SyncHistoryRepository.GetRunsAsync(new SyncHistoryFilter { PageSize = 5 });
                    Log.Information("Sincronizaciones recuperadas: {Count}", runs.Count);
                    if (runs.Count > 0)
                    {
                        var first = runs[0];
                        Log.Information("  -> Ventana de Datos: Desde {From:yyyy-MM-dd HH:mm} hasta {To:yyyy-MM-dd HH:mm}", first.DataFromDate, first.DataToDate);
                    }

                    var items = await SyncHistoryRepository.GetRunItemsAsync(savedId);
                    Log.Information("Ítems delta recuperados para la sincronización {Id}: {Count}", savedId, items.Count);
                    foreach (var itm in items)
                    {
                        Log.Information("  -> Tabla: {Table} | Regs: {Regs} | OK: {Ok} | Error: {Err}",
                            itm.TableName, itm.RecordsCount, itm.IsSuccess, itm.ErrorMessage ?? "Ninguno");
                    }

                    var stats = await SyncHistoryRepository.GetStatisticsAsync(DateTime.Today.AddDays(-7), DateTime.Today.AddDays(1));
                    Log.Information("Estadísticas del periodo: Total={Total}, OK={Ok}, Avisos={Warn}, Errores={Err}, Regs={Regs}",
                        stats.TotalRuns, stats.SuccessRuns, stats.WarningRuns, stats.ErrorRuns, stats.TotalRecordsTransferred);

                    var pruned = await SyncHistoryRepository.PruneOldRecordsAsync(60);
                    Log.Information("Purga de retención (60 días): {Pruned} eliminados.", pruned);

                    Log.Information("AUTO-DIAGNÓSTICO FINALIZADO CON ÉXITO 100%.");
                    return 0;
                }

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

                // Inicializar base de datos de auditoría SQLite
                await SyncHistoryRepository.InitializeDatabaseAsync();

                // 3. Fase 1: Descarga (Nube -> Local)
                Log.Information(">>> FASE 1: DESCARGA DE NOVEDADES (Nube -> Local) con Estado Granular");
                var downloadOk = await engine.ExecuteDownloadBatchAsync(
                    config.GetPublicApiBase(),
                    config.GetLocalApiBase(),
                    fechaInicio,
                    fechaCorte,
                    progressReporter,
                    tableFilter: null,
                    moduleFilter: null,
                    updateWatermark: true,
                    useIndividualTableDates: true,
                    trigger: SyncTriggerSource.Scheduled);

                // 4. Fase 2: Subida (Local -> Nube)
                Log.Information(">>> FASE 2: SUBIDA DE NOVEDADES (Local -> Nube) con Estado Granular");
                var uploadOk = await engine.ExecuteUploadBatchAsync(
                    config.GetLocalApiBase(),
                    config.GetPublicApiBase(),
                    fechaInicio,
                    fechaCorte,
                    progressReporter,
                    tableFilter: null,
                    moduleFilter: null,
                    updateWatermark: true,
                    useIndividualTableDates: true,
                    trigger: SyncTriggerSource.Scheduled);

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
