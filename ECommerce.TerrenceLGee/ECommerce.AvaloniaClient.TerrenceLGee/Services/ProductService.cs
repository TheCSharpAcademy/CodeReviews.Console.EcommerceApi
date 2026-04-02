using ECommerce.AvaloniaClient.TerrenceLGee.Data;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using ECommerce.Shared.TerrenceLGee.DTOs.ProductDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<ProductService> _logger;
    private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
    private string _errorMessage = string.Empty;
    private const string ClientName = "client";
    private const string MediaType = "application/json";
    private const string LogErrorString = "{msg}\n\n";

    public ProductService(IHttpClientFactory clientFactory, ILogger<ProductService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<ProductAdminData?> AddProductAsync(CreateProductDto product)
    {
        var productAdminDataForError = new ProductAdminData();

        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminAddProductUrl}";

            var content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, MediaType);

            var response = await httpClient.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var productAddedResponse = JsonSerializer.Deserialize<ProductAdminRoot>(responseContent, options);

            if (productAddedResponse is null)
            {
                productAdminDataForError.ErrorMessage = $"Unable to add new product in category {product.CategoryId} at this time, " +
                    $"please try again later";
                return productAdminDataForError;
            }

            if (!productAddedResponse.IsSuccess || productAddedResponse.StatusCode != 201)
            {
                productAdminDataForError.ErrorMessage = $"Unable to add new product in category {product.CategoryId}: " +
                    $"{string.Join('\n', productAddedResponse.Errors)}";
                return productAdminDataForError;
            }

            return productAddedResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(AddProductAsync)}\n" +
                $"There was an API error adding a new product in category {product.CategoryId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            productAdminDataForError.ErrorMessage = $"There was an API error adding a new product in category {product.CategoryId}";
            return productAdminDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(AddProductAsync)}\n" +
                $"There was an unexpected error adding a new product in category {product.CategoryId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            productAdminDataForError.ErrorMessage = $"There was an unexpected error adding a new product in category {product.CategoryId}";
            return productAdminDataForError;
        }
    }

    public async Task<ProductAdminData?> UpdateProductAsync(UpdateProductDto product)
    {
        var productAdminDataForError = new ProductAdminData();

        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminUpdateProductUrl}{product.Id}";

            var content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var productUpdatedResponse = JsonSerializer.Deserialize<ProductAdminRoot>(responseContent, options);

            if (productUpdatedResponse is null)
            {
                productAdminDataForError.ErrorMessage = $"Unable to update product {product.Id} in category " +
                    $"{product.CategoryId} at this time, please try again later";
                return productAdminDataForError;
            }

            if (!productUpdatedResponse.IsSuccess || productUpdatedResponse.StatusCode != 200)
            {
                productAdminDataForError.ErrorMessage = $"Unable to update product {product.Id} in category " +
                    $"{product.CategoryId}: {string.Join('\n', productUpdatedResponse.Errors)}";
                return productAdminDataForError;
            }

            return productUpdatedResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(UpdateProductAsync)}\n" +
                $"There was an API error updating product {product.Id} in category {product.CategoryId}: " +
                $"{ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            productAdminDataForError.ErrorMessage = $"There was an API error updating product {product.Id} in category {product.CategoryId}";
            return productAdminDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(UpdateProductAsync)}\n" +
                $"There was an unexpected error updating product {product.Id} in category {product.CategoryId}: " +
                $"{ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            productAdminDataForError.ErrorMessage = $"There was an unexpected error updating product {product.Id} in category {product.CategoryId}";
            return productAdminDataForError;
        }
    }

    public async Task<(bool, string?)> DeleteProductAsync(int productId)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminDeleteProductUrl}{productId}";

            var response = await httpClient.DeleteAsync(url);

            var responseContent = await response.Content.ReadAsStringAsync();
            var productDeletedResponse = JsonSerializer.Deserialize<ProductDeletionRoot>(responseContent, options);

            if (productDeletedResponse is null)
            {
                return (false, $"Unable to delete product {productId} at this time, please try again later");
            }

            if (!productDeletedResponse.IsSuccess || productDeletedResponse.StatusCode != 200)
            {
                return (false, $"Unable to delete product {productId}: {string.Join('\n', productDeletedResponse.Errors)}");
            }

            return (true, productDeletedResponse.Data);
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(DeleteProductAsync)}\n" +
                $"There was an API error deleting product {productId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (false, $"There was an API error deleting product {productId}");
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(DeleteProductAsync)}\n" +
                $"There was an unexpected error deleting product {productId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (false, $"There was an unexpected error deleting product {productId}");
        }
    }

    public async Task<(bool, string?)> RestoreProductAsync(int productId)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminRestoreProductUrl}{productId}";

            var response = await httpClient.PostAsync(url, null);

            var responseContent = await response.Content.ReadAsStringAsync();
            var productRestoredResponse = JsonSerializer.Deserialize<ProductRestorationRoot>(responseContent, options);

            if (productRestoredResponse is null)
            {
                return (false, $"Unable to restore product {productId} at this time, please try again later");
            }

            if (!productRestoredResponse.IsSuccess || productRestoredResponse.StatusCode != 200)
            {
                return (false, $"Unable to restore product {productId}: {string.Join('\n', productRestoredResponse.Errors)}");
            }

            return (true, productRestoredResponse.Data);
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(RestoreProductAsync)}\n" +
                $"There was an API error restoring product {productId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (false, $"There was an API error restoring product {productId}");
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(RestoreProductAsync)}\n" +
                $"There was an unexpected error restoring product {productId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (false, $"There was an unexpected error deleting product {productId}");
        }
    }

    public async Task<ProductAdminData?> GetProductForAdminAsync(int productId)
    {
        var productAdminDataForError = new ProductAdminData();

        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminGetProductByIdUrl}{productId}";

            var response = await httpClient.GetAsync(url);

            var responseContent = await response.Content.ReadAsStringAsync();
            var productResponse = JsonSerializer.Deserialize<ProductAdminRoot>(responseContent, options);

            if (productResponse is null)
            {
                productAdminDataForError.ErrorMessage = $"Unable to retrieve product {productId} at this time, please try again later";
                return productAdminDataForError;
            }

            if (!productResponse.IsSuccess || productResponse.StatusCode != 200)
            {
                productAdminDataForError.ErrorMessage = $"Unable to retrieve product {productId}: {string.Join('\n', productResponse.Errors)}";
                return productAdminDataForError;
            }

            return productResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(GetProductForAdminAsync)}\n" +
                $"There was an API error retrieving product {productId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            productAdminDataForError.ErrorMessage = $"There was an API error retrieving product {productId}";
            return productAdminDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(GetProductForAdminAsync)}\n" +
                $"There was an unexpected error retrieving product {productId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            productAdminDataForError.ErrorMessage = $"There was an unexpected error retrieving product {productId}";
            return productAdminDataForError;
        }
    }

    public async Task<ProductData?> GetProductAsync(int productId)
    {
        var productDataForError = new ProductData();

        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.CustomerGetProductByIdUrl}{productId}";

            var response = await httpClient.GetAsync(url);

            var responseContent = await response.Content.ReadAsStringAsync();
            var productResponse = JsonSerializer.Deserialize<ProductRoot>(responseContent, options);

            if (productResponse is null)
            {
                productDataForError.ErrorMessage = $"Unable to retrieve product {productId} at this time, please try again later";
                return productDataForError;
            }

            if (!productResponse.IsSuccess || productResponse.StatusCode != 200)
            {
                productDataForError.ErrorMessage = $"Unable to retrieve product {productId}: {string.Join('\n', productResponse.Errors)}";
                return productDataForError;
            }

            return productResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(GetProductAsync)}\n" +
                $"There was an API error retrieving product {productId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            productDataForError.ErrorMessage = $"There was an API error retrieving product {productId}";
            return productDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(GetProductAsync)}\n" +
                $"There was an unexpected error retrieving product {productId}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            productDataForError.ErrorMessage = $"There was an unexpected error retrieving product {productId}";
            return productDataForError;
        }
    }

    public async Task<ProductsAdminRoot?> GetProductsForAdminAsync(ProductQueryParams queryParams)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminGetProductsUrl}{BuildQueryString(queryParams)}";

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            var productsResponse = JsonSerializer.Deserialize<ProductsAdminRoot>(responseContent, options);

            if (productsResponse is null) return null;

            return productsResponse;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(GetProductsForAdminAsync)}\n" +
                $"There was an API error attempting to retrieve all of the products: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(GetProductsForAdminAsync)}\n" +
                $"There was an unexpected error attempting to retrieve all of the products: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
    }

    public async Task<ProductsRoot?> GetProductsAsync(ProductQueryParams queryParams)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.CustomerGetProductsUrl}{BuildQueryString(queryParams)}";

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            var productsResponse = JsonSerializer.Deserialize<ProductsRoot>(responseContent, options);

            if (productsResponse is null) return null;

            return productsResponse;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(GetProductsAsync)}\n" +
                $"There was an API error attempting to retrieve all of the products: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductService)}\n" +
                $"Method: {nameof(GetProductsAsync)}\n" +
                $"There was an unexpected error attempting to retrieve all of the products: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
    }

    private static string BuildQueryString(ProductQueryParams queryParams)
    {
        var query = new StringBuilder();
        query.Append($"?page={queryParams.Page}&pageSize={queryParams.PageSize}");

        if (queryParams.MinUnitPrice.HasValue)
        {
            query.Append($"&minUnitPrice={queryParams.MinUnitPrice}");
        }

        if (queryParams.MaxUnitPrice.HasValue)
        {
            query.Append($"&maxUnitPrice={queryParams.MaxUnitPrice}");
        }

        if (queryParams.MinStockQuantity.HasValue)
        {
            query.Append($"&minStockQuantity={queryParams.MinStockQuantity}");
        }

        if (queryParams.MaxStockQuantity.HasValue)
        {
            query.Append($"&maxStockQuantity={queryParams.MaxStockQuantity}");
        }

        if (queryParams.MinDiscountPercentage.HasValue)
        {
            query.Append($"&minDiscountPercentage={queryParams.MinDiscountPercentage}");
        }

        if (queryParams.MaxDiscountPercentage.HasValue)
        {
            query.Append($"&maxDiscountPercentage={queryParams.MaxDiscountPercentage}");
        }

        if (queryParams.CategoryId.HasValue)
        {
            query.Append($"&categoryId={queryParams.CategoryId}");
        }

        if (!string.IsNullOrEmpty(queryParams.CategoryName))
        {
            query.Append($"&categoryName={queryParams.CategoryName}");
        }

        if (!string.IsNullOrEmpty(queryParams.Description))
        {
            query.Append($"&description={queryParams.Description}");
        }

        if (queryParams.InStock.HasValue)
        {
            query.Append($"&inStock={queryParams.InStock}");
        }

        if (queryParams.IsDeleted.HasValue)
        {
            query.Append($"&isDeleted={queryParams.IsDeleted}");
        }

        return query.ToString();
    }
}
