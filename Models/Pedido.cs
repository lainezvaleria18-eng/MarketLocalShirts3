using System.ComponentModel.DataAnnotations;

namespace MarketLocalShirts3.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        public DateTime FechaPedido { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; } = "";

        public Usuario? Usuario { get; set; }

        public List<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
    }

}