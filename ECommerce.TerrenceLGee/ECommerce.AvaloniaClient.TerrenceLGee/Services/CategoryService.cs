using ECommerce.AvaloniaClient.TerrenceLGee.Data;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Category;
using ECommerce.Shared.TerrenceLGee.DTOs.CategoryDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.CategoryParameters;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services;

public class CategoryService : ICategoryService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<CategoryService> _logger;
    private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
    private string _errorMessage = string.Empty;
    private const string ClientName = "client";
    private const string MediaType = "application/json";
    private const string LogErrorString = "{msg}\n\n";

    public CategoryService(IHttpClientFactory clientFactory, ILogger<CategoryService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<CategoryAdminData?> AddCategoryAsync(CreateCategoryDto category)
    {
        var categoryAdminDataForError = new CategoryAdminData();
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminAddCategoryUrl}";

            var content = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, MediaType);

            var response = await httpClient.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var categoryAddedResponse = JsonSerializer.Deserialize<CategoryAdminRoot>(responseContent, options);

            if (categoryAddedResponse is null)
            {
                categoryAdminDataForError.ErrorMessage = $"Unable to add new category at this time, please try again later";
                return categoryAdminDataForError;
            }

            if (!categoryAddedResponse.IsSuccess || categoryAddedResponse.StatusCode != 201)
            {
                categoryAdminDataForError.ErrorMessage = $"Unable to add new category: {string.Join('\n', categoryAddedResponse.Errors)}";
                return categoryAdminDataForError;
            }

            return categoryAddedResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryService)}\n" +
                $"Method: {nameof(AddCategoryAsync)}\n" +
                $"There was an API error attempting to add the new category: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            categoryAdminDataForError.ErrorMessage = "There was an API error attempting to add the new category.";
            return categoryAdminDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryService)}\n" +
                $"Method: {nameof(AddCategoryAsync)}\n" +
                $"There was an unexpected error attempting to add the new category: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            categoryAdminDataForError.ErrorMessage = "There was an unexpected error attempting to add the new category.";
            return categoryAdminDataForError;
        }
    }

    public async Task<CategoryAdminData?> UpdateCategoryAsync(UpdateCategoryDto category)
    {
        var categoryAdminDataForError = new CategoryAdminData();
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminUpdateCategoryUrl}{category.Id}";

            var content = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, MediaType);

            var response = await httpClient.PutAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var categoryUpdatedResponse = JsonSerializer.Deserialize<CategoryAdminRoot>(responseContent, options);

            if (categoryUpdatedResponse is null)
            {
                categoryAdminDataForError.ErrorMessage = $"Unable to category {category.Id} at this time, please try again later.";
                return categoryAdminDataForError;
            }

            if (!categoryUpdatedResponse.IsSuccess || categoryUpdatedResponse.StatusCode != 200)
            {
                categoryAdminDataForError.ErrorMessage = $"Unable to update category {category.Id}: {string.Join('\n', categoryUpdatedResponse.Errors)}";
                return categoryAdminDataForError;
            }

            return categoryUpdatedResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryService)}\n" +
                $"Method: {nameof(UpdateCategoryAsync)}\n" +
                $"There was an API error attempting to update category {category.Id}: {ex.Message}.";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            categoryAdminDataForError.ErrorMessage = $"There was an API error attempting to update category {category.Id}.";
            return categoryAdminDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryService)}\n" +
                $"Method: {nameof(UpdateCategoryAsync)}\n" +
                $"There was an unexpected error attempting to update category {category.Id}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            categoryAdminDataForError.ErrorMessage = $"There was an unexpected error attempting to update category {category.Id}";
            return categoryAdminDataForError;
        }
    }

    public async Task<CategoryData?> GetCategoryAsync(int id)
    {
        var categoryDataForError = new CategoryData();
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.CustomerGetCategoryByIdUrl}{id}";

            var response = await httpClient.GetAsync(url);

            var responseContent = await response.Content.ReadAsStringAsync();
            var categoryResponse = JsonSerializer.Deserialize<CategoryRoot>(responseContent, options);

            if (categoryResponse is null)
            {
                categoryDataForError.ErrorMessage = $"Unable to retrieve category {id} at this time, please try again later";
                return categoryDataForError;
            }

            if (!categoryResponse.IsSuccess || categoryResponse.StatusCode != 200)
            {
                categoryDataForError.ErrorMessage = $"Unable to retrieve category {id}: {string.Join('\n', categoryResponse.Errors)}";
                return categoryDataForError;
            }

            return categoryResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryService)}\n" +
                $"Method: {nameof(GetCategoryAsync)}\n" +
                $"There was an API error retrieving category {id}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            categoryDataForError.ErrorMessage = $"There was an API error retrieving category {id}";
            return categoryDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryService)}\n" +
                $"Method: {nameof(GetCategoryAsync)}\n" +
                $"There was an unexpected error retrieving category {id}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            categoryDataForError.ErrorMessage = $"There was an unexpected error retrieving category {id}";
            return categoryDataForError;
        }
    }

    public async Task<CategoryAdminData?> GetCategoryForAdminAsync(int id)
    {
        var categoryAdminDataForError = new CategoryAdminData();
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminGetCategoryByIdUrl}{id}";

            var response = await httpClient.GetAsync(url);

            var responseContent = await response.Content.ReadAsStringAsync();
            var categoryResponse = JsonSerializer.Deserialize<CategoryAdminRoot>(responseContent, options);

            if (categoryResponse is null)
            {
                categoryAdminDataForError.ErrorMessage = $"Unable to retrieve category {id} at this time, please try again later";
                return categoryAdminDataForError;
            }

            if (!categoryResponse.IsSuccess || categoryResponse.StatusCode != 200)
            {
                categoryAdminDataForError.ErrorMessage = $"Unable to retrieve category {id}: {string.Join('\n', categoryResponse.Errors)}";
                return categoryAdminDataForError;
            }

            return categoryResponse.Data;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryService)}\n" +
                $"Method: {nameof(GetCategoryForAdminAsync)}\n" +
                $"There was an API error retrieving category {id}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            categoryAdminDataForError.ErrorMessage = $"There was an API error retrieving category {id}";
            return categoryAdminDataForError;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryService)}\n" +
                $"Method: {nameof(GetCategoryForAdminAsync)}\n" +
                $"There was an unexpected error retrieving category {id}: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            categoryAdminDataForError.ErrorMessage = $"There was an unexpected error retrieving category {id}";
            return categoryAdminDataForError;
        }
    }


    public async Task<CategoriesRoot?> GetCategoriesAsync(CategoryQueryParams queryParams)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.CustomerGetCategoriesUrl}{BuildQueryString(queryParams)}";

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            var categoriesResponse = JsonSerializer.Deserialize<CategoriesRoot>(responseContent, options);

            if (categoriesResponse is null) return null;

            return categoriesResponse;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryService)}\n" +
                $"Method: {nameof(GetCategoriesAsync)}\n" +
                $"There was an API error attempting to retrieve all of the categories: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryService)}\n" +
                $"Method: {nameof(GetCategoriesAsync)}\n" +
                $"There was an unexpected error attempting to retrieve all of the categories: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
    }

    public async Task<CategoriesAdminRoot?> GetCategoriesForAdminAsync(CategoryQueryParams queryParams)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.AdminGetCategoriesUrl}{BuildQueryString(queryParams)}";

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            var categoriesResponse = JsonSerializer.Deserialize<CategoriesAdminRoot>(responseContent, options);

            if (categoriesResponse is null) return null;

            return categoriesResponse;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryService)}\n" +
                $"Method: {nameof(GetCategoriesForAdminAsync)}\n" +
                $"There was an API error attempting to retrieve all of the categories: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryService)}\n" +
                $"Method: {nameof(GetCategoriesForAdminAsync)}\n" +
                $"There was an unexpected error attempting to retrieve all of the categories: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return null;
        }
    }

    private static string BuildQueryString(CategoryQueryParams queryParams)
    {
        var query = new StringBuilder();
        query.Append($"?page={queryParams.Page}&pageSize={queryParams.PageSize}");

        if (!string.IsNullOrEmpty(queryParams.Description))
        {
            query.Append($"&description={queryParams.Description}");
        }

        if (!string.IsNullOrEmpty(queryParams.OrderBy))
        {
            query.Append($"&orderBy={queryParams.OrderBy}");
        }

        return query.ToString();
    }
}
