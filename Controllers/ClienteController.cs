using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarketLocalShirts3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketLocalShirts3.Controllers
{
    public class ClienteController : Controller
    {
        private readonly MarketLocalShirts3Context _context;

        public ClienteController(MarketLocalShirts3Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Inicio()
        {
            return View();
        }

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

        public IActionResult Registro()
        {
            return View();
        }
    }
}