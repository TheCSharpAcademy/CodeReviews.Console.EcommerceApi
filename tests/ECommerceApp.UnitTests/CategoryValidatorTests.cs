using ECommerceApp.RyanW84.Data.Models;
using ECommerceApp.RyanW84.Validators;
using Xunit;

namespace ECommerceApp.UnitTests.Validators;

/// <summary>
/// Unit tests for CategoryValidator class
/// </summary>
public class CategoryValidatorTests
{
    private readonly CategoryValidator _validator = new();

    [Fact]
    public void Validate_WithValidCategory_ShouldSucceed()
    {
        // Arrange
        var category = new Category
        {
            Name = "Electronics",
            Description = "Electronic products"
        };

        // Act
        var result = _validator.Validate(category);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldFail()
    {
        // Arrange
        var category = new Category
        {
            Name = "",
            Description = "Description"
        };

        // Act
        var result = _validator.Validate(category);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }
}
