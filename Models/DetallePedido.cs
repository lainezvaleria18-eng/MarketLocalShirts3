namespace MarketLocalShirts3.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }

        public int PedidoId { get; set; }

        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }

        public string? DescripcionProducto { get; set; }

        public Producto? Producto { get; set; }
    }

}