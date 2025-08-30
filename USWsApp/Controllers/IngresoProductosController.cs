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
 //  [Authorize(Roles = "QueryDocs")]
    public class IngresoProductosController : ApiController
    {

        IngresoProductosServices _objsrv;

        public IngresoProductosController()
        {
            _objsrv = new IngresoProductosServices();

        }


        [HttpPost]
        public SimpleReturn SaveProductsIncome(INV_INGRESOS_DTO producto)
        {
            var retorno = _objsrv.SaveOffProductos(producto);
            return retorno;

        }





    }
}