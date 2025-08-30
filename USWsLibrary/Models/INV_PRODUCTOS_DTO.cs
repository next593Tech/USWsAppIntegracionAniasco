using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class INV_PRODUCTOS_DTO
    {

        [StringLength(10)]
        [XmlAttribute]
        public string ID { get; set; }
          
        [StringLength(200)]
        [XmlAttribute]
        public string Nombre { get; set; }



        [StringLength(25)]
        [XmlAttribute]
        public string Codigo { get; set; } 


        [StringLength(50)]
        [XmlAttribute]
        public string Descripcion  { get; set; } 


        [StringLength(200)]
        [XmlAttribute]
        public string Foto { get; set; }

        [XmlAttribute]
        public decimal  Precio  { get; set; }


        [XmlAttribute]
        public decimal Costo { get; set; }
         


        [XmlAttribute]
        public string Empaque  { get; set; }
         

        [XmlAttribute]
        public decimal Factor { get; set; }



        [XmlAttribute]
        public decimal  Stock { get; set; }

        public bool AIVA { get; set; }

        public decimal PIVA { get; set; }
   
        public bool AICE { get; set; }
        public decimal PICE { get; set; } 

        public bool AVERDE { get; set; }
        public decimal PVERDE { get; set; }


    }
}