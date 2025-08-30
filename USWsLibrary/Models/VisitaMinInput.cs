using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USWsLibrary.Models
{
    public class VisitaMinInput
    {
        public string ClienteID { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public DateTime FechaHora { get; set; }     
        public string VendedorID { get; set; }
        public string Nota { get; set; }
    }
}
