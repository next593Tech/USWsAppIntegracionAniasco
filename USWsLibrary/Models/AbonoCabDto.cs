using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace USWsLibrary.Models
{
    public class AbonoCabDto
    {
        [XmlAttribute]
        public string CodCliente { get; set; }

        [XmlAttribute] // Es el id del abono para consultar si ya fue grabado o no el abono
        public string Ban_Ingreso_ID { get; set; }

        [XmlAttribute] //Es el id divisa para obtener el cambio
        public string DivisaID_Cambio { get; set; }

        [XmlAttribute] //nombre cliente obtenido x la consulta
        public string ClienteNombre { get; set; }

        [XmlAttribute] // Es total de efectivo + cheques.
        public decimal ValorTotal { get; set; }

        [XmlAttribute]
        public string ID { get; set; }

        [XmlAttribute]
        public string BancoID { get; set; }

        [XmlAttribute]
        public string DeudorID { get; set; }

        [XmlAttribute]
        public string PcID { get; set; }

        [XmlAttribute]
        public string Numero { get; set; }
        
        [XmlAttribute]
        public DateTime Fecha { get; set; }

        [XmlAttribute]
        public string Detalle { get; set; }

        [XmlAttribute]
        public string Tipo { get; set; }

        [XmlAttribute]
        public decimal Valor { get; set; }
        
        [XmlAttribute]
        public decimal Descuento { get; set; }

        [XmlAttribute]
        public decimal Faltante { get; set; }

        [XmlAttribute]
        public decimal Sobrante { get; set; }
        
        [XmlAttribute]
        public decimal Valor_Base { get; set; }

        [XmlAttribute]
        public bool Anulado { get; set; }

        [XmlAttribute]
        public string DivisaID { get; set; }

        [XmlAttribute]
        public decimal Cambio { get; set; }

        [XmlAttribute]
        public string CreadoPor { get; set; }

        [XmlAttribute]
        public string NumRecibo { get; set; }

        [XmlAttribute]
        public string SucursalID { get; set; }

        [XmlAttribute]
        public bool EsPosfechado { get; set; }

        [XmlAttribute]
        public string MovUniqueId { get; set; }
        [XmlAttribute]
        public decimal Latitud { get; set; }
        [XmlAttribute]
        public decimal Longitud { get; set; }


        //BAN INGRESOS DEUDAS   OK  
        [XmlElement("Distribucion")]
        public virtual List<AbonoDistribucionDto> Distribucion { get; set; }


        // CHEQUES Y EFECTIVOS  OK  
        [XmlElement("Detalles")]
        public virtual List<AbonoDetDto> Detalles { get; set; }


    }
}
