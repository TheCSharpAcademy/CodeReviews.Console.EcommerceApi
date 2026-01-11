using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;

namespace ECommerceApp.RyanW84.Interfaces.Helpers;

public interface ISaleQueryHelper
{
    void NormalizeDateRange(SaleQueryParameters parameters);
    void FilterHistoricalItems(Sale sale);
    void FilterHistoricalItems(IEnumerable<Sale> sales);
}
