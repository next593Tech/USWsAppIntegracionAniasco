using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models
{
    public class VEN_CUADRE_DE_CAJADT
    {
        public int Anio { get; set; }

        public int Mes { get; set; }

        public int Dia { get; set; }

        public decimal SubtotalCero { get; set; }

        public decimal SubtotalIva { get; set; }


        public decimal Subtotal { get; set; }


        public decimal Descuento { get; set; }


        public decimal Impuesto { get; set; }


        public decimal Servicio { get; set; }


        public decimal Total { get; set; }
    }
}