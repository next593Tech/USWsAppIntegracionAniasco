using System.Collections.Generic;
using USWsLibrary.ModelDobraDatabase;

namespace USWsSync.Core.Registry
{
    public static class TableRegistry
    {
        // =========================================================================
        // DESCARGA (Nube -> Local): 32 Tablas ordenadas por dependencias
        // =========================================================================
        public static IReadOnlyList<TableSyncDefinition> DownloadTables { get; } = new List<TableSyncDefinition>
        {
            // 1. Sistema y Seguridad Base
            TableSyncDefinition.Create<SIS_PARAMETROS>("SIS_PARAMETROS", "listSisParametros", "saveSisParametros", SyncModule.Sistema),
            TableSyncDefinition.Create<SIS_DIVISIONES>("SIS_DIVISIONES", "listSisDivisiones", "saveDivisiones", SyncModule.Sistema),
            TableSyncDefinition.Create<SIS_SUCURSALES>("SIS_SUCURSALES", "listSisSucursales", "saveSisSucursales", SyncModule.Sistema),
            TableSyncDefinition.Create<SIS_ZONAS>("SIS_ZONAS", "listSisZonas", "saveSisZonas", SyncModule.Sistema),
            TableSyncDefinition.Create<SRI_SECUENCIAL>("SRI_SECUENCIAL", "listSriSecuencial", "saveSriSecuencial", SyncModule.Sistema),
            TableSyncDefinition.Create<SEG_RECURSOS>("SEG_RECURSOS", "listSegRecursos", "saveSegRecursos", SyncModule.Sistema),
            TableSyncDefinition.Create<SEG_USUARIOS>("SEG_USUARIOS", "listSegUsuarios", "saveSegUsuarios", SyncModule.Sistema),
            TableSyncDefinition.Create<SEG_PERFILES>("SEG_PERFILES", "listSegProfiles", "saveSegProfiles", SyncModule.Sistema),
            TableSyncDefinition.Create<SEG_PERFILES_RECURSOS>("SEG_PERFILES_RECURSOS", "listSegPerfilesRecursos", "saveSegPerfilesRecursos", SyncModule.Sistema),
            TableSyncDefinition.Create<ORG_BUZONES>("ORG_BUZONES", "listOrgBuzones", "saveOrgBuzones", SyncModule.Sistema),
            TableSyncDefinition.Create<ORG_DOCUMENTOS>("ORG_DOCUMENTOS", "listOrgDocumentos", "saveOrgDocumentos", SyncModule.Sistema),
            TableSyncDefinition.Create<ORG_TAREAS>("ORG_TAREAS", "listOrgTareas", "saveOrgTareas", SyncModule.Sistema),

            // 2. Cuentas y Terceros Maestros
            TableSyncDefinition.Create<ACC_CUENTAS>("ACC_CUENTAS", "listAccCuentas", "saveAccCuentas", SyncModule.Cuentas),
            TableSyncDefinition.Create<EMP_EMPLEADOS>("EMP_EMPLEADOS", "listEmployess", "saveEmployees", SyncModule.Personas),
            TableSyncDefinition.Create<BAN_BANCOS>("BAN_BANCOS", "listBancos", "saveBancos", SyncModule.Bancos),
            TableSyncDefinition.Create<CLI_RUBROS>("CLI_RUBROS", "listCliRubros", "saveCliRubros", SyncModule.Personas),
            TableSyncDefinition.Create<CLI_CLIENTES>("CLI_CLIENTES", "listClient", "saveCliente", SyncModule.Personas),
            TableSyncDefinition.Create<CLI_CLIENTES_DEUDAS>("CLI_CLIENTES_DEUDAS", "listClientesDeduas", "saveClienteDeudas", SyncModule.Personas, 500),

            // 3. Inventarios y Artículos Maestros
            TableSyncDefinition.Create<INV_EMPAQUES>("INV_EMPAQUES", "listInvPackages", "saveInvPackages", SyncModule.Inventarios),
            TableSyncDefinition.Create<INV_RUBROS>("INV_RUBROS", "listInvRubros", "saveInvRubros", SyncModule.Inventarios),
            TableSyncDefinition.Create<INV_PRODUCTOS>("INV_PRODUCTOS", "listProducts", "saveProducto", SyncModule.Inventarios),
            TableSyncDefinition.Create<INV_PRODUCTOS_EMPAQUES>("INV_PRODUCTOS_EMPAQUES", "listPackagesProducts", "savePackageProductos", SyncModule.Inventarios),
            TableSyncDefinition.Create<INV_PRECIOS>("INV_PRECIOS", "listInvPrecios", "saveInvPrecio", SyncModule.Inventarios),
            TableSyncDefinition.Create<INV_PRECIOS_DT>("INV_PRECIOS_DT", "listInvPreciosDt", "saveInvPrecioDt", SyncModule.Inventarios, 500),
            TableSyncDefinition.Create<INV_PRODUCTOS_PRECIOS>("INV_PRODUCTOS_PRECIOS", "listPriceProducts", "savePriceProducts", SyncModule.Inventarios, 500),
            TableSyncDefinition.Create<INV_COMBOS>("INV_COMBOS", "listComboProducts", "saveCombos", SyncModule.Inventarios),
            TableSyncDefinition.Create<INV_COMBOS_COMPONENTES>("INV_COMBOS_COMPONENTES", "listComboComponentesProducts", "saveComboComponente", SyncModule.Inventarios),
            TableSyncDefinition.Create<INV_PROMOCIONES>("INV_PROMOCIONES", "listInvPromociones", "saveInvPromociones", SyncModule.Inventarios),
            TableSyncDefinition.Create<INV_PROMOCIONES_DT>("INV_PROMOCIONES_DT", "listInvPromocionesDt", "saveInvPromocionesDt", SyncModule.Inventarios, 500),
            TableSyncDefinition.Create<INV_PROMOCIONES_DT2>("INV_PROMOCIONES_DT2", "listInvPromocionesDt2", "saveInvPromocionesDt2", SyncModule.Inventarios, 500),

            // 4. Movimientos y Transferencias
            TableSyncDefinition.Create<INV_TRANSFERENCIAS>("INV_TRANSFERENCIAS", "listInvTransferencias", "saveInvTransferencias", SyncModule.Movimientos),
            TableSyncDefinition.Create<INV_TRANSFERENCIAS_DT>("INV_TRANSFERENCIAS_DT", "listInvTransferenciasDt", "saveInvTransferenciasDt", SyncModule.Movimientos, 500)
        };

