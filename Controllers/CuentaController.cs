using MarketLocalShirts3.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            var usuario = _context.Usuarios
               .Include(u => u.Cliente)
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


            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);


            return RedirectToAction("Catalogo", "Cliente");
        }
        
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            var cliente = new Cliente
            {
                UsuarioId = usuario.Id
            };

            _context.Clientes.Add(cliente);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        
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

        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Inicio", "Cliente");
        }
    }
}