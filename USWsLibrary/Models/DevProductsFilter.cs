using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class DevProductsFilter
    {
        [XmlAttribute]
        public string Secuencia { get; set; }
    }
}