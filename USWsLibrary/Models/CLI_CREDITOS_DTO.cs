using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class CLI_CREDITOS_DTO
    {
        [XmlAttribute]
        public int IdCre { get; set; }         
        
        [XmlAttribute]
        public string ClienteId { get; set; }

        [XmlAttribute]
        public string Detalle { get; set; }

        [XmlAttribute]
        public DateTime Fecha { get; set; }        

        [XmlAttribute]
        public string Tipo { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal ValorBase { get; set; }

        [XmlAttribute]
        public string SucursalId { get; set; }

        [XmlAttribute]
        public string PcId { get; set; }

        [XmlAttribute]
        public string FacturaId { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Impuesto { get; set; }

        [XmlAttribute]
        public string Secuencia { get; set; }

        [XmlAttribute]
        public string SecuenciaFac { get; set; }

        [XmlAttribute]
        public int SecuenciaValor { get; set; }

        [XmlAttribute]
        public string VendedorId { get; set; }

        [XmlAttribute]
        public string CajaId { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Descuento { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Total { get; set; }

        [XmlAttribute]
        public string MovUniqueId { get; set; }

        public List<CLI_CREDITOS_PRODUCTOS> CliCreditosProducts { get; set; }

    }
}
