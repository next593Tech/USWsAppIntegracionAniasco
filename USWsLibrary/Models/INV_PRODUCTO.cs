using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace USWsLibrary.Models
{
    public class INV_PRODUCTO
    {
        public int Id { get; set; }
         
        [StringLength(10)]
        public string ProductoId { get; set; }

        [StringLength(25)]
        public string Codigo { get; set; }

        [StringLength(2)] 
        public string Clase { get; set; }


        [StringLength(200)] 
        public string Nombre { get; set; }
           
        [StringLength(50)] 
        public string Descripcion  { get; set; } 
         
        [StringLength(200)] 
        public string Foto { get; set; }

        public string Empaque { get; set; }

        public string CodEmpaque { get; set; }


        public decimal Costo { get; set; }

        public decimal Factor { get; set; }

        public decimal PrecioClase01 { get; set; }

        public decimal PrecioClase02 { get; set; }

        public decimal PrecioClase03 { get; set; }

        public decimal PrecioClase04 { get; set; }

        public decimal PrecioClase05 { get; set; }

        public decimal PrecioClase06 { get; set; }

        public decimal PrecioClase07 { get; set; }

        public decimal PrecioClaseDefault { get; set; }
         
        public decimal  Stock { get; set; }

        public bool AIVA { get; set; }

        public decimal PIVA { get; set; }
   
        public bool AICE { get; set; }

        public decimal PICE { get; set; } 

        public bool AVERDE { get; set; }

        public decimal PVERDE { get; set; }

        public DateTime PromocionFechaInicio { get; set; }
        public DateTime PromocionFechaFinal { get; set; }

  
    }
}