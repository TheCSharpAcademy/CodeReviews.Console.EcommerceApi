using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Customer;
using ECommerce.Shared.TerrenceLGee.Parameters.CustomerParameters;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Customer;

public interface ICustomerService
{
    Task<CustomerData?> GetCustomerProfileAsync();
    Task<CustomersAdminRoot?> GetCustomersForAdminAsync(CustomerQueryParams queryParams);
}
