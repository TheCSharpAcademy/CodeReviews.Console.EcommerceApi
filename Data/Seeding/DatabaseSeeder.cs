using ECommerceApp.RyanW84.Data.Models;

namespace ECommerceApp.RyanW84.Data.Seeding;

/// <summary>
/// Handles database seeding with initial test data for products, categories, and sales.
/// Follows SRP by isolating seed data logic from DbContext.
/// </summary>
public static class DatabaseSeeder
{
    public static void SeedDatabase(ECommerceDbContext context)
    {
        if (HasExistingData(context))
        {
            ClearExistingData(context);
        }

        var categories = SeedCategories(context);
        var products = SeedProducts(context, categories);
        SeedSales(context, products);

        context.SaveChanges();
    }

    private static bool HasExistingData(ECommerceDbContext context)
    {
        return context.Categories.Any() || context.Products.Any() || context.Sales.Any();
    }

    private static void ClearExistingData(ECommerceDbContext context)
    {
        context.Sales.RemoveRange(context.Sales);
        context.SaleItems.RemoveRange(context.SaleItems);
        context.Products.RemoveRange(context.Products);
        context.Categories.RemoveRange(context.Categories);
        context.SaveChanges();
    }

    private static List<Category> SeedCategories(ECommerceDbContext context)
    {
        var categories = new[]
        {
            new Category { Name = "Electronics", Description = "Electronic devices and gadgets" },
            new Category { Name = "Clothing", Description = "Apparel and fashion items" },
            new Category { Name = "Books", Description = "Books and publications" },
            new Category
            {
                Name = "Home & Garden",
                Description = "Home improvement and gardening supplies",
            },
            new Category { Name = "Sports", Description = "Sports and outdoor equipment" },
            new Category { Name = "Furniture", Description = "Home furniture and decor" },
            new Category { Name = "Toys", Description = "Toys and games for all ages" },
            new Category { Name = "Beauty", Description = "Beauty and personal care products" },
            new Category { Name = "Food & Beverage", Description = "Food and drink items" },
            new Category { Name = "Automotive", Description = "Car accessories and parts" },
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();
        return categories.ToList();
    }

    private static List<Product> SeedProducts(ECommerceDbContext context, List<Category> categories)
    {
        var products = GetProductSeedData(categories);
        context.Products.AddRange(products);
        context.SaveChanges();
        return products.ToList();
    }

    private static Product[] GetProductSeedData(List<Category> categories)
    {
        var products = new List<Product>();
        var random = new Random(42);
        var productNames = BuildProductNames();
        var descriptions = BuildProductDescriptions();

        int productIndex = 0;
        DistributeProductsAcrossCategories(
            categories,
            productNames,
            descriptions,
            random,
            products,
            ref productIndex
        );
        FillRemainingProducts(
            categories,
            productNames,
            descriptions,
            random,
            products,
            ref productIndex
        );

        return products.ToArray();
    }

    private static string[] BuildProductNames()
    {
        return new[]
        {
            "Laptop",
            "Mouse",
            "Keyboard",
            "Monitor",
            "Headphones",
            "Webcam",
            "USB Hub",
            "External SSD",
            "Graphics Card",
            "Power Supply",
            "RAM Module",
            "Motherboard",
            "CPU Cooler",
            "Case Fan",
            "HDMI Cable",
            "T-Shirt",
            "Jeans",
            "Jacket",
            "Sweater",
            "Shorts",
            "Socks",
            "Hat",
            "Scarf",
            "Gloves",
            "Shoes",
            "C# Programming",
            "Design Patterns",
            "Clean Code",
            "The Pragmatic Programmer",
            "Code Complete",
            "Garden Hose",
            "Lawn Mower",
            "Paint Set",
            "Tool Set",
            "Light Fixture",
            "Basketball",
            "Yoga Mat",
            "Running Shoes",
            "Tennis Racket",
            "Dumbbell Set",
            "Office Chair",
            "Desk",
            "Bookshelf",
            "Board Game", "Action Figure",
        };
    }

    private static string[] BuildProductDescriptions()
    {
        return new[]
        {
            "High-quality item",
            "Premium product",
            "Professional grade",
            "Best seller",
            "Durable and reliable",
            "Great value",
            "Excellent quality",
            "Top rated",
            "Customer favorite",
            "Long lasting",
        };
    }

    private static void DistributeProductsAcrossCategories(
        List<Category> categories,
        string[] productNames,
        string[] descriptions,
        Random random,
        List<Product> products,
        ref int productIndex
    )
    {
        foreach (var category in categories)
        {
            int productsPerCategory = productIndex < 45 ? 5 : (productIndex < 50 ? 10 : 0);
            for (int i = 0; i < productsPerCategory && productIndex < 50; i++)
            {
                products.Add(
                    CreateProduct(
                        productNames,
                        descriptions,
                        random,
                        category.CategoryId,
                        productIndex
                    )
                );
                productIndex++;
            }
        }
    }

    private static void FillRemainingProducts(
        List<Category> categories,
        string[] productNames,
        string[] descriptions,
        Random random,
        List<Product> products,
        ref int productIndex
    )
    {
        while (productIndex < 50)
        {
            var randomCategoryId = categories[random.Next(categories.Count)].CategoryId;
            products.Add(
                CreateProduct(productNames, descriptions, random, randomCategoryId, productIndex)
            );
            productIndex++;
        }
    }

    private static Product CreateProduct(
        string[] productNames,
        string[] descriptions,
        Random random,
        int categoryId,
        int productIndex
    )
    {
        return new Product
        {
            Name =
                $"{productNames[productIndex % productNames.Length]} {(productIndex / productNames.Length) + 1}",
            Description = descriptions[random.Next(descriptions.Length)],
            Price = Math.Round((decimal)(10 + random.Next(0, 500) + random.NextDouble()), 2),
            Stock = random.Next(5, 200),
            IsActive = true,
            CategoryId = categoryId,
        };
    }

    private static void SeedSales(ECommerceDbContext context, List<Product> products)
    {
        var random = new Random(42);
        const int saleCount = 100;

        for (int i = 0; i < saleCount; i++)
        {
            var saleDate = DateTime.UtcNow.AddDays(-random.Next(0, 730));
            var saleProducts = products
                .OrderBy(_ => random.Next())
                .Take(random.Next(1, 6))
                .ToList();
            var saleItems = new List<SaleItem>();
            decimal total = 0;

            foreach (var product in saleProducts)
            {
                var quantity = random.Next(1, 4);
                var lineTotal = product.Price * quantity;
                total += lineTotal;

                saleItems.Add(
                    new SaleItem
                    {
                        ProductId = product.ProductId,
                        Quantity = quantity,
                        UnitPrice = product.Price,
                    }
                );
            }

            var sale = new Sale
            {
                SaleDate = saleDate,
                CustomerName = $"Customer {i + 1}",
                CustomerEmail = $"customer{i + 1}@example.com",
                CustomerAddress = $"{i + 1} Main St",
                TotalAmount = total,
                SaleItems = saleItems,
            };

            context.Sales.Add(sale);
        }
    }
}
