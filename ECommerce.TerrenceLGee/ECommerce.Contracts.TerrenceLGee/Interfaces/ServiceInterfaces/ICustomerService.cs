using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Common.Results;
using ECommerce.Shared.TerrenceLGee.DTOs.CustomerDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.CustomerParameters;

namespace ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;

public interface ICustomerService
{
    Task<Result<RetrievedCustomerDto?>> GetCustomerProfileAsync(CustomerRetrievalDto customerRetrieval);
    Task<Result<PagedList<RetrievedCustomerDto>>> GetAllCustomersForAdminAsync(CustomerQueryParams customerQueryParams);
}
