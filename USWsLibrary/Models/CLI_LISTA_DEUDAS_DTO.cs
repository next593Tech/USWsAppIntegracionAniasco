using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models
{
    public class CLI_LISTA_DEUDAS_DTO 
    {
        public PagedList<CLI_DEUDA_DTO> Lista { get; set; }
        public double Total  { get; set; }

    }
}