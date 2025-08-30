namespace USWsLibrary.Models  
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;



    public class CLI_DEUDA_DTO 
    {


        [StringLength(10)]
        public string ClienteId { get; set; }

        [StringLength(10)]
        public string DeudaId { get; set; }


        
        public DateTime Fecha { get; set; }

     
        public DateTime Vencimiento { get; set; }

        public int Dias { get; set; }

        [StringLength(10)]
        public string TipoNombre { get; set; }


        [StringLength(100)]
        public string Detalle { get; set; }

        public decimal Valor { get; set; }        
        public decimal Saldo { get; set; }
        public decimal SaldoExport { get; set; }
        public decimal NuevoSaldo { get; set; }
        [StringLength(200)]
        public string Nombre { get; set; }
        [StringLength(25)]
        public string Numero { get; set; }

    }
}
