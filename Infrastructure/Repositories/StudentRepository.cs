using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories {
    public class StudentRepository : IStudentRepository {
		private readonly AppDBContext _context;

		public StudentRepository(AppDBContext context) {
			_context = context;
		}

		public async Task CreateStudent(Student student) {
			_context.Students.Add(student);
			await _context.SaveChangesAsync();
		}

		

        public Student GetStudentById(int studentId) {
			return _context.Students.FirstOrDefault(a => a.Id == studentId)!;
        }

        public IEnumerable<Student> GetStudents() {
			return _context.Students;
		}
	}
}
