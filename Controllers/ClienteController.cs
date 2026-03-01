#nullable disable
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

        [Authorize]
        public async Task<IActionResult> Catalogo()
        {
            var productos = await _context.Productos
                .Include(p => p.IdMarcaNavigation)
                .ToListAsync();

            return View(productos);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string password)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, correo),
                new Claim(ClaimTypes.Email, correo)
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

            return RedirectToAction("Catalogo");
        }

        public IActionResult Registro()
        {
            return View();
        }
    }
}