using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public partial class VEN_ENTREGAS_UPDATE_DTO
    {
        [XmlAttribute]
        public string Id { get; set; }
        [XmlAttribute]
        public string TransportistaId { get; set; }
        [XmlAttribute]
        public DateTime Fecha { get; set; }
        [XmlAttribute]
        public decimal Latitud { get; set; }
        [XmlAttribute]
        public decimal Longitud { get; set; }
    }
}