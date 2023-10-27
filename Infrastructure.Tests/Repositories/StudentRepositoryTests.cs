using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Models.Enums;
using Domain.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories {
	public class StudentRepositoryTests {

		[Fact]
		public async void CreateStudent_AddsStudentToDatabase() {
			// Arrange
			var repository = GetInMemoryStudentRepository();

			var student = new Student {
				Name = "Alice Jane",
				StudentNumber = 11111,
				DateOfBirth = new DateTime(1999, 4, 12),
				Email = "alice.jane@example.com",
				City = CityEnum.Breda,
				Phone = "555-555-5555"
			};

			// Act
			await repository.CreateStudent(student);

			// Assert
			var addedStudent = repository.GetStudents().FirstOrDefault(a => a.StudentNumber == 11111);

			Assert.Equal(5, repository.GetStudents().Count());
			Assert.Equal(student.Name, addedStudent.Name);
		}

		[Fact]
		public void GetStudentById_ReturnsStudentWithMatchingId() {
			// Arrange

			var repository = GetInMemoryStudentRepository();


			// Act
			var retrievedStudent = repository.GetStudentById(2);

			// Assert
			Assert.NotNull(retrievedStudent);
			Assert.Equal("Mike Evars", retrievedStudent.Name);
		}


		[Theory]
		[InlineData(99)]
		[InlineData(-1288)]
		[InlineData(0)]
		public void GetStudentById_WrongID_ReturnsNothing(int id) {
			// Arrange
			var repository = GetInMemoryStudentRepository();

			// Act
			var retrievedStudent = repository.GetStudentById(id);

			// Assert
			Assert.Null(retrievedStudent);
		}


		[Fact]
		public void GetStudents_ReturnsAllStudents() {
			// Arrange
			var repository = GetInMemoryStudentRepository();


			// Act
			var retrievedStudents = repository.GetStudents();

			// Assert
			Assert.NotNull(retrievedStudents);
			Assert.Equal(4, retrievedStudents.Count());
		}





		private IStudentRepository GetInMemoryStudentRepository() {
			DbContextOptions<AppDBContext> options;
			var builder = new DbContextOptionsBuilder<AppDBContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			options = builder.Options;

			AppDBContext _context = new AppDBContext(options);
			_context.Database.EnsureDeleted();
			_context.Database.EnsureCreated();


			Student[] students = {
				new Student {Name = "John Doe", StudentNumber = 12345, DateOfBirth = new DateTime(2000, 5, 15), Email = "john.doe@example.com", City = CityEnum.Breda, Phone = "555-555-5555" },
				new Student {Name = "Mike Evars", StudentNumber = 52341, DateOfBirth = new DateTime(1998, 3, 29),Email = "mike.evars@example.com",City = CityEnum.Tilburg,Phone = "555-555-5555" },
				new Student {Name = "Kaitlyn Marek", StudentNumber = 51221, DateOfBirth = new DateTime(2001, 12, 18),Email = "kaitlyn.marek@example.com",City = CityEnum.Breda,Phone = "555-555-5555" },
				new Student {Name = "Oliver Boko", StudentNumber = 11231, DateOfBirth = new DateTime(2009, 12, 18),Email = "oliver.boko@example.com",City = CityEnum.Breda,Phone = "555-555-5555" }
			};


			_context.Students.AddRange(students);
			_context.SaveChanges();

			return new StudentRepository(_context);
		}

	}
}
