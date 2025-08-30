namespace USWsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AbonoConsulta_OutputDto
    {
        public DateTime Fecha { get; set; }
        public DateTime Vencimiento { get; set; }
        public decimal Días { get; set; }
        public string Tipo { get; set; }
        public string Número { get; set; }
        public string Detalle { get; set; }
        public string Divisa { get; set; }
        public double Valor { get; set; }
        public double Saldo { get; set; }
        public string Nombre { get; set; }
        public string DocumentoID { get; set; }
        public string Código { get; set; }
        public string Orden { get; set; }
        public bool Crédito { get; set; }
        public string ClienteID { get; set; }
        public bool SortOrder { get; set; }
        public string Section { get; set; }
        public DateTime SortDate { get; set; }
        public string SortID { get; set; }
        public bool SortType { get; set; }

        //public Secuencia { get; set; }

    }
}
