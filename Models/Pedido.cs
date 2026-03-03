using System;
using System.Collections.Generic;

namespace MarketLocalShirts3.Models;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public int IdUsuario { get; set; }

    public DateTime Fecha { get; set; }
   
    /// Monto final calcula la compra  acordarme 
    public decimal Total { get; set; }

    public string TipoPago { get; set; } = null!;

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
