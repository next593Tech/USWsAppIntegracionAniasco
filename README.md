# Bizor Sync - Integración Dobra ERP

[![Platform](https://img.shields.io/badge/Platform-Windows%2011%20%7C%20Server-0078D4?style=flat-square&logo=windows)](https://microsoft.com)
[![Framework](https://img.shields.io/badge/Framework-.NET%208%20%7C%20.NET%20Framework%204.6.2-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com)
[![UI](https://img.shields.io/badge/UI-WinUI%203%20(Windows%20App%20SDK)-0078D6?style=flat-square)](https://learn.microsoft.com/windows/apps/winui/winui3/)
[![ORM](https://img.shields.io/badge/ORM-Entity%20Framework%206-68217A?style=flat-square)](https://learn.microsoft.com/ef/ef6/)
[![Database](https://img.shields.io/badge/Database-SQL%20Server-CC292B?style=flat-square&logo=microsoftsqlserver)](https://microsoft.com/sql-server)

**Bizor Sync** es una plataforma empresarial de sincronización bidireccional de alto rendimiento diseñada para la integración transaccional masiva entre sucursales locales y la nube para **Dobra ERP**. 

El sistema está optimizado para transferir cientos de miles de registros de forma incremental, garantizando cero caídas por agotamiento de memoria, prevención estricta de bloqueos N+1, trazabilidad granular por tabla y cumplimiento total de la integridad referencial sin sentencias SQL manuales ni dependencias propietarias no estándar.

> [!TIP]
> **¿Buscando la guía de uso u operaciones en el servidor?**  
> Hemos preparado una guía completa independiente para operadores y administradores:  
> 📖 **[Manual de Operación y Puesta en Producción (MANUAL_OPERATIVO.md)](./MANUAL_OPERATIVO.md)**  
> Incluye el instructivo de la interfaz gráfica WinUI 3, el nuevo Historial de Auditoría con corte contable vs técnico, explicación de `appsettings.json`, marcas de agua (Watermarks) y el paso a paso detallado para configurar la **Tarea Programada en Windows (Task Scheduler)**.

---

## 📑 Tabla de Contenidos

1. [Arquitectura General y Componentes](#-arquitectura-general-y-componentes)
2. [Stack Tecnológico](#-stack-tecnológico)
3. [Requisitos Previos y Herramientas](#-requisitos-previos-y-herramientas)
4. [Pilares de Rendimiento e Integridad](#-pilares-de-rendimiento-e-integridad)
5. [Estructura de la Solución](#-estructura-de-la-solución)
6. [Guía: Cómo Agregar una Nueva Tabla a la Sincronización](#-guía-cómo-agregar-una-nueva-tabla-a-la-sincronización)
7. [Flujo de Compilación y Despliegue (Publish)](#-flujo-de-compilación-y-despliegue-publish)
8. [Configuración y Monitoreo](#-configuración-y-monitoreo)
9. [📘 Manual de Operación y Configuración del Servidor (Task Scheduler, UI y appsettings)](./MANUAL_OPERATIVO.md)

---

## 🏛 Arquitectura General y Componentes

La solución conecta dos entornos mediante endpoints HTTP RESTful y motores de sincronización desacoplados:

```
┌─────────────────────────────────────────────────────────┐
│                    NUBE (API Pública)                  │
│   USWsApp (IIS) <───> Dobra ERP (Base de Datos Nube)   │
└───────────────────────────▲─────────────────────────────┘
                            │  HTTP JSON (Chunks de 500)
                            ▼
┌─────────────────────────────────────────────────────────┐
│                   MOTORES DE SINCRONIZACIÓN             │
│   ├── USWsSync.Console : Tarea programada en segundo plano │
│   └── USWsSync.UI      : Monitor ejecutivo y panel WinUI 3 │
└───────────────────────────▲─────────────────────────────┘
                            │  HTTP JSON / EF6 Directo
                            ▼
┌─────────────────────────────────────────────────────────┐
│                   LOCAL (ERP Sucursal)                  │
│   USWsApp (IIS) <───> Dobra ERP (Base de Datos Local)  │
└─────────────────────────────────────────────────────────┘
```

El ecosistema se compone de 5 proyectos especializados:

1. **`USWsSync.UI`**: Aplicación de escritorio nativa para Windows 11 desarrollada en **WinUI 3**. Cuenta con panel de monitoreo en tiempo real, métricas ejecutivas KPI, filtros por módulo y estado, sincronización manual y visores de diagnóstico con copia rápida al portapapeles.
2. **`USWsSync.Console`**: Aplicación autónoma .NET 8 diseñada para ser ejecutada periódicamente mediante el **Programador de Tareas de Windows (Task Scheduler)**.
3. **`USWsSync.Core`**: Biblioteca de clases central (.NET 8) que orquesta la lógica de negocio, clientes HTTP resilientes, catálogo de dependencias DAG (`TableRegistry`) y persistencia de estado temporal por tabla (`SyncStateManager`).
4. **`USWsApp`**: Servicio Web API RESTful (.NET Framework 4.6.2) desplegado en **Internet Information Services (IIS)** tanto local como en nube.
5. **`USWsLibrary`**: Capa de persistencia y lógica transaccional de Dobra ERP basada en **Entity Framework 6** con modelo de datos `DobraModel.edmx` y ayudantes de procesamiento por lotes `BatchSyncHelper`.

---

## 💻 Stack Tecnológico

| Componente | Tecnología | Versión | Propósito |
| :--- | :--- | :--- | :--- |
| **Interfaz de Usuario** | WinUI 3 / Windows App SDK | 2.4.0 / .NET 8 | Aplicación de escritorio fluida con Fluent Design nativo |
| **Motor de Sincronización** | .NET Runtime | 8.0 LTS | Procesamiento asíncrono, paralelismo y serialización System.Text.Json |
| **Capa de Negocio ERP** | .NET Framework | 4.6.2 | Compatibilidad nativa con el runtime de Dobra ERP |
| **Acceso a Datos** | Entity Framework | 6.2.0 | Mapeo objeto-relacional (ORM) puro sin sentencias SQL quemadas |
| **Base de Datos** | Microsoft SQL Server | 2016+ | Servidor de base de datos relacional transaccional |
| **Web API** | ASP.NET Web API 2 | 5.2.x | Exposición de endpoints de consulta y persistencia en IIS |
| **Logging & Métricas** | Serilog | 4.x | Registro estructurado a consola y archivos rotativos por componente |

---

## 🛠 Requisitos Previos y Herramientas

Para compilar, depurar y ejecutar la solución en un entorno de desarrollo o servidor, se requieren los siguientes componentes de software:

### 1. Visual Studio 2022 (v17.8 o superior)
Se recomienda **Visual Studio 2022** (Community, Professional o Enterprise).
- 🔗 **Descarga:** [Visual Studio 2022 Oficial](https://visualstudio.microsoft.com/es/downloads/)
- 📦 **Cargas de Trabajo requeridas en Visual Studio Installer:**
  - ✅ **Desarrollo de escritorio de .NET** (*.NET desktop development*)
  - ✅ **Desarrollo de ASP.NET y web** (*ASP.NET and web development*)
- 🧩 **Componentes Individuales obligatorios (pestaña 'Componentes individuales'):**
  - ✅ **Herramientas de Entity Framework 6** (*Entity Framework 6 Tools*) — Necesario para visualizar y compilar `DobraModel.edmx`.
  - ✅ **Herramientas de compilación de Windows App SDK en C#** (*Windows App SDK C# Build Tools*) — Necesario para compilar la UI WinUI 3.
  - ✅ **SDK de .NET Framework 4.6.2 y paquete de compatibilidad** (*.NET Framework 4.6.2 SDK / Targeting Pack*).
  - ✅ **MSBuild de Visual Studio 2022** (Ruta estándar: `C:\Program Files\Microsoft Visual Studio\2022\<Edición>\MSBuild\Current\Bin\MSBuild.exe`).

### 2. SDK de .NET 8.0 (LTS)
Requerido para compilar y ejecutar el motor central (`USWsSync.Core`), la tarea programada (`USWsSync.Console`) y la interfaz de usuario (`USWsSync.UI`).
- 🔗 **Descarga:** [Descargar .NET 8.0 SDK (x64)](https://dotnet.microsoft.com/es-es/download/dotnet/8.0)
- 💡 *Verificación en terminal:* `dotnet --version` (debe retornar `8.0.xxx`).

### 3. .NET Framework 4.6.2 Developer Pack / Targeting Pack
Requerido para compilar el backend de persistencia (`USWsLibrary`) y el servicio web (`USWsApp`).
- 🔗 **Descarga:** [Descargar .NET Framework 4.6.2 Developer Pack](https://dotnet.microsoft.com/es-es/download/dotnet-framework/net462)

### 4. Windows App SDK / WinUI 3 Runtime
Requerido para ejecutar la aplicación de escritorio `USWsSync.UI.exe`.
- **Sistema Operativo compatible:** Windows 10 (versión 1809, build 17763 o superior) o Windows 11.
- 🔗 **Descarga del instalador en tiempo de ejecución (Runtime):** [Windows App SDK Downloads](https://learn.microsoft.com/es-es/windows/apps/windows-app-sdk/downloads) (Instalar la versión 1.5 o superior x64).

### 5. Internet Information Services (IIS) con ASP.NET
Requerido en el servidor local y en el servidor de la nube para alojar la Web API `USWsApp`.
- **Habilitación en Windows:**
  1. Abrir `Ejecutar` (`Win + R`) y escribir `optionalfeatures` (o ir a *Activar o desactivar las características de Windows*).
  2. Marcar **Internet Information Services**.
  3. Desplegar: *Servicios World Wide Web* ➔ *Características de desarrollo de aplicaciones*.
  4. Marcar **ASP.NET 4.6** (o **ASP.NET 4.8**) y **Extensibilidad de .NET 4.6/4.8**.

### 6. Microsoft SQL Server & SQL Server Management Studio (SSMS)
- **Motor de Base de Datos:** SQL Server 2016, 2019 o 2022 con la base de datos de **Dobra ERP** cargada.
- 🔗 **Descarga de SSMS:** [Descargar SQL Server Management Studio (SSMS)](https://learn.microsoft.com/es-es/sql/ssms/download-sql-server-management-studio-ssms)

---

### 🔍 Script de Verificación Rápida de Entorno
Puedes ejecutar este bloque en **PowerShell** para comprobar de inmediato si tu equipo cuenta con las herramientas esenciales:

```powershell
Write-Host "=== VERIFICANDO REQUISITOS PREVIOS ===" -ForegroundColor Cyan

# 1. .NET 8 SDK
$dotnetVersion = dotnet --version 2>$null
if ($dotnetVersion -like "8.*") {
    Write-Host "[OK] .NET 8 SDK instalado: $dotnetVersion" -ForegroundColor Green
} else {
    Write-Host "[FALTA] .NET 8 SDK no encontrado o versión incorrecta: $dotnetVersion" -ForegroundColor Red
}

# 2. MSBuild de Visual Studio 2022
$msbuildPaths = @(
    "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
    "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe",
    "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
)
$msbuildFound = $msbuildPaths | Where-Object { Test-Path $_ } | Select-Object -First 1
if ($msbuildFound) {
    Write-Host "[OK] MSBuild VS2022 encontrado en: $msbuildFound" -ForegroundColor Green
} else {
    Write-Host "[FALTA] MSBuild de Visual Studio 2022 no encontrado." -ForegroundColor Red
}

# 3. .NET Framework 4.6.2 Targeting Pack
$net462Path = "${env:ProgramFiles(x86)}\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.2"
if (Test-Path $net462Path) {
    Write-Host "[OK] .NET Framework 4.6.2 Targeting Pack detectado." -ForegroundColor Green
} else {
    Write-Host "[FALTA] .NET Framework 4.6.2 Targeting Pack no detectado en: $net462Path" -ForegroundColor Red
}
```

---

## ⚡ Pilares de Rendimiento e Integridad

### 1. 100% Entity Framework Puro (Cero SQL Quemado / Sin UPSERT en texto plano)
Para mantener total compatibilidad con el modelo de objetos y evitar inyecciones o desincronizaciones de tipos, la persistencia se realiza bajo el patrón **Foreach + Contains + HashSet**:
1. **Extracción de claves por lote:** Se procesan bloques acotados de máximo **500 elementos**.
2. **Consulta LINQ masiva única:** Se extraen las claves ya existentes en SQL Server en una sola consulta:
   ```csharp
   var existingKeys = new HashSet<string>(db.TABLA.AsNoTracking().Where(x => ids.Contains(x.ID)).Select(x => x.ID));
   ```
3. **Clasificación en memoria (RAM):** En microsegundos se determina si la entidad se inserta (`db.TABLA.Add(item)`) o se actualiza (`db.Entry(item).State = EntityState.Modified`).
4. **Transacción atómica:** Un único `db.SaveChanges()` por bloque de 500 registros, liberando inmediatamente el `DbContext` para mantener un consumo plano de memoria en IIS.

### 2. Eliminación del Problema N+1 y Optimización de Lectura
- Se eliminaron miles de consultas repetitivas individuales `db.TABLA.Any(...)`.
- Todas las consultas de extracción para sincronización utilizan `.AsNoTracking()`, lo que desactiva el rastreador de cambios (*Change Tracker*) de EF6 durante la lectura, reduciendo el consumo de memoria RAM a la mitad.

### 3. Estado Granular por Tabla y Tolerancia a Fallos
A diferencia de los sincronizadores monolíticos tradicionales, Bizor Sync mantiene archivos de estado JSON independientes:
- `Publish\sync_state_download.json` (32 tablas de descarga).
- `Publish\sync_state_upload.json` (72 tablas de subida).

Si una tabla específica falla (por ejemplo, por una restricción de clave foránea o timeout momentáneo):
- **Solo esa tabla se marca como errónea**, almacenando la excepción exacta.
- **Todas las tablas previas exitosas avanzan su marca temporal de corte.**
- La siguiente ejecución reintentará únicamente las tablas pendientes o con incidencias, evitando reenviar innecesariamente decenas de tablas ya sincronizadas.

### 4. Orden Cronológico por Dependencias (Grafo Acíclico Dirigido - DAG)
El orden de sincronización registrado en `TableRegistry.cs` respeta estrictamente las relaciones de claves foráneas:
- **Descarga:** Sistema/Seguridad ➔ Cuentas y Terceros ➔ Inventarios/Precios ➔ Movimientos.
- **Subida:** Configuración Base ➔ Contabilidad ➔ Empleados ➔ Clientes ➔ Inventarios Maestros ➔ Movimientos/Kardex ➔ Facturación/POS ➔ Compras/Acreedores ➔ Bancos/Tesorería.

---

## 📁 Estructura de la Solución

```text
E:\Next\USWsAppIntegracionAniasco\
│
├── Publish\                          # Directorio de binarios listos para producción
│   ├── USWsSync.UI.exe               # Ejecutable WinUI 3 (Aplicación de escritorio)
│   ├── USWsSync.Console.exe          # Ejecutable de consola (Tarea programada)
│   ├── USWsLibrary.dll               # DLL con metadatos EDMX embebidos
│   ├── sync_state_download.json      # Estado granular de corte para descargas
│   └── sync_state_upload.json        # Estado granular de corte para subidas
│
├── USWsLibrary\                      # Backend / Persistencia (.NET Framework 4.6.2)
│   ├── ModelDobraDatabase\           # EDMX (DobraModel.edmx, .csdl, .msl, .ssdl)
│   └── Services\                     # ClientesServices.cs (117 métodos save/list) y BatchSyncHelper.cs
│
├── USWsApp\                          # Web API para IIS (.NET Framework 4.6.2)
│   ├── Controllers\                  # Controladores RESTful (ClientesController, etc.)
│   └── Web.config                    # Configuración de conexiones a SQL Server
│
├── USWsSync.Core\                    # Motor de Sincronización (.NET 8.0)
│   ├── Engine\                       # SyncEngine.cs (Descarga, Subida, HTTP resiliente)
│   ├── Registry\                     # TableRegistry.cs, TableSyncDefinition.cs, SyncModule.cs
│   ├── State\                        # SyncStateManager.cs, SyncTableState.cs
│   ├── Configuration\                # ConfigManager.cs, appsettings.json
│   └── Logging\                      # LogPathHelper.cs
│
├── USWsSync.UI\                      # Aplicación de Escritorio WinUI 3 (.NET 8.0)
│   ├── Pages\
│   │   ├── MonitorPage.xaml          # Monitor de Trazabilidad y Dashboard de KPIs
│   │   ├── DownloadPage.xaml         # Sincronización manual de descargas
│   │   ├── UploadPage.xaml           # Sincronización manual de subidas
│   │   └── SettingsPage.xaml         # Configuración de red y credenciales
│   └── MainWindow.xaml               # Shell de navegación Fluent Design
│
└── USWsSync.Console\                 # Runner de Fondo (.NET 8.0)
    └── Program.cs                    # Punto de entrada para Task Scheduler
```

---

## 🚀 Guía: Cómo Agregar una Nueva Tabla a la Sincronización

Gracias a la arquitectura desacoplada basada en `TableRegistry`, **no se requiere modificar ninguna pantalla XAML ni el motor de consola**. Al registrar la tabla, el sistema la incorpora de inmediato al ciclo de ejecución, a las tarjetas KPI, a los filtros por módulo y a la persistencia de estado granular.

Sigue estos pasos respetando el **estándar real del proyecto**:

---

### Paso 0: Asegurar la Clave Primaria y Actualizar el Modelo EDMX (`DobraModel.edmx`)
1. **Verificar Clave Primaria en SQL Server:** Toda tabla a sincronizar **debe tener una Primary Key** definida en la base de datos (simple o compuesta). Si no la tiene, agrégala en SQL Server:
   ```sql
   ALTER TABLE TABLA_NUEVA ADD CONSTRAINT PK_TABLA_NUEVA PRIMARY KEY (ID);
   ```
2. **Actualizar el EDMX:** En Visual Studio 2022, abre `USWsLibrary/ModelDobraDatabase/DobraModel.edmx`:
   - Clic derecho en el diseñador ➔ **Actualizar modelo desde la base de datos...**
   - En la pestaña *Agregar*, selecciona la nueva tabla `TABLA_NUEVA` y finaliza el asistente.
   - Guarda el archivo (`Ctrl + S`) para que Entity Framework 6 genere automáticamente la clase POCO (`TABLA_NUEVA.cs`) y agregue la propiedad `DbSet<TABLA_NUEVA> TABLA_NUEVA { get; set; }` en la clase `DobraConnection`.

---

### Paso 1: Implementar Métodos de Lectura y Guardado en `USWsLibrary`
Abre `USWsLibrary/Services/ClientesServices.cs` e implementa los dos métodos siguiendo el estándar de las 104 tablas existentes:

```csharp
// 1. LECTURA POR RANGO DE FECHAS (Con AsNoTracking)
public PagedList<TABLA_NUEVA> listTablaNueva(DateTime lastUpdate, DateTime lastUpdate2)
{
    PagedList<TABLA_NUEVA> packages = new PagedList<TABLA_NUEVA>();
    using (DobraConnection db = new DobraConnection())
    {
        packages.Results = db.TABLA_NUEVA
            .AsNoTracking()
            .Where(e => (e.CreadoDate >= lastUpdate && e.CreadoDate <= lastUpdate2) || 
                        (e.EditadoDate >= lastUpdate && e.EditadoDate <= lastUpdate2))
            .ToList<TABLA_NUEVA>();
        packages.Total = packages.Results.Count;
        packages.Count = packages.Results.Count;
    }
    return packages;
}

// 2. GUARDADO MASIVO OPTIMIZADO (Por lotes de 500 registros)
// Caso A: Clave Primaria Simple (ej. ID de tipo string o int)
public ErrorSave saveTablaNueva(PagedList<TABLA_NUEVA> tablaNueva)
{
    if (tablaNueva == null || tablaNueva.Results == null || tablaNueva.Results.Count == 0) 
        return new ErrorSave();

    return BatchSyncHelper.ExecuteBatchSave(
        tablaNueva.Results,
        "TABLA_NUEVA",
        x => x.ID,
        db => db.TABLA_NUEVA,
        (db, keys) => new HashSet<string>(db.TABLA_NUEVA.Where(x => keys.Contains(x.ID)).Select(x => x.ID))
    );
}

// Caso B: Si la tabla tiene Clave Primaria Compuesta (ej. ID + Linea)
// public ErrorSave saveTablaNueva(PagedList<TABLA_NUEVA> tablaNueva)
// {
//     if (tablaNueva == null || tablaNueva.Results == null || tablaNueva.Results.Count == 0) 
//         return new ErrorSave();
//
//     return BatchSyncHelper.ExecuteBatchSaveComposite(
//         tablaNueva.Results,
//         "TABLA_NUEVA",
//         x => $"{x.ID}|{x.LINEA}",
//         db => db.TABLA_NUEVA,
//         (db, keys) => new HashSet<string>(db.TABLA_NUEVA.Select(x => x.ID + "|" + x.LINEA))
//     );
// }
```

---

### Paso 2: Exponer los Endpoints en la Web API (`USWsApp`)
> [!IMPORTANT]
> **Estándar de Centralización:** En esta solución, **todas las 104 acciones de sincronización están centralizadas en `USWsApp/Controllers/ClientesController.cs`**.
> La ruta Web API por defecto es `api/{controller}/{action}/{id}` (definida en `WebApiConfig.cs`), por lo que **NO se utiliza `[Route]` explícito ni `IHttpActionResult`**. Los métodos reciben los tipos concretos y delegan directamente en el campo privado `_objclisrv`.

Abre `USWsApp/Controllers/ClientesController.cs` y agrega las acciones:

```csharp
[HttpPost]
public PagedList<TABLA_NUEVA> listTablaNueva(DateTime lastUpdate, DateTime lastUpdate2)
{
    return _objclisrv.listTablaNueva(lastUpdate, lastUpdate2);
}

[HttpPost]
public ErrorSave saveTablaNueva(PagedList<TABLA_NUEVA> tablaNueva)
{
    return _objclisrv.saveTablaNueva(tablaNueva);
}
```

---

### Paso 3: Registrar la Tabla en `TableRegistry.cs` (`USWsSync.Core`)
Abre `USWsSync.Core/Registry/TableRegistry.cs`:
- Si la tabla se descarga desde la nube hacia el servidor local, agrégala en la lista `DownloadTables`.
- Si la tabla se sube desde el servidor local hacia la nube, agrégala en la lista `UploadTables`.

```csharp
TableSyncDefinition.Create<TABLA_NUEVA>(
    "TABLA_NUEVA",                       // Nombre formal en base de datos (SQL Server)
    "listTablaNueva",                    // Nombre EXACTO de la acción de lectura en ClientesController
    "saveTablaNueva",                    // Nombre EXACTO de la acción de guardado en ClientesController
    SyncModule.Inventarios,               // Módulo empresarial (Sistema, Cuentas, Personas, Inventarios, etc.)
    batchSize: 500                        // Tamaño de bloque en memoria (opcional, default 500)
)
```

> [!WARNING]
> **Convención de Nombres de Métodos:** El segundo y tercer parámetro deben ser **únicamente el nombre de la acción** (ej. `"listTablaNueva"` y `"saveTablaNueva"`). **NO** incluyas prefijos como `"api/..."` ni `"/Clientes/..."`, ya que el motor `SyncEngine` construye las URLs automáticamente invocando `"{BaseUrl}/Clientes/{methodName}"`.

> [!TIP]
> **Orden de Dependencias (Foreign Keys):** Si `TABLA_NUEVA` hace referencia a otra tabla (por ejemplo, tiene clave foránea hacia `CLI_CLIENTES` o `INV_PRODUCTOS`), colócala en la lista **después** de su tabla padre para garantizar la integridad referencial durante la sincronización.

---

### Paso 4: Compilar y Desplegar
Una vez realizados los pasos anteriores, ejecuta el ciclo de compilación con MSBuild (ver sección siguiente). La nueva tabla quedará inmediatamente visible en el Monitor UI, los contadores de KPIs, el selector de módulos y el ejecutable de consola sin requerir ningún cambio adicional.

---

## 🔨 Flujo de Compilación y Despliegue (Publish)

> [!CAUTION]
> **Regla Crítica de Metadatos EDMX:** `dotnet publish` compila proyectos modernos de .NET 8, pero puede limpiar recursos embebidos legados de Entity Framework 6. Siempre se debe compilar `USWsLibrary.csproj` con **MSBuild de Visual Studio** para asegurar que `DobraModel.csdl`, `msl` y `ssdl` queden embebidos dentro de `USWsLibrary.dll`.

Ejecuta los siguientes comandos en PowerShell desde la raíz de la solución:

```powershell
# 1. Compilar y publicar la interfaz WinUI 3 y la Consola
dotnet publish USWsSync.UI\USWsSync.UI.csproj -c Release -o Publish\
dotnet publish USWsSync.Console\USWsSync.Console.csproj -c Release -o Publish\

# 2. Compilar USWsLibrary con MSBuild de Visual Studio 2022
& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" USWsLibrary\USWsLibrary.csproj /p:Configuration=Release

# 3. Copiar la DLL compilada con recursos EDMX hacia la carpeta Publish y a la Web API
Copy-Item 'USWsLibrary\bin\Release\USWsLibrary.dll' 'Publish\USWsLibrary.dll' -Force
Copy-Item 'USWsLibrary\bin\Release\USWsLibrary.dll' 'USWsApp\bin\USWsLibrary.dll' -Force

# 4. Verificar que los esquemas EDMX estén presentes en el binario final
powershell -Command "[System.Reflection.Assembly]::LoadFrom('Publish\USWsLibrary.dll').GetManifestResourceNames()"
# Salida esperada:
# ModelDobraDatabase.DobraModel.csdl
# ModelDobraDatabase.DobraModel.msl
# ModelDobraDatabase.DobraModel.ssdl
```

---

## ⚙️ Configuración y Monitoreo

> [!NOTE]
> Para una explicación detallada de cada parámetro, la regla de avance de marcas de agua (Watermarks), recuperación automática de la base de datos `sync_history.db` y el instructivo con capturas y pasos para crear la **Tarea Programada en Windows (Task Scheduler)**, consulta el [📘 Manual de Operación y Administración (MANUAL_OPERATIVO.md)](./MANUAL_OPERATIVO.md).

### Archivo `appsettings.json`
Ubicado en el directorio de ejecución (`Publish\appsettings.json`), compartido por la UI (`USWsSync.UI.exe`) y la Consola (`USWsSync.Console.exe`):

```json
{
  "syncConfig": {
    "ipLocal": "192.168.100.242:8484",
    "ipPublica": "186.3.193.198:444",
    "lastDateUpdate": "2026-09-01T00:00:00",
    "lastDateDownload": "2026-09-01T00:00:00",
    "timeoutSegundos": 300,
    "cleanIpLocal": "192.168.100.242:8484",
    "cleanIpPublica": "186.3.193.198:444"
  }
}
```

### Ubicación de Logs y Base de Auditoría
El sistema registra actividad estructurada con rotación diaria automática y trazabilidad histórica completa:
- **Consola (Tarea Programada):** `Publish\Logs\Consola\Consola-yyyyMMdd.log`
- **Interfaz Gráfica (UI):** `Publish\Logs\UI\UI-yyyyMMdd.log`
- **Base de Datos SQLite de Auditoría:** `Publish\sync_history.db` (modo WAL de alta concurrencia, auto-creable si se elimina y auto-purgada a 60 días).
- **Estados Temporales Granulares:** `Publish\sync_state_download.json` y `Publish\sync_state_upload.json` (para reintentar únicamente tablas rezagadas sin reiniciar todo el lote).

### Automatización en Windows
Para la puesta en marcha desatendida del sincronizador cada 5 minutos en el servidor mediante el **Programador de Tareas de Windows (Task Scheduler)**, revisa la sección dedicada en el manual:
👉 [Guía Paso a Paso: Tarea Programada en Windows (Task Scheduler)](./MANUAL_OPERATIVO.md#4-guía-paso-a-paso-tarea-programada-en-windows-task-scheduler)

---

## 🛡️ Licencia y Mantenimiento

Desarrollado para la integración corporativa de **Dobra ERP**. Todos los derechos reservados. Mantenido por el equipo de tecnología y soporte de sistemas.
