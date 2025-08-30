using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public partial class VEN_ENTREGAS_MIN_DT_DTO
    { 

        [XmlAttribute]
        public string Id { get; set; }
        [XmlAttribute]
        public string EntregaId { get; set; }
        [XmlAttribute]
        public string DetalleId { get; set; }
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
        public string ProductoNombre { get; set; }

    }
}