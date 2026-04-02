using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Common.Results;
using ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;
using ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;
using ECommerce.Contracts.TerrenceLGee.Mappings.CustomerMappings;
using ECommerce.Shared.TerrenceLGee.DTOs.CustomerDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.CustomerParameters;

namespace ECommerce.Api.TerrenceLGee.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<RetrievedCustomerDto?>> GetCustomerProfileAsync(CustomerRetrievalDto customerRetrieval)
    {
        var customer = await _customerRepository.GetCustomerProfileAsync(customerRetrieval.CustomerId);

        if (customer is null)
        {
            return Result<RetrievedCustomerDto?>.Fail("Unable to retrieve customer profile", ErrorType.NotFound);
        }

        return Result<RetrievedCustomerDto?>.Ok(customer.ToRetrievedCustomerDto());
    }

    public async Task<Result<PagedList<RetrievedCustomerDto>>> GetAllCustomersForAdminAsync(CustomerQueryParams customerQueryParams)
    {
        var customers = await _customerRepository.GetAllCustomersForAdminAsync(customerQueryParams);

        return Result<PagedList<RetrievedCustomerDto>>.Ok(new PagedList<RetrievedCustomerDto>(
            customers.Select(c => c.ToRetrievedCustomerDto()),
            customers.TotalEntities,
            customerQueryParams.Page,
            customerQueryParams.PageSize));
    }
}
