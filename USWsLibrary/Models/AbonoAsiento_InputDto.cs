namespace USWsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Xml.Serialization;

    public partial class AbonoAsiento_InputDto
    {
        [StringLength(10)]
        [XmlAttribute]
        public string ID { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string DocumentoID { get; set; }

        [Required]
        [StringLength(10)]
        [XmlAttribute]
        public string Número { get; set; }

        [XmlAttribute]
        public DateTime Fecha { get; set; }

        [Required]
        [StringLength(100)]
        [XmlAttribute]
        public string Detalle { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string Tipo { get; set; }

        [StringLength(1024)]
        [XmlAttribute]
        public string Nota { get; set; }

        [XmlAttribute]
        public bool Anulado { get; set; }

        [XmlAttribute]
        public bool Pendiente { get; set; }

        //[XmlAttribute]
        public DateTime ExportadoDate { get; set; }


        [StringLength(15)]
        [XmlAttribute]
        public string CreadoPor { get; set; }

        [StringLength(15)]
        [XmlAttribute]
        public string EditadoPor { get; set; }

        [StringLength(15)]
        [XmlAttribute]
        public string AnuladoPor { get; set; }

        [StringLength(50)]
        [XmlAttribute]
        public string PCID { get; set; }

        [StringLength(2)]
        [XmlAttribute]
        public string SucursalID { get; set; }

        [XmlAttribute]
        public DateTime CreadoDate { get; set; }

        [XmlAttribute]
        public DateTime EditadoDate { get; set; }

        [XmlAttribute]
        public DateTime AnuladoDate { get; set; }

        [StringLength(1024)]
        [XmlAttribute]
        public string AnuladoNota { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string DivisiónID { get; set; }

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


        [XmlElement("AbonoAsientoDT_InputDto")]
        public  List<AbonoAsientoDT_InputDto> AbonoAsientoDT_InputDto { get; set; }

        public AbonoCabDto PaymentCab_InputDto1 { get; set; }

    }
}
