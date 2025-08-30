using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class INV_PRODUCTO_PRECIO
    { 
        public int Id { get; set; }

        [StringLength(2)]
        public string Clase { get; set; } 

        [StringLength(10)]
        public string ProductoId { get; set; }
          
        public decimal  Rango1 { get; set; }
          
        public decimal  Rango2 { get; set; }
          
        public decimal  Precio { get; set; }

        public string Empaque { get; set; }

    }
}