using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketLocalShirts3.Models
{
    public class Marca
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = "";

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
