using Domain.Models;
using Domain.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data {
    public class AppDBContext : DbContext {


        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) {

        }
        public DbSet<Packet> Packets { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Canteen> Canteens { get; set; }

        public DbSet<DemoProducts> DemoProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Packet>(
                entity => {
                    entity.HasOne(d => d.ReservedBy)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey("ReservedById");
                });

            modelBuilder.Entity<Packet>()
                 .HasOne(p => p.CanteenNavigation)
                 .WithMany()
                 .HasForeignKey(p => p.Canteen)
                 .HasPrincipalKey(c => c.Location);


            modelBuilder.Entity<Packet>()
                .HasMany(p => p.Products)  // Packet has many Products
                .WithOne(product => product.Packet) // Product has one related Packet
                .HasForeignKey(product => product.PacketId) // Foreign key property in Product entity
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DemoProducts>()
                    .HasMany(dp => dp.Products) // DemoProducts has many Products
                    .WithOne() // Product has one DemoProducts
                    .HasForeignKey(p => p.DemoProductsId); // D


            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Packet>()
            //	.HasMany(p => p.DemoProducts)  // Packet has many DemoProducts
            //	.WithOne(product => product.DemoPacket) // Product has one related DemoPacket
            //	.HasForeignKey(product => product.DemoPacketId); // Foreign key property in Product entity

            //var products = new List<Product> {
            //	//Eten
            //	new Product{Id = 1, Name = "Worstenbroodje", Alcohol = false, ImageUrl =  "https://www.hetzeilbergsbakkertje.nl/wp-content/uploads/JPM0008-Worstenbroodje-open.jpg" },
            //	new Product{Id = 2, Name = "Saucijzenbroodje", Alcohol = false, ImageUrl = "https://www.bakkerijvoortman.nl/wp/wp-content/uploads/2015/02/saucbr..png"},
            //	new Product{Id = 3, Name = "Kaassoufflé", Alcohol = false, ImageUrl = "https://www.ahealthylife.nl/wp-content/uploads/2021/06/Kaassouffle_voedingswaarde-1.jpg"},
            //	new Product{Id = 4, Name = "Frikandelbroodje", Alcohol = false, ImageUrl = "https://klokhoogeveen.nl/wordpress/wp-content/uploads/2019/04/frikandellenbroodje.png"},
            //	new Product{Id = 5, Name = "Kroket", Alcohol = false, ImageUrl = "https://i0.wp.com/www.frituurwereld.nl/wp-content/uploads/2020/10/Frituurwereld-Krokettendag-hoera.jpg?fit=1212%2C823&ssl=1"},
            //	new Product{Id = 6, Name = "Pistoletje gezond", Alcohol = false, ImageUrl = "https://s-marktscholte.nl/cmcc/wp-content/uploads/2019/01/Broodje-gezond-s-markt.jpg"},
            //	new Product{Id = 7, Name = "Tomatensoep", Alcohol = false, ImageUrl = "https://us.123rf.com/450wm/kostrez/kostrez1509/kostrez150900011/44695927-tomatensoep-in-een-witte-kom-met-peterselie-en-specerijen-op-een-schotel-ge%C3%AFsoleerd-op-een-witte.jpg?ver=6"},
            //	//Zoet
            //	new Product{Id = 8, Name = "Chocolade", Alcohol = false, ImageUrl = "https://img.freepik.com/premium-photo/chocolade-op-witte-achtergrond_404043-1540.jpg"},
            //	new Product{Id = 9, Name = "Koekje", Alcohol = false, ImageUrl = "https://media.istockphoto.com/id/1251818705/photo/large-chocolate-chip-cookie-on-a-white-plate-with-a-white-background.jpg?s=612x612&w=0&k=20&c=vDhGIb54eGfIQKI1gfuDGp7j29ppw3ioTa-Wwwrg3Vc="},
            //	//Drank
            //	new Product{Id = 10, Name = "Bier", Alcohol = true, ImageUrl = "https://thumbs.dreamstime.com/b/koud-bier-43280582.jpg"},
            //	new Product{Id = 11, Name = "Wijn", Alcohol = true, ImageUrl = "https://static.vecteezy.com/ti/gratis-vector/p3/7324461-wijnglazen-met-witte-wijn-illustratie-van-wijnglazen-geisoleerd-op-witte-achtergrond-gratis-vector.jpg"},
            //	new Product{Id = 12, Name = "Whisky", Alcohol = true, ImageUrl = "https://img.freepik.com/premium-photo/glas-schotse-whisky-en-ijs-op-een-witte-achtergrond_38145-1376.jpg?w=2000"},
            //	new Product{Id = 13, Name = "Vodka", Alcohol = true, ImageUrl = "https://media.istockphoto.com/id/671705556/photo/vodka-in-vintage-glass.jpg?s=612x612&w=0&k=20&c=Tz58z7MkbFq2ziaK92nid4KEp84T30pFlS6J6pAmI08="},
            //	new Product{Id = 14, Name = "Tequila Sunrise", Alcohol = true, ImageUrl = "https://img.freepik.com/premium-photo/cocktail-tequila-sunrise-front-white-background_118454-21367.jpg?w=2000"},
            //};






            ////==========LIST===========\\

            //List<Product> drankList = new List<Product> {
            //new Product("Bier", true, "https://thumbs.dreamstime.com/b/koud-bier-43280582.jpg"),
            //new Product("Wijn", true, "https://static.vecteezy.com/ti/gratis-vector/p3/7324461-wijnglazen-met-witte-wijn-illustratie-van-wijnglazen-geisoleerd-op-witte-achtergrond-gratis-vector.jpg"),
            //};

            //List<Product> broodjeList = new List<Product> {
            //new Product("Worstenbroodje", false, "https://www.hetzeilbergsbakkertje.nl/wp-content/uploads/JPM0008-Worstenbroodje-open.jpg"),
            //new Product("Saucijzenbroodje", false, "https://www.bakkerijvoortman.nl/wp/wp-content/uploads/2015/02/saucbr..png")
            //};

            ////==========END LIST===========\\







            //var productIdsForLekkereBroodjes = broodjeList.Select(p => p.Id).ToList();
            //var productIdsForDrankpakket = drankList.Select(p => p.Id).ToList();




            //Student student1 = new Student { Id = 1, Name = "John Doe", StudentNumber = 12345, DateOfBirth = new DateTime(2000, 5, 15), Email = "john.doe@example.com", City = CityEnum.Breda, Phone = "555-555-5555" };

            //IEnumerable<Student> students = new List<Student> {
            //	student1,
            //	new Student{Id = 2, Name = "Mike Evars", StudentNumber = 52341, DateOfBirth = new DateTime(1998, 3, 29),Email = "mike.evars@example.com",City = CityEnum.Tilburg,Phone = "555-555-5555" },
            //	new Student{Id = 3, Name = "Kaitlyn Marek", StudentNumber = 51221, DateOfBirth = new DateTime(2001, 12, 18),Email = "kaitlyn.marek@example.com",City = CityEnum.Breda,Phone = "555-555-5555" }
            //};



            //Packet packet1 = new Packet { Name = "Lekkere Broodjes", Id = 1, City = CityEnum.Breda,/*Products = broodjeList,*/ Canteen = CanteenEnum.LA, DateTime = DateTime.Now, Price = 8.99m, Type = TypeEnum.Broodpakket, ReservedById = student1.Id, ImageUrl = "https://rosco-catering.nl/wp-content/uploads/2020/06/Rosco-Catering-Bake-off-box-scaled.jpg" };
            //Packet packet2 = new Packet { Name = "Drankpakket", Id = 2, City = CityEnum.Breda, /*Products=drankList, */Canteen = CanteenEnum.LA, DateTime = DateTime.Now, Price = 14.99m, Type = TypeEnum.Drankpakket, ReservedById = null, ImageUrl = "https://www.foodandwine.com/thmb/a3jODP_x_GpJpD71zT3t3BYbtp8=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/how-to-ship-wine-FT-BLOG1221-073f4b1897c34f04bff8ea71dadcba2c.jpg" };

            //var packets = new List<Packet> {
            //   packet1,
            //   packet2
            //};





            //IEnumerable<Employee> employees = new List<Employee> {
            //	new Employee {EmployeeID = 1, Name = "Derek Stor", Email="derekstor@mail.com", Canteen = CanteenEnum.LA},
            //	new Employee {EmployeeID = 2, Name = "Jenny Berk", Email="jennyberk@mail.com",Canteen = CanteenEnum.LA},
            //	new Employee {EmployeeID = 3, Name = "Sarah Lee",Email="sarahlee@mail.com", Canteen = CanteenEnum.HA},
            //	new Employee {EmployeeID = 4, Name = "Dom Peters",Email="dompeters@mail.com", Canteen = CanteenEnum.LD}
            //};

            //IEnumerable<Canteen> canteens = new List<Canteen> {
            //	new Canteen {ID = 1, City = CityEnum.Breda, Location = CanteenEnum.LA, OffersHotMeals= false},
            //	new Canteen {ID = 2, City = CityEnum.Tilburg, Location = CanteenEnum.HA, OffersHotMeals= true},
            //	new Canteen {ID = 3, City = CityEnum.Breda, Location = CanteenEnum.LD, OffersHotMeals= true}
            //};

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Product>().HasData(products);
            //modelBuilder.Entity<Packet>().HasData(packets);
            //modelBuilder.Entity<Student>().HasData(students);
            //modelBuilder.Entity<Employee>().HasData(employees);
            //modelBuilder.Entity<Canteen>().HasData(canteens);



            //var studentsList = modelBuilder.Entity<Student>(); // Replace 'Student' with your actual entity name
            //foreach (var student in students) {
            //	modelBuilder.Entity<IdentityUserClaim<string>>().HasData(
            //		new IdentityUserClaim<string> {
            //			Id = student.Id.GetHashCode(),
            //			UserId = student.Id.ToString(),
            //			ClaimType = "Student",
            //			ClaimValue = "true"
            //		}
            //	);
            //}




        }
    }
}



