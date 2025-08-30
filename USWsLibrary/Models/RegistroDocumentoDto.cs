namespace USWsLibrary.Models  
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RespuestaDocumentosDto")]
    public partial class RegistroDocumentoDto
    {
        public long Id { get; set; }

        [Required]
        [StringLength(3)]
        public string TipoDoc { get; set; }

        [StringLength(13)]
        public string Ruc { get; set; }

        [StringLength(200)]
        public string Nombre { get; set; }

        [StringLength(20)]
        public string Documento { get; set; }

        [StringLength(10)]
        public string FechaEmision { get; set; }

        [StringLength(10)]
        public string TipoFac { get; set; }

        [StringLength(50)]
        public string Autorizacion { get; set; }

        [StringLength(10)]
        public string FechaAutoriza { get; set; }

        [StringLength(30)]
        public string Total { get; set; }

        [StringLength(200)]
        public string EMAIL { get; set; }
    }
}
