using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public partial class VEN_ENTREGAS_DTO
    {
        [XmlAttribute]
        public string Id { get; set; }
        [XmlAttribute]
        public string Numero { get; set; }
        [XmlAttribute]
        public string Detalle { get; set; }
        [XmlAttribute]
        public string DivisionId { get; set; }
        [XmlAttribute]
        public string TipoDocumento { get; set; }
        [XmlAttribute]
        public string DocumentoId { get; set; }
        [XmlAttribute]
        public DateTime Fecha { get; set; }
        [XmlAttribute]
        public string ClienteId { get; set; }
        [XmlAttribute]
        public string ClienteNombre { get; set; }
        [XmlAttribute]
        public string Tipo { get; set; }
        [XmlAttribute]
        public string Transporte { get; set; }
        [XmlAttribute]
        public string Nota { get; set; }
        [XmlAttribute]
        public string Estado { get; set; }
        [XmlAttribute]
        public bool Despachado { get; set; }
        [XmlAttribute]
        public string SucursalId { get; set; }
        [XmlAttribute]
        public string PcId { get; set; }
        [XmlAttribute]
        public decimal Longitud { get; set; }
        [XmlAttribute]
        public decimal Latitud { get; set; }
        [XmlAttribute]
        public decimal BodegaId { get; set; }
        [XmlAttribute]
        public decimal TotalGral { get; set; }
        [XmlAttribute]
        public string MovUniqueId { get; set; }
    }
}