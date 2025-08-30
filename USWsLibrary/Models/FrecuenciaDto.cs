using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models
{
    public class FrecuenciaDto
    {
        public int ID { get; set; }
        public string ProductoId { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraLlegada { get; set; }
    }
}