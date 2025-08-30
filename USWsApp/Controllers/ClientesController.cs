using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;

using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using System.Web.Http.Description;

using USWsLibrary.Services;
using USWsLibrary.ModelDobraDatabase;
using USWsLibrary.Models;

namespace USWsApp.Controllers
{
    //[Authorize(Roles = "QueryDocs")]


    public class ClientesController : ApiController
    {

        #region Propiedades y Campos
        ClientesServices _objclisrv;
        #endregion

        #region Constructores 
        public ClientesController()
        {
            _objclisrv = new ClientesServices();

        }
        #endregion

        #region Acciones  
        //Muestra todos en general, los nombres de clientes.
        //--------------------------------------------------


        /*    [HttpPost]
            public SimpleReturn Grabar(ClienteMinInputDto Input)
            {
                return _objclisrv.Grabar(Input);
            }



            [HttpPost]
            public SimpleReturn GrabarClienteNuevo(ClienteMinInputDtoGrabar Input)
            {
                return _objclisrv.GrabarClienteNuevo(Input);
            }


            [HttpPost]
            public SimpleReturn ActualizaGeolocalizacion(ClienteMinInputDtoGeo Input)
            {
                return _objclisrv.ActualizaGeolocalizacion(Input);
            }

            [HttpPost]
            public SimpleReturn GrabarVisita(VisitaMinInput Input)
            {
                return _objclisrv.GrabarVisita(Input);
            }

            [HttpPost]
            public PagedList<ClienteMinOutputDto> Lista(ClienteFiltroInputDto Filtro)
            {

                var retorno = _objclisrv.Lista(Filtro);
                return retorno;
            }

      

            [HttpPost]
            public PagedList<CLI_CLIENTE> AllCustomersWithoutSeller(GenericFilterInp Filter)
            {
                var retorno = _objclisrv.AllCustomersWithoutSeller(Filter);
                return retorno;
            }




            [HttpGet]
            public ClienteOutputDto ObtenerXRuc(ErrorSave Ruc)
            {
                var retorno = _objclisrv.ObtenerXRuc(Ruc);
                return retorno;
            }




            [HttpPost]
            public PagedList<CLI_DEUDA_DTO> Deudas(CLI_DEUDA_FILTRO_DTO filtro)
            {
                var retorno = _objclisrv.Deudas(filtro);
                return retorno;

            }




            [HttpPost]
            public PagedList<CLI_DEUDA_DTO> AllDebsVendor(GenericFilterInp filtro)
            {
                var retorno = _objclisrv.DeudasVendedor(filtro);
                return retorno;

            }


            [HttpPost]
            public PagedList<CLI_DEUDA_DTO> SaveDeudas(CLI_DEUDA_FILTRO_DTO filtro)
            {
                var retorno = _objclisrv.Deudas(filtro);
                return retorno;


            }*/

        // [Authorize(Roles = "QueryDocs")]
      /*  [HttpPost]
        public PagedList<CLI_CLIENTE> AllCustomers(GenericFilterInpCliente Filter)
        {
            var retorno = _objclisrv.AllCustomers(Filter);
            return retorno;
        }*/

        [HttpPost]
        public ErrorSave saveCliente(USWsLibrary.Models.PagedList<CLI_CLIENTES> clients)
        {
            return _objclisrv.saveClient(clients);
        }
        [HttpPost]
        public ErrorSave update(CLI_CLIENTES clients)
        {
            return _objclisrv.updateClient(clients);
        }
        [HttpPost]
        public USWsLibrary.Models.PagedList<CLI_CLIENTES> listClient(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listClient(lastUpdate, lastUpdate2);

        }

        [HttpPost]
        public USWsLibrary.Models.PagedList<INV_PRODUCTOS> listProducts(DateTime lastUpdate, DateTime lastUpdate2)
        {
            return _objclisrv.listProducts(lastUpdate,lastUpdate2);

        }

        [HttpPost]
        public USWsLibrary.Models.PagedList<INV_PRODUCTOS> listProductsErroId(ErrorSave errorSave)
        {
            return _objclisrv.listProductsByIDerror(errorSave);

        }

        [HttpPost]
        public USWsLibrary.Models.PagedList<INV_PRODUCTOS_EMPAQUES> listProductsEmpaqueErrorID(ErrorSave errorSave)
        {
            return _objclisrv.listProductsEmpaqueByIDerror(errorSave);

        }

        public USWsLibrary.Models.PagedList<INV_PRODUCTOS_PRECIOS> listProductsPreciosErrorID(ErrorSave errorSave)
        {
            return _objclisrv.listProductoPrecioByIDerror(errorSave);

        }



        public ErrorSave saveProducto(USWsLibrary.Models.PagedList<INV_PRODUCTOS> prod)
        {
            return _objclisrv.saveProducts(prod);
        }



        [HttpPost]
        public USWsLibrary.Models.PagedList<INV_PRODUCTOS_EMPAQUES> listPackagesProducts(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listPackagesProducts( lastUpdate, lastUpdate2);
        }


        [HttpPost]

        public ErrorSave savePackageProductos(USWsLibrary.Models.PagedList<INV_PRODUCTOS_EMPAQUES> products)
        {
            return _objclisrv.savePackageProductos(products);

        }

