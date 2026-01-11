using ECommerceApp.RyanW84.Data.Models;
using ECommerceApp.RyanW84.Validators;
using Xunit;

namespace ECommerceApp.UnitTests.Validators;

/// <summary>
/// Unit tests for ProductValidator class
/// </summary>
public class ProductValidatorTests
{
    private readonly ProductValidator _validator = new();

    [Fact]
    public void Validate_WithValidProduct_ShouldSucceed()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test Product",
            Description = "A valid product description",
            Price = 99.99m,
            Stock = 10,
            IsActive = true,
            CategoryId = 1
        };

        // Act
        var result = _validator.Validate(product);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldFail()
    {
        // Arrange
        var product = new Product
        {
            Name = "",
            Description = "Description",
            Price = 10m,
            Stock = 5,
            IsActive = true,
            CategoryId = 1
        };

        // Act
        var result = _validator.Validate(product);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_WithNegativePrice_ShouldFail()
    {
        // Arrange
        var product = new Product
        {
            Name = "Product",
            Description = "Description",
            Price = -10m,
            Stock = 5,
            IsActive = true,
            CategoryId = 1
        };

        // Act
        var result = _validator.Validate(product);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Price");
    }
}
