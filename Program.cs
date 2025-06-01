using Microsoft.EntityFrameworkCore;
using SimpleApiServer.Data;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Filters;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();

var xmlFile = Environment.GetEnvironmentVariable("SWAGGER_XML_DOC") ?? "SimpleApiServer.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Simple API Server",
        Version = "v1",
        Description = "A simple Web API server built with .NET 9 for learning purposes",
        Contact = new OpenApiContact
        {
            Name = "Henry",
            Email = "henry@example.com"
        }
    });
    options.ExampleFilters();
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddMemoryCache();

builder.Services.AddControllers();



var app = builder.Build();

// Enable Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // Applies migrations
    DbSeeder.Seed(db);     // Seeds data
}

app.MapControllers();

app.Run();
