using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;


namespace Domain.Services {
	public interface IStudentRepository {
		IEnumerable<Student> GetStudents();

		Task CreateStudent(Student student);

		Student GetStudentById(int studentId);
	}
}
