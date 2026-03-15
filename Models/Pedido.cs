namespace MarketLocalShirts3.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public DateTime Fecha { get; set; }

        public decimal Total { get; set; }

        public string MetodoPago { get; set; } = "";
    }
}
