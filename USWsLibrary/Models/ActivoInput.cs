using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USWsLibrary.Models
{
    public class ActivoInput
    {

        public string AcitvoID { get; set; }
        public string Edificio { get; set; }
        public string Piso { get; set; }
        public string Room { get; set; }
        public string Receptor { get; set; }

        public int HorasUsadas { get; set; }
    }
}
