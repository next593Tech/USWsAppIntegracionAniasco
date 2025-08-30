namespace USWsLibrary.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public   class ClienteMinInputDto 
    {
        [StringLength(10)]
        public string Id { get; set; }

        [StringLength(200)]
        public string Nombre { get; set; }

        [StringLength(10)]
        public string ClaseCodigo { get; set; }

        [StringLength(2)]
        public string TipoPersonaCodigo { get; set; }

        [StringLength(10)]
        public string TipoIdentificacionCodigo { get; set; }


        [StringLength(10)]
        public string SecuenciaId { get; set; }
          

        [StringLength(10)]
        public string Vendedorid { get; set; }


        [StringLength(13)]
        public string CedulaRucPass { get; set; }
          

        [StringLength(100)]
        public string Direccion { get; set; }


        [StringLength(200)]
        public string Telefono  { get; set; }


        [StringLength(200)]
        public string Email { get; set; }

        
        public double Latitud { get; set; }

        
        public double Longitud { get; set; }


        public DateTime FechaHora { get; set; }

       
        public string MovUniqueId { get; set; }


    }
}
