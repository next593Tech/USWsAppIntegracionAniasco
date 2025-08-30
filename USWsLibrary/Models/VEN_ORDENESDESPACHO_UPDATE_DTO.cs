using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class VEN_ORDENESDESPACHO_UPDATE_DTO
    {
        [XmlAttribute]
        public string OrderId { get; set; }

        [XmlAttribute]
        public DateTime FechaEntrega { get; set; }
    }
}