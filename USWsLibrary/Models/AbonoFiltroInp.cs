using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models
{
    public class AbonoFiltroInp
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public string ClienteNombre { get; set; }

        public string VendedorUsername { get; set; }

        public int Dias { get; set; }
    }
}