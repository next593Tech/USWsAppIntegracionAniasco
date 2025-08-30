using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class FAC_DEV_PRODUCTO_DT
    {
        [XmlAttribute]
        public string FacturaIdDet { get; set; }

        [XmlAttribute]
        public string FacturaId { get; set; }

        [XmlAttribute]
        public string ProductoId { get; set; }

        [XmlAttribute]
        public string NombreProducto { get; set; }

        [Column(TypeName = "numeric")]
        [XmlAttribute]
        public decimal CantidadOriginal { get; set; }

        [Column(TypeName = "numeric")]
        [XmlAttribute]
        public decimal Cantidad { get; set; }

        [Column(TypeName = "numeric")]
        [XmlAttribute]
        public decimal Devuelto { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Precio { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Costo { get; set; }

        [XmlAttribute]
        public string Empaque { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal SubtotalDet { get; set; }

        [XmlAttribute]
        public decimal TasaDescuentoDet { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal DescuentoDet { get; set; }

        [XmlAttribute]
        public decimal TasaImpuestoDet { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal ImpuestoDet { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal TotalDet { get; set; }
    }
}