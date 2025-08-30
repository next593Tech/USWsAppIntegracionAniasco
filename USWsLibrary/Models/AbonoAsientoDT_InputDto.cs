namespace USWsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Xml.Serialization;

    public partial class AbonoAsientoDT_InputDto
    {
        [StringLength(10)]
        [XmlAttribute]
        public string ID { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string AsientoID { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string CuentaID { get; set; }

        [StringLength(2)]
        [XmlAttribute]
        public string SucursalID { get; set; }

        [Key]
        [Column(Order = 0)]
        [XmlAttribute]
        public bool Débito { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "money")]
        [XmlAttribute]
        public decimal Valor { get; set; }

        [StringLength(100)]
        [XmlAttribute]
        public string Detalle { get; set; }

        [XmlAttribute]
        public DateTime ExportadoDate { get; set; }

        [StringLength(50)]
        [XmlAttribute]
        public string PCID { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Valor_Base { get; set; }

        [StringLength(15)]
        [XmlAttribute]
        public string CreadoPor { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string DivisaID { get; set; }

        [Key]
        [Column(Order = 2)]
        [XmlAttribute]
        public decimal Cambio { get; set; }

        [XmlAttribute]
        public DateTime CreadoDate { get; set; }

        [StringLength(15)]
        [XmlAttribute]
        public string ExportadoPor { get; set; }

        [Column(TypeName = "numeric")]
        [XmlAttribute]
        public decimal Exportado { get; set; }

        [Column(TypeName = "numeric")]
        [XmlAttribute]
        public decimal ExportadoUpdate { get; set; }

        [Column(TypeName = "numeric")]
        [XmlAttribute]
        public decimal ExportadoCandidate { get; set; }

        [XmlAttribute]
        public bool Anulado { get; set; }

        [StringLength(60)]
        [XmlAttribute]
        public string EditadoPor { get; set; }

        public virtual AbonoAsiento_InputDto PaymentAccount_InputDto { get; set; }

    }
}
