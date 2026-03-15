using System.Collections.Generic;

namespace MarketLocalShirts3.Models
{
    public class Marca
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = "";

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
