using System.ComponentModel.DataAnnotations;

namespace MarketLocalShirts3.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }

        [Required]
        public int PedidoId { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Range(1, 1000)]
        public int Cantidad { get; set; }

        [Range(0.01, 10000)]
        public decimal Precio { get; set; }

        public string? DescripcionProducto { get; set; }

        public Producto? Producto { get; set; }
        public Pedido? Pedido { get; set; }
    }
}