using Domain.Models;
using Domain.Models.Enums;
using Xunit;

namespace Domain.Tests {
    public class CanteenTests {
        [Fact]
        public void Canteen_ParameterlessConstructor_SetsDefaultValues() {
            // Act
            var canteen = new Canteen();

            // Assert
            Assert.Equal(default(CityEnum), canteen.City);
            Assert.Equal(default(CanteenEnum), canteen.Location);
            Assert.False(canteen.OffersHotMeals);
        }

        [Fact]
        public void Canteen_Constructor_SetsProperties() {
            // Arrange
            CityEnum city = CityEnum.Breda;
            CanteenEnum location = CanteenEnum.LA;
            bool offersHotMeals = true;

            // Act
            var canteen = new Canteen(city, location, offersHotMeals);

            // Assert
            Assert.Equal(city, canteen.City);
            Assert.Equal(location, canteen.Location);
            Assert.Equal(offersHotMeals, canteen.OffersHotMeals);
        }
    }
}
