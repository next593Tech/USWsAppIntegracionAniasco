using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models 
{
    public class FiltroDto
    {
        public int PageNumber { get; set; }

        public string CodCliente { get; set; } 

        public int PageSize { get; set; }

        public string FechaDesde { get; set; }

        public string FechaHasta { get; set; }

        public string TipoDoc { get; set; }

        public string CodDocumento { get; set; }

        public int TenantId { get; set; }

    }
}