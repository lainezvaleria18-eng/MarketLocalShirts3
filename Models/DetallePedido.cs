namespace MarketLocalShirts3.Models
{
    public class DetallePedido
    {
        public int IdDetallePedido { get; set; }

        public int IdPedido { get; set; }

        public int Id { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal { get; set; }

        public Pedido? Pedido { get; set; }

        public Producto? Producto { get; set; }
    }

}