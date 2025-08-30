using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace USWsLibrary.Models
{
    public class GenericFilterInp
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string genericstring { get; set; } 


    }
}