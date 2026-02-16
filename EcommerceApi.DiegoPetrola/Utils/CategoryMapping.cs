using EcommerceApi.Models;
using EcommerceApi.Models.DTOs;

namespace EcommerceApi.Utils;

public static class CategoryMapping
{
    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto(category.Id, category.Name);
    }

    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(product.Id, product.Name, product.Price, product.Category.Id, product.Category.Name);
    }

    public static SaleItemDto ToDto(this SaleItem saleItem)
    {
        return new SaleItemDto(saleItem.ProductId, saleItem.ProductName, saleItem.Quantity, saleItem.ProductPrice);
    }

    public static SaleDto ToDto(this Sale sale)
    {
        return new SaleDto(
            sale.Id,
            sale.SaleDate,
            [.. sale.SaleItems.Select(si => si.ToDto())],
            sale.SaleItems.Sum(si => si.Quantity * si.ProductPrice)
        );
    }

}