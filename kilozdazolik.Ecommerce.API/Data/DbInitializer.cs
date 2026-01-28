using kilozdazolik.Ecommerce.API.Features.Categories;
using kilozdazolik.Ecommerce.API.Features.Products;
using kilozdazolik.Ecommerce.API.Features.Sales;

namespace kilozdazolik.Ecommerce.API.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Products.Any())
        {
            return;
        }

        var electronics = new Category { Id = Guid.NewGuid(), Name = "Electronics" };
        var books = new Category { Id = Guid.NewGuid(), Name = "Books" };
        var clothing = new Category { Id = Guid.NewGuid(), Name = "Clothing" };
        var home = new Category { Id = Guid.NewGuid(), Name = "Home & Garden" };
        var sports = new Category { Id = Guid.NewGuid(), Name = "Sports" };

        context.Categories.AddRange(electronics, books, clothing, home, sports);

        var products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Name = "Gaming Laptop", Price = 1200, CategoryId = electronics.Id },
            new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", Price = 25, CategoryId = electronics.Id },
            new Product { Id = Guid.NewGuid(), Name = "4K Monitor", Price = 300, CategoryId = electronics.Id },
            new Product { Id = Guid.NewGuid(), Name = "Smartphone Pro", Price = 999, CategoryId = electronics.Id },
            new Product { Id = Guid.NewGuid(), Name = "Mechanical Keyboard", Price = 80, CategoryId = electronics.Id },
            new Product { Id = Guid.NewGuid(), Name = "USB-C Cable", Price = 10, CategoryId = electronics.Id },
            new Product { Id = Guid.NewGuid(), Name = "Noise Cancelling Headphones", Price = 150, CategoryId = electronics.Id },

            new Product { Id = Guid.NewGuid(), Name = "Clean Code", Price = 45, CategoryId = books.Id },
            new Product { Id = Guid.NewGuid(), Name = "The Pragmatic Programmer", Price = 50, CategoryId = books.Id },
            new Product { Id = Guid.NewGuid(), Name = "Introduction to Algorithms", Price = 90, CategoryId = books.Id },
            new Product { Id = Guid.NewGuid(), Name = "Design Patterns", Price = 55, CategoryId = books.Id },

            new Product { Id = Guid.NewGuid(), Name = "Running Shoes", Price = 80, CategoryId = clothing.Id },
            new Product { Id = Guid.NewGuid(), Name = "Cotton T-Shirt", Price = 15, CategoryId = clothing.Id },
            new Product { Id = Guid.NewGuid(), Name = "Winter Jacket", Price = 120, CategoryId = clothing.Id },
            new Product { Id = Guid.NewGuid(), Name = "Jeans", Price = 40, CategoryId = clothing.Id },

            new Product { Id = Guid.NewGuid(), Name = "Coffee Maker", Price = 60, CategoryId = home.Id },
            new Product { Id = Guid.NewGuid(), Name = "Blender", Price = 35, CategoryId = home.Id },
            new Product { Id = Guid.NewGuid(), Name = "Office Desk", Price = 150, CategoryId = home.Id },
            new Product { Id = Guid.NewGuid(), Name = "Gaming Chair", Price = 200, CategoryId = home.Id },

            new Product { Id = Guid.NewGuid(), Name = "Yoga Mat", Price = 20, CategoryId = sports.Id },
            new Product { Id = Guid.NewGuid(), Name = "Dumbbell Set", Price = 70, CategoryId = sports.Id },
            new Product { Id = Guid.NewGuid(), Name = "Football", Price = 25, CategoryId = sports.Id }
        };

        context.Products.AddRange(products);
        context.SaveChanges();

        var sales = new List<Sale>();
        var random = new Random();
        var startDate = new DateTime(2024, 1, 1);

        for (int i = 0; i < 30; i++)
        {
            var saleId = Guid.NewGuid();
            var saleDate = startDate.AddDays(random.Next(0, 400));
            var saleDetails = new List<SaleDetail>();
            decimal totalAmount = 0;

            int itemsCount = random.Next(1, 5);
            for (int j = 0; j < itemsCount; j++)
            {
                var product = products[random.Next(products.Count)];
                var quantity = (byte)random.Next(1, 3);
                var detail = new SaleDetail
                {
                    Id = Guid.NewGuid(),
                    SaleId = saleId,
                    ProductId = product.Id,
                    Quantity = quantity,
                    UnitPrice = product.Price
                };
                saleDetails.Add(detail);
                totalAmount += detail.Quantity * detail.UnitPrice;
            }

            sales.Add(new Sale
            {
                Id = saleId,
                Date = saleDate,
                TotalAmount = totalAmount,
                SaleDetails = saleDetails
            });
        }

        context.Sales.AddRange(sales);
        context.SaveChanges();
    }
}