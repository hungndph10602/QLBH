using Assignment.Context;
using Assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.Services
{
    public class ServiceProduct
    {
        private DatabaseContext _context = new DatabaseContext();
        public List<Products> _lstProducts { get; set; }
        public ServiceProduct()
        {

        }

        public ServiceProduct(DatabaseContext context)
        {
            _context = context;
        }
        public List<Products> FakeDataProducts()
        {
            var _lstProducts = _context.Products.ToList();
            return _lstProducts;
        }

        public Products FindProduct(int id)
        {
            return FakeDataProducts().Where(c => c.Id == id).FirstOrDefault();
        }
    }
}
