
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarketLocalShirts3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace MarketLocalShirts3.Controllers
{
    public class ClienteController : Controller
    {
        private readonly MarketLocalShirts3Context _context;

        public ClienteController(MarketLocalShirts3Context context)
        {
            _context = context;
        }

       
        public IActionResult Inicio()
        {
            return View();
        }

       
        [AllowAnonymous]
        public async Task<IActionResult> Catalogo(string buscar)
        {
            var productos = _context.Productos
                .Include(p => p.Marca)
                .AsQueryable();

            if (!string.IsNullOrEmpty(buscar))
            {
                productos = productos.Where(p => p.Nombre.Contains(buscar));
            }

            var lista = await productos.ToListAsync();

            ViewBag.Busqueda = buscar;

            return View(lista);
        }


        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string correo, string password, string? returnUrl = null)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == correo && u.PasswordHash == password);

            if (usuario == null)
            {
                ViewBag.Error = "Email o contraseña incorrectos";
                return View();
            }

            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.RolId.ToString())
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Catalogo");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction("Inicio");
        }

        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(Usuario usuario, string Direccion, string Telefono, string Ciudad)
        {
            usuario.RolId = 2;

            usuario.FechaRegistro = DateTime.Now;
            usuario.EsActivo = true;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            var cliente = new Cliente
            {
                UsuarioId = usuario.Id,
                Direccion = Direccion,
                Telefono = Telefono,
                Ciudad = Ciudad
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }
    }
}