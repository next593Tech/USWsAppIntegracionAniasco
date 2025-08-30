namespace USWsLibrary.Models
{
    public class FiltroPedido
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string ClienteId { get; set; }

        public string ClienteNombre { get; set; }

        public string VendedorUserName { get; set; } 

        public string EstadoId { get; set; } 

        public int Dias { get; set; }
    }
}