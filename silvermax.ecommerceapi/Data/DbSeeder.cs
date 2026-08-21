// Data/DbSeeder.cs
using Bogus;
using Microsoft.EntityFrameworkCore;
using silvermax.ecommerceapi.Models;

namespace silvermax.ecommerceapi.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(EcommerceDbContext db)
    {
        if (await db.Products.AnyAsync())
            return;

        // 1. Categories — no dependencies
        var categoryFaker = new Faker<Category>()
            .RuleFor(c => c.CategoryId, f => Guid.NewGuid())
            .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0]);

        var categories = categoryFaker.Generate(10)
            .DistinctBy(c => c.Name)
            .ToList();

        // 2. Clients — no dependencies
        var clientFaker = new Faker<Client>()
            .RuleFor(c => c.ClientId, f => Guid.NewGuid())
            .RuleFor(c => c.Name, f => f.Name.FullName())
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber());

        var clients = clientFaker.Generate(50);

        // 3. Products — depend on Categories
        var productFaker = new Faker<Product>()
            .RuleFor(p => p.ProductId, f => Guid.NewGuid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Price, f => Math.Round(f.Random.Double(5, 500), 2))
            .RuleFor(p => p.ImgUrl, f => f.Image.PicsumUrl())
            .RuleFor(p => p.Category, f => f.PickRandom(categories))
            .FinishWith((f, p) => p.CategoryId = p.Category.CategoryId);

        var products = productFaker.Generate(60);

        db.Categories.AddRange(categories);
        db.Clients.AddRange(clients);
        db.Products.AddRange(products);
        await db.SaveChangesAsync(CancellationToken.None);

        // 4. Orders + OrderItems
        var faker = new Faker();
        var orders = new List<Order>();

        foreach (var client in clients)
        {
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                Moment = faker.Date.Past(1),
                OrderStatus = faker.PickRandom<OrderStatus>(),
                ClientId = client.ClientId,
                Client = client
            };

            var lineCount = faker.Random.Int(1, 5);
            var chosenProducts = faker.PickRandom(products, lineCount).ToList();

            double total = 0;
            foreach (var product in chosenProducts)
            {
                var quantity = faker.Random.Int(1, 4);
                var orderItem = new OrderItem
                {
                    OrderItemId = Guid.NewGuid(),
                    OrderId = order.OrderId,
                    Order = order,
                    ProductId = product.ProductId,
                    Product = product,
                    Quantity = quantity,
                    Price = product.Price
                };
                order.OrderItems.Add(orderItem);
                total += quantity * product.Price;
            }

            order.Total = total;
            orders.Add(order);
        }

        db.Orders.AddRange(orders);
        await db.SaveChangesAsync(CancellationToken.None);
    }
}
