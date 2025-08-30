using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class ReservaDto
    {
        [XmlAttribute]
        public string Id { get; set; }
        [XmlAttribute]
        public string FacturaID { get; set; }
        [XmlAttribute]
        public string Ruta { get; set; }
        [XmlAttribute]
        public string Bus { get; set; }
        [XmlAttribute]
        public DateTime Fecha { get; set; }
        [XmlAttribute]
        public string Frecuencia { get; set; }
        [XmlAttribute]
        public string Asiento { get; set; }
        [XmlAttribute]
        public string Estado { get; set; }
		[XmlAttribute]
        public string SucursalId { get; set; }
        [XmlAttribute]
        public DateTime HoraInicio { get; set; }
        [XmlAttribute]
        public DateTime HoraLlegada { get; set; }
        [XmlAttribute]
        public string MovUniqueId { get; set; }
    }
}