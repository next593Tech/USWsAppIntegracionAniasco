using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace USWsLibrary.Models
{
    public partial class VEN_ORDENESDESPACHO_DTO
    {
        public string Id { get; set; }
        public  string ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public  string Detalle { get; set; }
        public string VendedorId { get; set; }
        public string TerminoId { get; set; }
        public string DivisionId { get; set; }
        public string Numero { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Entregado { get; set; }
        public string Tipo { get; set; }
        public string DivisaId { get; set; }
        [Column(TypeName = "decimal")]
        public decimal Cambio { get; set; }
        [Column(TypeName = "money")]
        public decimal Subtotal { get; set; }
        [Column(TypeName = "money")]
        public decimal Descuento { get; set; }
         [Column(TypeName = "money")]
        public decimal Impuesto { get; set; }
        [Column(TypeName = "money")]
        public decimal Total { get; set; }
        public string Nota { get; set; }
        public string Nota2 { get; set; }
        public string Estado { get; set; }
        public string PcId { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; } 
    }
}