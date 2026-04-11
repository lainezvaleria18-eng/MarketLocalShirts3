using MarketLocalShirts3.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BCrypt.Net;

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
            
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Cliente)
                .FirstOrDefaultAsync(u => u.Email == Email);

            if (usuario != null)
            {
                bool esValido = false;

                try
                {
                    
                    esValido = BCrypt.Net.BCrypt.Verify(Password, usuario.PasswordHash ?? "");
                }
                catch (BCrypt.Net.SaltParseException)
                {
                   
                    esValido = (Password == usuario.PasswordHash);
                }

                
                if (esValido && (usuario.EsActivo == true))
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nombre ?? ""),
                new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "Usuario")
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    
                    HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
                    if (usuario.Cliente != null) HttpContext.Session.SetInt32("ClienteId", usuario.Cliente.Id);

                  
                    if (usuario.Rol?.Nombre == "Administrador") return RedirectToAction("Index", "Usuarios");
                    return RedirectToAction("Catalogo", "Cliente");
                }
            }

            ViewBag.Error = " Usuario o contraseña no validos";
            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(Usuario usuario)
        {
            
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash);
            usuario.EsActivo = true; 
            usuario.FechaRegistro = DateTime.Now;

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            var cliente = new Cliente { UsuarioId = usuario.Id };
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

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == Email);

            if (usuario == null)
            {
                ViewBag.Error = "El correo no existe";
                return View("RecuperarPassword");
            }

          
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(NuevaPassword);

            _context.SaveChanges();

            ViewBag.Mensaje = "Contraseña actualizada correctamente";
            return RedirectToAction("Login");
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear(); 
            return RedirectToAction("Inicio", "Cliente");
        }
    }
}