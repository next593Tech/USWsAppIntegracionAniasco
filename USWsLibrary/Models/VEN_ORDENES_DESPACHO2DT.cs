using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USWsLibrary.Models
{
	public class VEN_ORDENES_DESPACHO2DT
	{
		public string ID { get; set; }
		public string OrderID { get; set; }
		public string ProductoID { get; set; }

		public string ProductoCodigo { get; set; }
		public string ProductoName { get; set; }
		public decimal Cantidad { get; set; }
		public string Descripcion { get; set; }
		public string Empaque { get; set; }
	}
}
