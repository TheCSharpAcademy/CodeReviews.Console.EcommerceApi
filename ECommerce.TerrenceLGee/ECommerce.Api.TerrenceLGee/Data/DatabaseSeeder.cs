using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.TerrenceLGee.Data;

public static class DatabaseSeeder
{
    private static readonly string Admin = "admin";
    private static readonly string Customer = "customer";
    private static readonly string Password = "Pa$$w0rd";

    public static async Task SeedUsersAsync(
        ECommerceDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        if (await dbContext.Users.AnyAsync()) return;

        var adminRole = new IdentityRole { Name = Admin };
        var customerRole = new IdentityRole { Name = Customer };

        await roleManager.CreateAsync(adminRole);
        await roleManager.CreateAsync(customerRole);

        var admin = new ApplicationUser
        {
            FirstName = "Gordon",
            LastName = "Ramsay",
            DateOfBirth = DateOnly.Parse("11-08-1966"),
            RegistrationDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            Email = "admin@example.com",
            UserName = "admin@example.com"
        };

        await userManager.CreateAsync(admin, Password);
        await userManager.AddToRoleAsync(admin, Admin);

        var customer1 = new ApplicationUser
        {
            FirstName = "Tom",
            LastName = "Jones",
            DateOfBirth = DateOnly.Parse("03-10-1993"),
            RegistrationDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            Email = "tjones@example.com",
            UserName = "tjones@example.com",
            Addresses = new[]
            {
                new Address()
                {
                    AddressLine1 = "123 Main St.",
                    AddressLine2 = null,
                    City = "New York City",
                    State = "New York",
                    PostalCode = "123456",
                    Country = "USA",
                    IsBillingAddress = true,
                    IsShippingAddress = false
                },
                new Address()
                {
                    AddressLine1 = "999 State St.",
                    AddressLine2 = null,
                    City = "New York City",
                    State = "New York",
                    PostalCode = "654321",
                    Country = "USA",
                    IsBillingAddress = false,
                    IsShippingAddress = true
                }
            },
        };

        await userManager.CreateAsync(customer1, Password);
        await userManager.AddToRoleAsync(customer1, Customer);

        await SeedSalesAsync(dbContext, customer1.Id);
    }

