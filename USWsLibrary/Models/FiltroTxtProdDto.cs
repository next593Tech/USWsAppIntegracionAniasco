using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models 
{
    public class FiltroTxtProdDto
    {
        public int PageNumber { get; set; }
          
        public int PageSize { get; set; }

        public string Texto { get; set; }

        public string VendedorId { get; set; } 

        public string ClaseCliente { get; set; }

    }
}