using System.ComponentModel.DataAnnotations;

namespace MarketLocalShirts3.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        [StringLength(200)]
        public string Direccion { get; set; } = "";

        [Required]
        [StringLength(20)]
        public string Telefono { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string Ciudad { get; set; } = "";

        public Usuario? Usuario { get; set; }
    }
}