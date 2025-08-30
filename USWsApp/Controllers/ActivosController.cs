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
    public class ActivosController : ApiController
    {

        ActivosService _objActivoSrv;

        public ActivosController()
        {
            _objActivoSrv = new ActivosService();

        }

        [HttpPost]
        public List<ACT_ACTIVO> ListActivo(string EmpleadoID)
        {
            var retorno = _objActivoSrv.ListActivos(EmpleadoID);
            return retorno;
        }

        [HttpPost]
        public List<ACT_ASIGNACIONES> ListAsignaciones(string EmpleadoID)
        {
            var retorno = _objActivoSrv.ListAsignaciones(EmpleadoID);
            return retorno;
        }

        [HttpPost]

       public SimpleReturn UpdateEstado(AsignacionInput input)
       {
            var retonro = _objActivoSrv.UpadteEstado(input);
            return retonro;
       }


        [HttpPost]
       public SimpleReturn UpdateActivo(ActivoInput input)
        {
            var retorno = _objActivoSrv.UpdateActivo(input);
            return retorno;
        } 
    }
}