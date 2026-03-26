using System.ComponentModel.DataAnnotations;

namespace MarketLocalShirts3.ViewModels
{
    public class UsuarioViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string PasswordHash { get; set; } = "";

        [Required(ErrorMessage = "El rol es obligatorio")]
        public int RolId { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; } = "";

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public string Telefono { get; set; } = "";

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        public string Ciudad { get; set; } = "";
    }
}
