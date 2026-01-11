using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Interfaces.Helpers;

namespace ECommerceApp.RyanW84.Services.Helpers;

public class ProductQueryHelper : IProductQueryHelper
{
    public void NormalizePriceRange(ProductQueryParameters parameters)
    {
        if (parameters.MinPrice.HasValue && parameters.MaxPrice.HasValue && parameters.MinPrice > parameters.MaxPrice)
        {
            (parameters.MinPrice, parameters.MaxPrice) = (parameters.MaxPrice, parameters.MinPrice);
        }
    }
}
