using System.ComponentModel.DataAnnotations;

namespace MarketLocalShirts3.Models
{
    public class Rol
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = "";
    }

}