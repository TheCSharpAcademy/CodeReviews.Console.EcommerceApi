using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;
using ECommerce.Shared.TerrenceLGee.DTOs.AddressDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.AddressParameters;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Address;

public interface IAddressService
{
    Task<AddressData?> AddAddressAsync(CreateAddressDto address);
    Task<AddressData?> UpdateAddressAsync(UpdateAddressDto address);
    Task<(bool, string?)> DeleteAddressAsync(int addressId);
    Task<AddressData?> GetAddressAsync(int addressId);
    Task<AddressData?> GetCustomerAddressForAdminAsync(int addressId, string? customerId);
    Task<AddressesRoot?> GetAddressesForCustomerAsync(AddressQueryParams queryParams);
    Task<AddressesRoot?> GetAllCustomerAddressesForAdminAsync(AddressQueryParams queryParams);
}
