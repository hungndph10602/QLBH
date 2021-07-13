using Assignment.Context;
using Assignment.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.Controllers
{
    public class ShopController : Controller
    {
        private readonly DatabaseContext _context;

        public ShopController(DatabaseContext context)
        {
            _context = context;
        }
        private ServiceProduct _serviceProduct = new ServiceProduct();
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Products.Include(p => p.Category);
            return View(await databaseContext.ToListAsync());
        }
    }
}
