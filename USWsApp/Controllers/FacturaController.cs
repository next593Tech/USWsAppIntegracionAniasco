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
   // [Authorize(Roles = "QueryDocs")]
    public class FacturaController : ApiController
    {

        FacturasServices _objsrv;

        public FacturaController()
        {
            _objsrv = new FacturasServices();

        }


        [HttpPost]
        public SimpleReturn SaveInvoice(FAC_DTO Factura)
        {
            var retorno = _objsrv.GrabarOff(Factura);
            return retorno;
        }      



    }
}