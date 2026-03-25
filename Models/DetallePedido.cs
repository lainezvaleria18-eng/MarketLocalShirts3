using System.ComponentModel.DataAnnotations;

namespace MarketLocalShirts3.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El pedido es obligatorio")]
        public int PedidoId { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio")]
        public int ProductoId { get; set; }

        [Range(1, 1000, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Range(0.01, 10000, ErrorMessage = "El precio debe ser válido")]
        public decimal Precio { get; set; }

        [StringLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres")]
        public string? DescripcionProducto { get; set; }

        public Producto? Producto { get; set; }
        public Pedido? Pedido { get; set; }
    }
}