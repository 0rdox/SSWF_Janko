using Domain.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RestfulWebService.GraphQL;



using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
 
builder.Services
    .AddScoped<ICanteenRepository, CanteenRepository>()
    .AddScoped<IPacketRepository, PacketRepository>()
    .AddScoped<IProductRepository, ProductRepository>()
    .AddScoped<IEmployeeRepository, EmployeeRepository>()
    .AddScoped<IStudentRepository, StudentRepository>()
    .AddScoped<IDemoProductRepository, DemoProductRepository>();

var connectionStringApp = builder.Configuration.GetConnectionString("AzureAppDBString");
var connectionStringIdentity = builder.Configuration.GetConnectionString("AzureIdentityDBString");

//DatabaseApp
builder.Services.AddDbContext<AppDBContext>(options => {
    options.UseSqlServer(connectionStringApp);
}, ServiceLifetime.Singleton);


//IDENTITY
builder.Services.AddDbContext<AppIdentityDBContext>(options => {
	options.UseSqlServer(connectionStringIdentity);
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => 
options.SignIn.RequireConfirmedEmail = false)
			.AddEntityFrameworkStores<AppIdentityDBContext>().AddDefaultTokenProviders();

// Configure JWT usage.
builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
	options.TokenValidationParameters.ValidateAudience = false;
	options.TokenValidationParameters.ValidateIssuer = false;
	options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyVeryLong"));
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


builder.Services.AddGraphQLServer()
    .AddQueryType<OrderQuery>()
        .RegisterDbContext<AppDBContext>(DbContextKind.Pooled);




builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("restful-api", new OpenApiInfo { Title = "EcoTaste Web API", Version = "v1" });
});




////DatabaseIdentity
//builder.Services.AddDbContext<AppIdentityDBContext>(options => {
//    options.UseSqlServer(connectionStringIdentity);
//});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
}



//RESTful
app.UseSwagger();

app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/restful-api/swagger.json", "EcoTaste RESTful API V1");
});




//old
//if (app.Environment.IsDevelopment()) {
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.MapGraphQL();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
