using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class CIERRECAJA_DTO
    {
        [XmlAttribute]
        public string CajaID { get; set; }

        [XmlAttribute]
        public string UsuarioID { get; set; }

        [XmlAttribute]
        public string Número { get; set; }

        [XmlAttribute]
        public DateTime Fecha { get; set; }

        [XmlAttribute]
        public DateTime FechaCierre { get; set; }

        [XmlAttribute]
        public string Detalle { get; set; }

        [XmlAttribute]
        public int B100 { get; set; }

        [XmlAttribute]
        public int B50 { get; set; }

        [XmlAttribute]
        public int B20 { get; set; }

        [XmlAttribute]
        public int B10 { get; set; }

        [XmlAttribute]
        public int B5 { get; set; }

        [XmlAttribute]
        public int B1 { get; set; }

        [XmlAttribute]
        public int M100 { get; set; }

        [XmlAttribute]
        public int M1 { get; set; }


        [XmlAttribute]
        public decimal Efectivo { get; set; }

        [XmlAttribute]
        public int Cheque { get; set; }

        [XmlAttribute] 
        public decimal TotalCheque { get; set; }


        [XmlAttribute]
        public int  Voucher { get; set; }

        [XmlAttribute]
        public decimal TotalVoucher { get; set; }

        [XmlAttribute]
        public decimal TotalRetenciones { get; set; }

        [XmlAttribute]
        public decimal Creditos { get; set; }

        [XmlAttribute]
        public decimal Total { get; set; }

        [XmlAttribute]
        public decimal TotalCierre { get; set; }

        [XmlAttribute]
        public string Tipo { get; set; }

        [XmlAttribute]
        public string PCID { get; set; }

        [XmlAttribute]
        public string SucursalID { get; set; }

        [XmlAttribute]
        public string DivisiónID { get; set; }

        [XmlAttribute]
        public string MovUniqueId { get; set; }
    }
}