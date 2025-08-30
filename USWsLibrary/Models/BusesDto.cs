using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models
{
    public class BusesDto
    {
       
        public string Codigo { get; set; }
        public string Disco { get; set; }
        public string Placa { get; set; }
        public int CantidadAsientos { get; set; }
        public bool Activo { get; set; }
    }
}