using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models 
{
    public class CLI_CREDITOS_PRODUCTOS
    {
         
        [XmlAttribute]
        public int IdCrePro { get; set; }

        [XmlAttribute]
        public int IdCre { get; set; }        

        [XmlAttribute]
        public string FacturaId { get; set; }

        [XmlAttribute]
        public string FacturaIdDet { get; set; }

        [XmlAttribute]
        public string ProductoId { get; set; }

        [XmlAttribute]
        public string BodegaId { get; set; }

        [Column(TypeName = "numeric")]
        [XmlAttribute]
        public decimal Cantidad { get; set; }

        [Column(TypeName = "numeric")] 
        [XmlAttribute]
        public decimal Facturado { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Precio { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Costo { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal")]
        [XmlAttribute]
        public decimal TasaDescuento { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Descuento { get; set; }

        [Column(TypeName = "decimal")]
        [XmlAttribute]
        public decimal TasaImpuesto { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Impuesto { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Total { get; set; }

        [XmlAttribute]
        public string Empaque { get; set; }  
    }
}
