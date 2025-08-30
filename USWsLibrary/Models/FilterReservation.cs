using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class FilterReservation
    {

        //public int PageNumber { get; set; }
        //public int PageSize { get; set; }
        [XmlAttribute]
        public DateTime Fecha { get; set; }
        [XmlAttribute]
        public string Ruta { get; set; }
        [XmlAttribute]
        public string Frecuencia { get; set; }
        [XmlAttribute]
        public string Bus { get; set; }
        [XmlAttribute]
        public string SucursalId { get; set; }
        [XmlAttribute]
        public DateTime HoraInicio { get; set; }
        [XmlAttribute]
        public DateTime HoraLlegada { get; set; }



    }
}