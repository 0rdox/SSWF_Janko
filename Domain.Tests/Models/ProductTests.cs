using Domain.Models;
using System.Net.Sockets;
using Xunit;

public class ProductTests {

    [Fact]
    public void Product_Constructor_WithParameters_ShouldSetProperties() {
        // Arrange
        string name = "TestProduct";
        bool isAlcohol = false;
        string imageUrl = "test-image-url";

        // Act
        var product = new Product(name, isAlcohol, imageUrl);

        // Assert
        Assert.Equal(name, product.Name);
        Assert.Equal(isAlcohol, product.Alcohol);
        Assert.Equal(imageUrl, product.ImageUrl);
    }

    [Fact]
    public void Product_DefaultConstructor_ShouldInitializeProperties() {
        // Arrange
        // Act
        var product = new Product();

        // Assert
        Assert.Null(product.Name);
        Assert.False(product.Alcohol);
        Assert.Null(product.ImageUrl);
    }

    [Fact]
    public void Product_PacketProperty_ShouldGetAndSet() {
        // Arrange
        var product = new Product();
        var packet = new Packet();

        // Act
        product.Packet = packet;

        // Assert
        Assert.Equal(packet, product.Packet);
    }

    [Fact]
    public void Product_DemoProductsProperty_ShouldGetAndSet() {
        // Arrange
        var product = new Product();
        var demoProducts = new DemoProducts();

        // Act
        product.DemoProducts = demoProducts;

        // Assert
        Assert.Equal(demoProducts, product.DemoProducts);
    }
}
