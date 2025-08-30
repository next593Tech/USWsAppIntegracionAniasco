using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USWsLibrary.Models
{
    public class VEN_FACTURA
    {

		public string FactID { get; set; }
		public string Secuencia { get; set; }
		public string ClienteID { get; set; }
		public DateTime Fecha { get; set; }
		public string DivisionID { get; set; }	
		public string SucursaslID { get; set; }  
		public string Detalle { get; set; }
	}
}
