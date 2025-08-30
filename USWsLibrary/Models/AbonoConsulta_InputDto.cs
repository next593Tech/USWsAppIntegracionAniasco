using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace USWsLibrary.Models
{
    public class AbonoConsulta_InputDto
    {
        [Required]
        [StringLength(13, MinimumLength = 10)]
        public string CodCliente { get; set; }


        [Required]
        public DateTime Fecha { get; set; }


        [Required]
        [StringLength(10)]
        public string DivisaID { get; set; }


        //@ClienteID char(10),
        //@Fecha DateTime,
        //@DivisaID char(10)
    }
}