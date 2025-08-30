using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USWsLibrary.Models
{

    public class AbonoCargaOut  
    {
        public AbonoCargaOut()
        {
            Pagos = new List<Models.AbonoPagoOut>();
            Distribucion = new List<Models.AbonoDistribucionOut>();
        }

        public AbonoPagoCabOut Cab { get; set; } 

        public List<AbonoPagoOut> Pagos { get; set; } 
        public List<AbonoDistribucionOut> Distribucion { get; set; }

        public bool ExistError { get; set; }

        public string Text { get; set; }

        public  ErrorItem ErrorIfExist { get; set; }

    }



    public class AbonoPagoCabOut  
    {
        public string Id { get; set; }



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

    }


        public  class AbonoPagoOut
    {
        public string Id { get; set; }


        public string TipoId { get; set; }

        public string TipoNombre { get; set; }

        public string BancoId { get; set; }
        public string Cuenta { get; set; }
        public string NoCheque { get; set; }
        public string Girador { get; set; }
        public string DivisaId { get; set; }

        public string DivisaNombre { get; set; }

        public decimal DivisaValor { get; set; }

        public decimal Valor { get; set; }

        public decimal ValorBase { get; set; }


    }

    public class AbonoDistribucionOut
    {
        public string Id { get; set; }

        public string Detalle { get; set; }
        public DateTime Vencimiento { get; set; }


        public decimal Valor { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal Abono { get; set; }
        public decimal Saldo { get; set; }
 


    }


}