        // =========================================================================
        // SUBIDA (Local -> Nube): 72 Tablas ordenadas estrictamente por dependencias
        // =========================================================================
        public static IReadOnlyList<TableSyncDefinition> UploadTables { get; } = new List<TableSyncDefinition>
        {
            // 1. Sistema y Configuración Base
            TableSyncDefinition.Create<SIS_PARAMETROS>("SIS_PARAMETROS", "listSisParametros", "saveSisParametros", SyncModule.Sistema),
            TableSyncDefinition.Create<SIS_SUCURSALES>("SIS_SUCURSALES", "listSisSucursales", "saveSisSucursales", SyncModule.Sistema),
            TableSyncDefinition.Create<SIS_DIVISIONES>("SIS_DIVISIONES", "listSisDivisiones", "saveDivisiones", SyncModule.Sistema),
            TableSyncDefinition.Create<SRI_SECUENCIAL>("SRI_SECUENCIAL", "listSriSecuencial", "saveSriSecuencial", SyncModule.Sistema),
            TableSyncDefinition.Create<SEG_RECURSOS>("SEG_RECURSOS", "listSegRecursos", "saveSegRecursos", SyncModule.Sistema),
            TableSyncDefinition.Create<SEG_USUARIOS>("SEG_USUARIOS", "listSegUsuarios", "saveSegUsuarios", SyncModule.Sistema),
            TableSyncDefinition.Create<SEG_PERFILES>("SEG_PERFILES", "listSegProfiles", "saveSegProfiles", SyncModule.Sistema),
            TableSyncDefinition.Create<SEG_PERFILES_RECURSOS>("SEG_PERFILES_RECURSOS", "listSegPerfilesRecursos", "saveSegPerfilesRecursos", SyncModule.Sistema),
            TableSyncDefinition.Create<ORG_BUZONES>("ORG_BUZONES", "listOrgBuzones", "saveOrgBuzones", SyncModule.Sistema),
            TableSyncDefinition.Create<ORG_DOCUMENTOS>("ORG_DOCUMENTOS", "listOrgDocumentos", "saveOrgDocumentos", SyncModule.Sistema),
            TableSyncDefinition.Create<ORG_TAREAS>("ORG_TAREAS", "listOrgTareas", "saveOrgTareas", SyncModule.Sistema),

            // 2. Contabilidad
            TableSyncDefinition.Create<ACC_CUENTAS>("ACC_CUENTAS", "listAccCuentas", "saveAccCuentas", SyncModule.Cuentas),
            TableSyncDefinition.Create<ACC_ASIENTOS>("ACC_ASIENTOS", "listAccAsientos", "saveAccAsientos", SyncModule.Cuentas),
            TableSyncDefinition.Create<ACC_ASIENTOS_DT>("ACC_ASIENTOS_DT", "listAccAsientosDt", "saveAccAsientosDt", SyncModule.Cuentas, 500),

            // 3. Empleados y Terceros
            TableSyncDefinition.Create<EMP_EMPLEADOS>("EMP_EMPLEADOS", "listEmployess", "saveEmployees", SyncModule.Personas),
            TableSyncDefinition.Create<EMP_DEBITOS>("EMP_DEBITOS", "listEmpDebitos", "saveEmpDebitos", SyncModule.Personas),
            TableSyncDefinition.Create<EMP_DEBITOS_RUBROS>("EMP_DEBITOS_RUBROS", "listEmpDebitosRubros", "saveEmpDebitosRubros", SyncModule.Personas),
            TableSyncDefinition.Create<EMP_EMPLEADOS_DEUDAS>("EMP_EMPLEADOS_DEUDAS", "listEmpEmpleadosDeudas", "saveEmpEmpleadosDeudas", SyncModule.Personas, 500),

            // 4. Clientes y Cuentas por Cobrar
            TableSyncDefinition.Create<CLI_GRUPOS>("CLI_GRUPOS", "listCliGrupos", "saveCliGrupos", SyncModule.Personas),
            TableSyncDefinition.Create<CLI_RUBROS>("CLI_RUBROS", "listCliRubros", "saveCliRubros", SyncModule.Personas),
            TableSyncDefinition.Create<CLI_CLIENTES>("CLI_CLIENTES", "listClient", "saveCliente", SyncModule.Personas),
            TableSyncDefinition.Create<CLI_CLIENTES_DEUDAS>("CLI_CLIENTES_DEUDAS", "listClientesDeduas", "saveClienteDeudas", SyncModule.Personas, 500),
            TableSyncDefinition.Create<CLI_CREDITOS>("CLI_CREDITOS", "listCliCreditos", "saveCliCreditos", SyncModule.Personas),
            TableSyncDefinition.Create<CLI_CREDITOS_DEUDAS>("CLI_CREDITOS_DEUDAS", "listCliCreditosDeudas", "saveCliCreditosDeudas", SyncModule.Personas, 500),
            TableSyncDefinition.Create<CLI_CREDITOS_RUBROS>("CLI_CREDITOS_RUBROS", "listCliCreditosRubros", "saveCliCreditosRubros", SyncModule.Personas),
            TableSyncDefinition.Create<CLI_CREDITOS_PRODUCTOS>("CLI_CREDITOS_PRODUCTOS", "listCliCreditosProductos", "saveCliCreditosProductos", SyncModule.Personas, 500),
            TableSyncDefinition.Create<CLI_DEBITOS>("CLI_DEBITOS", "listCliDebitos", "saveCliDebitos", SyncModule.Personas),
            TableSyncDefinition.Create<CLI_DEBITOS_RUBROS>("CLI_DEBITOS_RUBROS", "listCliDebitosRubros", "saveCliDebitosRubros", SyncModule.Personas),
            TableSyncDefinition.Create<CLI_RETENCIONES>("CLI_RETENCIONES", "listCliRetenciones", "saveCliRetenciones", SyncModule.Personas),
            TableSyncDefinition.Create<CLI_RETENCIONES_DT>("CLI_RETENCIONES_DT", "listCliRetencionesDt", "saveCliRetencionesDt", SyncModule.Personas, 500),
            TableSyncDefinition.Create<CLI_RETENCIONES_DEUDAS>("CLI_RETENCIONES_DEUDAS", "listCliRetencionesDeudas", "saveCliRetencionesDeudas", SyncModule.Personas, 500),

            // 5. Inventarios (Maestros)
            TableSyncDefinition.Create<INV_BODEGAS>("INV_BODEGAS", "listInvBodegas", "saveInvBodegas", SyncModule.Inventarios),
            TableSyncDefinition.Create<INV_RUBROS>("INV_RUBROS", "listInvRubros", "saveInvRubros", SyncModule.Inventarios),
            TableSyncDefinition.Create<INV_EMPAQUES>("INV_EMPAQUES", "listInvPackages", "saveInvPackages", SyncModule.Inventarios),
            TableSyncDefinition.Create<INV_PRODUCTOS>("INV_PRODUCTOS", "listProducts", "saveProducto", SyncModule.Inventarios),
            TableSyncDefinition.Create<INV_PRODUCTOS_EMPAQUES>("INV_PRODUCTOS_EMPAQUES", "listPackagesProducts", "savePackageProductos", SyncModule.Inventarios),
            TableSyncDefinition.Create<INV_PRECIOS>("INV_PRECIOS", "listInvPrecios", "saveInvPrecio", SyncModule.Inventarios),
            TableSyncDefinition.Create<INV_PRECIOS_DT>("INV_PRECIOS_DT", "listInvPreciosDt", "saveInvPrecioDt", SyncModule.Inventarios, 500),
            TableSyncDefinition.Create<INV_PRODUCTOS_PRECIOS>("INV_PRODUCTOS_PRECIOS", "listPriceProducts", "savePriceProducts", SyncModule.Inventarios, 500),
            TableSyncDefinition.Create<INV_COMBOS>("INV_COMBOS", "listComboProducts", "saveCombos", SyncModule.Inventarios),
            TableSyncDefinition.Create<INV_COMBOS_COMPONENTES>("INV_COMBOS_COMPONENTES", "listComboComponentesProducts", "saveComboComponente", SyncModule.Inventarios),

            // 6. Inventarios (Movimientos y Kardex)
            TableSyncDefinition.Create<INV_INGRESOS>("INV_INGRESOS", "listinvIngresos", "saveinvIngresos", SyncModule.Movimientos),
            TableSyncDefinition.Create<INV_INGRESOS_PRODUCTOS>("INV_INGRESOS_PRODUCTOS", "listinvIngresosProductos", "saveinvIngresosProductos", SyncModule.Movimientos, 500),
            TableSyncDefinition.Create<INV_EGRESOS>("INV_EGRESOS", "listinvEgresos", "saveinvEgresos", SyncModule.Movimientos),
            TableSyncDefinition.Create<INV_EGRESOS_RUBROS>("INV_EGRESOS_RUBROS", "listinvEgresosRubros", "saveinvEgresosRubros", SyncModule.Movimientos),
            TableSyncDefinition.Create<INV_EGRESOS_PRODUCTOS>("INV_EGRESOS_PRODUCTOS", "listinvEgresosProductos", "saveinvEgresosProductos", SyncModule.Movimientos, 500),
            TableSyncDefinition.Create<INV_TRANSFERENCIAS>("INV_TRANSFERENCIAS", "listInvTransferencias", "saveInvTransferencias", SyncModule.Movimientos),
            TableSyncDefinition.Create<INV_TRANSFERENCIAS_DT>("INV_TRANSFERENCIAS_DT", "listInvTransferenciasDt", "saveInvTransferenciasDt", SyncModule.Movimientos, 500),
            TableSyncDefinition.Create<INV_PRODUCTOS_CARDEX>("INV_PRODUCTOS_CARDEX", "listInvProductosCardex", "saveInvProductosCardex", SyncModule.Movimientos, 500),

            // 7. Facturación, POS y Ventas
            TableSyncDefinition.Create<VEN_FACTURAS>("VEN_FACTURAS", "listVenFacturas", "saveVenFacturas", SyncModule.Ventas),
            TableSyncDefinition.Create<VEN_FACTURAS_PAGOS>("VEN_FACTURAS_PAGOS", "listvenFacturasPagos", "savevenFacturasPagos", SyncModule.Ventas, 500),
            TableSyncDefinition.Create<VEN_FACTURAS_DT>("VEN_FACTURAS_DT", "listVenFacturasDt", "saveVenFacturasDt", SyncModule.Ventas, 500),
            TableSyncDefinition.Create<POS_CIERRES>("POS_CIERRES", "listPosCierre", "savePosCierres", SyncModule.Ventas),
            TableSyncDefinition.Create<POS_CIERRES_CAJA>("POS_CIERRES_CAJA", "listPosCierresCaja", "savePosCierresCajas", SyncModule.Ventas),
            TableSyncDefinition.Create<POS_TRANSFERENCIAS>("POS_TRANSFERENCIAS", "listPosTransferencias", "savePosTransferencias", SyncModule.Ventas),
            TableSyncDefinition.Create<POS_TRANSFERENCIAS_DT>("POS_TRANSFERENCIAS_DT", "listPosTransferenciasDt", "savePosTransferenciasDt", SyncModule.Ventas, 500),

            // 8. Compras y Acreedores
            TableSyncDefinition.Create<COM_FACTURAS>("COM_FACTURAS", "listComFacturas", "saveComFacturas", SyncModule.Compras),
            TableSyncDefinition.Create<COM_FACTURAS_DT>("COM_FACTURAS_DT", "listComFacturasDt", "saveComFacturasDt", SyncModule.Compras, 500),
            TableSyncDefinition.Create<COM_FACTURAS_PAGOS>("COM_FACTURAS_PAGOS", "listComFacturasPagos", "saveComFacturasPagos", SyncModule.Compras, 500),
            TableSyncDefinition.Create<PRV_FACTURAS>("PRV_FACTURAS", "listPrvFacturas", "savePrvFacturas", SyncModule.Compras),
            TableSyncDefinition.Create<PRV_FACTURAS_DT>("PRV_FACTURAS_DT", "listPrvFacturasDt", "savePrvFacturasDt", SyncModule.Compras, 500),
            TableSyncDefinition.Create<PRV_FACTURASCTA_DT>("PRV_FACTURAS_CTA_DT", "listPrvFacturasCtaDt", "savePrvFacturasCtaDt", SyncModule.Compras, 500),
            TableSyncDefinition.Create<ACR_RETENCIONES>("ACR_RETENCIONES", "listAcrRetenciones", "saveAcrRetenciones", SyncModule.Compras),
            TableSyncDefinition.Create<ACR_RETENCIONES_DT>("ACR_RETENCIONES_DT", "listAcrRetencionesDt", "saveAcrRetencionesDt", SyncModule.Compras, 500),
            TableSyncDefinition.Create<ACR_RETENCIONES_DEUDAS>("ACR_RETENCIONES_DEUDAS", "listAcrRetencionesDeudas", "saveAcrRetencionesDeudas", SyncModule.Compras, 500),
            TableSyncDefinition.Create<ACR_ACREEDORES_DEUDAS>("ACR_ACREEDORES_DEUDAS", "listAcrAcreedoresDeudas", "saveAcrAcreedoresDeudas", SyncModule.Compras, 500),
            TableSyncDefinition.Create<ACR_CREDITOS>("ACR_CREDITOS", "listAcrCreditos", "saveAcrCreditos", SyncModule.Compras),
            TableSyncDefinition.Create<ACR_CREDITOS_DEUDAS>("ACR_CREDITOS_DEUDAS", "listAcrCreditosDeudas", "saveAcrCreditosDeudas", SyncModule.Compras, 500),
            TableSyncDefinition.Create<ACR_CREDITOS_RUBROS>("ACR_CREDITOS_RUBROS", "listAcrCreditosRubros", "saveAcrCreditosRubros", SyncModule.Compras),
            TableSyncDefinition.Create<ACR_DEBITOS>("ACR_DEBITOS", "listAcrDebitos", "saveAcrDebitos", SyncModule.Compras),
            TableSyncDefinition.Create<ACR_DEBITOS_DEUDAS>("ACR_DEBITOS_DEUDAS", "listAcrDebitosDeudas", "saveAcrDebitosDeudas", SyncModule.Compras, 500),
            TableSyncDefinition.Create<ACR_DEBITOS_RUBROS>("ACR_DEBITOS_RUBROS", "listAcrDebitosRubros", "saveAcrDebitosRubros", SyncModule.Compras),
            TableSyncDefinition.Create<ACR_DEBITOS_PRODUCTOS>("ACR_DEBITOS_PRODUCTOS", "listAcrDebitosProductos", "saveAcrDebitosProductos", SyncModule.Compras, 500),
            TableSyncDefinition.Create<ACR_RECIBOS>("ACR_RECIBOS", "listAcrRecibos", "saveAcrRecibos", SyncModule.Compras),
            TableSyncDefinition.Create<ACR_RECIBOS_DT>("ACR_RECIBOS_DT", "listAcrRecibosDt", "saveAcrRecibosDt", SyncModule.Compras, 500),
            TableSyncDefinition.Create<ACR_RECIBOS_DEUDAS>("ACR_RECIBOS_DEUDAS", "listAcrRecbiosDeudas", "saveAcrRecbiosDeudas", SyncModule.Compras, 500),

            // 9. Bancos y Tesorería
            TableSyncDefinition.Create<BAN_BANCOS>("BAN_BANCOS", "listBancos", "saveBancos", SyncModule.Bancos),
            TableSyncDefinition.Create<BAN_DEBITOS>("BAN_DEBITOS", "listBanDebitos", "saveBanDebitos", SyncModule.Bancos),
            TableSyncDefinition.Create<BAN_DEBITOS_CUENTAS>("BAN_DEBITOS_CUENTAS", "listBanDebitosCuentas", "saveBanDebitosCuentas", SyncModule.Bancos),
            TableSyncDefinition.Create<BAN_DEPOSITOS>("BAN_DEPOSITOS", "listBanDepositos", "saveBanDepositos", SyncModule.Bancos),
            TableSyncDefinition.Create<BAN_DEPOSITOS_DT>("BAN_DEPOSITOS_DT", "listBanDepositosDt", "saveBanDepositosDt", SyncModule.Bancos, 500),
            TableSyncDefinition.Create<BAN_EGRESOS>("BAN_EGRESOS", "listBanEgresos", "saveBanEgresos", SyncModule.Bancos),
            TableSyncDefinition.Create<BAN_EGRESOS_ANEXOS>("BAN_EGRESOS_ANEXOS", "listBanEgresosAnexos", "saveBanEgresosAnexos", SyncModule.Bancos),
            TableSyncDefinition.Create<BAN_EGRESOS_ANTICIPOS>("BAN_EGRESOS_ANTICIPOS", "listBanEgresosAnticipos", "saveBanEgresosAnticipos", SyncModule.Bancos),
            TableSyncDefinition.Create<BAN_EGRESOS_CUENTAS>("BAN_EGRESOS_CUENTAS", "listBanEgresosCuentas", "saveBanEgresosCuentas", SyncModule.Bancos),
            TableSyncDefinition.Create<BAN_EGRESOS_DEUDAS>("BAN_EGRESOS_DEUDAS", "listBanEgresosDeudas", "saveBanEgresosDeudas", SyncModule.Bancos, 500),
            TableSyncDefinition.Create<BAN_EGRESOS_DT>("BAN_EGRESOS_DT", "listbanEgresosDt", "savebanEgresosDt", SyncModule.Bancos, 500),
            TableSyncDefinition.Create<BAN_EGRESOS_PAGOS>("BAN_EGRESOS_PAGOS", "listbanEgresosPagos", "savebanEgresosPagos", SyncModule.Bancos, 500),
            TableSyncDefinition.Create<BAN_INGRESOS>("BAN_INGRESOS", "listBanIngresos", "saveBanIngresos", SyncModule.Bancos),
            TableSyncDefinition.Create<BAN_INGRESOS_CUENTAS>("BAN_INGRESOS_CUENTAS", "listbanIngresosCuentas", "savebanIngresosCuentas", SyncModule.Bancos),
            TableSyncDefinition.Create<BAN_INGRESOS_DEUDAS>("BAN_INGRESOS_DEUDAS", "listBanIngresosDeudas", "saveBanIngresoDeuda", SyncModule.Bancos, 500),
            TableSyncDefinition.Create<BAN_INGRESOS_DT>("BAN_INGRESOS_DT", "listBanIngresosDt", "saveBanIngresosDt", SyncModule.Bancos, 500),
            TableSyncDefinition.Create<BAN_PAPELETAS>("BAN_PAPELETAS", "listbanPapeletas", "savebanPapeletas", SyncModule.Bancos),
            TableSyncDefinition.Create<BAN_INGRESOS_PINPAD>("BAN_INGRESOS_PINPAD", "listBanIngresosPinPad", "saveBanIngresoPinpad", SyncModule.Bancos),
            TableSyncDefinition.Create<BAN_INGRESOS_TARJETAS>("BAN_INGRESOS_TARJETAS", "listbanIngresosTarjetas", "savebanIngresosTarjetas", SyncModule.Bancos),
            TableSyncDefinition.Create<BAN_TRANSFERENCIAS>("BAN_TRANSFERENCIAS", "listbanTransferencias", "savebanTransferencias", SyncModule.Bancos),
            TableSyncDefinition.Create<BAN_TRANSFERENCIAS_DT>("BAN_TRANSFERENCIAS_DT", "listbanTransferenciasDt", "savebanTransferenciasDt", SyncModule.Bancos, 500),
            TableSyncDefinition.Create<BAN_BANCOS_CARDEX>("BAN_BANCOS_CARDEX", "listBanBancosCardex", "saveBanBancosCardex", SyncModule.Bancos, 500)
        };
    }
}
