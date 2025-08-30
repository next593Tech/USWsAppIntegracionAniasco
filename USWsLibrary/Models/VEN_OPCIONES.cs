using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace USWsLibrary.Models
{
    public class VEN_OPCIONES
    {
        
        public string POS_CAJA_ID { get; set; }
     
        public string POS_BODEGA_ID { get; set; }
        public string POS_SUCURSALID { get; set; }
         

        public string POS_SUCURSALNOMBRE { get; set; }

        public string POS_SUCURSALDIRECCION { get; set; }

        public string POS_SUCURSALTELEFONOS { get; set; }



         public string POS_TIPO_DOCUMENTO { get; set; }

        public string POS_CAJERO_ID { get; set; }

        public string POS_SUPERVISOR_ID { get; set; }
        public int POS_MAX_ITEMS { get; set; }
        public decimal POS_MAX_DINERO_CAJA { get; set; }
        public bool POS_CONTROLAR_STOCK { get; set; }
        public bool POS_PRINT_AUTOMATIC { get; set; }
        public bool POS_ADD_PRODUCTS { get; set; }
        [MaxLength(50)]
        public string POS_SERIE_FACTURA { get; set; }
        [MaxLength(50)]
        public string POS_SECUENCIA_NVENTA { get; set; }
        public string POS_SECUENCIA_DEVOLUCION { get; set; }
        public bool POS_PRINT_BLUETOOTH { get; set; }
        public bool POS_PRINT_WIRELESS { get; set; }
        public bool POS_PRINT_TMU220D { get; set; }
        public int POS_CANTIDAD_CARACTERES { get; set; }
        [MaxLength(50)]
        public string POS_BLUETOOTH_MAC { get; set; }
        public string POS_DIRECCION_FISICA_BLUETOOTH { get; set; }
        public string IPSERVER_WIRELESS { get; set; }
        public int PORTSERVER_WIRELESS { get; set; }
        public string POS_EPSON_MAC { get; set; }
        public string POS_ENVIROMENT { get; set; }

        public bool POS_IMPRIME_COPIA { get; set; }

    }
}