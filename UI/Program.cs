using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.EntityFrameworkCore;
using Domain.Services;
using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

//TODO: Check all tests on business logic

//Azure price not working --> i think its sending a price from view instead of string which is weird. 

//TODO: 2. TESTS
//TODO: 3. Postman Collections
//TODO: 6. Validation error afhandelen
//TODO: 7. UML Diagrams



//TESTS:
//Domain.Services
//Infrastructure
//UI
//API

//--------------------------------SESSION-------------------------------\\
builder.Services.AddDistributedMemoryCache()
    .AddSession(options => {
        options.IdleTimeout = TimeSpan.FromMinutes(10);
    });
//--------------------------DEPENDENCY INJECTION--------------------------\\
builder.Services
    .AddScoped<ICanteenRepository, CanteenRepository>()
    .AddScoped<IPacketRepository, PacketRepository>()
    .AddScoped<IProductRepository, ProductRepository>()
    .AddScoped<IEmployeeRepository, EmployeeRepository>()
    .AddScoped<IStudentRepository, StudentRepository>()
    .AddScoped<IDemoProductRepository, DemoProductRepository>();


builder.Services.AddScoped<RoleAssigner>();
builder.Services.AddScoped<SeedData>();



//--------------------------DATABASE PARTS--------------------------\\
var connectionStringApp = builder.Configuration.GetConnectionString("AzureAppDBString");
var connectionStringIdentity = builder.Configuration.GetConnectionString("AzureIdentityDBString");

//AppDatabase
builder.Services.AddDbContext<AppDBContext>(options => {
    options.UseSqlServer(connectionStringApp);
});
//IdentityDatabase
builder.Services.AddDbContext<AppIdentityDBContext>(options => {
    options.UseSqlServer(connectionStringIdentity);
});

//IdentitySettings
builder.Services.AddIdentity<IdentityUser, IdentityRole>(conf => {
    conf.Password.RequiredLength = 4;
    conf.Password.RequireDigit = false;
    conf.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<AppIdentityDBContext>()
    .AddDefaultTokenProviders();



//--------------------------COOKIES & POLICIES--------------------------\\
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie(conf => {
        conf.Cookie.Name = "IdentityCookie";
        conf.LoginPath = "/Account/Login";
    });


//POLICIES
builder.Services.AddAuthorization(policyBuilder => {
    policyBuilder.AddPolicy("Student", policy => {
        policy.RequireAuthenticatedUser() 
              .RequireRole("Student");
    });
    policyBuilder.AddPolicy("Employee", policy => {
        policy.RequireAuthenticatedUser()
              .RequireRole("Employee");
    });
});


//--------------------------APP--------------------------\\
var app = builder.Build();


// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment()) {
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}



app.UseDeveloperExceptionPage();
app.UseDatabaseErrorPage();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

////catch 404
//app.MapControllerRoute(
//    name: "catchAll",
//    pattern: "{*url}", 
//    defaults: new { controller = "Account", action = "Login" }
//);




app.UseSession();


//ASSIGN ROLES + SEED DATABASE
using var scope = app.Services.CreateScope();
var roleAssigner = scope.ServiceProvider.GetRequiredService<RoleAssigner>();
var dataSeeder = scope.ServiceProvider.GetService<SeedData>();
//Assign roles
await roleAssigner.AssignRolesToStudentsAndEmployees();
//Seed database
dataSeeder?.SeedDatabase();
dataSeeder?.AddAdditionalPackets();









app.Run();



//NOTES
//      DESIGN CHANGES na UX-Design assessment
//ipv rode knop Annuleren ipv rode knop -> rode tekst op witte knop (minder prominent maken)
//opslaan switchen met product toevoegen
//wallpaper watermark -- login foto
//pakketdetails demoproducts - minimum 2 regels
//iconen ietsje kleiner - -meer ruimte - tekstlabel eronder (outline?)