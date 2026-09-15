# Manual de Operación y Administración: Bizor Sync (Dobra ERP)

[![Platform](https://img.shields.io/badge/Platform-Windows%2011%20%7C%20Server-0078D4?style=flat-square&logo=windows)](https://microsoft.com)
[![App](https://img.shields.io/badge/App-Bizor%20Sync%20UI%20%26%20Console-0078D6?style=flat-square)](https://dotnet.microsoft.com)
[![Database](https://img.shields.io/badge/Auditor%C3%ADa-SQLite%20WAL-003B57?style=flat-square&logo=sqlite)](https://sqlite.org)
[![ERP](https://img.shields.io/badge/ERP-Dobra%20ERP%20SQL%20Server-CC292B?style=flat-square)](https://microsoft.com/sql-server)

Guía completa técnica y operativa para administradores de sistemas, personal de soporte y operadores de **Bizor Sync**. Este documento detalla el uso de la interfaz gráfica de usuario, la configuración de parámetros del sistema, el monitoreo en tiempo real, la auditoría histórica de datos y la puesta en marcha de la sincronización automática mediante el Programador de Tareas de Windows.

---

## 📑 Tabla de Contenidos

1. [Visión General del Sistema](#1-visión-general-del-sistema)
2. [Manual de Operación de la Interfaz Gráfica (WinUI 3)](#2-manual-de-operación-de-la-interfaz-gráfica-winui-3)
   - [2.1 Descarga Manual (Nube ➔ ERP Local)](#21-descarga-manual-nube--erp-local)
   - [2.2 Subida Manual (ERP Local ➔ Nube)](#22-subida-manual-erp-local--nube)
   - [2.3 Monitor de Tablas en Tiempo Real](#23-monitor-de-tablas-en-tiempo-real)
   - [2.4 Historial de Auditoría y Trazabilidad (SQLite)](#24-historial-de-auditoría-y-trazabilidad-sqlite)
3. [Guía Técnica de Configuración (`appsettings.json`)](#3-guía-técnica-de-configuración-appsettingsjson)
   - [3.1 Estructura y Parámetros del Archivo](#31-estructura-y-parámetros-del-archivo)
   - [3.2 La Regla de Oro de la Marca de Agua (Watermark)](#32-la-regla-de-oro-de-la-marca-de-agua-watermark)
   - [3.3 Archivos de Estado Granular por Tabla](#33-archivos-de-estado-granular-por-tabla)
   - [3.4 Base de Datos SQLite (`sync_history.db`) y Auto-Regeneración](#34-base-de-datos-sqlite-sync_historydb-y-auto-regeneración)
4. [Guía Paso a Paso: Tarea Programada en Windows (Task Scheduler)](#4-guía-paso-a-paso-tarea-programada-en-windows-task-scheduler)
5. [Preguntas Frecuentes y Solución de Problemas (Troubleshooting)](#5-preguntas-frecuentes-y-solución-de-problemas-troubleshooting)

---

## 1. Visión General del Sistema

**Bizor Sync** opera como un puente bidireccional entre la base de datos central en la nube y las sucursales locales que ejecutan **Dobra ERP**.

El sistema cuenta con dos ejecutables principales ubicados en la carpeta `Publish\`:
* **`USWsSync.UI.exe`**: Panel administrativo de escritorio con diseño Windows 11 Fluent Design para operadores y soporte técnico. Permite supervisar el estado de cada tabla, ejecutar sincronizaciones manuales con filtros de módulo/fecha y realizar auditorías históricas con diagnósticos detallados.
* **`USWsSync.Console.exe`**: Motor silencioso en segundo plano diseñado para ejecutarse automáticamente cada 5 minutos mediante el Programador de Tareas de Windows.

```
┌────────────────────────────────────────────────────────┐
│                   NUBE (API Pública)                  │
│       USWsApp (IIS) <───> Dobra ERP Database Nube      │
└──────────────────────────▲─────────────────────────────┘
                           │ HTTP JSON (Chunks de 500)
                           ▼
┌────────────────────────────────────────────────────────┐
│                   MOTORES BIZOR SYNC                   │
│   ├── USWsSync.Console : Tarea programada (5 min)      │
│   └── USWsSync.UI      : Panel operativo y auditoría   │
│   └── sync_history.db  : Auditoría SQLite local (WAL)  │
└──────────────────────────▲─────────────────────────────┘
                           │ HTTP JSON / EF6 Directo
                           ▼
┌────────────────────────────────────────────────────────┐
│                   LOCAL (ERP Sucursal)                 │
│      USWsApp (IIS) <───> Dobra ERP Database Local      │
└────────────────────────────────────────────────────────┘
```

---

## 2. Manual de Operación de la Interfaz Gráfica (WinUI 3)

Al iniciar `USWsSync.UI.exe`, se presenta un menú de navegación lateral compacto y elegante:

### 2.1 Descarga Manual (Nube ➔ ERP Local)
Permite extraer novedades desde la base de datos de la nube e insertarlas o actualizarlas en el ERP local.

```text
┌──────────────────────────────────────────────────────────────────────────────┐
│ Descarga de Datos                                                            │
│ Sincroniza información desde la Nube (API Pública) hacia la Base Local (ERP).│
├───────────────────────────────────────────┬──────────────────────────────────┤
│ PARÁMETROS DE SINCRONIZACIÓN              │ PROGRESO Y REGISTRO EN VIVO      │
│                                           │                                  │
│ Fecha Inicio: [ 2026-09-01 ▾ ]            │ Progreso Global: [======== 100%] │
│ Fecha Final:  [ 2026-09-14 ▾ ]            │ Tabla: INV_ARTICULOS (32/32)     │
│ Módulo:       [ 🌐 Todos los Módulos ▾ ]  │                                  │
│ ☑ Actualizar fecha de corte al terminar   │ [LOG INTERACTIVO EN TIEMPO REAL] │
│                                           │ [OK] 45 registros insertados     │
│ [ ⬇️ Iniciar Descarga ]                   │ [OK] Descarga completada 100%    │
│ [ 🔄 Reintentar fallidas (0) ]            │                                  │
└───────────────────────────────────────────┴──────────────────────────────────┘
```

#### Pasos para ejecutar una descarga manual:
1. **Definir el Rango de Fechas:**
   - **Fecha Inicio:** Por defecto toma la última fecha de corte guardada. Puede modificarse para reprocesar días anteriores.
   - **Fecha Final:** Fecha y hora de corte superior (habitualmente el momento actual).
2. **Seleccionar Módulo:**
   - Puede sincronizar *"Todos los Módulos"* (32 tablas) o aislar un módulo específico (ej. *"Ventas y Facturación"*, *"Inventarios y Artículos"*, *"Cuentas y Terceros"*).
3. **Casilla de Marca de Agua ("Actualizar fecha de corte al terminar"):**
   - **Marcada (Recomendado):** Al finalizar con éxito, el sistema actualizará la fecha de corte para que la próxima ejecución solo descargue lo nuevo.
   - **Desmarcada:** Útil para hacer pruebas o auditorías de un día histórico sin alterar el avance oficial de producción.
4. **Iniciar Proceso:**
   - Pulse **"Iniciar Descarga"**. Verá el avance porcentual, la tabla en proceso y el registro en tiempo real.
5. **Reintento Inteligente:**
   - Si alguna tabla falla (por ejemplo por caída temporal de internet o bloqueo en base de datos), el botón contextual **"Reintentar tablas fallidas previas (X)"** se habilitará automáticamente para volver a intentar **únicamente las tablas afectadas**, sin reprocesar las que ya terminaron bien.

---

### 2.2 Subida Manual (ERP Local ➔ Nube)
Permite extraer del ERP local las ventas, cobros, transferencias y movimientos contables emitidos en la sucursal y enviarlos hacia la nube.

* La interfaz opera con la misma lógica que la pantalla de Descarga, evaluando las **72 tablas** del catálogo de subida en estricto orden de dependencias de claves foráneas (Foreign Keys).

---

### 2.3 Monitor de Tablas en Tiempo Real
Supervisa el estado de salud, corte cronológico e incidencias de cada tabla registrada en el catálogo.

```text
┌──────────────────────────────────────────────────────────────────────────────┐
│ Monitor de Sincronización                           [ 🔄 Actualizar Datos ]  │
├─────────────────┬─────────────────┬─────────────────┬────────────────────────┤
│ TABLAS TOTALES  │ ESTADO OPERATIVO│ ÚLTIMO CORTE    │ REGISTROS TRANSFERIDOS │
│ 72 en catálogo  │ 72 OK · 0 Error │ 2026-09-14 15:30│ 1,450 filas última vez │
├─────────────────┴─────────────────┴─────────────────┴────────────────────────┤
│ [◉ Subidas (Local a Nube)]  [🔍 Buscar tabla...]  [Módulo: Todos ▾]  [Estado]│
├────────┬──────────────────────┬──────────────┬──────────────────┬────────────┤
│ Estado │ Nombre de Tabla      │ Módulo       │ Último Corte OK  │ Acciones   │
├────────┼──────────────────────┼──────────────┼──────────────────┼────────────┤
│ [ OK ] │ CLI_CLIENTES         │ Personas     │ 2026-09-14 15:30 │ Sin errores│
│ [ OK ] │ VEN_FACTURAS         │ Ventas       │ 2026-09-14 15:30 │ Sin errores│
│ [FAIL] │ ACR_RETENCIONES_DT   │ Compras      │ 2026-09-12 10:00 │ [Incidencia│
└────────┴──────────────────────┴──────────────┴──────────────────┴────────────┘
```

#### Funcionalidades clave del Monitor:
* **Métricas Ejecutivas (KPIs):** Resumen visual instantáneo de salud operativa (cuántas tablas están al día y cuántas tienen alertas).
* **Segmentación de Flujo:** Conmutador rápido entre **Descargas (32 tablas)** y **Subidas (72 tablas)**.
* **Búsqueda Dinámica:** Escriba cualquier fragmento del nombre de la tabla (ej. `FACT` o `INV`) para filtrar al instante.
* **Inspección de Incidencias en 1 Clic:**
  - Cuando una tabla presente error, aparecerá un botón rojo **"Ver Incidencia"**.
  - Al pulsar, se despliega una ventana modal con el mensaje técnico completo (Stack Trace de SQL Server / Entity Framework).
  - Incluye el botón **"Copiar Diagnóstico"** que envía el reporte al portapapeles listo para ser pegado en un correo o chat de soporte.

---

### 2.4 Historial de Auditoría y Trazabilidad (SQLite)
Diseñado para que cualquier persona encargada o auditor pueda inspeccionar **qué ocurrió exactamente en una fecha y hora específica** sin afectar el rendimiento del servidor.

```text
┌────────────────────────────────────────────────────────────────────────────────────────┐
│ Historial de Auditoría                                                                 │
│ Trazabilidad histórica de sincronizaciones, deltas modificados y ventana de datos ERP. │
├─────────────────┬─────────────────┬─────────────────┬──────────────────────────────────┤
│ SINCRONIZACIONES│ EXITOSAS        │ CON INCIDENCIAS │ REGISTROS TRANSFERIDOS           │
│ 145 en periodo  │ 142 al 100%     │ 3 con aviso     │ 8,920 filas en total             │
├─────────────────┴─────────────────┴─────────────────┴──────────────────────────────────┤
│ FILTROS                                                                                │
│ [Desde: 2026-09-14] [Hasta: 2026-09-14] [Flujo: Todos ▾] [Estado: Todos ▾] [Hoy] [Filtrar]│
│ [🔍 Buscar por tabla: VEN_FACTURAS...] [⏱️ Hora: 14:00 a 16:00] [☑ Solo con cambios]     │
├──────────────────────────────────────┬─────────────────────────────────────────────────┤
│ HISTORIAL DE SINCRONIZACIONES        │ DETALLE DE LA SINCRONIZACIÓN SELECCIONADA       │
│                                      │                                                 │
│ ┌──────────────────────────────────┐ │ [EXITOSA] ID: #142 · GUID: 8a4b2c1d             │
│ │ [EXITOSA] [Descarga] [Auto 5m]   │ │                                [Copiar Auditoría]│
│ │ 2026-09-14 15:30:02        2.4 s │ │ ┌──────────────────────┬──────────────────────┐ │
│ │ 📅 Datos: 12-Sep ➔ 14-Sep (2d)   │ │ │ ⏱️ EJECUCIÓN PROCESO │ 📦 PERÍODO DE CORTE  │ │
│ │ 3 tablas cambiaron · 145 reg.    │ │ │ Inicio:   15:30:02   │ Desde: 12-Sep 10:00  │ │
│ └──────────────────────────────────┘ │ │ Fin:      15:30:04   │ Hasta: 14-Sep 15:30  │ │
│                                      │ │ Duración: 2,400 ms   │ Cobertura: 2d 5h     │ │
│                                      │ └──────────────────────┴──────────────────────┘ │
│                                      │                                                 │
│                                      │ TABLAS AFECTADAS (DELTAS):                      │
│                                      │ • VEN_FACTURAS: 120 reg. (850 ms)               │
│                                      │ • CXC_CLIENTES:  25 reg. (320 ms)               │
└──────────────────────────────────────┴─────────────────────────────────────────────────┘
```

#### Concepto Vital: Momento de Ejecución vs. Ventana de Datos
El módulo resuelve la duda más común de auditoría mediante **dos conceptos independientes**:
1. **Momento de Ejecución (Cuándo corrió la máquina):**
   - Fecha y hora exacta de disparo del proceso (`15:30:02`).
   - Duración real en segundos (`2.4 s`).
   - Disparador: *Automática (Consola)* o *Manual (UI)*.
2. **Período de Información Afectada (Qué corte contable abarcó):**
   - Rango de fechas de las facturas y cobros evaluados en Dobra ERP (`Desde 12-Sep 10:00 hasta 14-Sep 15:30`).
   - Cobertura: Tiempo contable cubierto (*"2 días y 5 horas de información"*).
   - *Ejemplo práctico:* Si el servidor estuvo apagado durante el fin de semana, este visor confirmará que la sincronización del lunes cubrió correctamente todas las facturas emitidas desde el viernes anterior.

#### Cómo realizar una auditoría puntual:
1. **Para buscar un momento específico:** Indique la fecha en *"Desde/Hasta"*, marque la casilla *"Hora específica"* y coloque el rango (ej. de `14:00` a `15:00`).
2. **Para buscar cuándo se sincronizó una tabla específica:** Escriba el nombre de la tabla en *"Búsqueda por tabla"* (ej. `VEN_FACTURAS`); el sistema mostrará únicamente los ciclos donde esa tabla tuvo movimientos o fallos.
3. **Casilla "Solo con modificaciones o incidencias":** Manténgala activa para ocultar automáticamente las sincronizaciones periódicas que salieron en 0 registros (cuando no hubo facturas nuevas en ese lapso de 5 minutos).
4. **Copiar Auditoría:** El botón *"Copiar Auditoría"* genera un reporte de texto estructurado con todos los datos del ciclo para enviar por correo o adjuntar a un ticket de soporte.

---

## 3. Guía Técnica de Configuración (`appsettings.json`)

Tanto la interfaz gráfica como la consola en segundo plano leen su configuración de un único archivo JSON ubicado en la carpeta de ejecución:
`Publish\appsettings.json`

### 3.1 Estructura y Parámetros del Archivo

```json
{
  "syncConfig": {
    "ipLocal": "192.168.100.242:8484",
    "ipPublica": "186.3.193.198:444",
    "lastDateUpdate": "2026-09-14T15:30:00",
    "lastDateDownload": "2026-09-14T15:30:00",
    "timeoutSegundos": 300
  }
}
```

#### Descripción detallada de cada parámetro:

| Clave | Tipo | Valor por Defecto | Descripción Técnica |
| :--- | :--- | :--- | :--- |
| `ipLocal` | String | `192.168.100.242:8484` | Host/IP y puerto donde está publicado el servicio IIS local de la sucursal. Acepta formato `IP:Puerto` o `http://IP:Puerto`. El motor añade automáticamente `/api` de forma segura. |
| `ipPublica` | String | `186.3.193.198:444` | Host/IP y puerto del servicio IIS de la nube. Si utiliza certificado SSL, puede configurar `https://api.tudominio.com`. |
| `lastDateUpdate` | ISO-8601 | Fecha actual - 1 día | **Marca de agua (Watermark) de Subida:** Fecha y hora del último corte exitoso subido a la nube. En cada ciclo automático, el motor busca registros modificados desde esta fecha hasta el momento actual. |
| `lastDateDownload` | ISO-8601 | Fecha actual - 1 día | **Marca de agua de Descarga:** Fecha y hora del último corte exitoso descargado desde la nube hacia el ERP local. |
| `timeoutSegundos` | Entero | `300` (5 minutos) | Tiempo máximo de espera por cada petición HTTP antes de abortar por timeout. Evita que un bloqueo transitorio congele el sincronizador indefinidamente. |

---

### 3.2 La Regla de Oro de la Marca de Agua (Watermark)

> [!IMPORTANT]
> **Principio de Integridad Contable (Regla de Oro):**
> La fecha de corte (`lastDateUpdate` / `lastDateDownload`) **SOLO avanza si el 100% de las tablas del lote terminaron con CERO errores**.
> * Si 71 tablas suben bien y 1 sola falla (ej. por bloqueo en SQL Server):
>   1. La fecha de corte **NO avanza**.
>   2. En el archivo `sync_state_upload.json`, las 71 tablas exitosas avanzan su marca individual para no volver a transferir datos repetidos.
>   3. En el siguiente ciclo de 5 minutos, el motor volverá a reintentar la tabla que falló desde la fecha pendiente.
>   4. Una vez que la tabla rezagada se recupera, la fecha general se pone al día.

#### ¿Cómo forzar una resincronización histórica completa?
Si por alguna razón administrativa se requiere reprocesar todos los movimientos de los últimos 7 días:
1. Abra `Publish\appsettings.json` con un editor de texto (Notepad, VS Code).
2. Modifique la fecha deseada, por ejemplo:
   `"lastDateUpdate": "2026-09-07T00:00:00"`
3. Guarde el archivo. El próximo ciclo del sincronizador evaluará y actualizará automáticamente todos los registros desde esa fecha.

---

### 3.3 Archivos de Estado Granular por Tabla

En la carpeta `Publish\` residen dos archivos JSON de control interno:
* `sync_state_download.json` (Control de las 32 tablas de Descarga).
* `sync_state_upload.json` (Control de las 72 tablas de Subida).

Cada archivo guarda un diccionario donde la clave es el nombre de la tabla y el valor contiene:
* `lastSuccessDate`: Última fecha/hora en que esa tabla específica completó su transferencia.
* `lastAttemptDate`: Momento exacto del último intento.
* `recordsCount`: Número de registros transferidos en la última ejecución.
* `isSuccess`: Indicador booleano (`true` / `false`).
* `lastError`: Texto detallado del error si la tabla falló.

*Nota:* Estos archivos son mantenidos automáticamente por el motor. No es necesario editarlos manualmente salvo para depuración avanzada de soporte.

---

### 3.4 Base de Datos SQLite (`sync_history.db`) y Auto-Regeneración

El historial de auditoría se guarda en un archivo local embebido en `Publish\sync_history.db`:
* **Modo WAL (Write-Ahead Logging):** Permite que la tarea programada de consola escriba registros de auditoría en segundo plano sin bloquear ni ser bloqueada cuando un usuario tiene abierta la interfaz gráfica.
* **Auto-Purga (Retención de 60 días):** Al terminar cada ciclo, el sistema elimina automáticamente registros con más de 60 días de antigüedad para mantener la base de datos siempre ligera (< 15 MB anuales).
* **Auto-Regeneración transparente:** Si por error o mantenimiento se elimina físicamente el archivo `sync_history.db`, **el sistema lo vuelve a crear desde cero automáticamente** la próxima vez que se ejecute la consola o la UI, con todas sus tablas, claves e índices íntegros. Los datos contables de Dobra ERP no sufren ningún riesgo.

---

## 4. Guía Paso a Paso: Tarea Programada en Windows (Task Scheduler)

Para que el sincronizador funcione de forma autónoma y continua cada 5 minutos en el servidor o máquina principal de la sucursal, siga los siguientes pasos:

### Paso 1: Abrir el Programador de Tareas
1. Presione las teclas `Windows + R`.
2. Escriba `taskschd.msc` y presione `Enter`.
3. En el panel lateral izquierdo, seleccione la carpeta **"Biblioteca del Programador de tareas"**.

---

### Paso 2: Crear la Tarea
En el panel derecho ("Acciones"), haga clic en **"Crear tarea..."** *(NO elija "Crear tarea básica" para poder configurar permisos elevados)*.

---

### Paso 3: Configurar la Pestaña "General"
Configure los campos exactamente como sigue:
* **Nombre:** `BizorSync_DobraERP_SyncTask`
* **Descripción:** `Sincronización periódica automática bidireccional entre Dobra ERP local y Nube.`
* **Cuenta de usuario:** Haga clic en *"Cambiar usuario o grupo..."*, escriba `SYSTEM` (o utilice la cuenta de Administrador del servidor) y presione Aceptar.
* **Opciones de seguridad:**
  - ✅ Seleccione: **"Ejecutar tanto si el usuario inició sesión como si no"**.
  - ✅ Marque la casilla: **"Ejecutar con los privilegios más elevados"** *(evita bloqueos de UAC)*.
* **Configurar para:** Elija `Windows 10 / Windows Server 2016` (o versión correspondiente a su servidor).

---

### Paso 4: Configurar la Pestaña "Desencadenadores" (Triggers)
1. Haga clic en el botón **"Nuevo..."**.
2. **Iniciar la tarea:** Seleccione *"Según una programación"*.
3. **Configuración:** Marque *"Diariamente"*, repetir cada `1` día.
4. **Configuración avanzada:**
   - ✅ Marque la casilla: **"Repetir la tarea cada:"** $\rightarrow$ Escriba o seleccione **`5 minutos`**.
   - **Durante:** Seleccione **`Indefinidamente`**.
   - ✅ Marque la casilla: **"Habilitada"**.
5. Haga clic en **Aceptar**.

---

### Paso 5: Configurar la Pestaña "Acciones" (Action)
1. Haga clic en el botón **"Nuevo..."**.
2. **Acción:** Seleccione *"Iniciar un programa"*.
3. **Programa o script:** Haga clic en *"Examinar..."* y seleccione el ejecutable de la consola publicado:
   ```text
   E:\Next\USWsAppIntegracionAniasco\Publish\USWsSync.Console.exe
   ```
4. > [!CAUTION]
   > **EL PASO CRÍTICO MÁS IMPORTANTE:**
   > En el campo **"Iniciar en (opcional):"**, DEBE ingresar la ruta del directorio contenedor **sin comillas**:
   > ```text
   > E:\Next\USWsAppIntegracionAniasco\Publish
   > ```
   > *Si deja este campo vacío, Windows ejecutará el proceso desde `C:\Windows\System32`, la consola no encontrará su `appsettings.json` ni sus DLLs y fallará.*
5. Haga clic en **Aceptar**.

---

### Paso 6: Configurar la Pestaña "Configuración" (Settings)
Ajuste las políticas de ejecución para evitar procesos duplicados:
* ✅ Marque: **"Permitir que la tarea se ejecute a petición"** *(permite probarla manualmente)*.
* ✅ Marque: **"Detener la tarea si se ejecuta durante más de:"** $\rightarrow$ Configure en **`1 hora`** *(evita procesos colgados en caso de fallas de red)*.
* En la lista inferior **"Si la tarea ya se está ejecutando, debe aplicarse la siguiente regla:"**:
  - Seleccione: **"No iniciar una nueva instancia"** *(garantiza que si una sincronización pesada toma 6 minutos, no se abra una segunda consola compitiendo por la base de datos)*.
* Haga clic en **Aceptar** para guardar la tarea. Si solicita contraseña de administrador, ingrésela.

---

### Paso 7: Comprobar y Probar la Tarea
1. En la lista central del Programador de Tareas, ubique `BizorSync_DobraERP_SyncTask`.
2. Haga clic derecho sobre ella y seleccione **"Ejecutar"**.
3. El estado cambiará brevemente a *"En ejecución"* y luego a *"Listo"*.
4. Verifique el resultado:
   - **"Resultado de la última ejecución":** Debe indicar `El operador o el administrador ha rechazado la solicitud (0x0)` o `Operación completada correctamente (0x0)`.
   - **Verificación en Logs:** Abra `Publish\Logs\Consola\Consola-yyyyMMdd.log` y confirme que registró el inicio y fin del ciclo exitoso.
   - **Verificación en la UI:** Abra `USWsSync.UI.exe`, vaya a la pestaña *"Historial de Auditoría"* y verá la nueva sincronización registrada con su hora exacta y ventana de corte contable.

---

## 5. Preguntas Frecuentes y Solución de Problemas (Troubleshooting)

### P1: Una factura emitida en el ERP local no aparece en la nube. ¿Cómo la audito?
1. Abra `USWsSync.UI.exe` y vaya a **"Historial de Auditoría"**.
2. En el buscador de tabla escriba: `VEN_FACTURAS`.
3. Revise las últimas sincronizaciones:
   - Verifique la **Ventana de Datos**: ¿La fecha y hora de emisión de la factura está dentro del rango *Desde/Hasta* de la sincronización?
   - Si la sincronización está en **"Error"**, haga clic en el botón rojo de la tabla `VEN_FACTURAS` para leer el diagnóstico exacto de SQL Server (por ejemplo: *"Falta cliente relacionado"*, *"Producto no existe en catálogo"* o *"Cédula inválida"*).

### P2: Se cortó la energía o se apagó el servidor durante 2 días. ¿Qué debo hacer?
**No tiene que hacer absolutamente nada manual.** 
Cuando el servidor se encienda y la tarea programada se ejecute:
1. El sincronizador leerá la última fecha de corte confirmada (`lastDateUpdate`).
2. Detectará automáticamente que la ventana de corte tiene 2 días de atraso.
3. Extraerá todos los registros emitidos en esos 2 días en bloques controlados de 500 registros y los enviará a la nube sin saturar la memoria.
4. Al terminar con éxito, avanzará la fecha de corte al momento actual.

### P3: ¿Cómo verificar que la Web API de IIS esté respondiendo antes de sincronizar?
Puede comprobar la conectividad en su navegador web o mediante PowerShell:
```powershell
# Probar API Local
Invoke-RestMethod -Uri "http://192.168.100.242:8484/api/Clientes/listClientes?f1=2026-09-01&f2=2026-09-14&page=1&pageSize=1"

# Probar API Pública (Nube)
Invoke-RestMethod -Uri "http://186.3.193.198:444/api/Clientes/listClientes?f1=2026-09-01&f2=2026-09-14&page=1&pageSize=1"
```
Ambas llamadas deben retornar un objeto JSON con la estructura del modelo contable sin errores 500.

### P4: ¿Dónde consultar los registros de auditoría si la UI no está disponible?
Toda la actividad se almacena en texto plano y en SQLite dentro de la carpeta `Publish\`:
* **Logs de Consola:** `Publish\Logs\Consola\Consola-yyyyMMdd.log`
* **Logs de la Interfaz:** `Publish\Logs\UI\UI-yyyyMMdd.log`
* **Base de Datos SQLite:** Puede abrir `Publish\sync_history.db` con cualquier herramienta gratuita como *DB Browser for SQLite* para ejecutar consultas SQL directas sobre las tablas `sync_runs` y `sync_run_items`.

---

*Manual elaborado y verificado para la arquitectura de sincronización de alto rendimiento de **Dobra ERP**.*