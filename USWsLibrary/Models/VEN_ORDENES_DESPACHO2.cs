using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USWsLibrary.Models
{
	public class VEN_ORDENES_DESPACHO2
	{
		public string ID { get; set; }
		
		public string ClientID { get; set; }
		public string ClientName { get; set; }
		public decimal Total { get; set; }
		public decimal Subtotal { get; set; }
		public decimal Iva { get; set; }
	}
}
