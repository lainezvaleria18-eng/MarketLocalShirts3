using System.ComponentModel.DataAnnotations;

namespace MarketLocalShirts3.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(200, ErrorMessage = "La dirección no puede superar los 200 caracteres")]
        public string Direccion { get; set; } = "";

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres")]
        public string Telefono { get; set; } = "";

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        [StringLength(100, ErrorMessage = "La ciudad no puede superar los 100 caracteres")]
        public string Ciudad { get; set; } = "";

        public Usuario? Usuario { get; set; }
    }
}