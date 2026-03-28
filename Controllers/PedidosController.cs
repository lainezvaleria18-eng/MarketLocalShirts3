using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarketLocalShirts3.Models;

namespace MarketLocalShirts3.Controllers
{
    public class PedidosController : Controller
    {
        private readonly MarketLocalShirts3Context _context;

        public PedidosController(MarketLocalShirts3Context context)
        {
            _context = context;
        }

        // 📋 LISTAR TODOS LOS PEDIDOS
        public async Task<IActionResult> Index()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Usuario)
                .ToListAsync();

            return View(pedidos);
        }

        // 📦 VER DETALLE DEL PEDIDO
        public async Task<IActionResult> Detalle(int id)
        {
            var detalles = await _context.PedidosDetalles
                .Include(d => d.Producto)
                .Where(d => d.PedidoId == id)
                .ToListAsync();

            return View(detalles);
        }
    }
}