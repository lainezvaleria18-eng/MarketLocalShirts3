using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarketLocalShirts3.Models;

namespace MarketLocalShirts3.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly MarketLocalShirts3Context _context;

        public UsuariosController(MarketLocalShirts3Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .ToListAsync();

            return View(usuarios);
        }

        public IActionResult Create()
        {
            ViewBag.IdRol = new SelectList(_context.Rols, "IdRol", "NombreRol");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            ViewBag.IdRol = new SelectList(_context.Rols, "IdRol", "NombreRol", usuario.IdRol);
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Usuario usuario)
        {
            var usuarioDb = await _context.Usuarios.FindAsync(usuario.IdUsuario);
            if (usuarioDb == null) return NotFound();

            usuarioDb.Nombre = usuario.Nombre;
            usuarioDb.Correo = usuario.Correo;
            usuarioDb.Contrasena = usuario.Contrasena;
            usuarioDb.IdRol = usuario.IdRol;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}