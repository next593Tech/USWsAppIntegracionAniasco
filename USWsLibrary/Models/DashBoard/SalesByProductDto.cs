using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models.DashBoard
{
    public class SalesByProductDto
    {
        [StringLength(200)]
        public string Name { get; set; }
        public decimal Total { get; set; }
        public int Ranking { get; set; }
    }
}