using ECommerce.AvaloniaClient.TerrenceLGee.Data;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Address;
using ECommerce.Shared.TerrenceLGee.DTOs.AddressDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.AddressParameters;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services;

public class AddressService : IAddressService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<AddressService> _logger;
    private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
    private string _errorMessage = string.Empty;
    private const string ClientName = "client";
    private const string MediaType = "application/json";
    private const string LogErrorString = "{msg}\n\n";

    public AddressService(IHttpClientFactory clientFactory, ILogger<AddressService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<AddressData?> AddAddressAsync(CreateAddressDto address)
    {
        var addressDataForError = new AddressData();

        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AddAddressUrl}";

            var content = new StringContent(JsonSerializer.Serialize(address), Encoding.UTF8, MediaType);

            var response = await httpClient.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var addressAddedResponse = JsonSerializer.Deserialize<AddressRoot>(responseContent, options);

            if (addressAddedResponse is null)
            {
                addressDataForError.ErrorMessage = $"Unable to add address at this time, please try again later";
                return addressDataForError;
            }

            if (!addressAddedResponse.IsSuccess || addressAddedResponse.StatusCode != 201)
            {
                addressDataForError.ErrorMessage = $"Unable to add address: {string.Join('\n', addressAddedResponse.Errors)}";
                return addressDataForError;
            }

            return addressAddedResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressService)}\n" +
                $"Method: {nameof(AddAddressAsync)}\n" +
                $"There was an API error adding the address: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            addressDataForError.ErrorMessage = $"There was an API error adding the address";
            return addressDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressService)}\n" +
                $"Method: {nameof(AddAddressAsync)}\n" +
                $"There was an unexpected error adding the address: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            addressDataForError.ErrorMessage = $"There was an unexpected error adding the address";
            return addressDataForError;
        }
    }

    public async Task<AddressData?> UpdateAddressAsync(UpdateAddressDto address)
    {
        var addressDataForError = new AddressData();

        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.UpdateAddressUrl}{address.Id}";

            var content = new StringContent(JsonSerializer.Serialize(address), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var addressUpdatedResponse = JsonSerializer.Deserialize<AddressRoot>(responseContent, options);

            if (addressUpdatedResponse is null)
            {
                addressDataForError.ErrorMessage = $"Unable to updated address {address.Id} at this time, please try again later";
                return addressDataForError;
            } 

            if (!addressUpdatedResponse.IsSuccess || addressUpdatedResponse.StatusCode != 200)
            {
                addressDataForError.ErrorMessage = $"Unable to update address {address.Id}: {string.Join('\n', addressUpdatedResponse.Errors)}";
                return addressDataForError;
            }

            return addressUpdatedResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressService)}\n" +
                $"Method: {nameof(UpdateAddressAsync)}\n" +
                $"There was an API error updating address {address.Id}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            addressDataForError.ErrorMessage = $"There was an API error updating address {address.Id}";
            return addressDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressService)}\n" +
                $"Method: {nameof(UpdateAddressAsync)}\n" +
                $"There was an unexpected error updating address {address.Id}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            addressDataForError.ErrorMessage = $"There was an unexpected error updating address {address.Id}";
            return addressDataForError;
        }
    }

    public async Task<(bool, string?)> DeleteAddressAsync(int addressId)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.DeleteAddressUrl}{addressId}";

            var response = await httpClient.DeleteAsync(url);

            var responseContent = await response.Content.ReadAsStringAsync();
            var addressDeletedResponse = JsonSerializer.Deserialize<AddressDeletionRoot>(responseContent, options);

            if (addressDeletedResponse is null)
            {
                return (false, $"Unable to delete address {addressId} at this time, please try again later");
            }

            if (!addressDeletedResponse.IsSuccess || addressDeletedResponse.StatusCode != 200)
            {
                return (false, $"Unable to delete address {addressId}: {string.Join('\n', addressDeletedResponse.Errors)}");
            }

            return (true, addressDeletedResponse.Data);
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressService)}\n" +
                $"Method: {nameof(DeleteAddressAsync)}\n" +
                $"There was an API error deleting address {addressId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (false, $"There was an API error deleting address {addressId}");
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressService)}\n" +
                $"Method: {nameof(DeleteAddressAsync)}\n" +
                $"There was an unexpected error deleting address {addressId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (false, $"There was an unexpected error deleting address {addressId}");
        }
    }

    public async Task<AddressData?> GetAddressAsync(int addressId)
    {
        var addressDataForError = new AddressData();

        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.GetAddressForCustomerUrl}{addressId}";

            var response = await httpClient.GetAsync(url);

            var responseContent = await response.Content.ReadAsStringAsync();
            var addressResponse = JsonSerializer.Deserialize<AddressRoot>(responseContent, options);

            if (addressResponse is null)
            {
                addressDataForError.ErrorMessage = $"Unable to retrieve address {addressId} at this time, please try again later";
                return addressDataForError;
            }

            if (!addressResponse.IsSuccess || addressResponse.StatusCode != 200)
            {
                addressDataForError.ErrorMessage = $"Unable to retrieve address {addressId}: {string.Join('\n', addressResponse.Errors)}";
                return addressDataForError;
            }

            return addressResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressService)}\n" +
                $"Method: {nameof(GetAddressAsync)}\n" +
                $"There was an API error retrieving address {addressId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            addressDataForError.ErrorMessage = $"There was an API error retrieving address {addressId}";
            return addressDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressService)}\n" +
                $"Method: {nameof(GetAddressAsync)}\n" +
                $"There was an unexpected error retrieving address {addressId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            addressDataForError.ErrorMessage = $"There was an unexpected error retrieving address {addressId}";
            return addressDataForError;
        }
    }

    public async Task<AddressData?> GetCustomerAddressForAdminAsync(int addressId, string? customerId)
    {
        var addressDataForError = new AddressData();

        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.GetCustomerAddressForAdminUrl}{addressId}";

            var addressIdDto = new AddressIdDto { CustomerId = customerId };

            var content = new StringContent(JsonSerializer.Serialize(addressIdDto), Encoding.UTF8, MediaType);

            var response = await httpClient.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var addressResponse = JsonSerializer.Deserialize<AddressRoot>(responseContent, options);

            if (addressResponse is null)
            {
                addressDataForError.ErrorMessage = $"Unable to retrieve address {addressId} at this time, plesae try again later";
                return addressDataForError;
            }

            if (!addressResponse.IsSuccess || addressResponse.StatusCode != 200)
            {
                addressDataForError.ErrorMessage = $"Unable to retrieve address {addressId}: {string.Join('\n', addressResponse.Errors)}";
                return addressDataForError;
            }

            return addressResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressService)}\n" +
                $"Method: {nameof(GetCustomerAddressForAdminAsync)}\n" +
                $"There was an API error retrieving address {addressId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            addressDataForError.ErrorMessage = $"There was an API error retrieving address {addressId}";
            return addressDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressService)}\n" +
                $"Method: {nameof(GetCustomerAddressForAdminAsync)}\n" +
                $"There was an unexpected error retrieving address {addressId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            addressDataForError.ErrorMessage = $"There was an unexpected error retrieving address {addressId}";
            return addressDataForError;
        }
    }

    public async Task<AddressesRoot?> GetAddressesForCustomerAsync(AddressQueryParams queryParams)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.GetAllAddressesForCustomerUrl}{BuildQueryString(queryParams)}";

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            var addressesResponse = JsonSerializer.Deserialize<AddressesRoot>(responseContent, options);

            if (addressesResponse is null) return null;

            return addressesResponse;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressService)}\n" +
                $"Method: {nameof(GetAddressesForCustomerAsync)}\n" +
                $"There was an API error attempting to retrieve al of the addresses: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressService)}\n" +
                $"Method: {nameof(GetAddressesForCustomerAsync)}\n" +
                $"There was an unexpected error attempting to retrieve all of the addresses: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
    }

    public async Task<AddressesRoot?> GetAllCustomerAddressesForAdminAsync(AddressQueryParams queryParams)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.GetAllAddressesForAdminUrl}{BuildQueryString(queryParams)}";

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            var addressesResponse = JsonSerializer.Deserialize<AddressesRoot>(responseContent, options);

            if (addressesResponse is null) return null;

            return addressesResponse;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressService)}\n" +
                $"Method: {nameof(GetAllCustomerAddressesForAdminAsync)}\n" +
                $"There was an API error attempting to retrieve all of the addresses: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AddressService)}\n" +
                $"Method: {nameof(GetAllCustomerAddressesForAdminAsync)}\n" +
                $"There was an unexpected error attempting to retrieve all of the addresses: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
    }

    private static string BuildQueryString(AddressQueryParams queryParams)
    {
        var query = new StringBuilder();
        query.Append($"?page={queryParams.Page}&pageSize={queryParams.PageSize}");

        if (!string.IsNullOrEmpty(queryParams.CustomerId))
        {
            query.Append($"&customerId={queryParams.CustomerId}");
        }

        if (!string.IsNullOrEmpty(queryParams.City))
        {
            query.Append($"&city={queryParams.City}");
        }

        if (!string.IsNullOrEmpty(queryParams.State))
        {
            query.Append($"&state={queryParams.State}");
        }

        if (!string.IsNullOrEmpty(queryParams.Country))
        {
            query.Append($"&country={queryParams.Country}");
        }

        return query.ToString();
    }


}
