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
using USWsLibrary.Services;

namespace USWsApp.Controllers
{
    //[Authorize(Roles = "QueryDocs")]
    public class ParametrosController : ApiController 
    {
        #region Propiedades y Campos
        ParametrosServices objserv;
        #endregion
           
        #region Constructores 
        public ParametrosController()
        {
            objserv = new ParametrosServices();

        }
        #endregion

        #region Operaciones: Listas basadas en parámetros generales del sistema

        [HttpGet]
        public List<ParametroOutputDto> TiposAbono( )
        {

            return objserv.TiposAbono();
        }

        [HttpGet]
        public List<ParametroOutputDto> TiposCambio()
        {

            return objserv.TiposCambio();
        }


        [HttpGet]
        public List<ParametroOutputDto> Bancos()
        {
            return objserv.Bancos();
        }


        [HttpGet]
        public List<ParametroOutputDto> Terminos()
        {
            return objserv.Terminos();
        }

        [HttpGet]
        public List<ParametroOutputDto> TiposIdentificacion()
        {
            return objserv.TiposIdentificacion();
        }


        [HttpGet]
        public List<ParametroOutputDto> List(string ParamCode)
        {
            return objserv.List(ParamCode);
        }

        #endregion




        #region Integracion Movil

        [HttpPost]
        public PagedList<SIS_PARAMETRO> AllParameters(GenericFilterInp Filter)
        {
             
            var retorno = objserv.AllParameters(Filter);
            return retorno;
        }

        public PagedList<SIS_PARAMETRO> ParametersFilter(GenericFilterInp Filter)
        {

            var retorno = objserv.ParametersFilter(Filter);
            return retorno;
        }
        #endregion

    }
}