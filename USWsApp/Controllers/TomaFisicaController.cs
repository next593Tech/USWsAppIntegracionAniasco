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
using USWsLibrary.Services;

namespace USWsApp.Controllers 
{
   // [Authorize(Roles = "QueryDocs")]
    public class TomaFisicaController : ApiController
    {

        #region Propiedades y Campos
        TomaFisicaServices _objTFsrv;
        #endregion

        #region Constructores 
        public TomaFisicaController()
        {
            _objTFsrv = new TomaFisicaServices();

        }
        #endregion

        #region Acciones  
      

        [HttpPost]
        //[Authorize(Roles = "QueryDocs")]
        public SimpleReturn Grabar(INV_CONTEO_INPUT Input)
        {
            return _objTFsrv.Grabar(Input);
        }
         

        //[Authorize(Roles = "QueryDocs")]
        [HttpPost]
        public PagedList<INV_FISICO_DTO> ListadoOrdenes(GenericFilterInp Filter)
        { 
            var retorno = _objTFsrv.ListadoOrdenes(Filter);
            return retorno;
        }

        //[Authorize(Roles = "QueryDocs")]
        [HttpGet]
        public int prueba ()
        {
            return 5;
        }

         

        #endregion



    }
}