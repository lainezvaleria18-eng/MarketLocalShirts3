using System.Collections.Generic;

namespace MarketLocalShirts3.Models
{
    public partial class Producto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public int IdMarca { get; set; }

        public virtual Marca IdMarcaNavigation { get; set; } = null!;
        public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();
    }
}