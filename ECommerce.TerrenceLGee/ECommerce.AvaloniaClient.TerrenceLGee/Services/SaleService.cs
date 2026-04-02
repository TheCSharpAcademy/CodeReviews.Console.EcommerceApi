using ECommerce.AvaloniaClient.TerrenceLGee.Data;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Sale;
using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using ECommerce.Shared.TerrenceLGee.DTOs.SaleDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.SaleParameters;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services;

public class SaleService : ISaleService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<SaleService> _logger;
    private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
    private string _errorMessage = string.Empty;
    private const string ClientName = "client";
    private const string MediaType = "application/json";
    private const string LogErrorString = "{msg}\n\n";

    public SaleService(IHttpClientFactory clientFactory, ILogger<SaleService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<SaleData?> CreateOrderAsync(CreateOrderDto sale)
    {
        var saleDataForError = new SaleData();
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.CustomerCreateSaleUrl}";

            var content = new StringContent(JsonSerializer.Serialize(sale), Encoding.UTF8, MediaType);

            var response = await httpClient.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var saleCreatedResponse = JsonSerializer.Deserialize<SaleRoot>(responseContent, options);

            if (saleCreatedResponse is null)
            {
                saleDataForError.ErrorMessage = $"Unable to complete sale at this time, please try again later";
                return saleDataForError;
            }

            if (!saleCreatedResponse.IsSuccess || saleCreatedResponse.StatusCode != 201)
            {
                saleDataForError.ErrorMessage = $"Unable to complete sale: {string.Join('\n', saleCreatedResponse.Errors)}";
                return saleDataForError;
            }

            return saleCreatedResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleService)}\n" +
                $"Method: {nameof(CreateOrderAsync)}\n" +
                $"There was an API error attempting to complete the sale: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            saleDataForError.ErrorMessage = "There was an API error attempting to complete the sale";
            return saleDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleService)}\n" +
                $"Method: {nameof(CreateOrderAsync)}\n" +
                $"There was an unexpected error attempting to comeplete the sale: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            saleDataForError.ErrorMessage = "There was an unexpected error attempting to complete the sale";
            return saleDataForError;
        }
    }

    public async Task<SaleData?> GetSaleForCustomerAsync(int saleId)
    {
        var saleDataForError = new SaleData();

        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.CustomerGetSaleByIdUrl}{saleId}";

            var response = await httpClient.GetAsync(url);

            var responseContent = await response.Content.ReadAsStringAsync();
            var saleResponse = JsonSerializer.Deserialize<SaleRoot>(responseContent, options);

            if (saleResponse is null)
            {
                saleDataForError.ErrorMessage = $"Unable to retrieve sale {saleId} at this time, please try again later";
                return saleDataForError;
            }

            if (!saleResponse.IsSuccess || saleResponse.StatusCode != 200)
            {
                saleDataForError.ErrorMessage = $"Unable to retrieve sale {saleId}: {string.Join('\n', saleResponse.Errors)}";
                return saleDataForError;
            }

            return saleResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleService)}\n" +
                $"Method: {nameof(GetSaleForCustomerAsync)}\n" +
                $"There was an API error retrieving sale {saleId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            saleDataForError.ErrorMessage = $"There was an API error retrieving sale {saleId}";
            return saleDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleService)}\n" +
                $"Method: {nameof(GetSaleForCustomerAsync)}\n" +
                $"There was an unexpected error retrieving sale {saleId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            saleDataForError.ErrorMessage = $"There was an unexpected error retrieving sale {saleId}";
            return saleDataForError;
        }
    }

    public async Task<SaleData?> GetSaleForAdminAsync(int saleId)
    {
        var saleDataForError = new SaleData();

        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminGetSaleByIdUrl}{saleId}";

            var response = await httpClient.GetAsync(url);

            var responseContent = await response.Content.ReadAsStringAsync();
            var saleResponse = JsonSerializer.Deserialize<SaleRoot>(responseContent, options);

            if (saleResponse is null)
            {
                saleDataForError.ErrorMessage = $"Unable to retrieve sale {saleId} at this time, please try again later";
                return saleDataForError;
            }

            if (!saleResponse.IsSuccess || saleResponse.StatusCode != 200)
            {
                saleDataForError.ErrorMessage = $"Unable to retrieve sale {saleId}: {string.Join('\n', saleResponse.Errors)}";
                return saleDataForError;
            }

            return saleResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleService)}\n" +
                $"Method: {nameof(GetSaleForAdminAsync)}\n" +
                $"There was an API error retrieving sale {saleId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            saleDataForError.ErrorMessage = $"There was an API error retrieving sale {saleId}";
            return saleDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleService)}\n" +
                $"Method: {nameof(GetSaleForAdminAsync)}\n" +
                $"There was an unexpected error retrieving sale {saleId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            saleDataForError.ErrorMessage = $"There was an unexpected error retrieving sale {saleId}";
            return saleDataForError;
        }
    }

    public async Task<SalesRoot?> GetSalesForCustomerAsync(SaleQueryParams queryParams)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.CustomerGetSalesUrl}{BuildQueryString(queryParams)}";

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            var salesResponse = JsonSerializer.Deserialize<SalesRoot>(responseContent, options);

            if (salesResponse is null) return null;

            return salesResponse;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleService)}\n" +
                $"Method: {nameof(GetSalesForCustomerAsync)}\n" +
                $"There was an API error attempting to retrieve all sales: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleService)}\n" +
                $"Method: {nameof(GetSalesForCustomerAsync)}\n" +
                $"There was an unexpected error attempting to retrieve all sales: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
    }

    public async Task<SalesRoot?> GetSalesForAdminAsync(SaleQueryParams queryParams)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminGetAllSalesUrl}{BuildQueryString(queryParams)}";

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            var salesResponse = JsonSerializer.Deserialize<SalesRoot>(responseContent, options);

            if (salesResponse is null) return null;

            return salesResponse;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleService)}\n" +
                $"Method: {nameof(GetSalesForAdminAsync)}\n" +
                $"There was an API error attempting to retrieve all sales: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleService)}\n" +
                $"Method: {nameof(GetSalesForAdminAsync)}\n" +
                $"There was an unexpected error attempting to retrieve all sales: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
    }

    public async Task<(bool, string?)> AdminUpdateSaleStatusAsync(int saleId, UpdateSaleStatusDto saleStatus)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminUpdateSaleStatusUrl}{saleId}";

            var content = new StringContent(JsonSerializer.Serialize(saleStatus), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var saleStatusUpdatedResponse = JsonSerializer.Deserialize<SaleAdminUpdateRoot>(responseContent, options);

            if (saleStatusUpdatedResponse is null)
            {
                return (false, $"Unable to update the status for sale {saleId} at this time, please try again later");
            }

            if (!saleStatusUpdatedResponse.IsSuccess || saleStatusUpdatedResponse.StatusCode != 200)
            {
                return (false, $"Unable to update the status for sale {saleId}: {string.Join('\n', saleStatusUpdatedResponse.Errors)}");
            }

            return (true, saleStatusUpdatedResponse.Data);
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleService)}\n" +
                $"Method: {nameof(AdminUpdateSaleStatusAsync)}\n" +
                $"There was an API error attempting to update the status of sale {saleId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (false, $"There was an API error attempting to update the status of sale {saleId}");
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleService)}\n" +
                $"Method: {nameof(AdminUpdateSaleStatusAsync)}\n" +
                $"There was an unexpected error attempting to update the status of sale {saleId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (false, $"There was an unexpected error attempting to update the status of sale {saleId}");
        }
    }

    public async Task<(bool, string?)> CustomerCancelSaleAsync(int saleId)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.CustomerCancelSaleUrl}{saleId}";

            var response = await httpClient.PostAsync(url, null);

            var responseContent = await response.Content.ReadAsStringAsync();
            var saleCanceledResponse = JsonSerializer.Deserialize<SaleCustomerCancelRoot>(responseContent, options);

            if (saleCanceledResponse is null)
            {
                return (false, $"Unable to cancel sale {saleId} at this time, please try again later");
            }

            if (!saleCanceledResponse.IsSuccess || saleCanceledResponse.StatusCode != 200)
            {
                return (false, $"Unable to cancel sale {saleId}: {string.Join('\n', saleCanceledResponse.Errors)}");
            }

            return (true, saleCanceledResponse.Data);
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleService)}\n" +
                $"Method: {nameof(CustomerCancelSaleAsync)}\n" +
                $"There was an API error attempting to cancel sale {saleId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (false, $"There was an API error attempting to cancel sale {saleId}");
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleService)}\n" +
                $"Method: {nameof(CustomerCancelSaleAsync)}\n" +
                $"There was an unexpected error attempting to cancel sale {saleId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (false, $"There was an unexpected error attempting to cancel sale {saleId}");
        }
    }

    private static string BuildQueryString(SaleQueryParams queryParams)
    {
        var query = new StringBuilder();
        query.Append($"?page={queryParams.Page}&pageSize={queryParams.PageSize}");

        if (!string.IsNullOrEmpty(queryParams.CustomerId))
        {
            query.Append($"&customerId={queryParams.CustomerId}");
        }

        if (queryParams.MinTotalAmount.HasValue)
        {
            query.Append($"&minTotalAmount={queryParams.MinTotalAmount}");
        }

        if (queryParams.MaxTotalAmount.HasValue)
        {
            query.Append($"&maxTotalAmount={queryParams.MaxTotalAmount}");
        }

        if (!string.IsNullOrEmpty(queryParams.Status))
        {
            query.Append($"&status={queryParams.Status}");
        }

        return query.ToString();
    }
}