        [HttpPost]
        public USWsLibrary.Models.PagedList<INV_PRODUCTOS_PRECIOS> listPriceProducts(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listPriceProducts( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savePriceProducts(USWsLibrary.Models.PagedList<INV_PRODUCTOS_PRECIOS> productsPrice)
        {

            return _objclisrv.savePriceProducts(productsPrice);
        }

        [HttpPost]
        public USWsLibrary.Models.PagedList<INV_COMBOS> listComboProducts(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listComboProducts( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveCombos(USWsLibrary.Models.PagedList<INV_COMBOS> combos)
        {
            return _objclisrv.saveCombos(combos);
        }

        [HttpPost]
        public USWsLibrary.Models.PagedList<INV_COMBOS_COMPONENTES> listComboComponentesProducts(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listComboComponentesProducts( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveComboComponente(USWsLibrary.Models.PagedList<INV_COMBOS_COMPONENTES> comboComponentes)
        {
            return _objclisrv.saveComboComponente(comboComponentes);
        }


        /*[HttpPost]
     public USWsLibrary.Models.PagedList<INV_PD_BODEGA_STOCK> listPdBodegaStock(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listPdBodegaStock( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savePdBodegaStock(USWsLibrary.Models.PagedList<INV_PD_BODEGA_STOCK> bodegaStock)
        {
            return _objclisrv.savePdBodegaStock(bodegaStock);
        }*/

        [HttpPost]

        public USWsLibrary.Models.PagedList<INV_PRECIOS> listInvPrecios(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listInvPrecios( lastUpdate, lastUpdate2);
        }


        [HttpPost]
        public ErrorSave saveInvPrecio(USWsLibrary.Models.PagedList<INV_PRECIOS> precios)
        {
            return _objclisrv.saveInvPrecio(precios);
        }

        [HttpPost]

        public USWsLibrary.Models.PagedList<INV_PRECIOS_DT> listInvPreciosDt(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listInvPreciosDt( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveInvPrecioDt(USWsLibrary.Models.PagedList<INV_PRECIOS_DT> precios)
        {
            return _objclisrv.saveInvPrecioDt(precios);
        }

        [HttpPost]
        public PagedList<INV_PRODUCTOS_STOCK> listInvProductsStock(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listInvProductsStock( lastUpdate, lastUpdate2);

        }

        [HttpPost]
        public ErrorSave saveInvProductsStock(USWsLibrary.Models.PagedList<INV_PRODUCTOS_STOCK> precios)
        {
            return _objclisrv.saveInvProductsStock(precios);
        }

        [HttpPost]
        public PagedList<INV_RUBROS> listInvRubros(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listInvRubros( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveInvRubros(PagedList<INV_RUBROS> precios)
        {
            return _objclisrv.saveInvRubros(precios);
        }


        [HttpPost]
        public PagedList<ACC_CUENTAS> listAccCuentas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAccCuentas( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveAccCuentas(PagedList<ACC_CUENTAS> cuentas)
        {
            return _objclisrv.saveAccCuentas(cuentas);
        }

        [HttpPost]
        public USWsLibrary.Models.PagedList<EMP_EMPLEADOS> listEmployess(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listEmployess( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveEmployees(PagedList<EMP_EMPLEADOS> employess)
        {
                return _objclisrv.saveEmployess(employess);
        }

        [HttpPost]
        public PagedList<BAN_BANCOS> listBancos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listBanks( lastUpdate, lastUpdate2);


        }


        [HttpPost]

        public ErrorSave saveBancos(PagedList<BAN_BANCOS> bancos)
        {
            return _objclisrv.saveBanks(bancos);
        }

        [HttpPost]
        public PagedList<CLI_RUBROS> listCliRubros(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listCliRubros( lastUpdate, lastUpdate2);

        }

        [HttpPost]
        public ErrorSave saveCliRubros(PagedList<CLI_RUBROS> cliRubros)
        {
            return _objclisrv.saveCliRubros(cliRubros);
        }

        #endregion
        [HttpPost]

        public PagedList<INV_EMPAQUES> listInvPackages(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listInvPackages( lastUpdate, lastUpdate2);
        }

        [HttpPost]

        public ErrorSave saveInvPackages(PagedList<INV_EMPAQUES> empaques)
		{
            return _objclisrv.saveInvPackages(empaques);
        }

        [HttpPost]
        public PagedList<SEG_PERFILES> listSegProfiles(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listSegProfiles( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveSegProfiles(PagedList<SEG_PERFILES> segPerfiles)
        {
            return _objclisrv.saveSegProfiles(segPerfiles);
        }

        [HttpPost]
        public PagedList<SEG_RECURSOS> listSegRecursos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listSegRecursos( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveSegRecursos(PagedList<SEG_RECURSOS> segRecursos)
        {
            return _objclisrv.saveSegRecursos(segRecursos);
        }

        [HttpPost]
        public PagedList<SEG_USUARIOS> listSegUsuarios(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listSegUsuarios( lastUpdate, lastUpdate2);

        }

        [HttpPost]
        public ErrorSave saveSegUsuarios(PagedList<SEG_USUARIOS> segUsuarios)
        {
            return _objclisrv.saveSegUsuarios(segUsuarios);
        }

        [HttpPost]
        public PagedList<SIS_DIVISIONES> listSisDivisiones(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listSisDivisiones( lastUpdate, lastUpdate2);

        }

        [HttpPost]

        public ErrorSave saveDivisiones(PagedList<SIS_DIVISIONES> sisDivisiones)
        {
            return _objclisrv.saveDivisiones(sisDivisiones);
        }


        [HttpPost]
        public PagedList<SIS_PARAMETROS> listSisParametros(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listSisParametros(lastUpdate,lastUpdate2);

        }

        [HttpPost]

        public ErrorSave saveSisParametros(PagedList<SIS_PARAMETROS> sisParametros)
        {
            return _objclisrv.saveSisParametros(sisParametros);
        }


        [HttpPost]
        public PagedList<SIS_SUCURSALES> listSisSucursales(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listSisSucursales( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveSisSucursales(PagedList<SIS_SUCURSALES> sisSucrsales)
        {
            return _objclisrv.saveSisSucursales(sisSucrsales);
        }

        [HttpPost]
        public PagedList<SRI_SECUENCIAL> listSriSecuencial(DateTime lastUpdate,DateTime lastUpdate2)
        {

            return _objclisrv.listSriSecuencial( lastUpdate, lastUpdate2);
        }


        [HttpPost]
        public ErrorSave saveSriSecuencial(PagedList<SRI_SECUENCIAL> sriSecuencial)
        {
            return _objclisrv.saveSriSecuencial(sriSecuencial);
        }

        [HttpPost]

        public PagedList<SEG_PERFILES_RECURSOS> listSegPerfilesRecursos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listSegPerfilesRecursos( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveSegPerfilesRecursos(PagedList<SEG_PERFILES_RECURSOS> perfilesRecuros)
        {
            return _objclisrv.saveSegPerfilesRecursos(perfilesRecuros);
        }
        [HttpPost]
        public PagedList<ACC_ASIENTOS> listAccAsientos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAccAsientos( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveAccAsientos(PagedList<ACC_ASIENTOS> accAsientos)
        {
            return _objclisrv.saveAccAsientos(accAsientos);
        }

        [HttpPost]
        public PagedList<ACC_ASIENTOS_DT> listAccAsientosDt(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAccAsientosDt( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveAccAsientosDt(PagedList<ACC_ASIENTOS_DT> accAsientosDT)
        {
            return _objclisrv.saveAccAsientosDt(accAsientosDT);
        }

        [HttpPost]
        public PagedList<BAN_INGRESOS> listBanIngresos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listBanIngresos( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveBanIngresos(PagedList<BAN_INGRESOS> banIngresos)
        {
            return _objclisrv.saveBanIngresos(banIngresos);
        }


        [HttpPost]
        public PagedList<BAN_INGRESOS_DT> listBanIngresosDt(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listBanIngresosDt( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveBanIngresosDt(PagedList<BAN_INGRESOS_DT> banIngresosDt)
        {
            return _objclisrv.saveBanIngresosDt(banIngresosDt);
        }

        [HttpPost]
        public PagedList<CLI_CLIENTES_DEUDAS> listClientesDeduas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listClientesDeduas( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveClienteDeudas(PagedList<CLI_CLIENTES_DEUDAS> clienteDeudas)
        {
            return _objclisrv.saveClienteDeudas(clienteDeudas);
        }

        [HttpPost]
        public PagedList<USWsLibrary.ModelDobraDatabase.CLI_CREDITOS> listCliCreditos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listCliCreditos( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveCliCreditos(PagedList<USWsLibrary.ModelDobraDatabase.CLI_CREDITOS> cliCreditos)
        {
            return _objclisrv.saveCliCreditos(cliCreditos);
        }


        [HttpPost]
        public PagedList<USWsLibrary.ModelDobraDatabase.CLI_CREDITOS_PRODUCTOS> listCliCreditosProductos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listCliCreditosProductos( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveCliCreditosProductos(PagedList<USWsLibrary.ModelDobraDatabase.CLI_CREDITOS_PRODUCTOS> cliCreditosProductos)
        {
            return _objclisrv.saveCliCreditosProductos(cliCreditosProductos);
        }

      [HttpPost]
        public PagedList<INV_PRODUCTOS_CARDEX> listInvProductosCardex(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listInvProductosCardex( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveInvProductosCardex(PagedList<INV_PRODUCTOS_CARDEX> invProductosCardex)
        {
            return _objclisrv.saveInvProductosCardex(invProductosCardex);
        }

       /* [HttpPost]
        public PagedList<POS_CIERRES_CAJA> listPosCierresCaja(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listPosCierresCajas( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savePosCierresCajas(PagedList<POS_CIERRES_CAJA> posCierresCajas)
        {
            return _objclisrv.savePosCierresCajas(posCierresCajas);
        }*/

        [HttpPost]
        public PagedList<POS_CIERRES> listPosCierre(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listPosCierres( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savePosCierres(PagedList<POS_CIERRES> posCierres)
        {
            return _objclisrv.savePosCierres(posCierres);
        }

        [HttpPost]
        public PagedList<VEN_FACTURAS> listVenFacturas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listVenFacturas( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveVenFacturas(PagedList<VEN_FACTURAS> venFacturas)
        {
            return _objclisrv.saveVenFacturas(venFacturas);
        }

        [HttpPost]
        public PagedList<VEN_FACTURAS_DT> listVenFacturasDt(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listVenFacturasDt( lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveVenFacturasDt(PagedList<VEN_FACTURAS_DT> venFacturasDT)
        {
            return _objclisrv.saveVenFacturasDt(venFacturasDT);
        }

        [HttpPost]
        public PagedList<BAN_INGRESOS_DEUDAS> listBanIngresosDeudas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listBanIngresoDeuda(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveBanIngresoDeuda(PagedList<BAN_INGRESOS_DEUDAS> banIngresosDeudas)
        {
            return _objclisrv.saveBanIngresoDeuda(banIngresosDeudas);
        }

        [HttpPost]
        public PagedList<BAN_BANCOS_CARDEX> listBanBancosCardex(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listBanBancosCardex(lastUpdate,lastUpdate2);
        }


        [HttpPost]
        public ErrorSave saveBanBancosCardex(PagedList<BAN_BANCOS_CARDEX> banIngresosDeudas)
        {
            return _objclisrv.saveBanBancosCardex(banIngresosDeudas);
        }

        [HttpPost]
        public PagedList<BAN_DEPOSITOS> listBanDepositos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listBanDepositos(lastUpdate,lastUpdate2);
        }


        [HttpPost]
        public ErrorSave saveBanDepositos(PagedList<BAN_DEPOSITOS> banDepositos)
        {
            return _objclisrv.saveBanDepositos(banDepositos);
        }


        [HttpPost]
        public PagedList<BAN_DEPOSITOS_DT> listBanDepositosDt(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listBanDepositosDt(lastUpdate,lastUpdate2);
        }


        [HttpPost]
        public ErrorSave saveBanDepositosDt(PagedList<BAN_DEPOSITOS_DT> banDepositosDt)
        {
            return _objclisrv.saveBanDepositosDt(banDepositosDt);
        }
/*
        [HttpPost]
        public PagedList<BAN_DEPOSITOS_PAPELETAS> listBanDepositoPapeletas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listBanDepositoPapeletas(lastUpdate,lastUpdate2);
        }


        [HttpPost]
        public ErrorSave saveBanDepositosPapeletas(PagedList<BAN_DEPOSITOS_PAPELETAS> banPapeletas)
        {
            return _objclisrv.saveBanDepositoPapeletas(banPapeletas);
        }
        */
        [HttpPost]
        public PagedList<COM_FACTURAS> listComFacturas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listComFacturas(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveComFacturas(PagedList<COM_FACTURAS> comFacturas)
        {
            return _objclisrv.saveComFacturas(comFacturas);
        }


        [HttpPost]
        public PagedList<COM_FACTURAS_DT> listComFacturasDt(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listComFacturasDt(lastUpdate,lastUpdate2);
        }


        [HttpPost]
        public ErrorSave saveComFacturasDt(PagedList<COM_FACTURAS_DT> comFacturasDt)
        {
            return _objclisrv.saveComFacturasDt(comFacturasDt);
        }


        [HttpPost]
        public PagedList<COM_FACTURAS_PAGOS> listComFacturasPagos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listComFacturasPagos(lastUpdate,lastUpdate2);
        }


        [HttpPost]
        public ErrorSave saveComFacturasPagos(PagedList<COM_FACTURAS_PAGOS> comFacturasPagos)
        {
            return _objclisrv.saveComFacturasPagos(comFacturasPagos);
        }


        [HttpPost]
        public PagedList<ACR_RETENCIONES> listAcrRetenciones(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAcrRetenciones(lastUpdate,lastUpdate2);
        }


        [HttpPost]
        public ErrorSave saveAcrRetenciones(PagedList<ACR_RETENCIONES> acrRetenciones)
        {
            return _objclisrv.saveAcrRetenciones(acrRetenciones);
        }

        [HttpPost]
        public PagedList<ACR_RETENCIONES_DEUDAS> listAcrRetencionesDeudas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAcrRetencionesDeudas(lastUpdate,lastUpdate2);
        }


        [HttpPost]
        public ErrorSave saveAcrRetencionesDeudas(PagedList<ACR_RETENCIONES_DEUDAS> acrRetencionesDeudas)
        {
            return _objclisrv.saveAcrRetencionesDeudas(acrRetencionesDeudas);
        }


        [HttpPost]
        public PagedList<ACR_RETENCIONES_DT> listAcrRetencionesDt(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAcrRetencionesDt(lastUpdate,lastUpdate2);
        }


        [HttpPost]
        public ErrorSave saveAcrRetencionesDt(PagedList<ACR_RETENCIONES_DT> acrRetencionesDt)
        {
            return _objclisrv.saveAcrRetencionesDt(acrRetencionesDt);
        }


        [HttpPost]
        public PagedList<ACR_ACREEDORES_DEUDAS> listAcrAcreedoresDeudas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAcrAcreedoresDeudas(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveAcrAcreedoresDeudas(PagedList<ACR_ACREEDORES_DEUDAS> acrAcreedoresDeduas)
        {
            return _objclisrv.saveAcrAcreedoresDeudas(acrAcreedoresDeduas);
        }


        [HttpPost]
        public PagedList<PRV_FACTURAS> listPrvFacturas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listPvrFacturas(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savePrvFacturas(PagedList<PRV_FACTURAS> prvFacturas)
        {
            return _objclisrv.savePvrFacturas(prvFacturas);
        }


        [HttpPost]
        public PagedList<PRV_FACTURAS_DT> listPrvFacturasDt(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listPvrFacturasDt(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savePrvFacturasDt(PagedList<PRV_FACTURAS_DT> prvFacturasDt)
        {
            return _objclisrv.savePvrFacturasDt(prvFacturasDt);
        }


        [HttpPost]
        public PagedList<PRV_FACTURASCTA_DT> listPrvFacturasCtaDt(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listPvrFacturasCtaDt(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savePrvFacturasCtaDt(PagedList<PRV_FACTURASCTA_DT> prvFacturasCtaDt)
        {
            return _objclisrv.savePvrFacturasCtaDt(prvFacturasCtaDt);
        }


        [HttpPost]
        public PagedList<EMP_ROLES> listEmpRoles(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listEmpRoles(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveEmpRoles(PagedList<EMP_ROLES> empRoles)
        {
            return _objclisrv.saveEmpRoles(empRoles);
        }


        [HttpPost]
        public PagedList<EMP_ROLES_EMPLEADOS> listEmpRolesEmpleados(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listEmpRolesEmpleados(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveEmpRolesEmpleados(PagedList<EMP_ROLES_EMPLEADOS> empRolesEmpleados)
        {
            return _objclisrv.saveEmpRolesEmpleados(empRolesEmpleados);
        }



        [HttpPost]
        public PagedList<EMP_ROLES_RUBROS> listEmpRolesRubros(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listEmpRolesRubros(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveEmpRolesRubros(PagedList<EMP_ROLES_RUBROS> prvFacturasCtaDt)
        {
            return _objclisrv.saveEmpRolesRubros(prvFacturasCtaDt);
        }



        [HttpPost]
        public PagedList<EMP_EMPLEADOS_DEUDAS> listEmpEmpleadosDeudas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listEmpEmpleadosDeudas(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveEmpEmpleadosDeudas(PagedList<EMP_EMPLEADOS_DEUDAS> prvFacturasCtaDt)
        {
            return _objclisrv.saveEmpEmpleadosDeudas(prvFacturasCtaDt);
        }



        [HttpPost]
        public PagedList<EMP_EMPLEADOS_HORAS> listEmpEmpleadoHoras(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listEmpEmpleadosHoras(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveEmpEmpleadoHoras(PagedList<EMP_EMPLEADOS_HORAS> prvFacturasCtaDt)
        {
            return _objclisrv.saveEmpEmpleadosHoras(prvFacturasCtaDt);
        }


        [HttpPost]
        public PagedList<EMP_DEBITOS> listEmpDebitos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listEmpDebitos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveEmpDebitos(PagedList<EMP_DEBITOS> prvFacturasCtaDt)
        {
            return _objclisrv.saveEmpDebitos(prvFacturasCtaDt);
        }



        [HttpPost]
        public PagedList<EMP_DEBITOS_RUBROS> listEmpDebitosRubros(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listEmpDebitosRubros(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveEmpDebitosRubros(PagedList<EMP_DEBITOS_RUBROS> prvFacturasCtaDt)
        {
            return _objclisrv.saveEmpDebitosRubros(prvFacturasCtaDt);
        }



        [HttpPost]
        public PagedList<CLI_GRUPOS> listCliGrupos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listCliGrupos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveCliGrupos(PagedList<CLI_GRUPOS> prvFacturasCtaDt)
        {
            return _objclisrv.saveCliGrupos(prvFacturasCtaDt);
        }

        [HttpPost]
        public PagedList<INV_BODEGAS> listInvBodegas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listInvBodegas(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveInvBodegas(PagedList<INV_BODEGAS> invBodegas)
        {
            return _objclisrv.saveInvBodegas(invBodegas);
        }

      /*  [HttpPost]
        public PagedList<INV_PRODUCTOS_EXHIBICION> listInvProductosExhibicion(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listInvProductosExhibicion(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveInvProductosExhibicion(PagedList<INV_PRODUCTOS_EXHIBICION> invProductoExhibicion)
        {
            return _objclisrv.saveInvProductosExhibicion(invProductoExhibicion);
        }*/

        [HttpPost]
        public PagedList<ACR_CREDITOS> listAcrCreditos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAcrCreditos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveAcrCreditos(PagedList<ACR_CREDITOS> acrCreditos)
        {
            return _objclisrv.saveAcrCreditos(acrCreditos);
        }

        [HttpPost]
        public PagedList<ACR_CREDITOS_DEUDAS> listAcrCreditosDeudas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAcrCreditosDeudas(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveAcrCreditosDeudas(PagedList<ACR_CREDITOS_DEUDAS> acrCreditosDeudas)
        {
            return _objclisrv.saveAcrCreditosDeudas(acrCreditosDeudas);
        }

        [HttpPost]
        public PagedList<ACR_CREDITOS_RUBROS> listAcrCreditosRubros(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAcrCreditosRubros(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveAcrCreditosRubros(PagedList<ACR_CREDITOS_RUBROS> acrCreditosDeudasRubros)
        {
            return _objclisrv.saveAcrCreditosRubros(acrCreditosDeudasRubros);
        }

        [HttpPost]
        public PagedList<ACR_DEBITOS> listAcrDebitos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAcrDebitos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveAcrDebitos(PagedList<ACR_DEBITOS> acrDebitos)
        {
            return _objclisrv.saveAcrDebitos(acrDebitos);
        }

        [HttpPost]
        public PagedList<ACR_DEBITOS_DEUDAS> listAcrDebitosDeudas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAcrDebitosDeudas(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveAcrDebitosDeudas(PagedList<ACR_DEBITOS_DEUDAS> acrDebitosDeudas)
        {
            return _objclisrv.saveAcrDebitosDeudas(acrDebitosDeudas);
        }


        [HttpPost]
        public PagedList<ACR_DEBITOS_RUBROS> listAcrDebitosRubros(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAcrDebitoRubros(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveAcrDebitosRubros(PagedList<ACR_DEBITOS_RUBROS> acrDebitosDeudasRubros)
        {
            return _objclisrv.saveAcrDebitoRubros(acrDebitosDeudasRubros);
        }


        [HttpPost]
        public PagedList<ACR_DEBITOS_PRODUCTOS> listAcrDebitosProductos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAcrDebitosProductos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveAcrDebitosProductos(PagedList<ACR_DEBITOS_PRODUCTOS> acrDebitosProductos)
        {
            return _objclisrv.saveAcrDebitosProductos(acrDebitosProductos);
        }


        [HttpPost]
        public PagedList<ACR_RECIBOS> listAcrRecibos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAcrRecibos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveAcrRecibos(PagedList<ACR_RECIBOS> acrRecivos)
        {
            return _objclisrv.saveAcrRecibos(acrRecivos);
        }


        [HttpPost]
        public PagedList<ACR_RECIBOS_DT> listAcrRecibosDt(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAcrRecibosDt(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveAcrRecibosDt(PagedList<ACR_RECIBOS_DT> acrRecibosDt)
        {
            return _objclisrv.saveAcrRecibosDt(acrRecibosDt);
        }


        [HttpPost]
        public PagedList<ACR_RECIBOS_DEUDAS> listAcrRecbiosDeudas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAcrReciboDeudas(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveAcrRecbiosDeudas(PagedList<ACR_RECIBOS_DEUDAS> acrRecivosDeudas)
        {
            return _objclisrv.saveAcrReciboDeudas(acrRecivosDeudas);
        }


        [HttpPost]
        public PagedList<BAN_DEBITOS> listBanDebitos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listBanDebitos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveBanDebitos(PagedList<BAN_DEBITOS> banDebitos)
        {
            return _objclisrv.saveBanDebitos(banDebitos);
        }


        [HttpPost]
        public PagedList<BAN_DEBITOS_CUENTAS> listBanDebitosCuentas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listBanDebitosCuentas(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveBanDebitosCuentas(PagedList<BAN_DEBITOS_CUENTAS> banDebitosCeuntas)
        {
            return _objclisrv.saveBanDebitosCuentas(banDebitosCeuntas);
        }



        [HttpPost]
        public PagedList<BAN_EGRESOS> listBanEgresos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listbanEgresos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveBanEgresos(PagedList<BAN_EGRESOS> banEgresos)
        {
            return _objclisrv.savebanEgresos(banEgresos);
        }

        [HttpPost]
        public PagedList<BAN_EGRESOS_ANEXOS> listBanEgresosAnexos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listbanEgresosAnexos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveBanEgresosAnexos(PagedList<BAN_EGRESOS_ANEXOS> banEgresosAnexos)
        {
            return _objclisrv.savebanEgresosAnexos(banEgresosAnexos);
        }


        [HttpPost]
        public PagedList<BAN_EGRESOS_ANTICIPOS> listBanEgresosAnticipos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listbanEgresosAnticipos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveBanEgresosAnticipos(PagedList<BAN_EGRESOS_ANTICIPOS> banEgresosAnticipos)
        {
            return _objclisrv.savebanEgresosAnticipos(banEgresosAnticipos);
        }



        [HttpPost]
        public PagedList<BAN_EGRESOS_CUENTAS> listBanEgresosCuentas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listbanEgresosCuentas(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveBanEgresosCuentas(PagedList<BAN_EGRESOS_CUENTAS> banEgresosCuentas)
        {
            return _objclisrv.savebanEgresosCuentas(banEgresosCuentas);
        }



        [HttpPost]
        public PagedList<BAN_EGRESOS_DEUDAS> listBanEgresosDeudas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listbanEgresosDeudas(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveBanEgresosDeudas(PagedList<BAN_EGRESOS_DEUDAS> banEgresosDeudas)
        {
            return _objclisrv.savebanEgresosDeudas(banEgresosDeudas);
        }


        [HttpPost]
        public PagedList<BAN_EGRESOS_DT> listbanEgresosDt(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listbanEgresosDt(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savebanEgresosDt(PagedList<BAN_EGRESOS_DT> acrRecivos)
        {
            return _objclisrv.savebanEgresosDt(acrRecivos);
        }

        [HttpPost]
        public PagedList<BAN_EGRESOS_PAGOS> listbanEgresosPagos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listbanEgresosPagos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savebanEgresosPagos(PagedList<BAN_EGRESOS_PAGOS> banEgresosPagos)
        {
            return _objclisrv.savebanEgresosPagos(banEgresosPagos);
        }



        [HttpPost]
        public PagedList<BAN_INGRESOS_CUENTAS> listbanIngresosCuentas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listbanIngresosCuentas(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savebanIngresosCuentas(PagedList<BAN_INGRESOS_CUENTAS> banIngresosCuentas)
        {
            return _objclisrv.savebanIngresosCuentas(banIngresosCuentas);
        }

        /*[HttpPost]
        public PagedList<BAN_INGRESOS_TARJETAS> listbanIngresosTarjetas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listbanIngresosTarjetas(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savebanIngresosTarjetas(PagedList<BAN_INGRESOS_TARJETAS> banIngresosTarjetas)
        {
            return _objclisrv.savebanIngresosTarjetas(banIngresosTarjetas);
        }*/


        [HttpPost]
        public PagedList<BAN_PAPELETAS> listbanPapeletas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listbanPapeletas(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savebanPapeletas(PagedList<BAN_PAPELETAS> banPapeletas)
        {
            return _objclisrv.savebanPapeletas(banPapeletas);
        }


        [HttpPost]
        public PagedList<BAN_TRANSFERENCIAS> listbanTransferencias(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listbanTransferencias(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savebanTransferencias(PagedList<BAN_TRANSFERENCIAS> banTransferencias)
        {
            return _objclisrv.savebanTransferencias(banTransferencias);
        }

        [HttpPost]
        public PagedList<BAN_TRANSFERENCIAS_DT> listbanTransferenciasDt(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listbanTransferenciasDt(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savebanTransferenciasDt(PagedList<BAN_TRANSFERENCIAS_DT> banTrasnferenciaDt)
        {
            return _objclisrv.savebanTransferenciasDt(banTrasnferenciaDt);
        }


        [HttpPost]
        public PagedList<VEN_FACTURAS_PAGOS> listvenFacturasPagos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listvenFacturasPagos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savevenFacturasPagos(PagedList<VEN_FACTURAS_PAGOS> venFacturasPagos)
        {
            return _objclisrv.savevenFacturasPagos(venFacturasPagos);
        }



        [HttpPost]
        public PagedList<INV_EGRESOS> listinvEgresos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listinvEgresos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveinvEgresos(PagedList<INV_EGRESOS> invEgresos)
        {
            return _objclisrv.saveinvEgresos(invEgresos);
        }

        [HttpPost]
        public PagedList<INV_EGRESOS_RUBROS> listinvEgresosRubros(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listinvEgresosRubros(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveinvEgresosRubros(PagedList<INV_EGRESOS_RUBROS> invEgresosRubros)
        {
            return _objclisrv.saveinvEgresosRubros(invEgresosRubros);
        }

        [HttpPost]
        public PagedList<INV_EGRESOS_PRODUCTOS> listinvEgresosProductos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listinvEgresosProductos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveinvEgresosProductos(PagedList<INV_EGRESOS_PRODUCTOS> invEgresosProductos)
        {
            return _objclisrv.saveinvEgresosProductos(invEgresosProductos);
        }


        [HttpPost]
        public PagedList<USWsLibrary.ModelDobraDatabase.INV_INGRESOS> listinvIngresos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listinvIngresos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveinvIngresos(PagedList<USWsLibrary.ModelDobraDatabase.INV_INGRESOS> invIngresos)
        {
            return _objclisrv.saveinvIngresos(invIngresos);
        }

        [HttpPost]
        public PagedList<USWsLibrary.ModelDobraDatabase.INV_INGRESOS_RUBROS> listinvIngresosRubros(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listinvIngresosRubros(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveinvIngresosRubros(PagedList<USWsLibrary.ModelDobraDatabase.INV_INGRESOS_RUBROS> invIngresosRubros)
        {
            return _objclisrv.saveinvIngresosRubros(invIngresosRubros);
        }


        [HttpPost]
        public PagedList<USWsLibrary.ModelDobraDatabase.INV_INGRESOS_PRODUCTOS> listinvIngresosProductos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listinvIngresosProductos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveinvIngresosProductos(PagedList<USWsLibrary.ModelDobraDatabase.INV_INGRESOS_PRODUCTOS> invIngresosProductos)
        {
            return _objclisrv.saveinvIngresosProductos(invIngresosProductos);
        }


      /*  [HttpPost]
        public PagedList<INV_PROMOCIONES> listInvPromociones(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listinvPromociones(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveInvPromociones(PagedList<INV_PROMOCIONES> invPromociones)
        {
            return _objclisrv.saveinvPromociones(invPromociones);
        }*/

        /*[HttpPost]
        public PagedList<INV_PROMOCIONES_DT> listInvPromocionesDt(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listInvPromocionesDt(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveInvPromocionesDt(PagedList<INV_PROMOCIONES_DT> invPromocionesDt)
        {
            return _objclisrv.saveInvPromocionesDt(invPromocionesDt);
        }*/

        /*[HttpPost]
        public PagedList<INV_PROMOCIONES_DT2> listInvPromocionesDt2(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listInvPromocionesDt2(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveInvPromocionesDt2(PagedList<INV_PROMOCIONES_DT2> invPromocionesDT2)
        {
            return _objclisrv.saveInvPromocionesDt2(invPromocionesDT2);
        }*/


        [HttpPost]
        public PagedList<INV_TRANSFERENCIAS> listInvTransferencias(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listinvTransferencias(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveInvTransferencias(PagedList<INV_TRANSFERENCIAS> invTransferencias)
        {
            return _objclisrv.saveinvTransferencias(invTransferencias);
        }

        [HttpPost]
        public PagedList<INV_TRANSFERENCIAS_DT> listInvTransferenciasDt(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listinvTransferenciasDt(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveInvTransferenciasDt(PagedList<INV_TRANSFERENCIAS_DT> invTransferenciasDt)
        {
            return _objclisrv.saveinvTransferenciasDt(invTransferenciasDt);
        }

        [HttpPost]
        public PagedList<POS_TRANSFERENCIAS> listPosTransferencias(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listPosTransferencias(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savePosTransferencias(PagedList<POS_TRANSFERENCIAS> posTransferencias)
        {
            return _objclisrv.savePosTransferencias(posTransferencias);
        }

        [HttpPost]
        public PagedList<POS_TRANSFERENCIAS_DT> listPosTransferenciasDt(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listPosTransferenciasDt(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savePosTransferenciasDt(PagedList<POS_TRANSFERENCIAS_DT> posTransferenciasDt)
        {
            return _objclisrv.savePosTransferenciasDt(posTransferenciasDt);
        }

        [HttpPost]
        public PagedList<CLI_CREDITOS_DEUDAS> listCliCreditosDeudas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listCliCreditosDuedas(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveCliCreditosDeudas(PagedList<CLI_CREDITOS_DEUDAS> cliCreditosDeudas)
        {
            return _objclisrv.saveCliCreditosDuedas(cliCreditosDeudas);
        }

        [HttpPost]
        public PagedList<CLI_CREDITOS_RUBROS> listCliCreditosRubros(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listCliCreditosRubros(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveCliCreditosRubros(PagedList<CLI_CREDITOS_RUBROS> cliCreditosRubros)
        {
            return _objclisrv.saveCliCreditosRubros(cliCreditosRubros);
        }

        [HttpPost]
        public PagedList<CLI_DEBITOS> listCliDebitos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listCliDebitos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveCliDebitos(PagedList<CLI_DEBITOS> cliDebitos)
        {
            return _objclisrv.saveCliDebitos(cliDebitos);
        }

        [HttpPost]
        public PagedList<CLI_DEBITOS_RUBROS> listCliDebitosRubros(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listCliDebitosRubros(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveCliDebitosRubros(PagedList<CLI_DEBITOS_RUBROS> cliDebitos)
        {
            return _objclisrv.saveCliDebitosRubros(cliDebitos);
        }

        [HttpPost]
        public PagedList<CLI_RETENCIONES> listCliRetenciones(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listCliRetenciones(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveCliRetenciones(PagedList<CLI_RETENCIONES> cliDebitos)
        {
            return _objclisrv.saveCliRetenciones(cliDebitos);
        }

        [HttpPost]
        public PagedList<CLI_RETENCIONES_DEUDAS> listCliRetencionesDeudas(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listCliRetencionesDeudas(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveCliRetencionesDeudas(PagedList<CLI_RETENCIONES_DEUDAS> cliDebitos)
        {
            return _objclisrv.saveCliRetencionesDeudas(cliDebitos);
        }


        [HttpPost]
        public PagedList<CLI_RETENCIONES_DT> listCliRetencionesDt(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listCliRetencionesDt(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveCliRetencionesDt(PagedList<CLI_RETENCIONES_DT> cliDebitos)
        {
            return _objclisrv.saveCliRetencionesDt(cliDebitos);
        }


        [HttpPost]
        public PagedList<ACR_ACREEDORES> listAcrAcreedores(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listAcrAcreedores(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveAcrAcreedores(PagedList<ACR_ACREEDORES> cliDebitos)
        {
            return _objclisrv.saveAcrAcreedores(cliDebitos);
        }


        [HttpPost]
        public PagedList<INV_GRUPOS> listInvGrupos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listInvGrupos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveInvGrupos(PagedList<INV_GRUPOS> cliDebitos)
        {
            return _objclisrv.saveInvGrupos(cliDebitos);
        }

        /*
        [HttpPost]
        public PagedList<PRV_FACTURAS_PAGOS> listPrvFacturasPagos(DateTime lastUpdate,DateTime lastUpdate2)
        {
            return _objclisrv.listPrvFacturasPagos(lastUpdate,lastUpdate2);
        }

        [HttpPost]
        public ErrorSave savePrvFacturasPagos(PagedList<PRV_FACTURAS_PAGOS> cliDebitos)
        {
            return _objclisrv.savePrvFacturasPagos(cliDebitos);
        }*/


        [HttpPost]
        public PagedList<BAN_CREDITOS> lisBanCreditos(DateTime lastUpdate, DateTime lastUpdate2)
        {
            return _objclisrv.listBanCreditos(lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveBanCreditos(PagedList<BAN_CREDITOS> banCreditos)
        {
            return _objclisrv.saveBanCreditos(banCreditos);
        }

        [HttpPost]
        public PagedList<BAN_CREDITOS_CUENTAS> lisBanCreditosCuentas(DateTime lastUpdate, DateTime lastUpdate2)
        {
            return _objclisrv.listBanCreditosCuentas(lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveBanCreditosCuentas(PagedList<BAN_CREDITOS_CUENTAS> banCreditosCuentas)
        {
            return _objclisrv.saveBanCreditosCuentas(banCreditosCuentas);
        }

        [HttpPost]
        public PagedList<ORG_BUZONES> listOrgBuzones(DateTime lastUpdate, DateTime lastUpdate2)
        {
            return _objclisrv.listOrgBuzones(lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveOrgBuzones(PagedList<ORG_BUZONES> orgBuzones)
        {
            return _objclisrv.saveOrgBuzones(orgBuzones);
        }



        [HttpPost]
        public PagedList<ORG_DOCUMENTOS> listOrgDocumentos(DateTime lastUpdate, DateTime lastUpdate2)
        {
            return _objclisrv.listOrgDocumentos(lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveOrgDocumentos(PagedList<ORG_DOCUMENTOS> orgDocumentos)
        {
            return _objclisrv.saveOrgDocumentos(orgDocumentos);
        }


        [HttpPost]
        public PagedList<ORG_TAREAS> listOrgTareas(DateTime lastUpdate, DateTime lastUpdate2)
        {
            return _objclisrv.listOrgTareas(lastUpdate, lastUpdate2);
        }

        [HttpPost]
        public ErrorSave saveOrgTareas(PagedList<ORG_TAREAS> orgTareas)
        {
            return _objclisrv.saveOrgTareas(orgTareas);
        }


    }
}