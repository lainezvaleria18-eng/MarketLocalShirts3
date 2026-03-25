using System.ComponentModel.DataAnnotations;

namespace MarketLocalShirts3.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres")]
        public string Descripcion { get; set; } = "";

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(0.01, 10000, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(0, 10000, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        public string Imagen { get; set; } = "";

        [Required(ErrorMessage = "La marca es obligatoria")]
        public int MarcaId { get; set; }

        public Marca? Marca { get; set; }
    }

}
