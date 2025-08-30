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
using USWsLibrary.Models;
using USWsLibrary.Common;
using USWsApp.Services;
using USWsLibrary.Services;

namespace USWsApp.Controllers
{
   // [Authorize(Roles = "QueryDocs")]
     public class AbonosController : ApiController
    {
        #region Propiedades y campos
            AbonosServices _objAbono;
        #endregion

        #region Constructores
            public AbonosController()
            {
                _objAbono = new AbonosServices();
            }
        #endregion

        #region Acciones            

        [HttpGet]
        public NextIDOutputDto NextPaymentID() 
                {
                    var retorno = _objAbono.NextPaymentID();

                    return retorno;
                }



        [HttpPost]
        public SimpleReturn Grabar(AbonoCabDto abono)
            {
                return _objAbono.Grabar(abono);
            }


        [HttpPost]
        public SimpleReturn GrabarOffline(AbonoCabDto abono)
        {
            return _objAbono.GrabarOffline(abono);
        }




        
        [HttpGet]
        public AbonoCargaOut Recuperar(String AbonoId)
        {
            return _objAbono.Recuperar(AbonoId);

        }


        [HttpGet]
        public AbonoReporteOut RecuperarImprimir(String AbonoId)
        {
            return _objAbono.RecuperarImprimir(AbonoId);

        }


        [HttpPost]
        public PagedList<AbonoMinOut> ListaXFiltro(AbonoFiltroInp Filtro)
        {
            return _objAbono.ListaXFiltro(Filtro);

        }

        #endregion

    }
}