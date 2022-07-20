using CancunHotel.DataContext;
using CancunHotel.DataContext.Data;
using CancunHotel.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<WebApiDbContext>(options => options.UseInMemoryDatabase(databaseName: "CancunHotelDB"));

builder.Services.AddControllers();
IoC.AddDependency(builder.Services);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Cancun Hotel",
        Description = "Test for ALTEN: Web API .NET 6 for managing room in a Cancun Hotel",
        Contact = new OpenApiContact
        {
            Name = "Gerardo Ariza Torres",
            Url = new Uri("https://www.linkedin.com/in/garizato")
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<WebApiDbContext>();
    
    DbInitializer.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
