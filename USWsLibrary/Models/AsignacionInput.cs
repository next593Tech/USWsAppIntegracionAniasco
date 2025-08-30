using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USWsLibrary.Models
{
    public class AsignacionInput
    {

        public string Estado { get; set; } 
        public string Usuario { get; set; }
        public string AsignacionID { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
    }
}
