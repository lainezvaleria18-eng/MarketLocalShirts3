using System.ComponentModel.DataAnnotations;

namespace MarketLocalShirts3.Models
{
    public class CarritoItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "La marca es obligatoria")]
        [StringLength(100, ErrorMessage = "La marca no puede superar los 100 caracteres")]
        public string Marca { get; set; } = "";

        [Range(0.01, 10000, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Range(1, 1000, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int Cantidad { get; set; }

        public string Imagen { get; set; } = "";

        public decimal Subtotal => Precio * Cantidad;
    }
}
