using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Common.Results;
using ECommerce.Shared.TerrenceLGee.DTOs.AddressDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.AddressParameters;

namespace ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;

public interface IAddressService
{
    Task<Result<RetrievedAddressDto?>> AddAddressAsync(CreateAddressDto address);
    Task<Result<RetrievedAddressDto?>> UpdateAddressAsync(UpdateAddressDto address);
    Task<Result> DeleteAddressAsync(AddressIdDto addressIdDto);
    Task<Result<RetrievedAddressDto?>> GetAddressAsync(AddressIdDto addressIdDto);
    Task<Result<PagedList<RetrievedAddressDto>>> GetCustomerAddressesAsync(AddressQueryParams addressQueryParams);
    Task<Result<PagedList<RetrievedAddressDto>>> GetAllCustomerAddressesForAdminAsync(AddressQueryParams addressQueryParams);
}
