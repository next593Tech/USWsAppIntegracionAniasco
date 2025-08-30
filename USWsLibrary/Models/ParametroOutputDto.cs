namespace USWsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   
    public partial class ParametroOutputDto 
    {

        [StringLength(10)]
        public string Id { get; set; }

        [StringLength(50)]
        public string Codigo { get; set; }

        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(10)]
        public string Tipo { get; set; }

        [StringLength(100)]
        public string Valor { get; set; }

 

    }
}
