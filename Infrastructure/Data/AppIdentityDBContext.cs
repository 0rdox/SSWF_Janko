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
    }
}
