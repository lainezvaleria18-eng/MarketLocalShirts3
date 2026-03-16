namespace MarketLocalShirts3.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public DateTime FechaPedido { get; set; }

        public string Estado { get; set; } = "";  
    }
}
