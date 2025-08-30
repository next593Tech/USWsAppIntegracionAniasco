using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class PasajeroDto
    {
        [XmlAttribute]
        public string FacturaID { get; set; }
        [XmlAttribute]
        public string CedulaRuc { get; set; }
        [XmlAttribute]
        public string Nombre { get; set; }
        [XmlAttribute]
        public string Asiento { get; set; }

 
    }
}