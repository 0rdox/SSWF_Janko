﻿using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;





using Microsoft.EntityFrameworkCore;

using System;

using System.Collections.Generic;

using System.Linq;



using System.Text;

using System.Text.RegularExpressions;

using System.Threading.Tasks;
using Infrastructure.Data;
using Domain.Models.Enums;

namespace Infrastructure {

    public class SeedData {

        private readonly string password = "Secret";

        private readonly AppDBContext _dbContext;

        private readonly AppIdentityDBContext _securityContext;

        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly UserManager<IdentityUser> _userManager;



        public SeedData(AppDBContext dbContext, AppIdentityDBContext securityContext, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager) {

            _dbContext = dbContext;
            _securityContext = securityContext;
            _signInManager = signInManager;
            _userManager = userManager;

        }



        public void SeedDatabase() {
            _securityContext.Database.Migrate();
            _dbContext.Database.Migrate();


            if (!_dbContext.Packets.Any() && !_dbContext.DemoProducts.Any()) {
                SeedDB().Wait();
            }


        }



        public async Task SeedDB() {



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
                new Student{Name = "Kaitlyn Marek", StudentNumber = 51221, DateOfBirth = new DateTime(2001, 12, 18),Email = "kaitlyn.marek@example.com",City = CityEnum.Breda,Phone = "555-555-5555" }
            };


            var packets = new List<Packet> {

            new Packet { Name = "Lekkere Broodjes" , City = CityEnum.Breda,/*Products = broodjeList,*/ Canteen = CanteenEnum.LA, DateTime = DateTime.Now, Price = 8.99m, Type = TypeEnum.Broodpakket, ReservedById = null, ImageUrl = "https://rosco-catering.nl/wp-content/uploads/2020/06/Rosco-Catering-Bake-off-box-scaled.jpg" },
            new Packet { Name = "Drankpakket", City = CityEnum.Breda, /*Products=drankList, */Canteen = CanteenEnum.LA, DateTime = DateTime.Now, Price = 14.99m, Type = TypeEnum.Drankpakket, ReservedById = null, ImageUrl = "https://www.foodandwine.com/thmb/a3jODP_x_GpJpD71zT3t3BYbtp8=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/how-to-ship-wine-FT-BLOG1221-073f4b1897c34f04bff8ea71dadcba2c.jpg" }

        };





            DemoProducts[] demoProducts = {

                    new DemoProducts() {Products = new List<Product>(){ products[0], products[1], products[2]}, Type = TypeEnum.Broodpakket},
                    new DemoProducts() {Products = new List<Product>(){ products[4], products[5], products[6]}, Type = TypeEnum.Drankpakket},
                    new DemoProducts() {Products = new List<Product>(){ products[9], products[10], products[11] }, Type = TypeEnum.WarmeMaaltijd},

            };





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


            _dbContext.Canteens.AddRange(canteens);
            _dbContext.Employees.AddRange(employees);
            _dbContext.Products.AddRange(products);
            _dbContext.Packets.AddRange(packets);
            _dbContext.DemoProducts.AddRange(demoProducts);
            _dbContext.Students.AddRange(students);

            await _dbContext.SaveChangesAsync();

        }

        public async Task AddAdditionalPackets() {
            // Create new packets
            var additionalPackets = new List<Packet>
            {
            new Packet { Name = "Warme Maaltijd 1" , City = CityEnum.Breda,Canteen = CanteenEnum.LA, DateTime = DateTime.Now.AddDays(1), Price = 8.99m, Type = TypeEnum.Broodpakket, ReservedById = null, ImageUrl = "https://rosco-catering.nl/wp-content/uploads/2020/06/Rosco-Catering-Bake-off-box-scaled.jpg" },
            new Packet { Name = "Warme Maaltijd 2" , City = CityEnum.Tilburg,Canteen = CanteenEnum.HA, DateTime = DateTime.Now.AddHours(4), Price = 8.99m, Type = TypeEnum.Broodpakket, ReservedById = null, ImageUrl = "https://rosco-catering.nl/wp-content/uploads/2020/06/Rosco-Catering-Bake-off-box-scaled.jpg" },
            new Packet { Name = "Warme Maaltijd 3" , City = CityEnum.Tilburg,Canteen = CanteenEnum.HA, DateTime = DateTime.Now.AddHours(3), Price = 8.99m, Type = TypeEnum.Broodpakket, ReservedById = null, ImageUrl = "https://rosco-catering.nl/wp-content/uploads/2020/06/Rosco-Catering-Bake-off-box-scaled.jpg" },
                    new Packet { Name = "Drankpakket 3", City = CityEnum.Breda, Canteen = CanteenEnum.LD, DateTime = DateTime.Now.AddHours(19), Price = 14.99m, Type = TypeEnum.Drankpakket, ReservedById = null, ImageUrl = "https://www.foodandwine.com/thmb/a3jODP_x_GpJpD71zT3t3BYbtp8=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/how-to-ship-wine-FT-BLOG1221-073f4b1897c34f04bff8ea71dadcba2c.jpg" }

                // Add more packets as needed
            };

            // Add the new packets to the database
            _dbContext.Packets.AddRange(additionalPackets);
            await _dbContext.SaveChangesAsync();

        }


    }


}