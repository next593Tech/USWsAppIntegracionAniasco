using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using USWsApp.Services;
using USWsLibrary.Models;
using USWsLibrary.Services;

namespace USWsApp.Controllers
{
    //[Authorize(Roles = "QueryDocs")]
    public class EmpleadosController : ApiController
    {
        #region Propiedades y Campos

        EmploymentsServices _objsrv;

        #endregion

        #region Constructores 

        public EmpleadosController()
        {
            _objsrv = new EmploymentsServices();

        }
        #endregion

        #region Acciones  
        [HttpPost]
         public CompositeReturn< EmploymentMinOut>  GetRestrictions(EmploymentFilterInp filter)
        {
            return _objsrv.GetRestrictions(filter);

        }
        #endregion

        #region Integracion Movil
     /*   [HttpPost]
        public PagedList<EMP_EMPLEADO> AllEmployees(GenericFilterInp filter)
        {
            return _objsrv.AllEmployees(filter);

        }




        [HttpPost]
        public PagedList<VEN_CAJEROS> AllEmployeesCashier(GenericFilterInp filter)
        {
            var retorno = _objsrv.ObtenerCajeros(filter);

            return retorno;

        }*/


        #endregion

    }
}