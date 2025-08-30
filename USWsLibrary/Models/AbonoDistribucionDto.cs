namespace USWsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Xml.Serialization;

    public partial class AbonoDistribucionDto
    {
        [XmlAttribute]
        public string IngresoID { get; set; }

        [XmlAttribute]
        public string DeudaID { get; set; }

        [XmlAttribute]
        public string SucursalID { get; set; }

        [XmlAttribute]
        public string PcID { get; set; }


        [XmlAttribute]
        public decimal Valor { get; set; }

        [XmlAttribute]
        public decimal Cambio { get; set; }

        [XmlAttribute]
        public string DivisaID { get; set; }
         
        [XmlAttribute]
        public decimal Saldo { get; set; }

        [XmlAttribute]
        public decimal Dif_Cambio { get; set; }

        [XmlAttribute]
        public DateTime Fecha { get; set; }

        [XmlAttribute]
        public DateTime Vencimiento { get; set; }

        [XmlAttribute] 
        public string Tipo { get; set; }

        [XmlAttribute]
        public string Número { get; set; }

        [XmlAttribute]
        public string Detalle { get; set; }

        [XmlAttribute]
        public bool Crédito { get; set; }

        [XmlAttribute]
        public string RubroID { get; set; }

        [XmlAttribute]
        public string CtaCxCID { get; set; }

        [XmlAttribute]
        public decimal CambioDia { get; set; }

        [XmlAttribute]
        public string DivisiónID { get; set; }
         
        [XmlAttribute]
        public string CreadoPor { get; set; }
 
        [XmlAttribute]
        public string VendedorID { get; set; }

        [XmlAttribute]
        public DateTime CreadoDate { get; set; }
 
        [XmlAttribute]
        public decimal Abono { get; set; }

        //[XmlAttribute]
        //public virtual AbonoCabDto PaymentCab { get; set; }
    }
}
