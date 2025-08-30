using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class INV_FISICO_DTO
    {
        [XmlAttribute]
        public string ID { get; set; }

        [XmlAttribute]
        public string NUMERO { get; set; }

        [XmlAttribute]
        public string TIPO { get; set; }

        [XmlAttribute]
        public DateTime FECHA { get; set; }

        [XmlAttribute]
        public string DETALLE { get; set; }

        [XmlAttribute]
        public string SUCURSALID { get; set; }

        [XmlAttribute]
        public string DIVISIONID { get; set; }

        [XmlAttribute]
        public string SUCURSALNOMBRE { get; set; }

        [XmlAttribute]
        public string BODEGAID { get; set; }

        [XmlAttribute]
        public string BODEGANOMBRE { get; set; }

        [XmlAttribute]
        public string SUCURSALBODEGANUMERO { get; set; }

    }
}