using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Services;
using Infrastructure.Data;

namespace Infrastructure.Repositories {
	public class EmployeeRepository : IEmployeeRepository {
		private readonly AppDBContext _context;

		public EmployeeRepository(AppDBContext context) {
			_context = context;
		}


		public IEnumerable<Employee> GetEmployees() {
			return _context.Employees;
		}
	}
}
