using Application.Activities.Queries;
using Application.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();


builder.Services.AddMediatR(opt =>
    opt.RegisterServicesFromAssemblyContaining<GetActivityList.Handler>());

/*  
    Registering mapping profiles to the application
    Using .Assembly tells the program the location it can expect to find 
    all mapping profiles

*/
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(opt => opt.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("http://localhost:3000", "https://localhost:3000"));

app.MapControllers();

// Use service locator pattern
// Creating a services scope w/ using keyword
// --> When this function goes out of scope AKA the app is run anything that is used in this scope
// --> will be disposed of by the framework so we do not need to clean up after ourselvs
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    // get access to our database context and use that context to query the database
    var context = services.GetRequiredService<AppDbContext>();
    // aync applies any migrations for the context to the database
    await context.Database.MigrateAsync();
    await DbInitializaer.SeedData(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration.");
}

app.Run();
