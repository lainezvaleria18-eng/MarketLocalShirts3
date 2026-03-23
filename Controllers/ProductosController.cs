using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarketLocalShirts3.Models;

namespace MarketLocalShirts3.Controllers
{
    public class ProductosController : Controller
    {
        private readonly MarketLocalShirts3Context _context;
        private readonly IWebHostEnvironment _env;

        public ProductosController(MarketLocalShirts3Context context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(string buscar)
        {
            var productos = _context.Productos.AsQueryable();

            if (!string.IsNullOrEmpty(buscar))
            {
                productos = productos.Where(p =>
                    p.Nombre.Contains(buscar) ||
                    p.Descripcion.Contains(buscar));
            }

            return View(await productos.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.MarcaId = new SelectList(_context.Marcas, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Producto producto, IFormFile archivoImagen)
        {
            if (archivoImagen != null)
            {
                var nombreArchivo = Guid.NewGuid() + Path.GetExtension(archivoImagen.FileName);
                var ruta = Path.Combine(_env.WebRootPath, "imagenes", nombreArchivo);

                using var stream = new FileStream(ruta, FileMode.Create);
                await archivoImagen.CopyToAsync(stream);

                producto.Imagen = nombreArchivo;
            }

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            ViewBag.MarcaId = new SelectList(_context.Marcas, "Id", "Nombre", producto.MarcaId);
            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Producto producto, IFormFile archivoImagen)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            var productoDb = await _context.Productos.FindAsync(id);
            if (productoDb == null)
            {
                return NotFound();
            }

            if (archivoImagen != null)
            {
                var nombreArchivo = Guid.NewGuid() + Path.GetExtension(archivoImagen.FileName);
                var ruta = Path.Combine(_env.WebRootPath, "imagenes", nombreArchivo);

                using var stream = new FileStream(ruta, FileMode.Create);
                await archivoImagen.CopyToAsync(stream);

                productoDb.Imagen = nombreArchivo;
            }

            productoDb.Nombre = producto.Nombre;
            productoDb.Descripcion = producto.Descripcion;
            productoDb.Precio = producto.Precio;
            productoDb.Stock = producto.Stock;
            productoDb.MarcaId = producto.MarcaId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Marca)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}