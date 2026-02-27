using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarketLocalShirts3.Models;

namespace MarketLocalShirts3.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly MarketLocalShirts3Context _context;

        public CatalogoController(MarketLocalShirts3Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }
    }
}