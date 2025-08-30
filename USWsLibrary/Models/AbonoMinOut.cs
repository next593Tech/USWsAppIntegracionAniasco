using System;
using System.Collections.Generic;
 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USWsLibrary.Models
{
    public class AbonoMinOut
    {
        public string Id { get; set; } 

        public string NumDcto { get; set; }
        public string Concepto { get; set; }
        public string Tipo { get; set; }
        public DateTime Fecha { get; set; }

        public string Asiento { get; set; }
        public string Estado { get; set; }

        public string Codigo { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteEmail { get; set; }

        public string NumRecibo { get; set; }
        public string Cuenta { get; set; }
        public string NoCheque { get; set; }
        public string Girador { get; set; }
        public string DivisaID { get; set; } 
        public decimal Valor { get; set; }
         
    }
}
