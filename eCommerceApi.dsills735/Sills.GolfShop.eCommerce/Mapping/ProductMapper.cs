namespace Sills.GolfShop.eCommerceAPI.Mapping;

public static class ProductMapper
{
    public static DTO.ProductUpdateDto? ToDTO(this Models.Product product)
    {
        if (product == null) return null;

        return new DTO.ProductUpdateDto(
            product.Name,
            product.Description,
            product.QuantityInStock
        );
    }
}
