namespace EcommerceApi.Models.DTOs;

public record CategoryDto(int Id, string Name);
public record CreateCategoryDto(string Name);
public record ProductDto(int Id, string Name, decimal Price, int CategoryId, string CategoryName);
public record CreateProductDto(string Name, decimal Price, int CategoryId);
public record SaleItemDto(int? ProductId, string ProductName, int Quantity, decimal UnitPrice);
public record SaleDto(int Id, DateTime SaleDate, List<SaleItemDto> SaleItems, decimal TotalAmount);
public record CreateSaleDto(List<CreateSaleItemDto> Items);
public record CreateSaleItemDto(int ProductId, int Quantity);
