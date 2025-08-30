 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USWsLibrary.Models
{
    public class VEN_ORDENESDESPACHO_DT_DTO_PRINT 
    {

        public string NombreEmpresa { get; set; }

        public string RucEmpresa { get; set; }

        [StringLength(10)]
        public string ID { get; set; }

       
        [StringLength(10)]
        public string Número { get; set; }


        [StringLength(400)]
        public string Nota { get; set; }

        public string Estado { get; set; }


        [StringLength(10)]
        public string ClienteID { get; set; }

        public string ClienteNombre { get; set; }

        public string ClienteEmail { get; set; } 

        [StringLength(10)]
        public string VendedorID { get; set; }

        [StringLength(15)]
        public string VendedorUsername { get; set; }

        public string VendedorNombre { get; set; }

       
        [StringLength(10)]
        public string TerminoID { get; set; }

        public string TerminoNombre { get; set; }

        public DateTime Fecha { get; set; }

        public decimal SubtotalG { get; set; }

        public decimal DescuentoG { get; set; }

        public decimal ImpuestoG { get; set; }

        public decimal ImpuestoIceG { get; set; }

        public decimal ImpuestoVerdeG { get; set; }

        public decimal TotalG { get; set; }

          

        [StringLength(10)]
        public string DETAILID { get; set; }

        [StringLength(10)]
        public string OrdenID { get; set; }

        [Required]
        [StringLength(10)]
        public string ProductoID { get; set; }

        [StringLength(200)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string Descripcion { get; set; }

        public decimal Stock { get; set; }
        
        public decimal Cantidad { get; set; }
        
        public decimal Precio { get; set; }
        
        public decimal Costo { get; set; }
        
        public decimal Subtotal { get; set; }
        
        public decimal TasaDescuento { get; set; }
        
        public decimal Descuento { get; set; }
        
        public decimal TasaImpuesto { get; set; }
        
        public decimal Impuesto { get; set; }
        
        public decimal TasaImpuestoVerde { get; set; }
        
        public decimal ImpuestoVerde { get; set; }
        
        public decimal TasaImpuestoIce { get; set; }
        
        public decimal ImpuestoIce { get; set; }
        
        public decimal Total { get; set; }

        [StringLength(2)]
        public string Clase { get; set; }

        [StringLength(40)]
        public string Empaque { get; set; }
        
        public bool Embarque { get; set; }

        [StringLength(40)]
        public string PrecioName { get; set; }
        
        public decimal Factor { get; set; }
        
        public decimal SaldoAcum { get; set; }

        public bool Promocion { get; set; }

        public bool AIVA { get; set; }
        
        public bool AICE { get; set; }
        
        public bool AVER { get; set; }

          
    }
}
