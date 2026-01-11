using System.Net;
using System.Net.Http.Json;
using ECommerceApp.RyanW84.Data;
using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ECommerceApp.IntegrationTests;

public class ApiIntegrationTests
{
    private static HttpClient CreateHttpsClient(EcommerceApiFactory factory) =>
        factory.CreateClient(
            new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost"),
                AllowAutoRedirect = false,
            }
        );

    private static async Task SeedAsync(
        EcommerceApiFactory factory,
        Action<ECommerceDbContext> seed
    )
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();
        await db.Database.EnsureCreatedAsync();
        seed(db);
        await db.SaveChangesAsync();
    }

    private static Task EnsureDatabaseCreatedAsync(EcommerceApiFactory factory) =>
        SeedAsync(factory, _ => { });

    [Fact]
    public async Task GetCategories_PaginatesCorrectly()
    {
        await using var factory = new EcommerceApiFactory();
        var client = CreateHttpsClient(factory);

        await SeedAsync(
            factory,
            db =>
            {
                for (var i = 1; i <= 25; i++)
                {
                    db.Categories.Add(
                        new Category { Name = $"Category {i:00}", Description = "Test category" }
                    );
                }
            }
        );

        var response = await client.GetAsync("/api/categories?page=1&pageSize=10");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var dto = await response.Content.ReadFromJsonAsync<PaginatedResponseDto<List<Category>>>();
        Assert.NotNull(dto);
        Assert.False(dto!.RequestFailed);
        Assert.NotNull(dto.Data);
        Assert.Equal(10, dto.Data!.Count);
        Assert.Equal(25, dto.TotalCount);
        Assert.Equal(1, dto.CurrentPage);
        Assert.Equal(10, dto.PageSize);
        Assert.True(dto.HasNextPage);
        Assert.False(dto.HasPreviousPage);
    }

    [Fact]
    public async Task GetProducts_Page2_ReturnsRemainingItems()
    {
        await using var factory = new EcommerceApiFactory();
        var client = CreateHttpsClient(factory);

        await SeedAsync(
            factory,
            db =>
            {
                var cat = new Category { Name = "Cat", Description = "Cat desc" };
                db.Categories.Add(cat);
                db.SaveChanges();

                for (var i = 1; i <= 40; i++)
                {
                    db.Products.Add(
                        new Product
                        {
                            Name = $"Product {i:00}",
                            Description = "Test product",
                            Price = 9.99m,
                            Stock = 10,
                            IsActive = true,
                            CategoryId = cat.CategoryId,
                        }
                    );
                }
            }
        );

        var page1 = await client.GetFromJsonAsync<PaginatedResponseDto<List<Product>>>(
            "/api/product?page=1&pageSize=32"
        );
        Assert.NotNull(page1);
        Assert.False(page1!.RequestFailed);
        Assert.NotNull(page1.Data);
        Assert.Equal(32, page1.Data!.Count);
        Assert.Equal(40, page1.TotalCount);
        Assert.True(page1.HasNextPage);

        var page2 = await client.GetFromJsonAsync<PaginatedResponseDto<List<Product>>>(
            "/api/product?page=2&pageSize=32"
        );
        Assert.NotNull(page2);
        Assert.False(page2!.RequestFailed);
        Assert.NotNull(page2.Data);
        Assert.Equal(8, page2.Data!.Count);
        Assert.Equal(40, page2.TotalCount);
        Assert.False(page2.HasNextPage);
        Assert.True(page2.HasPreviousPage);

        // Ensure page 2 starts after page 1
        Assert.Equal("Product 33", page2.Data![0].Name);
    }

    [Fact]
    public async Task CreateCategory_ThenGetById_Works()
    {
        await using var factory = new EcommerceApiFactory();
        var client = CreateHttpsClient(factory);

        await EnsureDatabaseCreatedAsync(factory);

        var createResponse = await client.PostAsJsonAsync(
            "/api/categories",
            new ApiRequestDto<Category>(
                new Category { Name = "New Category", Description = "New category description" }
            )
        );

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<ApiResponseDto<Category>>();
        Assert.NotNull(created);
        Assert.False(created!.RequestFailed);
        Assert.NotNull(created.Data);
        Assert.True(created.Data!.CategoryId > 0);

        var getResponse = await client.GetAsync($"/api/categories/{created.Data.CategoryId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var fetched = await getResponse.Content.ReadFromJsonAsync<ApiResponseDto<Category>>();
        Assert.NotNull(fetched);
        Assert.False(fetched!.RequestFailed);
        Assert.NotNull(fetched.Data);
        Assert.Equal("New Category", fetched.Data!.Name);
    }

    [Fact]
    public async Task CreateProduct_ThenGetById_Works()
    {
        await using var factory = new EcommerceApiFactory();
        var client = CreateHttpsClient(factory);

        int categoryId = 0;
        await SeedAsync(
            factory,
            db =>
            {
                var cat = new Category { Name = "Cat", Description = "Cat desc" };
                db.Categories.Add(cat);
                db.SaveChanges();
                categoryId = cat.CategoryId;
            }
        );

        var createResponse = await client.PostAsJsonAsync(
            "/api/product",
            new ApiRequestDto<Product>(
                new Product
                {
                    Name = "Created Product",
                    Description = "Created product description",
                    Price = 19.99m,
                    Stock = 5,
                    IsActive = true,
                    CategoryId = categoryId,
                }
            )
        );

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<ApiResponseDto<Product>>();
        Assert.NotNull(created);
        Assert.False(created!.RequestFailed);
        Assert.NotNull(created.Data);
        Assert.True(created.Data!.ProductId > 0);

        var getResponse = await client.GetAsync($"/api/product/{created.Data.ProductId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var fetched = await getResponse.Content.ReadFromJsonAsync<ApiResponseDto<Product>>();
        Assert.NotNull(fetched);
        Assert.False(fetched!.RequestFailed);
        Assert.NotNull(fetched.Data);
        Assert.Equal("Created Product", fetched.Data!.Name);
    }

    [Fact]
    public async Task DeleteCategory_SoftDeletes_AndIsExcludedFromList()
    {
        await using var factory = new EcommerceApiFactory();
        var client = CreateHttpsClient(factory);

        int categoryId = 0;
        await SeedAsync(
            factory,
            db =>
            {
                var cat = new Category { Name = "ToDelete", Description = "ToDelete desc" };
                db.Categories.Add(cat);
                db.SaveChanges();
                categoryId = cat.CategoryId;
            }
        );

        var deleteResponse = await client.DeleteAsync($"/api/categories/{categoryId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var listDto = await client.GetFromJsonAsync<PaginatedResponseDto<List<Category>>>(
            "/api/categories?page=1&pageSize=50"
        );
        Assert.NotNull(listDto);
        Assert.False(listDto!.RequestFailed);
        Assert.NotNull(listDto.Data);
        Assert.DoesNotContain(listDto.Data!, c => c.CategoryId == categoryId);
    }
}
