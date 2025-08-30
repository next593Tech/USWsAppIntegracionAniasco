using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class CLI_RETENCION_DTO
    {
        [XmlAttribute]
        public string FacturaID { get; set; }
        [XmlAttribute]
        public string NumeroRF { get; set; }
        [XmlAttribute]
        public decimal BaseFuente { get; set; }
        [XmlAttribute]
        public DateTime Fecha { get; set; }
        [XmlAttribute]
        public string Tipo_RFIR { get; set; }
        [XmlAttribute]
        public decimal Tasa_RFIR { get; set; }
        [XmlAttribute]
        public decimal Total_RFIR { get; set; }
        [XmlAttribute]
        public string CuentaID_RFIR { get; set; }
        [XmlAttribute]
        public decimal BaseIVA { get; set; }
        [XmlAttribute]
        public string Tipo_RFIVA { get; set; }
        [XmlAttribute]
        public decimal Tasa_RFIVA { get; set; }
        [XmlAttribute]
        public decimal Total_RFIVA { get; set; }
        [XmlAttribute]
        public string CuentaID_RFIVA { get; set; }
        [XmlAttribute]
        public string SucursalID { get; set; }
        [XmlAttribute]
        public string PcID { get; set; }
        [XmlAttribute]
        public string CreadoPor { get; set; }
        [XmlAttribute]
        public string TipoDocumento { get; set; }
        [XmlAttribute]
        public string RetencionIVAID { get; set; }
        [XmlAttribute]
        public string RetencionFuenteID { get; set; }
  


    }
}