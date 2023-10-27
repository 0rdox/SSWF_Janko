using Domain.Models;
using Domain.Models.Enums;
using System;
using System.Collections.Generic;
using Xunit;
namespace Domain.Tests {
    public class PacketTests {
        [Fact]
        public void Packet_Constructor_WithParameters_ShouldSetProperties() {
            // Arrange
            string name = "TestPacket";
            decimal price = 9.99m;
            TypeEnum type = TypeEnum.Broodpakket;
            string imageUrl = "test-image-url";

            // Act
            var packet = new Packet(name, price, imageUrl, type);

            // Assert
            Assert.Equal(name, packet.Name);
            Assert.Equal(price, packet.Price);
            Assert.Equal(imageUrl, packet.ImageUrl);
            Assert.Equal(type, packet.Type);
            Assert.Null(packet.ReservedBy);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(24)]
        [InlineData(48)]
        public void Packet_PickupTimeIsValid_ShouldReturnTrueForValidTime(int hours) {
            // Arrange
            var validPickupTime = DateTime.Now.AddHours(hours);

            // Act
            var isValid = new Packet().PickupTimeIsValid(validPickupTime);

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(49)]
        public void Packet_PickupTimeIsValid_ShouldReturnFalseForInvalidTime(int hours) {
            // Arrange
            var now = DateTime.Now;
            var invalidPickupTime = DateTime.Now.AddHours(hours);



            // Act
            var isValid = new Packet().PickupTimeIsValid(invalidPickupTime);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void Packet_IsOverEighteen_ShouldReturnTrueForAlcoholProducts() {
            // Arrange
            var alcoholProduct = new Product { Alcohol = true };
            var nonAlcoholProduct = new Product { Alcohol = false };
            var products = new List<Product> { alcoholProduct, nonAlcoholProduct };

            // Act
            var isOverEighteen = new Packet().IsOverEighteen(products);

            // Assert
            Assert.True(isOverEighteen);
        }

        [Fact]
        public void Packet_IsOverEighteen_ShouldReturnFalseForNonAlcoholProducts() {
            // Arrange
            var nonAlcoholProduct1 = new Product { Alcohol = false };
            var nonAlcoholProduct2 = new Product { Alcohol = false };
            var products = new List<Product> { nonAlcoholProduct1, nonAlcoholProduct2 };

            // Act
            var isOverEighteen = new Packet().IsOverEighteen(products);

            // Assert
            Assert.False(isOverEighteen);
        }
    }
}