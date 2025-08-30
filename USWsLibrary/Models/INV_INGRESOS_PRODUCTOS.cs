using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class INV_INGRESOS_PRODUCTOS
    {
        [XmlAttribute]
        public int Id { get; set; }
 
        [XmlAttribute]
        public int CabId { get; set; }

        [XmlAttribute]
        public string BodegaId { get; set; }

        [XmlAttribute]
        public string ProductoId { get; set; }

        [XmlAttribute]
        public string NombreProducto { get; set; }
 
        [XmlAttribute]
        public int Cantidad { get; set; }
 
        [XmlAttribute]
        public decimal Costo { get; set; }
 
        [XmlAttribute]
        public decimal Total { get; set; }
    }
}