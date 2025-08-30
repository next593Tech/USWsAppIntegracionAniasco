using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models
{
    public class INV_PRODUCTO_PROMOCIONES
    {
        [StringLength(10)]
        public string ProductoID { get; set; }

        [StringLength(10)]
        public string ProductoPromoID { get; set; }

        [StringLength(10)]
        public string CuentaID { get; set; }

        public decimal Cantidad { get; set; }

        public decimal UnidadesPromo { get; set; }

        public bool Adicional { get; set; }

        [StringLength(2)]
        public string SucursalID { get; set; }

        public decimal Maximo { get; set; }

        public decimal Stock { get; set; }

        public decimal Saldo { get; set; }

        public decimal TasaInv { get; set; }

        [StringLength(2)]
        public string clase { get; set; }

        public bool AplicaPOS { get; set; }

        


    }
}