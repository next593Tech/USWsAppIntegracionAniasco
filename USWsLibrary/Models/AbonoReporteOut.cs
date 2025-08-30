using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USWsLibrary.Models
{

    public class AbonoReporteOut 
    {
        public AbonoReporteOut()
        {
            DataSet01 = new List<Models.AbonoReporte01Out>();
            DataSet02 = new List<Models.AbonoReporte02Out>();
        }

        public List<AbonoReporte01Out> DataSet01 { get; set; }
        public List<AbonoReporte02Out> DataSet02 { get; set; } 

    }


    public  class AbonoReporte01Out
    {
        public string Id { get; set; }

        public string NombreEmpresa { get; set; }
        public string RucEmpresa { get; set; }

        public string NumDcto { get; set; }
        public string Concepto { get; set; }
        public string Tipo { get; set; }
        public DateTime Fecha { get; set; }
          
        public string Asiento { get; set; }
        public string Estado { get; set; }

        public string Codigo { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteEmail { get; set; }
        public string NumRecibo { get; set; }

        public string Banco { get; set; }

        public string Numero { get; set; }
        public string Cuenta { get; set; }
        public string NoCheque { get; set; }
        public string Girador { get; set; }
        public decimal DivisaID { get; set; }

        public decimal Valor { get; set; }
 
         
    }

    public class AbonoReporte02Out
    {
        public int Id { get; set; }

        public string NumDcto { get; set; }
        public string Concepto { get; set; }
        public string Tipo { get; set; }
        public DateTime Fecha { get; set; }

        public string Asiento { get; set; }
        public string Estado { get; set; }
        public string Detalle { get; set; }
        public DateTime Vencimiento { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal Abono { get; set; }
        public decimal Saldo { get; set; }
 


    }


}
