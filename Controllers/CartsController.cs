using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment.Context;
using Assignment.Models;
using Assignment.Ultilities;
using Assignment.Services;
using Assignment.Global;

namespace Assignment.Controllers
{
    public class CartsController : Controller
    {
        private readonly DatabaseContext _context;
        _Global _gl = new _Global();

        public CartsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            //var databaseContext = _context.Carts.Include(c => c.User);
            //return View(await databaseContext.ToListAsync());
            //var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            var cart = from c in _context.Carts
                       join cd in _context.CartDetail on c.Id equals cd.CartId
                       join pr in _context.Products on cd.ProductId equals pr.Id
                       join u in _context.Users on c.UserId equals u.Id
                       where c.UserId == _gl.User_Id
                       select new
                       {
                           c.UserId,
                           cd.ProductId,
                           pr.Name,
                           pr.Price,
                           cd.Quantity,
                       };
            ViewBag.cart = cart;
            ViewBag.total = _context.CartDetail.Where(x=>x.Cart.UserId==_gl.User_Id).Sum(c => c.Product.Price * c.Quantity);
            
            return View();
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carts = await _context.Carts
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carts == null)
            {
                return NotFound();
            }

            return View(carts);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId")] Carts carts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", carts.UserId);
            return View(carts);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carts = await _context.Carts.FindAsync(id);
            if (carts == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", carts.UserId);
            return View(carts);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId")] Carts carts)
        {
            if (id != carts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartsExists(carts.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", carts.UserId);
            return View(carts);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carts = await _context.Carts
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carts == null)
            {
                return NotFound();
            }

            return View(carts);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carts = await _context.Carts.FindAsync(id);
            _context.Carts.Remove(carts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartsExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
        private int isExist(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            var realCart = (from c in _context.Carts
                           select c).ToList();
            for (int i = 0; i < realCart.Count; i++)
            {
                if (realCart[i].Id.Equals(id))
                {
                    return i;
                }
            }

            return -1;
        }
        [HttpPost]
        public IActionResult Buy(int id)
        {
            var cartId = (from c in _context.Carts
                          where c.UserId == _gl.User_Id
                          select c.Id).FirstOrDefault();
            ServiceProduct serviceProduct = new ServiceProduct();
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item { Product = serviceProduct.FindProduct(id), Quantity = 1 });
                SessionHelper.setObjectAsJson(HttpContext.Session, "cart", cart);
               
               if(cartId == null)
                {
                    var detail = new CartDetail()
                    {
                        CartId = cartId,
                        ProductId = id,
                        Quantity = 1,
                    };
                    _context.CartDetail.Add(detail);
                    _context.SaveChanges(); 
                }
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = isExist(id);
                var count = (from c in _context.CartDetail
                              where c.CartId == cartId && c.ProductId == id
                              select c.Quantity).FirstOrDefault();
                var updateQuantity = _context.CartDetail.Where(x => x.ProductId == id && x.CartId == cartId).FirstOrDefault();
                var proid = (from c in _context.CartDetail
                              where c.ProductId == id && c.CartId == cartId
                              select c.ProductId).FirstOrDefault();
                if (proid !=0)
                {
                    updateQuantity.Quantity++;
                    _context.CartDetail.Update(updateQuantity);
                    _context.SaveChanges();
                    //var detail = new CartDetail()
                    //{
                    //    CartId = cartId,
                    //    ProductId = id,
                    //    Quantity = count+1,
                    //};
                    //_context.CartDetail.Update(detail);
                    //_context.SaveChanges();
                }
                else
                {
                    cart.Add(new Item { Product =  serviceProduct.FindProduct(id), Quantity = 1 });
                    var detail = new CartDetail()
                    {
                        CartId = cartId,
                        ProductId = id,
                        Quantity = 1,
                    };
                    _context.CartDetail.Add(detail);
                    _context.SaveChanges();
                }
                SessionHelper.setObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return RedirectToAction("Index");
        }
    }
}
