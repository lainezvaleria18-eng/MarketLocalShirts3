
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

       //Buscar por nombre y marca
        [AllowAnonymous]
        public async Task<IActionResult> Catalogo(string buscar, int? marcaId)
        {
            var productos = _context.Productos
                .Include(p => p.Marca)
                .AsQueryable();

            if (!string.IsNullOrEmpty(buscar))
            {
                productos = productos.Where(p => p.Nombre.Contains(buscar));
            }

            if (marcaId.HasValue && marcaId.Value > 0)
            {
                productos = productos.Where(p => p.MarcaId == marcaId.Value);
            }

            var lista = await productos.ToListAsync();

           
            ViewBag.Marcas = await _context.Marcas.ToListAsync();

           
            ViewBag.Busqueda = buscar;
            ViewBag.MarcaSeleccionada = marcaId;

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
                .Include(u => u.Cliente)
                 .FirstOrDefaultAsync(u => u.Email == correo && u.PasswordHash == password);

            if (usuario == null)
            {
                ViewBag.Error = "Email o contraseña incorrectos";
                return View();
            }

            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);

            if (usuario.Cliente != null)
            {
                HttpContext.Session.SetInt32("ClienteId", usuario.Cliente.Id);
            }

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
      
        [Authorize]
        public async Task<IActionResult> MisPedidos(string? buscar, DateTime? fecha)
        {
            var clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (clienteId == null)
            {
                return RedirectToAction("Login", "Cliente");
            }

            var pedidos = _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
                .Where(p => p.ClienteId == clienteId.Value)
                .AsQueryable();

            //  FILTRO POR NOMBRE
            if (!string.IsNullOrEmpty(buscar))
            {
                pedidos = pedidos.Where(p =>
                    p.Detalles.Any(d =>
                        d.Producto != null &&
                        d.Producto.Nombre.Contains(buscar)));
            }

            // FILTRO POR FECHA
            if (fecha.HasValue)
            {
                pedidos = pedidos.Where(p =>
                    p.FechaPedido.Date == fecha.Value.Date);
            }

            return View(await pedidos.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> DetallePedido(int id)
        {
            var detalles = await _context.PedidosDetalles
                .Include(d => d.Producto)
                .Where(d => d.PedidoId == id)
                .ToListAsync();

            return View(detalles);
        }
    }
}