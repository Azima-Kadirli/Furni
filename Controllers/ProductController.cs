using Furni.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furni.Controllers
{
    public class ProductController : Controller
    {
        readonly FurniDbContext _context;

        public ProductController(FurniDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.AsQueryable().ToListAsync());
        }
    }
}
