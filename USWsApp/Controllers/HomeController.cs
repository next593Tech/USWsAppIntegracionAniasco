using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
 
using System.Web.Mvc;
using USWsLibrary.Common;
using USWsLibrary.Models;
using USWsLibrary.Services;

namespace USWsApp.Controllers
{
    public class HomeController : Controller
    {
        private DataModel db = new DataModel();

        public ActionResult Index()
        {
            return View();
        }

        //[EnableCors(origins: "http://www.example.com", headers: "*", methods: "*")]
        [Authorize(Roles = "Test")]
        public ActionResult Test()
        {

            //ProductosServices _productoServices = new ProductosServices();


            //GenericFilterInp a = new GenericFilterInp();

            //a.PageNumber = 1;
            //a.PageSize = 100;
            //a.genericstring = "mrodas";


            ////var data = _productoServices.AllProductsUnitary(a);

            ////int b;


            //OrdenesServices _opcionServices = new OrdenesServices();



            //var data = _opcionServices.ObtenerVenOpciones(a);

            //int b;









            int Cantidad = 0;
            string mensaje = "";

            try
            {
                Cantidad = db.ColeccionDocumentosDto.ToList().Count;
                mensaje = "Consulta exitosa a la base de datos";
            }
            catch (Exception eee)
            {
                mensaje = "Error de comunicación con el repositorio de datos";
                throw eee;
            }

            ViewBag.Message = mensaje;

            return View();
        }





        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}