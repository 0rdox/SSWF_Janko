using Domain.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RestfulWebService.GraphQL;

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

app.UseAuthorization();

app.MapControllers();

app.Run();
