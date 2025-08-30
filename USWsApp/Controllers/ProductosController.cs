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
   //[Authorize(Roles = "QueryDocs")]
    public class ProductosController : ApiController
    {

        #region Propiedades y campos
        ProductosServices _objC;
        #endregion


        #region Constructores
        public ProductosController()
        {
            _objC = new ProductosServices();

        }
        #endregion


        

    }
}