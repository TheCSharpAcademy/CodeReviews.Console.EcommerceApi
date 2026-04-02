using ECommerce.AvaloniaClient.TerrenceLGee.Data;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Customer;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Customer;
using ECommerce.Shared.TerrenceLGee.Parameters.CustomerParameters;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services;

public class CustomerService : ICustomerService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<CustomerService> _logger;
    private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
    private string _errorMessage = string.Empty;
    private const string ClientName = "client";
    private const string MediaType = "application/json";
    private const string LogErrorString = "{msg}\n\n";

    public CustomerService(IHttpClientFactory clientFactory, ILogger<CustomerService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<CustomerData?> GetCustomerProfileAsync()
    {
        var customerDataForError = new CustomerData();

        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.CustomerGetProfileUrl}";

            var response = await httpClient.GetAsync(url);

            var responseContent = await response.Content.ReadAsStringAsync();
            var customerResponse = JsonSerializer.Deserialize<CustomerRoot>(responseContent, options);

            if (customerResponse is null)
            {
                customerDataForError.ErrorMessage = $"Unable to retrieve customer profile at this time, please try again later";
                return customerDataForError;
            }

            if (!customerResponse.IsSuccess || customerResponse.StatusCode != 200)
            {
                customerDataForError.ErrorMessage = $"Unable to retrieve customer profile: {string.Join('\n', customerResponse.Errors)}";
                return customerDataForError;
            }

            return customerResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(CustomerService)}\n" +
                $"Method: {nameof(GetCustomerProfileAsync)}\n" +
                $"There was an API error retrieving the customer profile: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            customerDataForError.ErrorMessage = "There was an API error retrieving the customer profile";
            return customerDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CustomerService)}\n" +
                $"Method: {nameof(GetCustomerProfileAsync)}\n" +
                $"There was an unexpected error retrieving the customer profile: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            customerDataForError.ErrorMessage = $"There was an unexpected error retrieving the customer profile";
            return customerDataForError;
        }
    }

    public async Task<CustomersAdminRoot?> GetCustomersForAdminAsync(CustomerQueryParams queryParams)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.GetAllCustomersForAdminUrl}{BuildQueryString(queryParams)}";

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            var customersResponse = JsonSerializer.Deserialize<CustomersAdminRoot>(responseContent, options);

            if (customersResponse is null) return null;

            return customersResponse;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(CustomerService)}\n" +
                $"Method: {nameof(GetCustomersForAdminAsync)}\n" +
                $"There was an API error attempting to retrieve all customers: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CustomerService)}\n" +
                $"Method: {nameof(GetCustomersForAdminAsync)}\n" +
                $"There was an unexpected error attempting to retrieve all customers: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
    }

    private static string BuildQueryString(CustomerQueryParams queryParams)
    {
        var query = new StringBuilder();
        query.Append($"?page={queryParams.Page}&pageSize={queryParams.PageSize}");

        if (queryParams.MinSaleCount.HasValue)
        {
            query.Append($"&minSaleCount={queryParams.MinSaleCount}");
        }

        if (queryParams.MaxSaleCount.HasValue)
        {
            query.Append($"&maxSaleCount={queryParams.MaxSaleCount}");
        }

        if (queryParams.MinTotalSpent.HasValue)
        {
            query.Append($"&minTotalSpent={queryParams.MinTotalSpent}");
        }

        if (queryParams.MaxTotalSpent.HasValue)
        {
            query.Append($"&maxTotalSpend={queryParams.MaxTotalSpent}");
        }

        return query.ToString();
    }
}
