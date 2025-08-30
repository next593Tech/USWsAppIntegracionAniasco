namespace USWsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Xml.Serialization;

    public partial class AbonoDetDto
    {
        [XmlAttribute]
        public string ID { get; set; }
        
        [XmlAttribute]
        public string IngresoID { get; set; }//CabeceraID
        
        [XmlAttribute]
        public string SucursalID { get; set; }
        
        [XmlAttribute]
        public string PcID { get; set; }
        
        [XmlAttribute]
        public string Tipo { get; set; } //Efectivo o CHEQUE
        
        [XmlAttribute]
        public string Numero { get; set; } //numero de cheque
        
        [XmlAttribute]
        public string Banco { get; set; } //nombre del banco
        
        [XmlAttribute]
        public string Cuenta { get; set; } //numero de cuenta
     
        [XmlAttribute]
        public DateTime Fecha { get; set; }
               
        [XmlAttribute]
        public DateTime Vencimiento { get; set; }
        
        [XmlAttribute]
        public string Girador { get; set; }

        [XmlAttribute]
        public decimal Abono { get; set; } // valor

        [XmlAttribute]
        public decimal Valor { get; set; } // valor

        [XmlAttribute]
        public decimal Valor_Base { get; set; } // valor * cambio

        [XmlAttribute]
        public string DivisaID { get; set; }

        [XmlAttribute]
        public decimal Cambio { get; set; }

        [XmlAttribute]
        public string CreadoPor { get; set; }

        //[XmlAttribute]
        //public virtual AbonoCabDto PaymentCab { get; set; }

    }
}
