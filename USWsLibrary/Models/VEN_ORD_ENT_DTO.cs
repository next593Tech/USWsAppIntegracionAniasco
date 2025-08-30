using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public partial class VEN_ORD_ENT_DTO
    {
        [XmlAttribute]
        public string OrderId { get; set; }

        public VEN_ENTREGAS_DTO Entregas { get; set; } 

        public virtual List<VEN_ENTREGAS_DT_DTO> EntregasDt { get; set; } 
    }
}