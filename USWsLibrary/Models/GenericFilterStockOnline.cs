using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USWsLibrary.Models
{
    public class GenericFilterStockOnline
    {

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string genericstring { get; set; }
        public string ProductID { get; set; }
        public string ProdCodigo { get; set; }
        public string EmpaCodigo { get; set; }

    }
}
