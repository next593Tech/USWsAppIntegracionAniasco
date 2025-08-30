using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
   public class INV_CONTEO_INPUT 
    {
        [XmlAttribute]
        public string ID { get; set; }

        [XmlAttribute]
        public string TomaFisicaID { get; set; }

        [XmlAttribute]
        public string Tipo { get; set; }

        [XmlAttribute]
        public string BodegaID { get; set; }

        [XmlAttribute]
        public string BodegaNombre { get; set; }

        [XmlAttribute]
        public string SucursalNombre { get; set; }

        [XmlAttribute]
        public string SucursalID { get; set; }

        [XmlAttribute]
        public string DivisionID { get; set; }

        [XmlAttribute]
        public string Numero { get; set; }

        [XmlAttribute]
        public DateTime Fecha { get; set; }

        [XmlAttribute]
        public string Detalle { get; set; }

        [XmlAttribute]
        public decimal Latitud { get; set; }

        [XmlAttribute]
        public decimal Longitud { get; set; }

        [XmlAttribute]
        public string Estado { get; set; }


        [XmlAttribute]
        public string PcID { get; set; }

        [XmlAttribute]
        public string NombreUsuario { get; set; }

        [XmlAttribute]
        public bool Exportado { get; set; }

        [XmlAttribute]
        public bool Eliminar { get; set; }

        [XmlAttribute]
        public string IDRetorno { get; set; }

        [XmlAttribute]
        public string MovUniqueId { get; set; }

        public virtual List<INV_CONTEO_DT> LIST_INV_CONTEOS_DT { get; set; }


    }
}
