using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models.DashBoard
{
    public class SalesRankedDto
    {
        public string Name { get; set; }
        public decimal Total { get; set; }
        public int Ranking { get; set; }
    }
}