    public static async Task SeedCategoriesAsync(ECommerceDbContext dbContext)
    {
        if (await dbContext.Categories.AnyAsync()) return;

        var categories = new[]
        {
            new Category
            {
                Name = "Clothing",
                Description = "A wide assortment of men's, women's and children's clothing in a variety of sizes.",
                CreatedAt = DateTime.UtcNow,
            },
            new Category
            {
                Name = "Electronics",
                Description = "A wide assortment of electronics ranging from stereo systems to computers and everything in between.",
                CreatedAt = DateTime.UtcNow,
            },
            new Category
            {
                Name = "Music",
                Description = "Almost any musical genre that you can image with a wide range of music artists available on both vinyl and CD.",
                CreatedAt = DateTime.UtcNow,
            },
            new Category
            {
                Name = "Books",
                Description = "Books on almost every subject from A-Z.",
                CreatedAt = DateTime.UtcNow,
            },
            new Category
            {
                Name = "Movies",
                Description = "Almost any movie or television show that you can think of it is available on DVD, we have it.",
                CreatedAt = DateTime.UtcNow,
            },
            new Category
            {
                Name = "Fitness",
                Description = "Everything from cardiovascular machines to heavy weight lifting equipment.",
                CreatedAt = DateTime.UtcNow,
            }
        };

        await dbContext.Categories.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedProductsAsync(ECommerceDbContext dbContext)
    {
        if (await dbContext.Products.AnyAsync()) return;

        var categories = dbContext.Categories;

        if (!await categories.AnyAsync()) return;

        var clothingCategory = categories.FirstOrDefault(c => c.Name.Equals("Clothing"));
        var electronicsCategory = categories.FirstOrDefault(c => c.Name.Equals("Electronics"));
        var musicCategory = categories.FirstOrDefault(c => c.Name.Equals("Music"));
        var booksCategory = categories.FirstOrDefault(c => c.Name.Equals("Books"));
        var movieCategory = categories.FirstOrDefault(c => c.Name.Equals("Movies"));
        var fitnessCategory = categories.FirstOrDefault(c => c.Name.Equals("Fitness"));

        if (clothingCategory is null
            || electronicsCategory is null
            || musicCategory is null
            || booksCategory is null
            || movieCategory is null
            || fitnessCategory is null) return;

        var products = new[]
        {
            new Product
            {
                CategoryId = clothingCategory.Id,
                Name = "Women's night gown",
                Description = "A very comfortable women's night gown available in all sizes and a variety of colors. Perfect for a good night's sleep or just relaxing around the house",
                StockQuantity = 30,
                UnitPrice = 15.99m,
                DiscountPercentage = 0,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/A1eIf+Hj4YL._AC_SX679_.jpg"
            },
            new Product
            {
                CategoryId = clothingCategory.Id,
                Name = "Unisex jogging suit",
                Description = "2 piece casual unisex jogging suit, comfortable and available in S, M, L, XL and XXL. Comes in a wide choice of colors.",
                StockQuantity = 45,
                UnitPrice = 27.99m,
                DiscountPercentage = 10,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/71ZfDaCP4nL._AC_SX679_.jpg"
            },
            new Product
            {
                CategoryId = clothingCategory.Id,
                Name = "Children's pajams",
                Description = "Made for children male or female ages 3 to 11. Comes in a variety of sizes and colors.",
                StockQuantity = 120,
                UnitPrice = 12.99m,
                DiscountPercentage = 0,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/71oGda6DyEL._AC_SX569_.jpg"
            },
            new Product
            {
                CategoryId = clothingCategory.Id,
                Name = "Men's dress shirt",
                Description = "Perfect for everything from religious services to job interviews to date night at a nice resturant. Comes in White, Navy Blue, and Grey. Sizes: S, M, L, XL, XXL.",
                StockQuantity = 20,
                UnitPrice = 45.99m,
                DiscountPercentage = 15,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/51rkKPruYvL._AC_SX679_.jpg"
            },
            new Product
            {
                CategoryId = electronicsCategory.Id,
                Name = "Apple iMac",
                Description = "Apple iMac All-in-One Desktop Computer with M4 chip with 8-core CPU and 8-core GPU: Built for Apple Intelligence, 24-inch Retina Display, 16GB Unified Memory, 256GB SSD Storage.",
                StockQuantity = 10,
                UnitPrice = 1031.38m,
                DiscountPercentage = 0,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/71gqlRrQCuL.jpg"
            },
            new Product
            {
                CategoryId = electronicsCategory.Id,
                Name = "HP All-In-One Desktop PC",
                Description = "HP 24 inch All-in-One Desktop PC, FHD Display, AMD Ryzen 7 7730U, 16 GB RAM, 512 GB SSD, AMD Radeon Graphics, Windows 11 Home, 24-cr0032.",
                StockQuantity = 20,
                UnitPrice = 779.20m,
                DiscountPercentage = 5,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/81OlLtbjieL.jpg"
            },
            new Product
            {
                CategoryId = electronicsCategory.Id,
                Name = "PHILIPS FX10 Bluetooth Stereo System",
                Description = "PHILIPS FX10 Bluetooth Stereo System for Home with CD Player , MP3, USB, FM Radio, Bass Reflex Speaker, 230 W, Remote Control Included.",
                StockQuantity = 25,
                UnitPrice = 249.99m,
                DiscountPercentage = 12,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/815e4H+YBaL.jpg"
            },
            new Product
            {
                CategoryId = electronicsCategory.Id,
                Name = "MECHEN M30 HiFi MP3 Player",
                Description = "Lossless DSD High Resolution Digital Audio Music Player, High-Res Portable Audio Player with 64GB Memory Card.",
                StockQuantity = 30,
                UnitPrice = 79.98m,
                DiscountPercentage = 0,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/71xJEJVlVwL.jpg"
            },
            new Product
            {
                CategoryId = musicCategory.Id,
                Name = "The Temptations With A Lot O' Soul",
                Description = "Considered one of The Temptations best and most beloved albums.",
                StockQuantity = 60,
                UnitPrice = 21.99m,
                DiscountPercentage = 5,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/61-AAZI6B1L._SY300_SX300_QL70_FMwebp_.jpg"
            },
            new Product
            {
                CategoryId = musicCategory.Id,
                Name = "Sgt. Pepper's Lonely Hearts Club Band Deluxe",
                Description = "Deluxe edition of one of the most beloved albums of all time. The Beatles Sgt. Pepper's Lonely Hearts Club Band.",
                StockQuantity = 50,
                UnitPrice = 39.99m,
                DiscountPercentage = 0,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/91wYFN2CBRL._SX425_.jpg"
            },
            new Product
            {
                CategoryId = musicCategory.Id,
                Name = "60 Greatest Hits of Sam Cooke",
                Description = "Enjoy some of the greatest hits of Sam Cooke includes all of your favorites.",
                StockQuantity = 40,
                UnitPrice = 39.99m,
                DiscountPercentage = 0,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/71Q3UNQRYHL._SX425_.jpg"
            },
            new Product
            {
                CategoryId = musicCategory.Id,
                Name = "Black Sabbath Self-Title Debut",
                Description = "Go back to where it started for the godfathers of heavy metal.",
                StockQuantity = 30,
                UnitPrice = 15.99m,
                DiscountPercentage = 5,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/81RqWZVeexL._SX425_.jpg"
            },
            new Product
            {
                CategoryId = booksCategory.Id,
                Name = "YOU TOO CAN BE PROSPEROUS",
                Description = "Learn the spiritual secrets of an abundant, successful and prosperous life. By Robert A. Russel.",
                StockQuantity = 15,
                UnitPrice = 9.99m,
                DiscountPercentage = 0,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/51nO--WOBkL._SY445_SX342_FMwebp_.jpg"
            },
            new Product
            {
                CategoryId = booksCategory.Id,
                Name = "The C# Player's Guide (5th Edition)",
                Description = "One of the best books for beginners looking to begin their journey into C# Programming. By RB Whitaker.",
                StockQuantity = 45,
                UnitPrice = 34.99m,
                DiscountPercentage = 15,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/619vzxml9jL._AC_UF1000,1000_QL80_.jpg"
            },
            new Product
            {
                CategoryId = booksCategory.Id,
                Name = "C# Data Structures and Algorithms 2nd Edition",
                Description = "When you are ready to take your C# development and programming know how to the next level. By Marcin Jamro.",
                StockQuantity = 30,
                UnitPrice = 26.37m,
                DiscountPercentage = 0,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/610Svp+mOkL._AC_UF1000,1000_QL80_.jpg"
            },
            new Product
            {
                CategoryId = booksCategory.Id,
                Name = "Clean Code with C# 2nd Edition",
                Description = "Enhance your programming skills through code reviews, TDD and BDD implementation, and API design to overcome code inefficiency, redundancy, and other issues arising from bad code Key Features. By Jason Alls.",
                StockQuantity = 15,
                UnitPrice = 45.99m,
                DiscountPercentage = 0,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/418LKBgGA2L._SX342_SY445_FMwebp_.jpg"
            },
            new Product
            {
                CategoryId = movieCategory.Id,
                Name = "Rope",
                Description = "Classic Alfred Hitchcock movie from 1948 in color. Starring James Stewart, Farley Granger and John Dall.",
                StockQuantity = 30,
                UnitPrice = 6.99m,
                DiscountPercentage = 0,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/513VY7Vkt9L._SY300_SX300_QL70_FMwebp_.jpg"
            },
            new Product
            {
                CategoryId = movieCategory.Id,
                Name = "Blue Steel",
                Description = "Action packed thriller that will have you on the edge of your seat. Starring Jamie Lee Curtis and Ron Silver.",
                StockQuantity = 50,
                UnitPrice = 13.29m,
                DiscountPercentage = 0,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/71gwSJPuX6L._SY445_.jpg"
            },
            new Product
            {
                CategoryId = movieCategory.Id,
                Name = "Saw: 10-Film Collection",
                Description = "Relive all of the terror of Jigsaw in this 10 film collection. Starring Tobin Bell, Shawnee Smith and others",
                StockQuantity = 50,
                UnitPrice = 39.96m,
                DiscountPercentage = 10,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/51IQSyPXYBL._SY300_SX300_QL70_FMwebp_.jpg"
            },
            new Product
            {
                CategoryId = movieCategory.Id,
                Name = "4 Film Favorites: Denzel Washington",
                Description = "Enjoy 4 action packed films starring Denzel Washington in this budget priced collection.",
                StockQuantity = 50,
                UnitPrice = 14.97m,
                DiscountPercentage = 10,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/8164b7a5DvL._SX342_.jpg"
            },
            new Product
            {
                CategoryId = fitnessCategory.Id,
                Name = "Adjustable Dumbbells Set of 2",
                Description = "Free Weights Dumbbells Set，Adjustable Dumbbell Set，52.5 lbs pair 105 lbs，15 in 1，for Men/Women Gym Equipment for Home Strength Training Equipment.",
                StockQuantity = 30,
                UnitPrice = 249.99m,
                DiscountPercentage = 0,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/71V3y5K2QRL._AC_SY300_SX300_QL70_FMwebp_.jpg"
            },
            new Product
            {
                CategoryId = fitnessCategory.Id,
                Name = "Body-Solid Multi-Station",
                Description = "Single Weight Stack Home Gym Machine, Arm & Leg Strength Training Functional Exercise Workout Station, 210lbs. Black Weight Stacks.",
                StockQuantity = 50,
                UnitPrice = 1695.00m,
                DiscountPercentage = 30,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/61NGD9LBiVL._AC_SL1028_.jpg"
            },
            new Product
            {
                CategoryId = fitnessCategory.Id,
                Name = "Niceday Elliptical Machine",
                Description = "Elliptical Exercise Machine for Home with Hyper-Quiet Magnetic Driving System, Elliptical Trainer with 15.5IN & 20IN Stride, 16 Resistance Levels, 500LBS Loading Capacity.",
                StockQuantity = 10,
                UnitPrice = 369.99m,
                DiscountPercentage = 20,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/71out2dpOAL._AC_UF1000,1000_QL80_.jpg"
            },
            new Product
            {
                CategoryId = fitnessCategory.Id,
                Name = "MERACH Recumbent Exercise Bike",
                Description = "For Home useage with Smart Bluetooth Equipment Exercise Bikes App,LCD,Heart Rate Handle Stationary Bikes for Home, Magnetic Recumbent Exercise Bike for Seniors Gym S08/S23.",
                StockQuantity = 50,
                UnitPrice = 170.88m,
                DiscountPercentage = 15,
                IsDeleted = false,
                IsInStock = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "https://m.media-amazon.com/images/I/71bd5H+u3PL._AC_SY300_SX300_QL70_FMwebp_.jpg"
            },
        };

        await dbContext.Products.AddRangeAsync(products);
        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedSalesAsync(ECommerceDbContext dbContext, string? customerId)
    {
        if (string.IsNullOrEmpty(customerId)) return;

        if (await dbContext.Sales.AnyAsync()) return;

        var products = await dbContext.Products.ToListAsync();

        var sales = new[]
        {
            new Sale()
            {
                CustomerId = customerId,
                TotalBaseAmount = 403.26m,
                TotalDiscountAmount = 73.99m,
                TotalAmount = 329.27m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                SaleStatus = SaleStatus.Processing,
                SaleProducts = new List<SaleProduct>
                {
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("Niceday Elliptical Machine"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 369.99m,
                        Discount = 73.99m,
                        TotalPrice = 296.00m
                    },
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("YOU TOO CAN BE PROSPEROUS"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 2,
                        Price = 9.99m,
                        Discount = 0m,
                        TotalPrice = 19.98m
                    },
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("Blue Steel"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 13.29m,
                        Discount = 0m,
                        TotalPrice = 13.29m
                    }
                }
            },
            new Sale()
            {
                CustomerId = customerId,
                TotalBaseAmount = 1992.93m,
                TotalDiscountAmount = 529.20m,
                TotalAmount = 1643.73m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                SaleStatus = SaleStatus.Shipped,
                SaleProducts = new List<SaleProduct>
                {
                    new SaleProduct
                    {
                        ProductId = products.Where(p => p.Name.Equals("Body-Solid Multi-Station"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 1695.00m,
                        Discount = 508.50m,
                        TotalPrice = 1186.50m
                    },
                    new SaleProduct
                    {
                        ProductId = products.Where(p => p.Name.Equals("MECHEN M30 HiFi MP3 Player"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 2,
                        Price = 79.98m,
                        Discount = 0m,
                        TotalPrice = 159.96m
                    },
                    new SaleProduct
                    {
                        ProductId = products.Where(p => p.Name.Equals("Men's dress shirt"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 3,
                        Price = 45.99m,
                        Discount = 6.90m,
                        TotalPrice = 117.27m
                    }
                }
            },
            new Sale()
            {
                CustomerId = customerId,
                SaleStatus = SaleStatus.Delivered,
                TotalBaseAmount = 2314.98m,
                TotalDiscountAmount = 582.50m,
                TotalAmount = 1732.48m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                SaleProducts = new List<SaleProduct>
                {
                    new SaleProduct
                    {
                        ProductId = products.Where(p => p.Name.Equals("Adjustable Dumbbells Set of 2"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 249.99m,
                        Discount = 0m,
                        TotalPrice = 249.99m
                    },
                    new SaleProduct
                    {
                        ProductId = products.Where(p => p.Name.Equals("Niceday Elliptical Machine"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 369.99m,
                        Discount = 74.00m,
                        TotalPrice = 295.99m
                    },
                    new SaleProduct
                    {
                        ProductId = products.Where(p => p.Name.Equals("Body-Solid Multi-Station"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 1695.00m,
                        Discount = 508.50m,
                        TotalPrice = 1186.50m,
                    }
                }
            },
            new Sale()
            {
                CustomerId = customerId,
                SaleStatus = SaleStatus.Pending,
                TotalBaseAmount = 85.97m,
                TotalDiscountAmount = 11.30m,
                TotalAmount = 74.67m,
                CreatedAt = DateTime.UtcNow,
                SaleProducts = new List<SaleProduct>
                {
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("Black Sabbath Self-Title Debut"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 15.99m,
                        Discount = 0.80m,
                        TotalPrice = 15.19m
                    },
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("The C# Player's Guide (5th Edition)"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 2,
                        Price = 34.99m,
                        Discount = 5.25m,
                        TotalPrice = 59.48m
                    }
                }
            },
            new Sale()
            {
                CustomerId = customerId,
                SaleStatus = SaleStatus.Delivered,
                TotalBaseAmount = 845.56m,
                TotalDiscountAmount = 38.96m,
                TotalAmount = 806.60m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                SaleProducts = new List<SaleProduct>
                {
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("C# Data Structures and Algorithms 2nd Edition"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 26.37m,
                        Discount = 0.0m,
                        TotalPrice = 26.37m
                    },
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("HP All-In-One Desktop PC"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 779.20m,
                        Discount = 38.96m,
                        TotalPrice = 740.24m
                    },
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("60 Greatest Hits of Sam Cooke"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 39.99m,
                        Discount = 0.0m,
                        TotalPrice = 39.99m
                    }
                }
            },
            new Sale()
            {
                CustomerId = customerId,
                SaleStatus = SaleStatus.Canceled,
                TotalBaseAmount = 95.94m,
                TotalDiscountAmount = 0.0m,
                TotalAmount = 95.94m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                SaleProducts = new List<SaleProduct>
                {
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("Women's night gown"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 6,
                        Price = 15.99m,
                        Discount = 0.0m,
                        TotalPrice = 95.94m
                    },
                }
            },
            new Sale()
            {
                CustomerId = customerId,
                SaleStatus = SaleStatus.Shipped,
                TotalBaseAmount = 2472.68m,
                TotalDiscountAmount = 257.81m,
                TotalAmount = 2214.87m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                SaleProducts = new List<SaleProduct>
                {
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("Unisex jogging suit"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 3,
                        Price = 27.99m,
                        Discount = 2.80m,
                        TotalPrice = 75.57m
                    },
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("Apple iMac"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 2,
                        Price = 1031.38m,
                        Discount = 0.0m,
                        TotalPrice = 2062.76m
                    },
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("PHILIPS FX10 Bluetooth Stereo System"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 249.99m,
                        Discount = 30.00m,
                        TotalPrice = 219.99m
                    },
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("The Temptations With A Lot O' Soul"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 21.99m,
                        Discount = 1.10m,
                        TotalPrice = 20.89m,
                    },
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("Sgt. Pepper's Lonely Hearts Club Band Deluxe"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 39.99m,
                        Discount = 0.0m,
                        TotalPrice = 39.99m
                    },
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("Rope"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 2,
                        Price = 6.99m,
                        Discount = 0.0m,
                        TotalPrice = 13.98m
                    }
                }
            },
            new Sale()
            {
                CustomerId = customerId,
                SaleStatus = SaleStatus.Pending,
                TotalBaseAmount = 256.83m,
                TotalDiscountAmount = 36.53m,
                TotalAmount = 220.30m,
                CreatedAt = DateTime.UtcNow,
                SaleProducts = new List<SaleProduct>
                {
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("Saw: 10-Film Collection"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 39.96m,
                        Discount = 4.00m,
                        TotalPrice = 35.96m
                    },
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("MERACH Recumbent Exercise Bike"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 170.88m,
                        Discount = 25.63m,
                        TotalPrice = 145.25m
                    },
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("Men's dress shirt"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 45.99m,
                        Discount = 6.90m,
                        TotalPrice = 39.09m
                    }
                }
            },
            new Sale()
            {
                CustomerId = customerId,
                SaleStatus = SaleStatus.Canceled,
                TotalBaseAmount = 514.95m,
                TotalDiscountAmount = 31.50m,
                TotalAmount = 483.45m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                SaleProducts = new List<SaleProduct>
                {
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("PHILIPS FX10 Bluetooth Stereo System"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 249.99m,
                        Discount = 30.00m,
                        TotalPrice = 219.99m,
                    },
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("4 Film Favorites: Denzel Washington"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 14.97m,
                        Discount = 1.50m,
                        TotalPrice = 13.47m
                    },
                    new SaleProduct()
                    {
                        ProductId = products.Where(p => p.Name.Equals("Adjustable Dumbbells Set of 2"))
                        .Select(p => p.Id)
                        .First(),
                        Quantity = 1,
                        Price = 249.99m,
                        Discount = 0.0m,
                        TotalPrice = 249.99m
                    }
                }
            }
        };

        await dbContext.Sales.AddRangeAsync(sales);
        await dbContext.SaveChangesAsync();
    } 
}
