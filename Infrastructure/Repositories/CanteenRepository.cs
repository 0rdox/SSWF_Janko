using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Services;
using Infrastructure.Data;

namespace Infrastructure.Repositories {
	public class CanteenRepository : ICanteenRepository {
		private readonly AppDBContext _context;

		public CanteenRepository(AppDBContext context) {
			_context = context;
		}



		public Canteen GetCanteenById(int id) {
			return _context.Canteens.FirstOrDefault(a => a.ID == id);
		}

		public IEnumerable<Canteen> GetCanteens() {
			return _context.Canteens;
		}
	}
}
