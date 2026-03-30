using System.ComponentModel.DataAnnotations;

namespace MarketLocalShirts3.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la playera es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "La descripción del producto es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio del producto es obligatorio")]
        [Range(0.01, 10000, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La cantidad de stock es obligatoria")]
        [Range(0, 10000, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        public string? Imagen { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una marca para el producto")]
        public int MarcaId { get; set; }

        public Marca? Marca { get; set; }
    }

}

