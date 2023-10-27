using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using Domain.Models.Enums;
using Domain.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;

namespace Infrastructure.Tests.Repositories {
	public class PacketRepositoryTests {

		[Fact]
		public void GetPackets_ReturnsAllPackets() {
			// Arrange
			var repository = GetInMemoryPacketRepository();

			// Act
			var packets = repository.GetPackets();

			// Assert
			Assert.NotNull(packets);
			Assert.Equal(5, packets.Count());
		

		}
	

		[Fact]
		public void GetNotReservedPackets_ReturnsUnreservedPacketsOrderedByPickupTime() {
			// Arrange
			var repository = GetInMemoryPacketRepository();

			// Act
			var packets = repository.GetNotReservedPackets();

			// Assert
			Assert.NotNull(packets);
			Assert.Equal(3, packets.Count());
			Assert.Collection(packets,
				packets => Assert.Equal("Broodjes om van te smullen", packets.Name),
				packets => Assert.Equal("Alcohol.com", packets.Name),
				packets => Assert.Equal("Warme Maaltijd", packets.Name));

		}

		[Fact]
		public void GetPacketById_ReturnsPacketWithCorrectId() {
			// Arrange
			var repository = GetInMemoryPacketRepository();
			var packetId = 1;

			// Act
			var packet = repository.GetPacketById(packetId);

			// Assert
			Assert.NotNull(packet);
			Assert.Equal(packetId, packet.Id);

		}

		[Fact]
		public void CreatePacket_AddsPacketToDatabase() {
			// Arrange
			var repository = GetInMemoryPacketRepository();

			var packetName = "New Packet";
			var packetPrice = "10.99";
			var pickupTime = DateTime.Now.AddHours(1);
			var products = "";
			var type = TypeEnum.Broodpakket;
			var imageUrl = "https://image.jpg";
			var canteen = new Canteen { Location = CanteenEnum.XP };


			// Act
			repository.CreatePacket(packetName, packetPrice, pickupTime, products, type, imageUrl, canteen);

			// Assert
			var createdPacket = repository.GetPacketById(6);

			Assert.NotNull(createdPacket);
			Assert.Equal(packetName, createdPacket.Name);
		}

		[Theory]
		[InlineData(null, "10.99", "2023-10-25", "[]", TypeEnum.Broodpakket, "image.jpg")]
		[InlineData("New Packet", null, "2023-10-25", "[]", TypeEnum.Broodpakket, "image.jpg")]
		[InlineData("New Packet", "10.99", "2023-10-25", "", null, "image.jpg")]
		public void CreatePacket_WithMissingData_DoesNotAddToDatabase(
		 string packetName, string packetPrice, string pickupTime, string products, TypeEnum type, string imageUrl) {
			// Arrange
			var repository = GetInMemoryPacketRepository();
			Canteen canteen = new Canteen { Location = CanteenEnum.XP };
			DateTime.TryParse(pickupTime, out var parsedPickupTime);

			// Act
			repository.CreatePacket(packetName, packetPrice, parsedPickupTime, products, type, imageUrl, canteen);

			// Assert

			var createdPacket = repository.GetPacketById(6);
			Assert.Null(createdPacket);
		}

		[Fact]
		public void GetMyCanteenPackets_ReturnsPacketsBelongingToCanteen() {
			// Arrange
			var repository = GetInMemoryPacketRepository();
			var canteen = new Canteen { Id = 1 };

			// Act
			var packets = repository.GetMyCanteenPackets(canteen);

			// Assert
			Assert.NotNull(packets);
			Assert.Equal(4, packets.Count());
			Assert.Collection(packets,
				packets => Assert.Equal("Broodjes om van te smullen", packets.Name),
				packets => Assert.Equal("Alcohol.com", packets.Name),
				packets => Assert.Equal("Gereserveerd door student 1", packets.Name),
				packets => Assert.Equal("Gereserveerd door student 2", packets.Name));
		}

		[Fact]
		public void GetOtherCanteenPackets_ReturnsPacketsNotBelongingToCanteen() {
			// Arrange
			var repository = GetInMemoryPacketRepository();
			var canteen = new Canteen { Id = 1 };

			// Act
			var packets = repository.GetOtherCanteenPackets(canteen);

			// Assert
			Assert.NotNull(packets);
			Assert.Collection(packets,
				packets => Assert.Equal("Warme Maaltijd", packets.Name));
		}





		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public void GetReservedPackets_ReturnsPacketsReservedByStudent(int studentId) {
			// Arrange
			var repository = GetInMemoryPacketRepository();

			// Act
			var packets = repository.GetReservedPackets(studentId);

			// Assert
			Assert.NotNull(packets);
			Assert.Equal(1, packets.Count());
			Assert.Collection(packets,
			packets => Assert.Equal(("Gereserveerd door student " + studentId), packets.Name));
		}


