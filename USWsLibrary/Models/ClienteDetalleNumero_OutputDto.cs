namespace USWsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ClienteDetalleNumero_OutputDto
    {
        [StringLength(100)]
        public string Detalle { get; set; }

        public string Número { get; set; }

    }
}
