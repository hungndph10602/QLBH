using Assignment.Context;
using Assignment.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment.Global;

namespace Assignment.Controllers
{
    public class LoginController : Controller
    {
        DatabaseContext _context = new DatabaseContext();
        _Global _gl = new _Global();
        public IActionResult Index()
        {
            return View();
        }

       
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var check = (from u in _context.Users
                           where u.Email == model.username && u.Password == model.password
                           select u).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (check != null)
                {
                    ViewBag.ThongBao = "Đăng nhập thành công";
                    _gl.User_Id = check.Id;
                    var user_cart = (from c in _context.Carts
                                     where c.UserId == check.Id
                                     select c).FirstOrDefault();
                    if (user_cart == null)
                    {
                        var cart = new Carts()
                        {
                            UserId = check.Id
                        };
                        _context.Carts.Add(cart);
                        _context.SaveChanges();
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ThongBao = "Đăng nhập thất bại";
                }
                
            }
            else
            {
                ModelState.AddModelError("", "Đăng nhập thất bại");
            }
            return View("Index");
        }
    }
}
