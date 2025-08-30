using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models.DashBoard
{
    public class SalesByHourDto
    {
        public string Hour { get; set; }
        public decimal Total { get; set; }
        public int Invoices { get; set; }
    }
}