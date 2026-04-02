using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.Parameters.AddressParameters;

namespace ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;

public interface IAddressRepository
{
    public Task<Address?> AddAddressAsync(Address address);
    public Task<Address?> UpdateAddressAsync(Address address);
    public Task<bool> DeleteAddressAsync(int addressId, string? customerId);
    public Task<Address?> GetAddressAsync(int addressId, string? customerId);
    public Task<PagedList<Address>> GetCustomerAddressesAsync(AddressQueryParams addressQueryParams);
    public Task<PagedList<Address>> GetAllCustomerAddressesForAdminAsync(AddressQueryParams addressQueryParams);
}
