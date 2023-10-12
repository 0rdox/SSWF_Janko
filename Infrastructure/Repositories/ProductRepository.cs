using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Services;
using Infrastructure.Data;

namespace Infrastructure.Repositories {
    public class ProductRepository : IProductRepository {
        private readonly AppDBContext _context;

        public ProductRepository(AppDBContext context) {
            _context = context;
        }

        public async Task CreateProduct(Product product) {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        //public Product GetProductById(int id) {
        //    return _context.Products.FirstOrDefault(a => a.Id == id);
        //}

        public IEnumerable<Product> GetProducts() {
            return _context.Products;
        }
    }
}
