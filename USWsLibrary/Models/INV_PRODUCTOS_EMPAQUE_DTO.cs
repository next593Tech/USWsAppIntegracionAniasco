using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class INV_PRODUCTO_EMPAQUE_DTO
    {
        [XmlAttribute]
        public string ID { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string ProductoID { get; set; }

        [StringLength(25)]
        [XmlAttribute]
        public string ProductoCodigo { get; set; }

        [StringLength(200)]
        [XmlAttribute]
        public string ProductoNombre { get; set; }

        [StringLength(2)]
        [XmlAttribute]
        public string ProductoClase { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string EmpaqueID { get; set; }

        [XmlAttribute]
        [StringLength(15)]
        public string Codigo { get; set; }

        [XmlAttribute]
        [StringLength(25)]
        public string CodigoBarra { get; set; }

        [XmlAttribute]
        public string Nombre { get; set; }

        [XmlAttribute]
        public decimal Factor { get; set; }

        [XmlAttribute]
        public decimal Precio { get; set; }


        public bool AIVA { get; set; }


        public decimal PIVA { get; set; }

        public bool AICE { get; set; }
        public decimal PICE { get; set; }

        public bool AVERDE { get; set; }
        public decimal PVERDE { get; set; }

        public DateTime PromocionFechaInicio { get; set; }
        public DateTime PromocionFechaFinal { get; set; }
    }
}