using MarketLocalShirts3.Models;
using MarketLocalShirts3.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization; 
namespace MarketLocalShirts3.Controllers
{
    
    [Authorize(Roles = "Administrador")]
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
                   
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash),
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

           
            if (!string.IsNullOrEmpty(usuario.PasswordHash))
            {
                if (!usuario.PasswordHash.StartsWith("$2a$"))
                {
                    usuarioDb.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash);
                }
                else
                {
                    usuarioDb.PasswordHash = usuario.PasswordHash;
                }
            }

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

            if (usuario == null) return NotFound();

            return View(usuario);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario != null)
            {
                
                try
                {
                 
                    await _context.Database.ExecuteSqlRawAsync(
                        "DELETE FROM PedidosDetalles WHERE PedidoId IN (SELECT Id FROM Pedidos WHERE ClienteId IN (SELECT Id FROM Clientes WHERE UsuarioId = {0}))", id);

                    await _context.Database.ExecuteSqlRawAsync(
                        "DELETE FROM Pedidos WHERE ClienteId IN (SELECT Id FROM Clientes WHERE UsuarioId = {0})", id);

                  
                    await _context.Database.ExecuteSqlRawAsync(
                        "DELETE FROM Clientes WHERE UsuarioId = {0}", id);
                }
                catch (Exception)
                {
                    
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

}