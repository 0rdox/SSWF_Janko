using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data {

    public class AppIdentityDBContext : IdentityDbContext {

        public AppIdentityDBContext(DbContextOptions<AppIdentityDBContext> options) : base(options) {
        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder) {
//            base.OnModelCreating(modelBuilder);

//            // Create a new role 'Student'
//            var studentRole = new IdentityRole {
//                Id = "1", // Assign a unique role ID
//                Name = "Student",
//                NormalizedName = "STUDENT"
//            };

//            var employeeRole = new IdentityRole {
//                Id = "2",
//                Name = "Employee",
//                NormalizedName = "EMPLOYEE"
//            };

//           // var roles = new List<IdentityRole>
//           //{
//           //     new IdentityRole { Id = "1", Name = "Student", NormalizedName = "STUDENT" },
//           //     new IdentityRole { Id = "2", Name = "Employee", NormalizedName = "EMPLOYEE" }
//           //     // Add more roles if needed
//           // };


//            var claims = new List<IdentityRoleClaim<string>>
//{
//                new IdentityRoleClaim<string> { Id = 1, RoleId = "1", ClaimType = "Role", ClaimValue = "Student" },
//                new IdentityRoleClaim<string> { Id = 2, RoleId = "2", ClaimType = "Role", ClaimValue = "Employee" }
//                // Add more claims if needed
//            };

//            modelBuilder.Entity<IdentityRole>().HasData(studentRole);
//            modelBuilder.Entity<IdentityRole>().HasData(employeeRole);
//            modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(claims);
//            ////GET STUDENT_ID


//            //// Configure the Student entities to have the 'Student' role
//            //modelBuilder.Entity<IdentityUserRole<string>>()
//            //    .HasData(new IdentityUserRole<string> {
//            //        RoleId = studentRole.Id,
//            //        UserId = "STUDENT_USER_ID_HERE" // Replace with the actual user ID
//            //    });
//        }

    }
}
