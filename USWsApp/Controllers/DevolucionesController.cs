using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using USWsLibrary.Models;
using USWsLibrary.Services;

namespace USWsApp.Controllers
{
   // [Authorize(Roles = "QueryDocs")]
    public class DevolucionesController: ApiController
    {
        DevolucionesServices _objsrv;

        public DevolucionesController()
        {
            _objsrv = new DevolucionesServices();
        }

        [HttpPost]
        public CompositeReturn<FAC_DEV_PRODUCTO_CB> GetDevProductsCab(DevProductsFilter filter)
        {
            return _objsrv.GetDevProductsCab(filter);
        }

        [HttpPost]
        public CompositeReturn<List<FAC_DEV_PRODUCTO_DT>> GetDevProductsDet(DevProductsFilter filter)
        {
            return _objsrv.GetDevProductsDet(filter);
        }


        [HttpPost]
        public SimpleReturn Grabar_CliCreditos(CLI_CREDITOS_DTO filter)
        {
            return _objsrv.Grabar_CliCreditos(filter);
        }

        [HttpPost]
        public CompositeReturn<FAC_DEV_PRODUCTO_DTO> UpdateFacturaDevuelto(UpdateDevueltoFilter filter)
        {
            return _objsrv.UpdateFacturaDevuelto(filter);
        }

        [HttpPost]
        public PagedList<Croquis> GetCroquis()
        {
            return _objsrv.GetCroquis();
        }

        [HttpPost]
        public PagedList<VEN_FACTURA> GetFactura(GenericFilterInp filter)
        {
            return _objsrv.GetFactura(filter);
        }

        [HttpPost]
        public PagedList<VEN_FACTURA_DT> GetFacturaDt(GenericFilterInp filter)
        {
            return _objsrv.GetFacturaDt(filter);
        }

        [HttpPost]
        public SimpleReturn UpdateFacturaDT(VEN_FACTURA_DT_LIST Input)
        {
            return _objsrv.UpdateFacturaDT(Input);
        }

    }
}