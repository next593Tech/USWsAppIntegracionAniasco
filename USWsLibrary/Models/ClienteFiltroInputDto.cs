using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace USWsLibrary.Models
{
    public class ClienteFiltroInputDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        [StringLength(200)]
        public string Nombre { get; set; }

        [StringLength(25)]
        public string Username { get; set; }


    }
}