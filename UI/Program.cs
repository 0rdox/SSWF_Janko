using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.EntityFrameworkCore;
using Domain.Services;
using Infrastructure.Repositories;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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
//change database connection strings to azure
var connectionStringApp = builder.Configuration.GetConnectionString("AzureAppDBString");
var connectionStringIdentity = builder.Configuration.GetConnectionString("AzureIdentityDBString");

//DatabaseApp
builder.Services.AddDbContext<AppDBContext>(options => {
    options.UseSqlServer(connectionStringApp);
});
//DatabaseIdentity
builder.Services.AddDbContext<AppIdentityDBContext>(options => {
    options.UseSqlServer(connectionStringIdentity);
});

//IDENTITY
builder.Services.AddIdentity<IdentityUser, IdentityRole>(conf => {
    conf.Password.RequiredLength = 4;
    conf.Password.RequireDigit = false;
    conf.Password.RequireNonAlphanumeric = false;

    // conf.SignIn.RequireConfirmedEmail = true;
})
    .AddEntityFrameworkStores<AppIdentityDBContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication("CookieAuth")
    .AddCookie(conf => {
        conf.Cookie.Name = "Cookie";
        conf.LoginPath = "/Account/Login";
    });


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

//todo: access denied method

var app = builder.Build();

//session



//// Configure the HTTP request pipeline.
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

app.UseSession();


//ASSIGN ROLES
using var scope = app.Services.CreateScope();
var roleAssigner = scope.ServiceProvider.GetRequiredService<RoleAssigner>();
await roleAssigner.AssignRolesToStudentsAndEmployees();

//Seed date for PacketContext and SecurityContext
using (var roleScope = app.Services.CreateScope()) {
    var service = roleScope.ServiceProvider;
    var dataSeeder = service.GetService<SeedData>();
    dataSeeder?.SeedDatabase();
}

//using var scope3 = app.Services.CreateScope();
//var dataSeeder2 = scope.ServiceProvider.GetRequiredService<SeedData>();
//await dataSeeder2.AddAdditionalPackets();

app.Run();

//WARNING USING AZURE DATABASE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//maybe i first need to push git before the database updates in azure? that seems dumb but maybe
//check why add-migration and update-database doesnt update the azure database
//maybe in code clear database and then rebuild using seedData and add dummy data
//
//TODO: change demo products

//TODO: add input validation into form. 
//TODO: Update and delete packet. 


//todo:
//cookie policy?


////--------------------------USERSTORY1--------------------------\\//--------------------------USERSTORY1--------------------------\\
//US_01 ALS STUDENT WIL IK KUNNEN ZIEN WELKE PAKKETTEN ER AANGEBODEN
//WORDEN EN WELKE IK GERESERVEERD HEB, ZODAT IK MAKKELIJK NAAR DEZE KAN
//NAVIGEREN

//Toelichting:
//Pakketten kunnen een paar dagen van tevoren al gepubliceerd worden door een kantine op basis van een
//inschatting van de medewerkers. Als student wil je natuurlijk een makkelijk overzicht hebben over de
//beschikbare pakketten.

//Acceptatiecriteria:
//Er zijn twee pagina’s. Een waarin alle aangeboden pakketten te zien zijn die nog niet gereserveerd zijn
//en één waarin de door de gebruiker gereserveerde pakketten te zien zijn, met informatie wanneer elk
//pakket opgehaald moet worden.
////--------------------------USERSTORY2--------------------------\\//--------------------------USERSTORY2--------------------------\\
//US_02 ALS KANTINEMEDEWERKER WIL IK KUNNEN ZIEN WELKE PAKKETTEN ER
//AANGEBODEN WORDEN, ZODAT IK WEET WAT HET HUIDIGE AANBOD AL IS

//Toelichting:
//Een kantinemedewerker heeft natuurlijk inzicht nodig in de pakketten die aangeboden worden. Om het
//aanbod eventueel zo breed mogelijk te maken, moet een kantinemedewerker ook het aanbod van andere
//kantines kunnen zien.

//Acceptatiecriteria:
//Een kantinemedewerker kan het aanbod aan pakketten van zijn eigen kantine zien. De lijst moet
//gesorteerd worden oplopend op datum.

//Een kantinemedewerker moet ook het aanbod van pakketten kunnen zien van de andere kantines.
//Ook deze lijst moet oplopend op datum worden gesorteerd.
////--------------------------USERSTORY3--------------------------\\//--------------------------USERSTORY3--------------------------\\
//US_03 ALS KANTINEMEDEWERKER WIL IK EEN PAKKET KUNNEN AANBIEDEN, ZODAT EEN
//STUDENT DEZE KAN RESERVEREN.

//Toelichting:
//Als kantinemedewerker kan ik op basis van ervaring en de evenementen die op een dag gehouden worden een
//pakket samen kunnen stellen. Ik mag dit voor maximaal 2 dagen vooruit plannen, om teleurstellingen te
//voorkomen doordat pakketten aangeboden worden die niet gevuld kunnen worden met producten.


//Acceptatiecriteria:

//Een kantinemedewerker moet een nieuw pakket, inclusief producten, toe kunnen voegen, kunnen
//wijzigen en verwijderen. De locatie van de medewerker moet hierbij als locatie voor het pakket
//worden gebruikt.

//Wijzigen of verwijderen van een pakket, inclusief gekoppelde producten, mag alleen als er nog geen
//reserveringen voor zijn.

//De kantinemedewerker moet een overzicht hebben van de pakketten die op zijn locatie aangeboden
//worden. De lijst moet oplopend op datum gesorteerd worden.
////--------------------------USERSTORY4--------------------------\\//--------------------------USERSTORY4--------------------------\\
//US_04 ALS KANTINEMEDEWERKER WIL IK EEN PAKKET ALLEEN VOOR VOLWASSENEN
//BESCHIKBAAR STELLEN, ZODAT MINDERJARIGEN NIET KUNNEN RESERVEREN.

//Toelichting:
//Bij een aantal evenementen blijven soms ook alcoholhoudende producten over. Deze mogen natuurlijk niet
//aan minderjarigen verstrekt worden.

//Acceptatiecriteria:

//Een product heeft een eigenschap of het alcoholhoudend kan zijn.

//Een pakket heeft een 18+ indicator. Deze wordt automatisch gezet als er een product in het pakket zit
//dat alcoholhoudend is.

//Iemand van onder de 18 kan geen 18+ pakket reserveren. (Controle dient plaats te vinden ten
//opzichte van de ophaaldatum).
////--------------------------USERSTORY5--------------------------\\//--------------------------USERSTORY5--------------------------\\
//US_05 ALS STUDENT WIL IK EEN PAKKET KUNNEN RESERVEREN, ZODAT IK GOEDKOOP
//PRODUCTEN KAN BEMACHTIGEN.

//Toelichting:
//Als student is het natuurlijk handig om goedkoop producten te kunnen kopen. Het systeem werkt op basis van
//reservering, dus als een student interesse heeft in een pakket, dan kan hij of zij dat product reserveren.

//Acceptatiecriteria:
//Als student moet ik een pakket kunnen reserveren.
//Een student mag maximaal 1 pakket per afhaaldag reserveren.
////--------------------------USERSTORY6--------------------------\\//--------------------------USERSTORY6--------------------------\\
//US_06 ALS STUDENT WIL IK WETEN WAT VOOR PRODUCTEN ER ONGEVEER IN EEN
//PAKKET ZITTEN, ZODAT IK WEET OF HET PAKKET INTERESSANT VOOR MIJ IS.

//Toelichting:
//Natuurlijk wil je als student wel weten wat je ongeveer kunt verwachten in je pakket. Daarom wordt een aantal
//voorbeeldproducten aan een pakket toegevoegd, zodat een student een indruk kan krijgen wat er in het
//verleden in een soortgelijk pakket zit.

//Acceptatiecriteria:
//In de eigenschappen van een pakket zijn de producten zichtbaar

//In de eigenschappen van een product is duidelijk en aantrekkelijk weergegeven.
////--------------------------USERSTORY7--------------------------\\//--------------------------USERSTORY7--------------------------\\
//US_07 ALS KANTINEMEDEWERKER WIL IK GEEN DUBBELE RESERVERVINGEN HEBBEN
//VOOR EEN PAKKET, ZODAT EEN PAKKET MAAR EEN KEER GERESERVEEERD KAN WORDEN

//Toelichting:
//Meerdere studenten kunnen natuurlijk interesse hebben in een pakket. Binnen ons systeem gaan we dit heel
//simpel oplossen door de reservering te gunnen aan de eerste student die op reserveren klikt.
//Acceptatiecriteria:

//Het systeem moet controleren of er al een reservering voor een pakket is. Als deze er al is, dan moet
//het systeem een klantvriendelijke foutmelding geven aan de student.

//Als er nog geen reservering is, dan wordt de student geregistreerd als de student die het pakket mag
//komen ophalen.
////------------------------EXTRA USERSTORY------------------------\\//------------------------EXTRA USERSTORY------------------------\\
//US_09 ALS KANTINEMEDEWERKER MAG IK ALLEEN EEN WARME MAALTIJD PAKKET
//OPVOEREN ALS DAT OOK OP MIJN LOCATIE AANGEBODEN WORDT.

//Toelichting:
//Niet alle locaties bieden warme maaltijden als diner aan. Om vergissingen te voorkomen mag een
//kantinemedewerker alleen warme maaltijd pakketten opvoeren als deze ook op de locatie van de medewerker
//aangeboden worden.

//Acceptatiecriteria:
//Een pakket heeft een indicatie of het een warme maaltijd is.

//Bij een medewerker wordt bijgehouden op welke locatie hij of zij werkzaam is en of op deze locatie
//warme avondmaaltijden aangeboden worden.

//Een medewerker mag alleen een warme avondmaaltijd opvoeren als dit ook op zijn locatie
//aangeboden wordt




//NOTE
//Add migration and update database. oppassen met welke data erin zit --> Seed Database niet vergeten.