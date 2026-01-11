using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;
using ECommerceApp.RyanW84.Interfaces.Helpers;

namespace ECommerceApp.RyanW84.Services.Helpers;

public class SaleQueryHelper : ISaleQueryHelper
{
    public void NormalizeDateRange(SaleQueryParameters parameters)
    {
        if (
            parameters is { StartDate: not null, EndDate: not null }
            && parameters.StartDate > parameters.EndDate
        )
            (parameters.StartDate, parameters.EndDate) = (parameters.EndDate, parameters.StartDate);
    }

    public void FilterHistoricalItems(Sale sale)
    {
        DateTime saleDate = sale.SaleDate;
        sale.SaleItems = sale
            .SaleItems.Where(si => si.Product != null && (!si.Product.IsDeleted || si.Product.DeletedAt > saleDate))
            .ToList();
    }

    public void FilterHistoricalItems(IEnumerable<Sale> sales)
    {
        foreach (var sale in sales)
        {
            FilterHistoricalItems(sale);
        }
    }
}
