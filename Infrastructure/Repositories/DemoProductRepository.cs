using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Models.Enums;
using Domain.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories {
	public class DemoProductRepository : IDemoProductRepository {
		private readonly AppDBContext _context;

		public DemoProductRepository(AppDBContext context) {
			_context = context;
		}

		public IEnumerable<Product> GetDemoProducts(TypeEnum type) {
			var products = _context.DemoProducts
		.Include(demo => demo.Products)
		.Where(a => a.Type == type)
		.SelectMany(demo => demo.Products)
		.ToList();

			return products;
		}
	}
}