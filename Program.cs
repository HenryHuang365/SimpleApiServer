using Microsoft.EntityFrameworkCore;
using SimpleApiServer.Data;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Simple API Server",
        Version = "v1",
        Description = "A simple Web API built with .NET 8 for learning purposes",
        Contact = new OpenApiContact
        {
            Name = "Henry",
            Email = "henry@example.com"
        }
    });
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

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

// ---------------------- USERS ----------------------

// app.MapGet("/users", async (AppDbContext db) =>
//     await db.Users
//         .Include(u => u.Orders)
//             .ThenInclude(o => o.Products)
//         .Select(u => new
//         {
//             u.Id,
//             u.Name,
//             Orders = u.Orders.Select(o => new
//             {
//                 o.Id,
//                 o.Date,
//                 Products = o.Products.Select(p => new
//                 {
//                     p.Id,
//                     p.Name,
//                     p.Price
//                 })
//             })
//         })
//         .ToListAsync());
