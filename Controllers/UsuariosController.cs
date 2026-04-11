using MarketLocalShirts3.Models;
using MarketLocalShirts3.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MarketLocalShirts3.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly MarketLocalShirts3Context _context;

        public UsuariosController(MarketLocalShirts3Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string buscar, int pagina = 1)
        {
            int registrosPorPagina = 5;
            var usuarios = _context.Usuarios
                   .Include(u => u.Rol)
                   .Include(u => u.Cliente)
                   .AsQueryable();

            if (!string.IsNullOrEmpty(buscar))
            {
                usuarios = usuarios.Where(u =>
                    u.Nombre.Contains(buscar) ||
                    u.Email.Contains(buscar));
            }

            int totalRegistros = await usuarios.CountAsync();

            var listaUsuarios = await usuarios
                .OrderBy(u => u.Id)
                .Skip((pagina - 1) * registrosPorPagina)
                .Take(registrosPorPagina)
                .ToListAsync();

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);
            ViewBag.Buscar = buscar;

            return View(listaUsuarios);
        }

        public IActionResult Create()
        {
            ViewBag.RolId = new SelectList(_context.Roles, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = new Usuario
                {
                    Nombre = model.Nombre,
                    Email = model.Email,
                    PasswordHash = model.PasswordHash,
                    RolId = model.RolId,
                    FechaRegistro = DateTime.Now,
                    EsActivo = true
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                var cliente = new Cliente
                {
                    UsuarioId = usuario.Id,
                    Direccion = model.Direccion,
                    Telefono = model.Telefono,
                    Ciudad = model.Ciudad
                };

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewBag.RolId = new SelectList(_context.Roles, "Id", "Nombre", model.RolId);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Cliente)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null) return NotFound();

            ViewBag.RolId = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Usuario usuario)
        {
            var usuarioDb = await _context.Usuarios
                .Include(u => u.Cliente)
                .FirstOrDefaultAsync(u => u.Id == usuario.Id);

            if (usuarioDb == null) return NotFound();

            usuarioDb.Nombre = usuario.Nombre;
            usuarioDb.Email = usuario.Email;
            usuarioDb.RolId = usuario.RolId;

            if (usuarioDb.Cliente != null && usuario.Cliente != null)
            {
                usuarioDb.Cliente.Direccion = usuario.Cliente.Direccion;
                usuarioDb.Cliente.Telefono = usuario.Cliente.Telefono;
                usuarioDb.Cliente.Ciudad = usuario.Cliente.Ciudad;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                var pedidosIds = await _context.Pedidos
                    .Where(p => p.UsuarioId == id)
                    .Select(p => p.Id)
                    .ToListAsync();

                if (pedidosIds.Any())
                {
                    var detalles = await _context.PedidosDetalles
                        .Where(d => pedidosIds.Contains(d.PedidoId))
                        .ToListAsync();

                    if (detalles.Any())
                    {
                        _context.PedidosDetalles.RemoveRange(detalles);
                    }

                  
                    var pedidos = await _context.Pedidos
                        .Where(p => p.UsuarioId == id)
                        .ToListAsync();

                    _context.Pedidos.RemoveRange(pedidos);
                }

                var cliente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.UsuarioId == id);

                if (cliente != null)
                {
                    _context.Clientes.Remove(cliente);
                }

                
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Bloquear(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario != null)
            {
                usuario.EsActivo = !usuario.EsActivo;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Inicio", "Cliente");
        }
    }

