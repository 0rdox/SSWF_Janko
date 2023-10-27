using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using Domain.Models.Enums;
using Domain.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Infrastructure.Tests.Repositories {
	public class EmployeeRepositoryTests {
		[Fact]
		public void GetEmployees_ReturnsAllEmployees() {
			// Arrange
			var repository = GetInMemoryEmployeeRepository();

			// Act
			var employees = repository.GetEmployees();

			// Assert
			Assert.NotNull(employees);
			Assert.Equal(4, employees.Count()); // Ensure that there are exactly 3 employees

			// Check that the names are as expected
			Assert.Collection(employees,
				employee => Assert.Equal("Derek Stor", employee.Name),
				employee => Assert.Equal("Jenny Berk", employee.Name),
				employee => Assert.Equal("Sarah Lee", employee.Name),
				employee => Assert.Equal("Dom Peters", employee.Name));
		}

		[Fact]
		public void GetEmployees_ReturnsNoEmployeesWhenDatabaseIsEmpty() {
			var options = new DbContextOptionsBuilder<AppDBContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			// Act
			using (var context = new AppDBContext(options)) {
				var repository = new EmployeeRepository(context);
				var employees = repository.GetEmployees();

				// Assert
				Assert.Empty(employees);
			}
		}


		// SET UP IN-MEMORY DATABASE
		private IEmployeeRepository GetInMemoryEmployeeRepository() {
			DbContextOptions<AppDBContext> options;
			var builder = new DbContextOptionsBuilder<AppDBContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			options = builder.Options;

			AppDBContext _context = new AppDBContext(options);
			_context.Database.EnsureDeleted();
			_context.Database.EnsureCreated();

			// Add some employees to the in-memory database

			Canteen[] canteens = new Canteen[3] {
				new Canteen() {City = CityEnum.Breda, Location = CanteenEnum.LA, OffersHotMeals= false},
				new Canteen() {City = CityEnum.Tilburg, Location = CanteenEnum.HA, OffersHotMeals= true},
				new Canteen() {City = CityEnum.Breda, Location = CanteenEnum.LD, OffersHotMeals= true}
			};

			IEnumerable<Employee> employees = new List<Employee> {
				new Employee {Name = "Derek Stor", Email="derekstor@mail.com", Canteen = canteens[0]},
				new Employee {Name = "Jenny Berk", Email="jennyberk@mail.com",Canteen = canteens[0]},
				new Employee {Name = "Sarah Lee",Email="sarahlee@mail.com", Canteen = canteens[1]},
				new Employee {Name = "Dom Peters",Email="dompeters@mail.com", Canteen = canteens[2]}
			};

			_context.Canteens.AddRange(canteens);
			_context.Employees.AddRange(employees);
			_context.SaveChanges();

			return new EmployeeRepository(_context);
		}

	}
}
