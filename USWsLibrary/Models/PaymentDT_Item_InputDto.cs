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
    public class PaymentDT_Item_InputDto
    {
        //
        [StringLength(10)]
        [XmlAttribute]
        public string ID { get; set; }

        //
        [Required]
        [StringLength(10)]
        [XmlAttribute]
        public string Tipo { get; set; } //Efectivo o CHEQUE

        //
        [Required]
        [StringLength(10)]
        [XmlAttribute]
        public string Número { get; set; }

        //
        [Required]
        [StringLength(30)]
        [XmlAttribute]
        public string Banco { get; set; }

        //
        [Required]
        [StringLength(15)]
        [XmlAttribute]
        public string Cuenta { get; set; }
        
        //
        [Required]
        [XmlAttribute]
        public decimal Valor { get; set; }
    }
}
