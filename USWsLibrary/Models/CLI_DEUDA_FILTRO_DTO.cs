using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace USWsLibrary.Models 
{


    public class CLI_DEUDA_FILTRO_DTO
    {
        public int PageNumber { get; set; }        
        public int PageSize { get; set; }

        public string RucCliente { get; set; }

        public bool Vencidos { get; set; }
        public bool Cheques { get; set; }
        public bool ValorFavor { get; set; }       

    }
}