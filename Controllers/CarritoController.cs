#nullable disable
using Microsoft.AspNetCore.Mvc;
using MarketLocalShirts3.Models;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace MarketLocalShirts3.Controllers
{

    [Authorize]
    public class CarritoController : Controller
    {
        private readonly MarketLocalShirts3Context _context;

        public CarritoController(MarketLocalShirts3Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UsuarioId") == null)
            {
                return RedirectToAction("Login", "Cliente");
            }
            return View(ObtenerCarrito());
        }

        public IActionResult Agregar(int id)
        {
            var producto = _context.Productos
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    Nombre = p.Nombre,
                    Marca = p.Marca.Nombre,
                    p.Precio,
                    p.Imagen
                })
                .FirstOrDefault();
            var productoDB = _context.Productos.FirstOrDefault(p => p.Id == id);

            if (productoDB.Stock == 0)
            {
                TempData["SinStock"] = "Producto no disponible";
                return RedirectToAction("Catalogo", "Cliente");
            }

            if (producto == null)
                return RedirectToAction("Catalogo", "Cliente");

            var carrito = ObtenerCarrito();

            var item = carrito.FirstOrDefault(c => c.Id == id);

            if (item == null)
            {
                carrito.Add(new CarritoItem
                {
                    Id = producto.Id,
                    Nombre = producto.Nombre,
                    Marca = producto.Marca,
                    Precio = producto.Precio,
                    Cantidad = 1,
                    Imagen = producto.Imagen
                });
            }
            else
            {
                item.Cantidad++;
            }

            GuardarCarrito(carrito);
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(c => c.Id == id);

            if (item != null)
            {
                carrito.Remove(item);
                GuardarCarrito(carrito);
            }

            return RedirectToAction("Index");
        }

       
        public IActionResult PedidoConfirmado(string metodoPago)
        {
            var carrito = ObtenerCarrito();

            decimal total = carrito.Sum(x => x.Precio * x.Cantidad);

            ViewBag.Total = total;

            HttpContext.Session.SetString("MetodoPago", metodoPago);

            ViewBag.MetodoPago = metodoPago;

            var usuarioIdSession = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioIdSession == null)
            {
                return RedirectToAction("Login", "Cliente",
                    new { returnUrl = "/Carrito/Index" });
            }

            int usuarioId = usuarioIdSession.Value;

            var cliente = _context.Clientes
                .FirstOrDefault(c => c.UsuarioId == usuarioId);

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Id == usuarioId);

            if (cliente != null)
            {
                ViewBag.Direccion = cliente.Direccion;
                ViewBag.Telefono = cliente.Telefono;
            }

            if (usuario != null)
            {
                ViewBag.Nombre = usuario.Nombre;
            }

            return View(carrito);
        }

        [HttpPost]
        public IActionResult FinalizarPedido()
        {
            var carrito = ObtenerCarrito();

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            var clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (usuarioId != null)
            {
                var pedido = new Pedido
                {
                    UsuarioId = usuarioId.Value,
                    ClienteId = clienteId,
                    FechaPedido = DateTime.Now,
                    Estado = "Pendiente"
                };

                _context.Pedidos.Add(pedido);
                _context.SaveChanges();

                foreach (var item in carrito)
                {
                    var detalle = new DetallePedido
                    {
                        PedidoId = pedido.Id,
                        ProductoId = item.Id,
                        Cantidad = item.Cantidad,
                        Precio = Convert.ToDecimal(item.Precio),
                        DescripcionProducto = item.Nombre
                    };

                    _context.PedidosDetalles.Add(detalle);
                }

                _context.SaveChanges();
            }

            foreach (var item in carrito)
            {
                var producto = _context.Productos
                    .FirstOrDefault(p => p.Id == item.Id);

                if (producto != null)
                {
                    producto.Stock -= item.Cantidad;

                    if (producto.Stock < 0)
                        producto.Stock = 0;
                }
            }

            _context.SaveChanges();

            HttpContext.Session.Remove("Carrito");

            HttpContext.Session.Remove("MetodoPago");

            return RedirectToAction("Catalogo", "Cliente");
        }

        private List<CarritoItem> ObtenerCarrito()
        {
            var data = HttpContext.Session.GetString("Carrito");

            if (string.IsNullOrEmpty(data))
                return new List<CarritoItem>();

            return JsonSerializer.Deserialize<List<CarritoItem>>(data) ?? new List<CarritoItem>();
        }

        private void GuardarCarrito(List<CarritoItem> carrito)

        {
            HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(carrito));
        }
    }
}
