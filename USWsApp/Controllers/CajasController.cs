using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using USWsLibrary.Models;
using USWsLibrary.Services;

namespace USWsApp.Controllers
{
    //[Authorize(Roles = "QueryDocs")]
    public class CajasController : ApiController
    {
        CajasServices _objCaja;

        public CajasController(){

            _objCaja = new CajasServices();
        }
        


        [HttpPost]
        public SimpleReturn GrabarOffline(CIERRECAJA_DTO caja)
        {
            return _objCaja.GrabarOffline(caja);
        }


    }
}