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
using NSubstitute;

namespace Infrastructure.Tests.Repositories {
	public class CanteenRepositoryTests {


		[Fact]
		public void GetAllCanteens_ReturnsAllCanteens() {
			// Arrange
			var repository = GetInMemoryCanteenRepository();

			// Act
			var canteens = repository.GetCanteens();

			// Assert
			Assert.Equal(3, canteens.Count());

		}




		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		public void GetCanteenById_WithValidId_ReturnsCanteen(int canteenId) {
			// Arrange
			var repository = GetInMemoryCanteenRepository();

			// Act

			var canteen = repository.GetCanteenById(canteenId);

			// Assert
			Assert.NotNull(canteen);
			Assert.Equal(canteenId, canteen.Id);
		}







		[Theory]
		[InlineData(44)]
		[InlineData(-10)]
		[InlineData(192)]
		public void GetCanteenById_WithInvalidId_ReturnsNull(int canteenId) {
			// Arrange
			var options = new DbContextOptionsBuilder<AppDBContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			// Act
			using (var context = new AppDBContext(options)) {
				var repository = new CanteenRepository(context);
				var canteen = repository.GetCanteenById(1);

				// Assert
				Assert.Null(canteen);
			}
		}




		//SETUP IN MEMORY DATABASE
		private ICanteenRepository GetInMemoryCanteenRepository() {
			DbContextOptions<AppDBContext> options;
			var builder = new DbContextOptionsBuilder<AppDBContext>()
		.UseInMemoryDatabase(Guid.NewGuid().ToString());
			options = builder.Options;

			AppDBContext _context = new AppDBContext(options);
			_context.Database.EnsureDeleted();
			_context.Database.EnsureCreated();


			_context.Canteens.Add(new Canteen { Id = 1, City = CityEnum.Breda, Location = CanteenEnum.LA, OffersHotMeals = false });
			_context.Canteens.Add(new Canteen { Id = 2, City = CityEnum.Tilburg, Location = CanteenEnum.HA, OffersHotMeals = true });
			_context.Canteens.Add(new Canteen { Id = 3, City = CityEnum.Breda, Location = CanteenEnum.LD, OffersHotMeals = false });
			_context.SaveChanges();

			return new CanteenRepository(_context);
		}

	}
}
