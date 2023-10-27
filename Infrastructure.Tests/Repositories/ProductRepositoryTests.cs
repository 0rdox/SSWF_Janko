using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Enums;
using Domain.Models;
using Domain.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories {
	public class ProductRepositoryTests {



		[Fact]
		public void CreateProduct_AddsProductToDatabase() {
			// Arrange
			var repository = GetInMemoryProductRepository();
			var newProduct = new Product {
				Name = "New Product",
				Alcohol = false,
				ImageUrl = "https://example.com/new-product-image.jpg"
			};

			// Act
			repository.CreateProduct(newProduct);
			var products = repository.GetProducts().ToList();

			// Assert
			Assert.Contains(newProduct, products);
		}

		[Fact]
		public void GetProducts_ReturnsAllProducts() {
			// Arrange
			var repository = GetInMemoryProductRepository();

			// Act
			var products = repository.GetProducts().ToList();
			var productsTop3 = repository.GetProducts().Take(3).ToList();

			// Assert
			Assert.Equal(14, products.Count); 
			Assert.Collection(productsTop3,
				product => Assert.Equal("Worstenbroodje", product.Name),
				product => Assert.Equal("Saucijzenbroodje", product.Name),
				product => Assert.Equal("Kaassoufflé", product.Name)
			);
		}



		// SET UP IN-MEMORY DATABASE
		private IProductRepository GetInMemoryProductRepository() {
			DbContextOptions<AppDBContext> options;
			var builder = new DbContextOptionsBuilder<AppDBContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			options = builder.Options;

			AppDBContext _context = new AppDBContext(options);
			_context.Database.EnsureDeleted();
			_context.Database.EnsureCreated();


			Product[] products = {
				new Product() { Name = "Worstenbroodje", Alcohol = false, ImageUrl = "https://www.hetzeilbergsbakkertje.nl/wp-content/uploads/JPM0008-Worstenbroodje-open.jpg" },
				new Product() { Name = "Saucijzenbroodje", Alcohol = false, ImageUrl = "https://www.bakkerijvoortman.nl/wp/wp-content/uploads/2015/02/saucbr..png" },
				new Product() { Name = "Kaassoufflé", Alcohol = false, ImageUrl = "https://www.ahealthylife.nl/wp-content/uploads/2021/06/Kaassouffle_voedingswaarde-1.jpg" },
				new Product() { Name = "Frikandelbroodje", Alcohol = false, ImageUrl = "https://klokhoogeveen.nl/wordpress/wp-content/uploads/2019/04/frikandellenbroodje.png" },
				new Product() { Name = "Kroket", Alcohol = false, ImageUrl = "https://i0.wp.com/www.frituurwereld.nl/wp-content/uploads/2020/10/Frituurwereld-Krokettendag-hoera.jpg?fit=1212%2C823&ssl=1" },
				new Product() { Name = "Pistoletje gezond", Alcohol = false, ImageUrl = "https://s-marktscholte.nl/cmcc/wp-content/uploads/2019/01/Broodje-gezond-s-markt.jpg" },
				new Product() { Name = "Tomatensoep", Alcohol = false, ImageUrl = "https://us.123rf.com/450wm/kostrez/kostrez1509/kostrez150900011/44695927-tomatensoep-in-een-witte-kom-met-peterselie-en-specerijen-op-een-schotel-ge%C3%AFsoleerd-op-een-witte.jpg?ver=6" },
				//Zoet
				new Product() { Name = "Chocolade", Alcohol = false, ImageUrl = "https://img.freepik.com/premium-photo/chocolade-op-witte-achtergrond_404043-1540.jpg" },
				new Product() { Name = "Koekje", Alcohol = false, ImageUrl = "https://media.istockphoto.com/id/1251818705/photo/large-chocolate-chip-cookie-on-a-white-plate-with-a-white-background.jpg?s=612x612&w=0&k=20&c=vDhGIb54eGfIQKI1gfuDGp7j29ppw3ioTa-Wwwrg3Vc=" },
				//Drank
				new Product() { Name = "Bier", Alcohol = true, ImageUrl = "https://thumbs.dreamstime.com/b/koud-bier-43280582.jpg" },
				new Product() { Name = "Wijn", Alcohol = true, ImageUrl = "https://static.vecteezy.com/ti/gratis-vector/p3/7324461-wijnglazen-met-witte-wijn-illustratie-van-wijnglazen-geisoleerd-op-witte-achtergrond-gratis-vector.jpg" },
				new Product() { Name = "Whisky", Alcohol = true, ImageUrl = "https://img.freepik.com/premium-photo/glas-schotse-whisky-en-ijs-op-een-witte-achtergrond_38145-1376.jpg?w=2000" },
				new Product() { Name = "Vodka", Alcohol = true, ImageUrl = "https://media.istockphoto.com/id/671705556/photo/vodka-in-vintage-glass.jpg?s=612x612&w=0&k=20&c=Tz58z7MkbFq2ziaK92nid4KEp84T30pFlS6J6pAmI08=" },
				new Product() { Name = "Tequila Sunrise", Alcohol = true, ImageUrl = "https://img.freepik.com/premium-photo/cocktail-tequila-sunrise-front-white-background_118454-21367.jpg?w=2000" },

			};

			_context.Products.AddRange(products);
			_context.SaveChanges();

			return new ProductRepository(_context);
		}

	}
}
