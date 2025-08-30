using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class FAC_Filter
    {
        
        public DateTime FechaDesde { get; set; }
 
        public DateTime FechaHasta { get; set; }

        public string ClienteID { get; set; }

        public string SucursalID { get; set; }

        public string Unidades { get; set; }

        public string Categoria { get; set; }

        public string Producto { get; set; }



    }
}