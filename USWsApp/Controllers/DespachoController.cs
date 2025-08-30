using System;
using System.Web.Http;
using USWsLibrary.Models;
using USWsLibrary.Services;

namespace USWsApp.Controllers
{
    //[Authorize(Roles = "QueryDocs")]
    public class DespachoController : ApiController
    {
        private DespachoService _Despacho;

        public DespachoController()
        {
            _Despacho = new DespachoService();

        }

        [HttpPost]
        public PagedList<VEN_ORDENESDESPACHO_DTO> GetAllOrdenesAsync(GenericFilterInp filter)
        {
            //InputFilter filter = new InputFilter();
            //filter.PageNumber = 1;
            //filter.PageSize = 1;
            return _Despacho.GetAllOrdenesAsync(filter);

        }

        [HttpPost]
        public PagedList<VEN_ORDENESDESPACHO_DT_DTO> GetAllOrdenesDtAsync(GenericFilterInp filter)
        {
            //InputFilter filter = new InputFilter();
            //filter.PageNumber = 1;
            //filter.PageSize = 1;
            return _Despacho.GetAllOrdenesDtAsync(filter);
        }

        [HttpPost]
        public PagedList<VEN_ENTREGAS_MIN_DTO> GetAllEntregasAsync(GenericFilterInp filter)
        {
            return _Despacho.GetAllEntregasAsync(filter);
        }

        [HttpPost]
        public PagedList<VEN_ENTREGAS_MIN_DT_DTO> GetAllEntregasDtAsync(GenericFilterInp filter)
        {
            return _Despacho.GetAllEntregasDtAsync(filter);
        }

        [HttpPost]
        public SimpleReturn InsertEntregasOrdenes(VEN_ORD_ENT_DTO input)
        {
            return _Despacho.InsertEntregasOrdenes(input);
        }

        [HttpPost]
        public SimpleReturn UpdateDeliveryStatus(VEN_ENTREGAS_UPDATE_DTO input)
        {

            return _Despacho.UpdateDeliveryStatus(input);
        }


        [HttpPost]
        public SimpleReturn UpdateDeliveryDate(VEN_ORDENESDESPACHO_UPDATE_DTO input)
        {

            return _Despacho.UpdateDateDelivery(input);
        }
        [HttpPost]

        public PagedList<VEN_ORDENES_DESPACHO2> getOrdersPending(){
            return _Despacho.getOrdersPending();
        }

        [HttpPost]
        public PagedList<VEN_ORDENES_DESPACHO2DT> getOrdersPendingDT(GenericFilterInp input)
		{
            return _Despacho.getOrdersPendingDT(input);
        }





    }
}
