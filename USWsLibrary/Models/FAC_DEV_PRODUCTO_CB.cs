using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class FAC_DEV_PRODUCTO_CB
    {
        [XmlAttribute]
        public string FacturaId { get; set; }
         
        [XmlAttribute]
        public string ClienteId { get; set; }

        [XmlAttribute]
        public string Ruc { get; set; }

        [XmlAttribute]
        public string NombreCliente { get; set; }

        [XmlAttribute]
        public string VendedorId { get; set; }

        [XmlAttribute]
        public string CajaId { get; set; }

        [XmlAttribute]
        public DateTime Fecha { get; set; }

        [XmlAttribute]
        public string Tipo { get; set; }

        [XmlAttribute]
        public string SucursalId  { get; set; }

        [XmlAttribute]
        public string PcId { get; set; }

        [XmlAttribute]
        public string SecuenciaFac { get; set; }

        [XmlAttribute]
        public decimal SubtotalCab { get; set; }
         
        [XmlAttribute]
        public decimal ImpuestoCab { get; set; }

        [XmlAttribute]
        public decimal TotalCab { get; set; }

    }
}