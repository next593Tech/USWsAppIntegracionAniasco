namespace USWsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Xml.Serialization;

    public class VEN_ORDENES_DT_DTO
    {
        [StringLength(10)]
        [XmlAttribute]
        public string ID { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string OrdenID { get; set; }

        [Required]
        [StringLength(10)]
        [XmlAttribute]
        public string ProductoID { get; set; }


        [StringLength(200)]
        [XmlAttribute]
        public string Nombre { get; set; }


        [StringLength(50)]
        [XmlAttribute]
        public string Descripcion { get; set; }


        [Column(TypeName = "numeric")]
        [XmlAttribute]
        public decimal Stock { get; set; }

        [Column(TypeName = "numeric")]
        [XmlAttribute]
        public decimal Cantidad { get; set; }


        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Precio { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Costo { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Subtotal { get; set; }

        [XmlAttribute]
        public decimal TasaDescuento { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Descuento { get; set; }

        [XmlAttribute]
        public decimal TasaImpuesto { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Impuesto { get; set; }

        [XmlAttribute]
        public decimal TasaImpuestoVerde { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal ImpuestoVerde { get; set; }

        [XmlAttribute]
        public decimal TasaImpuestoIce { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal ImpuestoIce { get; set; }


        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Total { get; set; }

        [StringLength(2)]
        [XmlAttribute]
        public string Clase { get; set; }



        [StringLength(40)]
        [XmlAttribute]
        public string Empaque { get; set; }


        [MaxLength(15)]
        [XmlAttribute]
        public string CodEmpaque { get; set; }




        [XmlAttribute]
        public bool Embarque { get; set; }

        [StringLength(40)]
        [XmlAttribute]
        public string PrecioName { get; set; }

        [Column(TypeName = "numeric")]
        [XmlAttribute]
        public decimal Factor { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal SaldoAcum { get; set; }

        [XmlAttribute]
        public bool Promocion { get; set; }


        [XmlAttribute]
        public bool AIVA { get; set; }

        [XmlAttribute]
        public bool AICE { get; set; }

        [XmlAttribute]
        public bool AVER { get; set; }



        [XmlElement("VEN_ORDENES_DTO")]
        public virtual VEN_ORDENES_DTO VEN_ORDENES { get; set; }

        [XmlElement("INV_PRODUCTOS_PRECIO_DTO")]
        public virtual List<INV_PRODUCTOS_PRECIO_DTO> RANGO_PRECIOS { get; set; }

    }
}
