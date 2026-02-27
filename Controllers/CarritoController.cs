using Microsoft.AspNetCore.Mvc;
using MarketLocalShirts3.Models;
using System.Text.Json;

namespace MarketLocalShirts3.Controllers
{
    public class CarritoController : Controller
    {
        private readonly MarketLocalShirts3Context _context;

        public CarritoController(MarketLocalShirts3Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var carrito = ObtenerCarrito();
            return View(carrito);
        }

        public IActionResult Agregar(int id)
        {
            var producto = _context.Productos
                .Where(p => p.IdProducto == id)
                .Select(p => new
                {
                    p.IdProducto,
                    Nombre = p.NombreProducto,
                    Marca = p.IdMarcaNavigation.NombreMarca,
                    p.Precio,
                    p.Imagen
                })
                .FirstOrDefault();

            if (producto == null)
                return RedirectToAction("Catalogo", "Cliente");

            var carrito = ObtenerCarrito();

            var item = carrito.FirstOrDefault(c => c.IdProducto == id);

            if (item == null)
            {
                carrito.Add(new CarritoItem
                {
                    IdProducto = producto.IdProducto,
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
            var item = carrito.FirstOrDefault(c => c.IdProducto == id);

            if (item != null)
            {
                carrito.Remove(item);
                GuardarCarrito(carrito);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ConfirmarPedido(string metodoPago)
        {
            HttpContext.Session.Remove("Carrito");
            return RedirectToAction("PedidoConfirmado");
        }

        public IActionResult PedidoConfirmado()
        {
            return View();
        }

        public IActionResult AgregarMas()
        {
            return RedirectToAction("Catalogo", "Cliente");
        }

        private List<CarritoItem> ObtenerCarrito()
        {
            var data = HttpContext.Session.GetString("Carrito");
            if (data == null)
                return new List<CarritoItem>();

            return JsonSerializer.Deserialize<List<CarritoItem>>(data);
        }

        private void GuardarCarrito(List<CarritoItem> carrito)
        {
            HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(carrito));
        }
    }
}