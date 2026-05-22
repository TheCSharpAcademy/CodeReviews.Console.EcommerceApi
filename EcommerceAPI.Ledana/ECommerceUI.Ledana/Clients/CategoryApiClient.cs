using EcommerceAPI.Ledana.DTOs;
using EcommerceAPI.Ledana.Models;
using System.Net.Http.Json;

namespace ECommerceUI.Ledana.Clients
{
    internal class CategoryApiClient
    {
        private static readonly HttpClient _client = new();

        internal async Task<string> CreateCategory(CategoryDto categoryDto)
        {
            try
            {
                var response = await _client.PostAsJsonAsync<CategoryDto>("https://localhost:7077/api/category", categoryDto);

                if (response.IsSuccessStatusCode)
                    return "Category was added successfully";

                return "Creating category went wrong";
            }
            catch (Exception e)
            {
                return "Creating category went wrong " + e.Message;
            }
        }

        internal async Task<string> DeleteCategory(int categoryId)
        {
            try
            {
                var response = await _client.DeleteAsync($"https://localhost:7077/api/category/{categoryId}");

                if (response.IsSuccessStatusCode)
                    return "Category was deleted successfully";

                return "Deleting category went wrong";
            }
            catch (Exception e)
            {
                return "Deleting category went wrong " + e.Message;
            }
        }

        internal async Task<List<Category>?> GetCategories()
        {
            try
            {
                var response = await _client.GetFromJsonAsync<ApiResponseDto<List<Category>>>("https://localhost:7077/api/category");
                if (response is null) return null;

                return response.Data;

            }
            catch (Exception e)
            {
                Console.WriteLine("Getting categories didn't work " + e.Message);
                return null;
            }
        }

        internal async Task<Category?> GetCategoryById(int categoryId)
        {
            try
            {
                var response = await _client.GetFromJsonAsync<ApiResponseDto<Category>>($"https://localhost:7077/api/category/{categoryId}");
                if (response is null) return null;

                return response.Data;

            }
            catch (Exception e)
            {
                Console.WriteLine("Getting category didn't work " + e.Message);
                return null;
            }
        }

        internal async Task<string> UpdateCategory(int categoryId, CategoryDto categoryDto)
        {
            try
            {
                var response = await _client.PutAsJsonAsync($"https://localhost:7077/api/category/{categoryId}", categoryDto);

                if (response.IsSuccessStatusCode)
                    return "Category was updated successfully";

                return "Updating category went wrong";
            }
            catch (Exception e)
            {
                return "Updating category went wrong " + e.Message;
            }
        }
    }
}
