using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class INV_INGRESOS
    {
        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string BodegaId { get; set; }

        [XmlAttribute]
        public DateTime Fecha { get; set; }
        
        [XmlAttribute]
        public decimal Total { get; set; }
        
        [XmlAttribute]
        public decimal ValorBase { get; set; }

        [XmlAttribute]
        public string SucursalId { get; set; }

        [XmlAttribute]
        public string PcID { get; set; }

    }
}