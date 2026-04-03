using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarketLocalShirts3.Models;
using Microsoft.AspNetCore.Authorization;
namespace MarketLocalShirts3.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        private readonly MarketLocalShirts3Context _context;

        public PedidosController(MarketLocalShirts3Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string buscar, DateTime? fecha)
        {
            var consulta = _context.Pedidos
                .Include(p => p.Usuario!)
                .AsQueryable();

            if (!string.IsNullOrEmpty(buscar))
            {
                consulta = consulta.Where(p => p.Usuario!.Nombre.Contains(buscar));
            }

            if (fecha.HasValue)
            {
                consulta = consulta.Where(p => p.FechaPedido.Date == fecha.Value.Date);
            }

            var pedidos = await consulta
                .OrderByDescending(p => p.FechaPedido)
                .Take(10)
                .ToListAsync();

            return View(pedidos);
        }


        public async Task<IActionResult> DetallePedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido != null)
            {
                ViewBag.IdPedido = pedido.Id;
                ViewBag.Fecha = pedido.FechaPedido.ToString("dd/MM/yyyy");
                ViewBag.Cliente = pedido.Usuario?.Nombre;
                ViewBag.Estado = pedido.Estado;
            }
            else
            {
                ViewBag.Fecha = DateTime.Now.ToString("dd/MM/yyyy"); // Fecha de respaldo
            }

            var detalles = await _context.PedidosDetalles
                .Include(d => d.Producto)
                .Where(d => d.PedidoId == id)
                .ToListAsync();

            return View(detalles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarEstado(int idPedido, string nuevoEstado)
        {
            var pedido = await _context.Pedidos.FindAsync(idPedido);

            if (pedido != null)
            {
                pedido.Estado = nuevoEstado;
                _context.Update(pedido);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}