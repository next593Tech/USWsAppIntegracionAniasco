using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models.DashBoard
{
	public class SalesByDayDto
	{
        public int DayID { get; set; }
        [StringLength(10)]
        public string DayName { get; set; }
        public decimal Total { get; set; }
        public int Invoices { get; set; }
    }
}