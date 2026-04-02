using ECommerce.Api.TerrenceLGee.Data;
using ECommerce.Api.TerrenceLGee.Repositories.Helpers;
using ECommerce.Contracts.TerrenceLGee.Common.Extensions;
using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.Parameters.AddressParameters;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.TerrenceLGee.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly ECommerceDbContext _context;
    private readonly ILogger<AddressRepository> _logger;
    private string _errorMessage = string.Empty;

    public AddressRepository(ECommerceDbContext context, ILogger<AddressRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Address?> AddAddressAsync(Address address)
    {
        try
        {
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();

            return address;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressRepository)}\n" +
                $"Method: {nameof(AddAddressAsync)}\n" +
                $"There was an unexpected error adding the address for customer with Id {address.CustomerId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }

    }

    public async Task<Address?> UpdateAddressAsync(Address address)
    {
        try
        {
            var addressToUpdate = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == address.Id && 
                !string.IsNullOrEmpty(a.CustomerId) &&
                a.CustomerId.Equals(address.CustomerId));

            if (addressToUpdate is null) return null;

            addressToUpdate.AddressLine1 = address.AddressLine1;
            addressToUpdate.AddressLine2 = address.AddressLine2;
            addressToUpdate.City = address.City;
            addressToUpdate.State = address.State;
            addressToUpdate.PostalCode = address.PostalCode;
            addressToUpdate.Country = address.Country;
            addressToUpdate.IsBillingAddress = address.IsBillingAddress;
            addressToUpdate.IsShippingAddress = address.IsShippingAddress;

            await _context.SaveChangesAsync();

            return addressToUpdate;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressRepository)}\n" +
                $"Method: {nameof(UpdateAddressAsync)}\n" +
                $"There was an unexpected error updating {address.Id} for customer {address.CustomerId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<bool> DeleteAddressAsync(int addressId, string? customerId)
    {
        try
        {
            var addressToDelete = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == addressId &&
                !string.IsNullOrEmpty(a.CustomerId) &&
                a.CustomerId.Equals(customerId));

            if (addressToDelete is null) return false;

            _context.Addresses.Remove(addressToDelete);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressRepository)}\n" +
                $"Method: {nameof(DeleteAddressAsync)}\n" +
                $"There was an unexpected error deleting address {addressId} for customer {customerId}: " +
                $"{ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return false;
        }
    }

    public async Task<Address?> GetAddressAsync(int addressId, string? customerId)
    {
        try
        {
            var address = await _context.Addresses
                .Include(a => a.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == addressId &&
                !string.IsNullOrEmpty(a.CustomerId) &&
                a.CustomerId.Equals(customerId));

            return address;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressRepository)}\n" +
                $"Method: {nameof(GetAddressAsync)}\n" +
                $"There was an unexpected error retrieving address {addressId} for customer {customerId}: " +
                $"{ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<PagedList<Address>> GetCustomerAddressesAsync(AddressQueryParams addressQueryParams)
    {
        try
        {
            var addresses = _context.Addresses
                .Where(a => a.CustomerId != null &&
                a.CustomerId.Equals(addressQueryParams.CustomerId))
                .Include(a => a.Customer)
                .AsNoTracking();

            SetFilteringAndSorting(ref addresses, addressQueryParams);

            return await addresses.ToPagedListAsync(addresses.Count(), addressQueryParams.Page, addressQueryParams.PageSize);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressRepository)}\n" +
                $"Method: {nameof(GetAddressAsync)}\n" +
                $"There was an unexpected error retrieving the addresses for customer {addressQueryParams.CustomerId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }

    }

    public async Task<PagedList<Address>> GetAllCustomerAddressesForAdminAsync(AddressQueryParams addressQueryParams)
    {
        try
        {
            var addresses = _context.Addresses
                .Include(a => a.Customer)
                .AsNoTracking();

            SetFilteringAndSorting(ref addresses, addressQueryParams);

            return await addresses.ToPagedListAsync(addresses.Count(), addressQueryParams.Page, addressQueryParams.PageSize);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressRepository)}\n" +
                $"Method: {nameof(GetAllCustomerAddressesForAdminAsync)}\n" +
                $"There was an unexpected error retrieving all customer addresses: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
    }

    private static void SetFilteringAndSorting(ref IQueryable<Address> addresses, AddressQueryParams addressQueryParams)
    {
        if (!string.IsNullOrEmpty(addressQueryParams.CustomerId))
        {
            addresses = addresses
                .Where(a => a.Customer != null && !string.IsNullOrEmpty(a.CustomerId) && 
                a.CustomerId.Equals(addressQueryParams.CustomerId));
        }

        if (!string.IsNullOrEmpty(addressQueryParams.CustomerFirstName))
        {
            addresses = addresses
                .Where(a => a.Customer != null &&
                a.Customer.FirstName.ToLower()
                .Equals(addressQueryParams.CustomerFirstName.ToLower()));
        }

        if (!string.IsNullOrEmpty(addressQueryParams.CustomerLastName))
        {
            addresses = addresses
                .Where(a => a.Customer != null &&
                a.Customer.LastName.ToLower()
                .Equals(addressQueryParams.CustomerLastName.ToLower()));
        }

        if (!string.IsNullOrEmpty(addressQueryParams.City))
        {
            addresses = addresses
                .Where(a => a.City.ToLower().Equals(addressQueryParams.City.ToLower()));
        }

        if (!string.IsNullOrEmpty(addressQueryParams.State))
        {
            addresses = addresses
                .Where(a => a.State.ToLower().Equals(addressQueryParams.State.ToLower()));
        }

        if (!string.IsNullOrEmpty(addressQueryParams.Country))
        {
            addresses = addresses
                .Where(a => a.Country.ToLower().Equals(addressQueryParams.Country.ToLower()));
        }

        addresses = SortHelper<Address>.ApplySorting(addresses, addressQueryParams.OrderBy);
    }
}
