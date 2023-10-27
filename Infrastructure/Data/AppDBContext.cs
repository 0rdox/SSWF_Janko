using Domain.Models;
using Domain.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Options;
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
                .HasMany(p => p.Products) 
                .WithOne(product => product.Packet) 
                .HasForeignKey(product => product.PacketId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DemoProducts>()
                    .HasMany(dp => dp.Products)
                    .WithOne() 
                    .HasForeignKey(p => p.DemoProductsId); 


            base.OnModelCreating(modelBuilder);
        }
    }
}



