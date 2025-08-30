using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace USWsLibrary.Models
{
    public class ClienteDetalleNumero_InputDto
    {
        [Required]
        [StringLength(13, MinimumLength = 10)]
        public string CodCliente { get; set; }


        [Required]
        [StringLength(10)]
        public string IdClienteDeuda { get; set; }
    }
}