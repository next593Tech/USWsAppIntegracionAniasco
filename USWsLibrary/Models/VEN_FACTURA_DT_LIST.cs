using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class VEN_FACTURA_DT_LIST
    {
        [XmlElement("VEN_FACTURA_DT")]
        public virtual List<VEN_FACTURA_DT> ven_factura_dt { get; set; }
            

    }
}
