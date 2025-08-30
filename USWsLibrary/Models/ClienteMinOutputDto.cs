namespace USWsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ClienteMinOutputDto 
    {
        [StringLength(13, MinimumLength =10)]
        public string RucCliente { get; set; }

       [StringLength(200)]
        public string Nombre { get; set; }       

    }
}
