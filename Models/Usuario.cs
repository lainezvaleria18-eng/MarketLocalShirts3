using System.ComponentModel.DataAnnotations;

namespace MarketLocalShirts3.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; } = "";

        public string Email { get; set; } = "";

        public string PasswordHash { get; set; } = "";

        public int RolId { get; set; }

        public bool EsActivo { get; set; }

        public DateTime FechaRegistro { get; set; }

        public Rol? Rol { get; set; }

        public Cliente? Cliente { get; set; }
    }

}