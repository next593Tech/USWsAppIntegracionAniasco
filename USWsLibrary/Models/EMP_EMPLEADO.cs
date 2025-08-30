using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models
{
    public class EMP_EMPLEADO
    {
        public int Id { get; set; }
        public string EmpleadoId { get; set; }
        public string Nombre { get; set; } 
        public string UserName { get; set; }
        public string Code { get; set; }
        public string Clase { get; set; }
        public bool ControlarStock { get; set; }
        public string BodegaId { get; set; }
  

    }
}