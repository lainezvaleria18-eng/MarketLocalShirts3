using System.ComponentModel.DataAnnotations;

namespace MarketLocalShirts3.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string PasswordHash { get; set; } = "";

        [Required]
        public int RolId { get; set; }

        public bool EsActivo { get; set; }

        public DateTime FechaRegistro { get; set; }

        public Rol? Rol { get; set; }

        public Cliente? Cliente { get; set; }
    }

}