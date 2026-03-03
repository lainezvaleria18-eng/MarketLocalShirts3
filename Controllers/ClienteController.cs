
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
        public async Task<IActionResult> Catalogo()
        {
            var productos = await _context.Productos
                .Include(p => p.IdMarcaNavigation)
                .ToListAsync();

            return View(productos);
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
                .FirstOrDefaultAsync(u => u.Correo == correo && u.Contrasena == password);

            if (usuario == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.IdRol.ToString())
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
        public async Task<IActionResult> Registro(Usuario usuario)
        {
            usuario.IdRol = 2;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }
    }
}