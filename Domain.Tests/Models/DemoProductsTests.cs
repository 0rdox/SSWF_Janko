using Domain.Models;
using Domain.Models.Enums;
using System.Collections.Generic;
using Xunit;

namespace Domain.Tests {
    public class DemoProductsTests {

        [Fact]
        public void DemoProducts_Constructor_SetsProperties() {
            // Arrange
            List<Product> products = new List<Product>
            {
                new Product { Id = 1, Name = "Product1" },
                new Product { Id = 2, Name = "Product2" }
            };
            TypeEnum type = TypeEnum.Broodpakket;

            // Act
            var demoProducts = new DemoProducts(products, type);

            // Assert
            Assert.Equal(products, demoProducts.Products);
            Assert.Equal(type, demoProducts.Type);
        }
    }
}
