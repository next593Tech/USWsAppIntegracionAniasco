using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using USWsLibrary.Models;
using USWsLibrary.Services;

namespace USWsApp.Controllers
{
  //  [Authorize(Roles = "QueryDocs")]
    public class PasajeController : ApiController
    {
        PasajesServices objserv;

        #region Constructores
        public PasajeController()
        {
            objserv = new PasajesServices();

        }
        





        [HttpPost]
        public PagedList<BusesDto>ObtenerBuses()
        {
            return objserv.ListadoBuses();

        }


        [HttpPost]
        public PagedList<FrecuenciaDto> ObtenerFrecuencias()
        {
            return objserv.ListadoFrecuencias();

        }

        [HttpPost]
        public PagedList<TarifarioDto> ObtenerTarifarios()
        {
            return objserv.ListadoTarifario();

        }

        [HttpPost]
        public PagedList<RutasDto> ObtenerRutas()
        {
            return objserv.ListadoRutas();

        }

        [HttpPost]
        public PagedList<RutasDetalleDto> ObtenerRutasDetalle()
        {
            return objserv.ListadoRutasDetalle();

        }


        [HttpPost]
        public ReturnReservation GrabarReservacion(ReservaDto reserva)
        {
            var retorno = objserv.GrabarReservacion(reserva);
            return retorno;

        }

      


        [HttpPost]
        public SimpleReturn ActualizarReservacionOnline(FilterIdReservation reserva)
        {
            var retorno = objserv.ActualizarReservacionOnline(reserva);
            return retorno;

        }


        [HttpPost]
        public ReturnReservation EliminarReservacion(ReservaDto reserva)
        {
            var retorno = objserv.EliminarReservacion(reserva);
            return retorno;

        }

        [HttpPost]
        public PagedList<ReservaDto> ObtenerReservaciones(FilterReservation reservacion)
        {
            return objserv.ListadoReservaciones(reservacion);

        }


        


        #endregion
    }
}