namespace WebApiApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Xml.Serialization;

    public   class VEN_ORDENESDESPACHO_DTO 
    {

        [Required]
        [StringLength(10)]
        [XmlAttribute]
        public string ID { get; set; }

        [Required]
        [StringLength(10)]
        [XmlAttribute]
        public string Número { get; set; }
         

        [Required]
        [StringLength(10)]
        [XmlAttribute]
        public string ClienteID { get; set; }



        [Required]
        [StringLength(400)]
        [XmlAttribute]
        public string Nota { get; set; }


        [StringLength(13)]
        [XmlAttribute]
        public string ClienteRUC { get; set; }

        [StringLength(2)]
        [XmlAttribute]
        public string ClienteClase { get; set; }

        [XmlAttribute]
        public decimal Factor { get; set; }


        [StringLength(200)]
        [XmlAttribute]
        public string ClienteNombre { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string VendedorID { get; set; }

         
        [XmlAttribute]
        [StringLength(15)]
        public string VendedorUsername { get; set; }


        [XmlAttribute]
        [StringLength(15)]  
        public string NombreUsuario { get; set; }

        [XmlAttribute]
        public DateTime FechaEntrega { get; set; }

        [XmlAttribute]
        public string TransportistaId { get; set; }

        [XmlAttribute]
        public string TransportistaName { get; set; }

        [Required]
        [StringLength(10)]
        [XmlAttribute]
        public string TerminoID { get; set; } 

        [XmlAttribute]
        public DateTime Fecha { get; set; }

        [XmlAttribute]
        public decimal Subtotal { get; set; }

        [XmlAttribute]
        public decimal Descuento { get; set; }


        [XmlAttribute]
        public decimal Impuesto { get; set; }

        [XmlAttribute]
        public decimal ImpuestoIce { get; set; }


        [XmlAttribute]
        public decimal ImpuestoVerde { get; set; }

        [XmlAttribute]
        public decimal SubtotalImpuestoIva { get; set; }

        [XmlAttribute]
        public decimal SubtotalImpuestoIce { get; set; }

        [XmlAttribute]
        public decimal SubtotalImpuestoVerde { get; set; }
         

        [XmlAttribute]
        public decimal Total { get; set; }

   
        [StringLength(10)]
        [XmlAttribute]
        public string Estado { get; set; }
 
                
        [StringLength(50)]
        [XmlAttribute]
        public string PcID { get; set; }

        [XmlAttribute]
        public double Latitud { get; set; }

        [XmlAttribute]
        public double Longitud { get; set; }

        public virtual List<VEN_ORDENESDESPACHO_DT_DTO> LIST_VEN_ORDENESDESPACHO_DT { get; set; } 
    }
}