		[Theory]
		[InlineData(3)]
		[InlineData(4)]
		public void GetReservedPackets_ReturnsNoPacketsReservedByStudent(int studentId) {
			// Arrange
			var repository = GetInMemoryPacketRepository();

			// Act
			var packets = repository.GetReservedPackets(studentId);

			// Assert
			Assert.Empty(packets);
			Assert.Equal(0, packets.Count());

		}

		[Fact]
		public async Task DeletePacket_RemovesPacketFromDatabase() {
			// Arrange
			var repository = GetInMemoryPacketRepository();
			var packet = repository.GetPacketById(1);

			// Act: Delete the packet
			await repository.DeletePacket(packet);

			var deletedPacket = repository.GetPacketById(1);
			Assert.Null(deletedPacket);
			Assert.Equal(2, repository.GetNotReservedPackets().Count());
		}


		[Fact]
		public async Task DeletePacket_CannotRemoveReservedPacketFromDatabase() {
			// Arrange
			var repository = GetInMemoryPacketRepository();
			var packet = repository.GetPacketById(4);

			// Act: Delete the packet
			await repository.DeletePacket(packet);

			var deletedPacket = repository.GetPacketById(2);
			Assert.NotNull(deletedPacket);

		}

		[Fact]
		public async Task ReservePacketBool_ReservationSuccessful_ReturnsTrue() {
			// Arrange
			var packetId = 1;
			var studentId = 3;
			var packetRepository = GetInMemoryPacketRepository();


			// Act
			var packet = packetRepository.GetPacketById(packetId);
			var result = await packetRepository.ReservePacketBool(packetId, studentId);

			// Assert
			Assert.True(result);
		}

		[Fact]
		public async Task ReservePacketBool_CantReserveOnSameDay_ReturnsFalse() {
			// Arrange
			var packetId = 1;
			var studentId = 2;
			var packetRepository = GetInMemoryPacketRepository();


			// Act
			var packet = packetRepository.GetPacketById(packetId);
			var result = await packetRepository.ReservePacketBool(packetId, studentId);

			// Assert
			Assert.False(result);
		}

		[Fact]
		public async Task ReservePacketBool_CantReservePacketOverEighteen_ReturnsFalse() {
			// Arrange
			var packetId = 2;
			var studentId = 4;
			var packetRepository = GetInMemoryPacketRepository();


			// Act
			var packet = packetRepository.GetPacketById(packetId);
			var result = await packetRepository.ReservePacketBool(packetId, studentId);

			// Assert
			Assert.False(result);
		}

		[Fact]
		public async Task ReservePacketBool_CanReservePacketOverEighteen_ReturnsTrue() {
			// Arrange
			var packetId = 2;
			var studentId = 3;
			var packetRepository = GetInMemoryPacketRepository();


			// Act
			var packet = packetRepository.GetPacketById(packetId);
			var result = await packetRepository.ReservePacketBool(packetId, studentId);

			// Assert
			Assert.True(result);
		}


