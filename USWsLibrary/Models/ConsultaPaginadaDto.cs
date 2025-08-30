using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models
{
    public class PagedList <T> //salida
    {
        public List<T> Results { get; set; }
        public int Count { get; set; }

        public double Total { get; set; }

        public DateTime LastUpdate { get; set; }

        public List<String> id { get; set; }
    }
}