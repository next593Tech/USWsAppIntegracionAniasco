using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models
{
    public class CLI_CLIENTE
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public string TipoIdCodigo { get; set; }

        public string TipoPersonaCodigo { get; set; }

        public string RucCliente { get; set; }

        public string Nombre { get; set; }

        public string Clase { get; set; }

        public string ClaseNombre { get; set; }

        public string ClienteId { get; set; }

        public string TerminoId { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        public decimal? Cupo { get; set; }

        public decimal? Saldo { get; set; }

        public string Ciudad { get; set; }

        public bool Movil { get; set; }

        public string ClaseContado { get; set; }

        public string TerminoContadoId { get; set; }

        public decimal? TasaDescuento { get; set; }

        public decimal? Longitud { get; set; }

        public decimal? Latitud { get; set; }
    }
}