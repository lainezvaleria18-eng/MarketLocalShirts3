using System.ComponentModel.DataAnnotations;

namespace MarketLocalShirts3.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = "";

        [Required]
        public string Descripcion { get; set; } = "";

        [Range(0.01, 10000)]
        public decimal Precio { get; set; }

        [Range(0, 10000)]
        public int Stock { get; set; }

        public string Imagen { get; set; } = "";

        [Required]
        public int MarcaId { get; set; }

        public Marca? Marca { get; set; }
    }

}