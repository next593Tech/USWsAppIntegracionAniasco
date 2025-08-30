namespace USWsLibrary.Models  
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataModel : DbContext
    {
        public DataModel()
            : base("name=DefaultConnection")

        {
            this.Database.CommandTimeout = 180;
        }

        public virtual DbSet<RegistroDocumentoDto> ColeccionDocumentosDto { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegistroDocumentoDto>()
                .Property(e => e.TipoDoc)
                .IsUnicode(false);

            modelBuilder.Entity<RegistroDocumentoDto>()
                .Property(e => e.Ruc)
                .IsUnicode(false);

            modelBuilder.Entity<RegistroDocumentoDto>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<RegistroDocumentoDto>()
                .Property(e => e.Documento)
                .IsUnicode(false);

            modelBuilder.Entity<RegistroDocumentoDto>()
                .Property(e => e.FechaEmision)
                .IsUnicode(false);

            modelBuilder.Entity<RegistroDocumentoDto>()
                .Property(e => e.TipoFac)
                .IsUnicode(false);

            modelBuilder.Entity<RegistroDocumentoDto>()
                .Property(e => e.Autorizacion)
                .IsUnicode(false);

            modelBuilder.Entity<RegistroDocumentoDto>()
                .Property(e => e.FechaAutoriza)
                .IsUnicode(false);

            modelBuilder.Entity<RegistroDocumentoDto>()
                .Property(e => e.Total)
                .IsUnicode(false);

            modelBuilder.Entity<RegistroDocumentoDto>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);
        }
    }
}
