namespace USWsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ClienteOutputDto
    {
        public int Id { get; set; }


        [StringLength(13, MinimumLength =10)]
        public string RucCliente { get; set; }

        [StringLength(200)]
        public string Nombre { get; set; }

        [StringLength(2)]
        public string Clase { get; set; }

        [StringLength(10)]
        public string ClienteId { get; set; }

        [StringLength(10)]
        public string TerminoId { get; set; }
          
        public decimal Cupo { get; set; }

        public decimal Saldo { get; set; }

       

    }
}
