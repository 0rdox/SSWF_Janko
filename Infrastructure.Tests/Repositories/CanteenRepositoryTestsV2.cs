using Domain.Models;
using Domain.Models.Enums;
using Domain.Services;
using Infrastructure.Repositories;
using Microsoft.Identity.Client;
using NSubstitute;
using System;
using Xunit;

namespace Infrastructure.Tests.Repositories {
	public class CanteenRepositoryTestsV2{
		private ICanteenRepository _canteenRepository; 

		public CanteenRepositoryTestsV2() {
			_canteenRepository = Substitute.For<ICanteenRepository>();

			//Substitute Behaviour
			_canteenRepository.GetCanteens().Returns(new[]
			{
				new Canteen { Id = 1, City = CityEnum.Breda, Location = CanteenEnum.LA, OffersHotMeals = false },
				new Canteen { Id = 2, City = CityEnum.Tilburg, Location = CanteenEnum.HA, OffersHotMeals = true },
				new Canteen { Id = 3, City = CityEnum.Breda, Location = CanteenEnum.LD, OffersHotMeals = false }
			});

			_canteenRepository.GetCanteenById(Arg.Any<int>()).Returns(info => {
				int canteenId = info.Arg<int>();
				return canteenId switch {
					1 => new Canteen { Id = 1, City = CityEnum.Breda, Location = CanteenEnum.LA, OffersHotMeals = false },
					2 => new Canteen { Id = 2, City = CityEnum.Tilburg, Location = CanteenEnum.HA, OffersHotMeals = true },
					3 => new Canteen { Id = 3, City = CityEnum.Breda, Location = CanteenEnum.LD, OffersHotMeals = false },
					_ => null,
				};
			});
		}

		[Fact]
		public void GetAllCanteens_ReturnsAllCanteens() {
			// Act
			var canteens = _canteenRepository.GetCanteens();

			// Assert
			Assert.Equal(3, canteens.Count());
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		public void GetCanteenById_WithValidId_ReturnsCanteen(int canteenId) {
			// Act
			var canteen = _canteenRepository.GetCanteenById(canteenId);

			// Assert
			Assert.NotNull(canteen);
			Assert.Equal(canteenId, canteen.Id);
		}

		[Theory]
		[InlineData(44)]
		[InlineData(-10)]
		[InlineData(192)]
		public void GetCanteenById_WithInvalidId_ReturnsNull(int canteenId) {
			// Act
			var canteen = _canteenRepository.GetCanteenById(canteenId);

			// Assert
			Assert.Null(canteen);
		}
	}
}