		[Fact]
		public async Task UpdatePacket_PacketUpdatedSuccessfully() {
			// Arrange
			var repository = GetInMemoryPacketRepository();
			var packetBefore = repository.GetPacketById(1);


			var packetId = 1;
			var name = "Updated Packet Name";
			var price = "10.99";
			var pickupTime = DateTime.Now.AddDays(2);
			var products = "[{\"Name\":\"Product1\",\"Alcohol\":false,\"ImageUrl\":\"Image1.jpg\"}]";
			var type = TypeEnum.Broodpakket;
			var imageUrl = "UpdatedImageUrl";



			// Act
			await repository.UpdatePacket(packetId, name, price, pickupTime, products, type, imageUrl);

			// Assert
			var updatedPacket = repository.GetPacketById(packetId);

			Assert.NotNull(updatedPacket);
			Assert.Equal(name, updatedPacket.Name);
			Assert.Equal(10.99m, updatedPacket.Price);
			Assert.Equal(pickupTime, updatedPacket.PickupTime);
			Assert.Equal(type, updatedPacket.Type);
			Assert.Equal(imageUrl, updatedPacket.ImageUrl);
			Assert.Single(updatedPacket.Products);
			Assert.Equal("Product1", updatedPacket.Products[0].Name);
			Assert.Equal("Image1.jpg", updatedPacket.Products[0].ImageUrl);
		}
	



// SET UP IN-MEMORY DATABASE
private IPacketRepository GetInMemoryPacketRepository() {
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

	Student[] students = {
				new Student {Name = "John Doe", StudentNumber = 12345, DateOfBirth = new DateTime(2000, 5, 15), Email = "john.doe@example.com", City = CityEnum.Breda, Phone = "555-555-5555" },
				new Student {Name = "Mike Evars", StudentNumber = 52341, DateOfBirth = new DateTime(1998, 3, 29),Email = "mike.evars@example.com",City = CityEnum.Tilburg,Phone = "555-555-5555" },
				new Student {Name = "Kaitlyn Marek", StudentNumber = 51221, DateOfBirth = new DateTime(2001, 12, 18),Email = "kaitlyn.marek@example.com",City = CityEnum.Breda,Phone = "555-555-5555" },
				new Student {Name = "Oliver Boko", StudentNumber = 11231, DateOfBirth = new DateTime(2009, 12, 18),Email = "oliver.boko@example.com",City = CityEnum.Breda,Phone = "555-555-5555" }
			};

	var Canteen1 = new Canteen { Id = 1, City = CityEnum.Breda, Location = CanteenEnum.LA, OffersHotMeals = false };
	var Canteen2 = new Canteen { Id = 2, City = CityEnum.Tilburg, Location = CanteenEnum.HA, OffersHotMeals = true };

	var packets = new List<Packet> {

			new Packet {Name = "Broodjes om van te smullen" , CanteenNavigation = Canteen1,  Products = new List<Product> { products[3] } , City = CityEnum.Breda, Canteen = CanteenEnum.LA, PickupTime = DateTime.Now, Price = 8.99m, Type = TypeEnum.Broodpakket, ReservedById = null, ImageUrl = "https://rosco-catering.nl/wp-content/uploads/2020/06/Rosco-Catering-Bake-off-box-scaled.jpg" },
			new Packet {Name = "Alcohol.com", CanteenNavigation = Canteen1,Products = new List<Product> { products[11] }, OverEighteen = true,City = CityEnum.Breda, Canteen = CanteenEnum.LA, PickupTime = DateTime.Now, Price = 14.99m, Type = TypeEnum.Drankpakket, ReservedById = null, ImageUrl = "https://www.foodandwine.com/thmb/a3jODP_x_GpJpD71zT3t3BYbtp8=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/how-to-ship-wine-FT-BLOG1221-073f4b1897c34f04bff8ea71dadcba2c.jpg" },
			new Packet {Name = "Warme Maaltijd",CanteenNavigation = Canteen2,Products = new List<Product> { products[6] } ,City = CityEnum.Tilburg, Canteen = CanteenEnum.LD, PickupTime = DateTime.Now, Price = 14.99m, Type = TypeEnum.WarmeMaaltijd, ReservedById = null, ImageUrl = "https://www.framedcooks.com/wp-content/uploads/2021/07/steamed-cheeseburgers.jpg" },
			new Packet {Name = "Gereserveerd door student 1" , CanteenNavigation = Canteen1,  Products = new List<Product> { products[3] } , City = CityEnum.Breda, Canteen = CanteenEnum.LA, PickupTime = DateTime.Now, Price = 8.99m, Type = TypeEnum.Broodpakket, ReservedById = 1, ImageUrl = "https://rosco-catering.nl/wp-content/uploads/2020/06/Rosco-Catering-Bake-off-box-scaled.jpg" },
			new Packet {Name = "Gereserveerd door student 2" , CanteenNavigation = Canteen1,  Products = new List<Product> { products[3] } , City = CityEnum.Breda, Canteen = CanteenEnum.LA, PickupTime = DateTime.Now, Price = 8.99m, Type = TypeEnum.Broodpakket, ReservedById = 2, ImageUrl = "https://rosco-catering.nl/wp-content/uploads/2020/06/Rosco-Catering-Bake-off-box-scaled.jpg" },

		};

	_context.Products.AddRange(products);
	_context.Students.AddRange(students);
	_context.Packets.AddRange(packets);
	_context.SaveChanges();

	return new PacketRepository(_context);
}
	}
}
