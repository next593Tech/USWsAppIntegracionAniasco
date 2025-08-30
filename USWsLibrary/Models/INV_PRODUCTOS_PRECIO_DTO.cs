using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class INV_PRODUCTOS_PRECIO_DTO 
    {

        [StringLength(10)]
        [XmlAttribute]
        public string ProductoID { get; set; }
         

        [XmlAttribute]
        public decimal  Rango1 { get; set; }


        [XmlAttribute]
        public decimal  Rango2 { get; set; }

        [XmlAttribute]
        public decimal  Precio { get; set; }

        public string Empaque { get; set; }

    }
}