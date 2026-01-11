using ECommerceApp.RyanW84.Data.DTO;

namespace ECommerceApp.RyanW84.Interfaces.Helpers;

public interface IProductQueryHelper
{
    void NormalizePriceRange(ProductQueryParameters parameters);
}
