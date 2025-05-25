using Bogus;

namespace SimpleApiServer.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        // Clear and re-seed only in dev or clean-up scenarios
        if (db.Users.Any() || db.Products.Any() || db.Orders.Any())
            return;

        var productFaker = new Faker<Product>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()));

        var products = productFaker.Generate(50);
        db.Products.AddRange(products);

        var userFaker = new Faker<User>()
            .RuleFor(u => u.Name, f => f.Name.FullName());

        var users = userFaker.Generate(100);
        db.Users.AddRange(users);
        db.SaveChanges(); // Save to get valid User and Product IDs

        var rand = new Random();
        var orders = new List<Order>();

        foreach (var user in users)
        {
            int orderCount = rand.Next(3, 10); // each user gets 3–10 orders

            for (int i = 0; i < orderCount; i++)
            {
                var order = new Order
                {
                    Date = DateTime.UtcNow.AddDays(-rand.Next(1, 365)),
                    UserId = user.Id,
                    Products = new List<Product>()
                };

                var selectedProducts = products
                    .OrderBy(_ => rand.Next())
                    .Take(rand.Next(1, 5))
                    .ToList();

                foreach (var product in selectedProducts)
                {
                    var trackedProduct = db.Products.Local.FirstOrDefault(p => p.Id == product.Id)
                                        ?? db.Products.Find(product.Id); // ensure it's tracked
                    if (trackedProduct != null)
                    {
                        order.Products.Add(trackedProduct);
                    }
                }

                orders.Add(order);
            }
        }

        db.Orders.AddRange(orders);
        db.SaveChanges();
    }
}
