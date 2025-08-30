using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public partial class VEN_ENTREGAS_DT_DTO
    {
        [XmlAttribute]
        public string Id { get; set; }
        [XmlAttribute]
        public string LineaId { get; set; }
        [XmlAttribute]
        public string EntregaId{ get; set; }
        [XmlAttribute]
        public string OrdenId { get; set; }
        [XmlAttribute]
        public string ProductoId { get; set; }
        [XmlAttribute]
        public string BodegaId { get; set; }
        [XmlAttribute]
        public decimal TotalOrder { get; set; }
        [XmlAttribute]
        public decimal Cantidad { get; set; }
        [XmlAttribute]
        public decimal Despachado { get; set; }
        [XmlAttribute]
        public string Empaque { get; set; }
        [XmlAttribute]
        public decimal Factor { get; set; }
        [XmlAttribute]
        public string SucursalId { get; set; }
        [XmlAttribute]
        public string PcId { get; set; }
        [XmlAttribute]
        public decimal Egresado { get; set; }
        [XmlAttribute]
        public decimal Stock { get; set; }
        [XmlAttribute]
        public decimal Costo { get; set; }
        [XmlAttribute]
        public decimal TotalDetalle { get; set; }

    }
}