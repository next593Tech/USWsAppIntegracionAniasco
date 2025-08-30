using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using USWsLibrary.Models;
using USWsLibrary.Services;

namespace USWsApp.Controllers
{
    //[Authorize(Roles = "QueryDocs")]
    public class OrdenesController : ApiController
    {

        #region Propiedades y Campos
        OrdenesServices _objsrv;
        #endregion

        #region Constructores 
        public OrdenesController()
        {
            _objsrv = new OrdenesServices();

        }
        #endregion

        #region Acciones  

       /* [HttpPost]
        public SimpleReturn Grabar(VEN_ORDENES_DTO Orden)
        {
            return _objsrv.Grabar(Orden);
        }

        [HttpPost]
        public SimpleReturn GrabarOff(VEN_ORDENES_DTO Orden)
        {
            return _objsrv.GrabarOff(Orden);
        }


        [HttpGet]
        public NextIDOutputDto NextID()
        {
            var retorno = _objsrv.NextID();

            return retorno;
        }

        [HttpGet]
        public VEN_ORDENES_DTO Recuperar(String OrdenId, int OptionId)
        {
            return _objsrv.Recuperar(OrdenId, OptionId);

        }


        [HttpGet]
        public List<VEN_ORDENES_DT_DTO_PRINT> RecuperarImprimir(String OrdenId)
        {
            return _objsrv.RecuperarImprimir(OrdenId);

        }




        [HttpGet]
        public ErrorItem Eliminar(String OrdenId)
        {
            return _objsrv.Eliminar(OrdenId);

        }


        [HttpPost]
        public PagedList<VEN_ORDENES_MIN_DTO> ListaXFiltro(FiltroPedido Filtro)
        {
            return _objsrv.ListaXFiltro(Filtro);

        }




        [HttpPost]
        public PagedList<VEN_OPCIONES> TodasOpciones(GenericFilterInp filter)
        {
            return _objsrv.ObtenerVenOpciones(filter);

        }*/


        #endregion
    }





}
