using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class FAC_DTO
    {

        [Required]
        [StringLength(10)]
        [XmlAttribute]
        public string ID { get; set; }

        [Required]
        [StringLength(10)]
        [XmlAttribute]
        public string Número { get; set; }


        [Required]
        [StringLength(10)]
        [XmlAttribute]
        public string ClienteID { get; set; }


        [StringLength(400)]
        [XmlAttribute]
        public string Nota { get; set; }



        [StringLength(50)]
        [XmlAttribute]
        public string Detalle { get; set; }




        [StringLength(13)]
        [XmlAttribute]
        public string ClienteRUC { get; set; }

        [StringLength(2)]
        [XmlAttribute]
        public string ClienteClase { get; set; }

        [XmlAttribute]
        public decimal Factor { get; set; }


        [StringLength(200)]
        [XmlAttribute]
        public string ClienteNombre { get; set; }



        [StringLength(10)]
        [XmlAttribute]
        public string VendedorID { get; set; }


        [XmlAttribute]
        [StringLength(15)]
        public string VendedorUsername { get; set; }


        [XmlAttribute]
        [StringLength(15)]
        public string NombreUsuario { get; set; }


        [StringLength(10)]
        [XmlAttribute]
        public string BodegaID { get; set; }

        [StringLength(10)]
        [XmlAttribute]
        public string TérminoID { get; set; }


        [StringLength(50)]
        [XmlAttribute]
        public string Secuencia { get; set; }

        [XmlAttribute]
        public int SecuenciaValor { get; set; }


        [StringLength(20)]
        [XmlAttribute]
        public string Serie { get; set; }

        [XmlAttribute]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Descuento { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Impuesto { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal SubtotalIvaCero { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal SubtotalIva { get; set; }

        [XmlAttribute]
        public decimal ImpuestoIce { get; set; }


        [XmlAttribute]
        public decimal ImpuestoVerde { get; set; }

        [XmlAttribute]
        public decimal SubtotalImpuestoIva { get; set; }

        [XmlAttribute]
        public decimal SubtotalImpuestoIce { get; set; }

        [XmlAttribute]
        public decimal SubtotalImpuestoVerde { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Total { get; set; }


        [StringLength(10)]
        [XmlAttribute]
        public string Estado { get; set; }


        [StringLength(50)]
        [XmlAttribute]
        public string PcID { get; set; }

        [StringLength(50)]
        [XmlAttribute]
        public string CajaId { get; set; }

        [XmlAttribute]
        public bool Contado { get; set; }

        [XmlAttribute]
        public bool PagoCredito { get; set; }


        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Efectivo { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Cheque { get; set; }

        [Column(TypeName = "money")]
        [XmlAttribute]
        public decimal Tarjeta { get; set; }


        [XmlAttribute]
        public decimal Crédito { get; set; }


        [StringLength(50)]
        [XmlAttribute]
        public string Banco { get; set; }



        [XmlAttribute]
        public DateTime Fecha_Cheque { get; set; }


        [StringLength(50)]
        [XmlAttribute]
        public string NoCuenta { get; set; }


        [StringLength(10)]
        [XmlAttribute]
        public string NoCheque { get; set; }


        [StringLength(50)]
        [XmlAttribute]
        public string Nombre_Tarjeta { get; set; }


        [XmlAttribute]
        public DateTime Vence { get; set; }

        [StringLength(50)]
        [XmlAttribute]
        public string NoTarjeta { get; set; }

        [StringLength(50)]
        [XmlAttribute]
        public string Autorización { get; set; }

        [StringLength(50)]
        [XmlAttribute]
        public string SRI_ACK { get; set; }

        [XmlAttribute]
        public decimal Saldo { get; set; }

        [XmlAttribute]
        public bool Exportado { get; set; }

        [XmlAttribute]
        public decimal RetencionFuente { get; set; }

        [XmlAttribute]
        public decimal RetencionIva { get; set; }


        [XmlAttribute]
        public double Latitud { get; set; }

        [XmlAttribute]
        public double Longitud { get; set; }

        [XmlAttribute]
        public bool Anulado { get; set; }

        [XmlAttribute]
        public bool EsPasajeFactura { get; set; }

        [XmlAttribute]
        public string NumPlaca { get; set; }

        [XmlAttribute]
        public string  MovUniqueId { get; set; }

        public virtual List<FAC_DT_DTO> LIST_FACT_DT { get; set; }

        public virtual List<CLI_RETENCION_DTO> LIST_RETENCIONES { get; set; }

        public virtual List<PasajeroDto> LIST_PASAJEROS_DTO { get; set; }

        public virtual List<FilterIdReservation> LIST_RESERVAID { get; set; }
    }
}