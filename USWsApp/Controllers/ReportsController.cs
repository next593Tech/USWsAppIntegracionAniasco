using NexoMobile;
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
    public class ReportsController : ApiController
    {

        ReportsServices _objsrv;

        public ReportsController()
        {
            _objsrv = new ReportsServices();

        }



        [HttpPost]
        public CompositeReturn<List<VEN_CUADRE_DE_CAJADT>> ObtenerCuadreCaja(FAC_Filter filtro)
        {
            return _objsrv.ObtenerCuadreCaja(filtro);
        }


        [HttpPost]
        public CompositeReturn<List<VEN_REPORTE_VENTAS>> Obtener_Ventas(FAC_Filter filtro)
        {
            return _objsrv.Obtener_Ventas(filtro);
        }


    }
}
