using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USWsLibrary.Models
{
   public  class ClienteMinInputDtoGrabar: ClienteMinInputDto
    {
        public string Ciudad { get; set; }
        public string Provincia { get; set; }
        public string Razonsocial { get; set; }

        public string TipoNegocioID { get; set; }

        public string Telefono2 { get; set; }
    }
}
