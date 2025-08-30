using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace USWsLibrary.Models
{
    public class ClienteAbonoInputDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 10)]
        public string CodCliente { get; set; }



    }
}