namespace USWsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ClienteAbonoOutputDto
    {
        [StringLength(100)]
        public string Detalle { get; set; }

        [StringLength(10)]
        public string Vencimiento { get; set; }

        public decimal Valor { get; set; }

        public decimal Saldo { get; set; }

        public string IdClienteDeuda { get; set; }

        public string IdCliente { get; set; }

    }
}
