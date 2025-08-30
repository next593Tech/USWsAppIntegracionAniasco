namespace USWsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Xml.Serialization;

    public partial class AbonoBanco_Kardex_InputDto
    {
        [StringLength(10)]
        [XmlAttribute]
        public string ID { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string BancoID { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string DocumentoID { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string AsientoID { get; set; }

        [StringLength(2)]
        [XmlAttribute]
        public string SucursalID { get; set; }

        [StringLength(50)]
        [XmlAttribute]
        public string PcID { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string DivisaID { get; set; }

        [XmlAttribute]
        public decimal Cambio { get; set; }

        [XmlAttribute]
        public DateTime Fecha { get; set; }


        [XmlAttribute]
        public DateTime  Fecha_Banco { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string Tipo { get; set; }

        [Required]
        [StringLength(100)]
        [XmlAttribute]
        public string Detalle { get; set; }

        [XmlAttribute]
        public bool Débito { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Valor { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal  Valor_Base { get; set; }

        [XmlAttribute]
        public bool Conciliado { get; set; }

        //[XmlAttribute]
        public DateTime  ExportadoDate { get; set; }

        [StringLength(15)]
        [XmlAttribute]
        public string CreadoPor { get; set; }

        [XmlAttribute]
        public DateTime  CreadoDate { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string Número { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string Cheque { get; set; }

        [XmlAttribute]
        public DateTime  Fecha_Cheque { get; set; }

        [StringLength(50)]
        [XmlAttribute]
        public string Beneficiario { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string DivisiónID { get; set; }

        [XmlAttribute]
        public bool Anulado { get; set; }

        [StringLength(15)]
        [XmlAttribute]
        public string AnuladoPor { get; set; }

        [XmlAttribute]
        public DateTime  AnuladoDate { get; set; }

        [StringLength(1024)]
        [XmlAttribute]
        public string AnuladoNota { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        [XmlAttribute]
        public byte[] Secuencia { get; set; }

        [StringLength(15)]
        [XmlAttribute]
        public string ExportadoPor { get; set; }

        [Column(TypeName = "numeric")]
        [XmlAttribute]
        public decimal  Exportado { get; set; }

        [Column(TypeName = "numeric")]
        [XmlAttribute]
        public decimal  ExportadoUpdate { get; set; }

        [Column(TypeName = "numeric")]
        [XmlAttribute]
        public decimal  ExportadoCandidate { get; set; }


        public  AbonoCabDto PaymentCab_InputDto { get; set; }
    }
}
