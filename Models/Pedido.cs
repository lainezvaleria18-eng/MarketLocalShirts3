using System.ComponentModel.DataAnnotations;

namespace MarketLocalShirts3.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "La fecha del pedido es obligatoria")]
        public DateTime FechaPedido { get; set; }

        [Required(ErrorMessage = "El estado del pedido es obligatorio")]
        [StringLength(50, ErrorMessage = "El estado no puede superar los 50 caracteres")]
        public string Estado { get; set; } = "";

        public Usuario? Usuario { get; set; }

        public List<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
    }

}
