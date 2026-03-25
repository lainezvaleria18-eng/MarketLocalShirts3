using System.ComponentModel.DataAnnotations;

namespace MarketLocalShirts3.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido")]
        [StringLength(150, ErrorMessage = "El correo no puede superar los 150 caracteres")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(200, ErrorMessage = "La contraseña es demasiado larga")]
        public string PasswordHash { get; set; } = "";

        [Required(ErrorMessage = "El rol es obligatorio")]
        public int RolId { get; set; }

        public bool EsActivo { get; set; }

        [Required(ErrorMessage = "La fecha de registro es obligatoria")]
        public DateTime FechaRegistro { get; set; }

        public Rol? Rol { get; set; }

        public Cliente? Cliente { get; set; }
    }

}
