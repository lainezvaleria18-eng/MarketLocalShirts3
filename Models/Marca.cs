using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketLocalShirts3.Models
{
    public class Marca
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la marca es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Nombre { get; set; } = "";

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
