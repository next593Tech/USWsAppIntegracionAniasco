namespace USWsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Xml.Serialization;

    public class VEN_ORDENESDESPACHO_MIN_DTO 
    { 
        [StringLength(10)]
        [XmlAttribute]
        public string ID { get; set; }


        public string BackColor { get; set; } 

        [StringLength(10)]
        [XmlAttribute]
        public string ClienteID { get; set; }
         
        public string ClienteRUC { get; set; } 


        [StringLength(200)]
        [XmlAttribute]
        public string ClienteNombre { get; set; } 

         
        [StringLength(10)]
        [XmlAttribute]
        public string VendedorID { get; set; }

        [StringLength(15)]
        [XmlAttribute]
        public string VendedorUserName { get; set; } 

        [StringLength(200)]
        [XmlAttribute]
        public string VendedorNombre { get; set; }
          

        [XmlAttribute]
        public DateTime Fecha { get; set; }

          
        [XmlAttribute]
        public decimal Total { get; set; }

         
        [XmlAttribute]
        public string Estado { get; set; }

        [XmlAttribute]
        public bool Negado { get; set; }

        [XmlAttribute]
        public bool Aprobado { get; set; }

        [XmlAttribute]
        public bool Egresado { get; set; }

        [XmlAttribute]
        public bool Facturado { get; set; }

        [XmlAttribute]
        public bool Anulado { get; set; }
         


    }
}
