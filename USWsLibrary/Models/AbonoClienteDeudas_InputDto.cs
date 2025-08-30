namespace USWsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Xml.Serialization;

    public partial class AbonoClienteDeudas_InputDto
    {
        [XmlAttribute]
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]

        public string ID { get; set; }

        [XmlAttribute]
        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string ClienteID { get; set; }

        [XmlAttribute]
        [StringLength(10)]
        public string DocumentoID { get; set; }

        [XmlAttribute]
        [StringLength(10)]
        public string AsientoID { get; set; }

        [XmlAttribute]
        [StringLength(17)]
        public string Número { get; set; }

        [XmlAttribute]
        [StringLength(100)]
        public string Detalle { get; set; }

        [XmlAttribute]
        [Key]
        [Column(Order = 2, TypeName = "money")]
        public decimal Valor { get; set; }

        [XmlAttribute]
        [Key]
        [Column(Order = 3, TypeName = "money")]
        public decimal ValorBase { get; set; }

        [XmlAttribute]
        [Key]
        [Column(Order = 4)]
        public DateTime Fecha { get; set; }

        [XmlAttribute]
        [Key]
        [Column(Order = 5)]
        public DateTime Vencimiento { get; set; }

        [XmlAttribute]
        [StringLength(50)]
        public string RubroID { get; set; }

        [XmlAttribute]
        [StringLength(50)]
        public string CtaCxCID { get; set; }

        [XmlAttribute]
        [Key]
        [Column(Order = 6)]
        [StringLength(10)]
        public string DivisaID { get; set; }

        [XmlAttribute]
        [Key]
        [Column(Order = 7)]
        public decimal Cambio { get; set; }

        [XmlAttribute]
        [Key]
        [Column(Order = 8, TypeName = "money")]
        public decimal Saldo { get; set; }

        [XmlAttribute]
        [StringLength(10)]
        public string Tipo { get; set; }

        [XmlAttribute]
        [Key]
        [Column(Order = 9)]
        public bool Crédito { get; set; }

        [XmlAttribute]
        [Key]
        [Column(Order = 10)]
        public bool Facturado { get; set; }

        [XmlAttribute]
        [StringLength(10)]
        public string DeudaID { get; set; }

        [XmlAttribute]
        public decimal  CambioDeuda { get; set; }

        [XmlAttribute]
        [StringLength(10)]
        public string VendedorID { get; set; }

        [XmlAttribute]
        [Key]
        [Column(Order = 11)]
        public bool Anulado { get; set; }

        //[XmlAttribute]
        public DateTime  ExportadoDate { get; set; }

        [XmlAttribute]
        [StringLength(15)]
        public string CreadoPor { get; set; }

        [XmlAttribute]
        public DateTime  CreadoDate { get; set; }

        [XmlAttribute]
        [StringLength(15)]
        public string EditadoPor { get; set; }

        [XmlAttribute]
        public DateTime  EditadoDate { get; set; }

        [XmlAttribute]
        [StringLength(15)]
        public string AnuladoPor { get; set; }

        [XmlAttribute]
        public DateTime  AnuladoDate { get; set; }

        [XmlAttribute]
        [StringLength(1024)]
        public string AnuladoNota { get; set; }

        [XmlAttribute]
        [StringLength(2)]
        public string SucursalID { get; set; }

        [XmlAttribute]
        [StringLength(50)]
        public string PcID { get; set; }

        [XmlAttribute]
        [StringLength(10)]
        public string DivisiónID { get; set; }

        [XmlAttribute]
        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] Secuencia { get; set; }

        [XmlAttribute]
        public bool  Retenido { get; set; }

        [XmlAttribute]
        [StringLength(15)]
        public string ExportadoPor { get; set; }

        [XmlAttribute]
        [Column(TypeName = "numeric")]
        public decimal  Exportado { get; set; }

        [XmlAttribute]
        [Column(TypeName = "numeric")]
        public decimal  ExportadoUpdate { get; set; }

        [XmlAttribute]
        [Column(TypeName = "numeric")]
        public decimal  ExportadoCandidate { get; set; }

        [XmlAttribute]
        [StringLength(2)]
        public string MesArriendo { get; set; }

        [XmlAttribute]
        [StringLength(10)]
        public string ContratoID { get; set; }

        [XmlAttribute]
        public bool  Inmobiliaria { get; set; }

        
        public  AbonoCabDto PaymentCab_InputDto { get; set; }
    }
}
