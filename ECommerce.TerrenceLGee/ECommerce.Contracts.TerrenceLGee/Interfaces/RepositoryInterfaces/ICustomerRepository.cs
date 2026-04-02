using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.Parameters.CustomerParameters;

namespace ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;

public interface ICustomerRepository
{
    Task<ApplicationUser?> GetCustomerProfileAsync(string? customerId);
    Task<PagedList<ApplicationUser>> GetAllCustomersForAdminAsync(CustomerQueryParams customerQueryParams);
}
