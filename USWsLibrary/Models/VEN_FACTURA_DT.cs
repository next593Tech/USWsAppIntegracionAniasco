using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class VEN_FACTURA_DT
    {
        [XmlAttribute]
        public string FacturaID { get; set; }

        [XmlAttribute]
        public string DetalleID { get; set; }

        [XmlAttribute]
        public decimal Cantidad { get; set; }

        [XmlAttribute]
        public decimal Precio{ get; set; }

        [XmlAttribute]
        public decimal TasaDescuento { get; set; }

        [XmlAttribute]
        public decimal Factor { get; set; }

        [XmlAttribute]
        public string Empaque { get; set; }

        [XmlAttribute]
        public string Clase { get; set; }

        [XmlAttribute]
        public decimal Devuelto { get; set; }

        [XmlAttribute]
        public decimal Subtotal { get; set; }

        [XmlAttribute]
        public decimal Descuento { get; set; }

        [XmlAttribute]
        public decimal Impuesto { get; set; }

        [XmlAttribute]
        public decimal Total { get; set; }

        [XmlAttribute]
        public decimal TasaImpuesto { get; set; }

        [XmlAttribute]
        public string ProductoID { get; set; }

        [XmlAttribute]
        public string ProductoNombre { get; set; }

        [XmlAttribute]

        public string ProductoCodigo { get; set; }

        [XmlAttribute]
        public decimal DevueltoDobra { get; set; }

    }
}
