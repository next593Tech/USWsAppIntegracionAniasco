using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class INV_CONTEO_DT_DTO
    {

        //        ConteoID char (10)	Unchecked
        //ProductoID  char (10)	Unchecked
        //Cantidad    numeric(11, 2)  Unchecked
        //Empaque varchar(10) Checked
        //factor  numeric(6, 2)   Checked
        //TotalUnidades   numeric(11, 2)  Checked
        //Anulado bit Unchecked
        //CreadoPor varchar(15) Checked
        //CreadoDate  datetime Checked
        //EditadoPor varchar(15) Checked
        //EditadoDate datetime Checked
        //SucursalID char (2)	Checked
        //PcID    varchar(50) Checked
        //ExportadoDate   datetime Checked
        //ExportadoPor varchar(15) Checked
        //Exportado   numeric(1, 0)   Checked
        //ExportadoUpdate numeric(1, 0)   Checked
        //ExportadoCandidate  numeric(1, 0)   Checked
        //     Unchecked

        [XmlAttribute]
        [StringLength(10)]
        public string ConteoID { get; set; }

        [XmlAttribute]
        [StringLength(10)]
        public string ProductoID { get; set; }

        [XmlAttribute]
        public decimal Cantidad { get; set; }

        [XmlAttribute]
        [StringLength(10)]
        public string Empaque { get; set; }

        [XmlAttribute]
        public decimal factor { get; set; }

        [XmlAttribute]
        public decimal TotalUnidades { get; set; }

        [XmlAttribute]
        public bool Anulado { get; set; }

        [XmlAttribute]
        [StringLength(2)]
        public string SucursalID { get; set; }







    }
}