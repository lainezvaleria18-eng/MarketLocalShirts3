using Microsoft.AspNetCore.Mvc;
using MarketLocalShirts3.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace MarketLocalShirts3.Controllers
{
    public class CuentaController : Controller
    {
        private readonly MarketLocalShirts3Context _context;

        public CuentaController(MarketLocalShirts3Context context)
        {
            _context = context;
        }

        // =========================
        // LOGIN
        // =========================

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == Email && u.PasswordHash == Password);

            if (usuario == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nombre)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            return RedirectToAction("Catalogo", "Cliente");
        }

        // =========================
        // REGISTRO
        // =========================

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // =========================
        // RECUPERAR PASSWORD
        // =========================

        public IActionResult RecuperarPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GuardarPassword(string Email, string NuevaPassword, string ConfirmarPassword)
        {
            if (NuevaPassword != ConfirmarPassword)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View("RecuperarPassword");
            }

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == Email);

            if (usuario == null)
            {
                ViewBag.Error = "El correo no existe";
                return View("RecuperarPassword");
            }

            usuario.PasswordHash = NuevaPassword;

            _context.SaveChanges();

            ViewBag.Mensaje = "Contraseña actualizada correctamente";

            return RedirectToAction("Login");
        }

        // =========================
        // LOGOUT
        // =========================

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Inicio", "Cliente");
        }
    }
}