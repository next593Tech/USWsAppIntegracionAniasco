using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models.DashBoard
{
    public class SalesTopValues
    {
        public int Id { get; set; }

        public int TotalCustomerOfMonth { get; set; }

        public decimal PTotalCustomerOfMonth { get; set; }

        public decimal TotalExpensesOfMonth { get; set; }

        public decimal PTotalExpensesOfMonth { get; set; }

        public decimal TotalIncomeOfMonth { get; set; }

        public decimal PTotalIncomeOfMonth { get; set; }

        public decimal TotalSalesOfMonth { get; set; }

        public decimal PTotalSalesOfMonth { get; set; }

    }
}