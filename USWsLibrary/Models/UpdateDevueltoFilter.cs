using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class UpdateDevueltoFilter
    {
        
        [XmlAttribute]
        public string FacturaId { get; set; }

        public List<FAC_DEV_PRODUCTO_DT> productsList { get; set; }

    }
}