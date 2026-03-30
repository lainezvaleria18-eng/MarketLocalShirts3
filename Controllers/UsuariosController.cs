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

        public async Task<IActionResult> Index(string buscar)
        {
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

            return View(await usuarios.ToListAsync());
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
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            ViewBag.RolId = new SelectList(_context.Roles, "Id", "Nombre", usuario.RolId);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Usuario usuario)
        {
            var usuarioDb = await _context.Usuarios.FindAsync(usuario.Id);
            if (usuarioDb == null) return NotFound();

            usuarioDb.Nombre = usuario.Nombre;
            usuarioDb.Email = usuario.Email;
            usuarioDb.PasswordHash = usuario.PasswordHash;
            usuarioDb.RolId = usuario.RolId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
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
    }
}