using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
   public class INV_CONTEO_DT 
    {
        [XmlAttribute]
        public string ID { get; set; }

        [XmlAttribute]
        public string SucursalID { get; set; }

        [XmlAttribute]
        public string ConteoID { get; set; }

        [XmlAttribute]
        public string ProductoID { get; set; }

        [XmlAttribute]
        public string ProductoCodigo { get; set; }

        [XmlAttribute]
        public string ProductoNombre { get; set; }

        [XmlAttribute]
        public string Empaque { get; set; }

        [XmlAttribute]
        public decimal Cantidad { get; set; }

        [XmlAttribute]
        public decimal Factor { get; set; }

        [XmlAttribute]
        public decimal TotalUnidades { get; set; }

      

    }
}
