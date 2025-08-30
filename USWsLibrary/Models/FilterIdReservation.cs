using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class FilterIdReservation
    {
        [XmlAttribute]
        public string ReservationId { get; set; }
    }
}