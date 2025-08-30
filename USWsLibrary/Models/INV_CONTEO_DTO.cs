using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class INV_CONTEO_DTO
    {
        //        ID char (10)	Unchecked
        //AsientoID   char (10)	Checked
        // TomaFísicaID    char (10)	Unchecked
        //  Número  char (10)	Unchecked
        //   Fecha   datetime Unchecked
        //Tipo varchar(10) Checked
        //Detalle varchar(100)    Unchecked
        //Ubicación   varchar(50) Checked
        //Nota    varchar(1024)   Checked
        //Procesado   bit Unchecked
        //Anulado bit Unchecked
        //AnuladoPor  varchar(15) Checked
        //AnuladoDate datetime Checked
        //AnuladoNota varchar(1024)   Checked
        //CreadoPor   varchar(15) Checked
        //CreadoDate  datetime Checked
        //ExportadoDate datetime    Checked
        //EditadoPor  varchar(15) Checked
        //EditadoDate datetime Checked
        //SucursalID char (2)	Checked
        //PcID    varchar(50) Checked
        //DivisiónID  char (10)	Checked
        // ExportadoPor    varchar(15) Checked
        // Exportado   bit Checked
        //ExportadoUpdate numeric(1, 0)   Checked
        //ExportadoCandidate  numeric(1, 0)   Checked
        //        Unchecked


        [Required]
        [StringLength(10)]
        [XmlAttribute]
        public string ID { get; set; }

        [XmlAttribute]
        public DateTime Fecha { get; set; }

        [XmlAttribute]
        [StringLength(10)]
        public string Tipo { get; set; }

        [XmlAttribute]
        [StringLength(10)]
        public string OrdenID { get; set; }

        [XmlAttribute]
        [StringLength(100)]
        public string Detalle { get; set; }

        [XmlAttribute]
        [StringLength(50)]
        public string Ubicación { get; set; }

        [XmlAttribute]
        public string Nota { get; set; }

        [XmlAttribute]
        public bool Procesado { get; set; }

        [XmlAttribute]
        public bool Anulado { get; set; }



        [XmlAttribute]
        [StringLength(10)]
        public string SucursalID { get; set; }

        [XmlAttribute]
        [StringLength(10)]
        public string DivisionID { get; set; }

        [XmlAttribute]
        public string TomaFisicaID { get; set; }


        public virtual List<INV_CONTEO_DT> LIST_INV_CONTEOS_DT { get; set; }

    }
}