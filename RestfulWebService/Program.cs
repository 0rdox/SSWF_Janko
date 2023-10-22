using Domain.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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
var connectionStringApp = builder.Configuration.GetConnectionString("AppDBString");
var connectionStringIdentity = builder.Configuration.GetConnectionString("IdentityDBString");

//DatabaseApp
builder.Services.AddDbContext<AppDBContext>(options => {
    options.UseSqlServer(connectionStringApp);
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
app.UseSwagger();
app.UseSwaggerUI();

//old
//if (app.Environment.IsDevelopment()) {
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
