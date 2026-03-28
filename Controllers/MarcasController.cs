using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarketLocalShirts3.Models;

namespace MarketLocalShirts3.Controllers
{
    public class MarcasController : Controller
    {
        private readonly MarketLocalShirts3Context _context;

        public MarcasController(MarketLocalShirts3Context context)
        {
            _context = context;
        }

        // LISTAR MARCAS
        public async Task<IActionResult> Index()
        {
            return View(await _context.Marcas.ToListAsync());
        }

        // CREAR MARCA
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Marca marca)
        {
            if (ModelState.IsValid)
            {
                _context.Marcas.Add(marca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(marca);
        }

        // EDITAR MARCA
        public async Task<IActionResult> Edit(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);

            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Marca marca)
        {
            if (id != marca.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(marca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(marca);
        }

        // ELIMINAR MARCA
        public async Task<IActionResult> Delete(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);

            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);

            if (marca != null)
            {
                _context.Marcas.Remove(marca);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}