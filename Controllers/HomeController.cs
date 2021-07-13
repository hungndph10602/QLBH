using Assignment.Context;
using Assignment.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search(string strSearch)
        {
            //Products pro = new Products();
            //if (!string.IsNullOrEmpty(strSearch))
            //{
            //    var list = _context.Products.Where(c => c.Name.Contains(strSearch)).ToList();
            //    pro._lstPro = list;
               
            //}
            ViewBag.str = strSearch;
            return View();
        }
    }
}
