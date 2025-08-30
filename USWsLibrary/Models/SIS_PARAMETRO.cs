using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models
{
    public class SIS_PARAMETRO
    {

        public int Id { get; set; }

        [StringLength(10)]
        public string ParametroId { get; set; }

        [StringLength(50)]
        public string Codigo { get; set; }

        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(10)]
        public string Tipo { get; set; }

        [StringLength(100)]
        public string Valor { get; set; }

        [StringLength(10)]
        public string PadreId { get; set; }

        [StringLength(50)]
        public string PadreCodigo { get; set; }
    }
}