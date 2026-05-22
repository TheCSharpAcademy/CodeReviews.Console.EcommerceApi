using EcommerceAPI.Ledana.DTOs;
using EcommerceAPI.Ledana.Models;
using System.Net.Http.Json;

namespace ECommerceUI.Ledana.Clients
{
    internal class ProductApiClient
    {
        private static readonly HttpClient _client = new();
        internal async Task<string> CreateProduct(ProductDto product)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("https://localhost:7077/api/product", product);
                var result = await response.Content.ReadFromJsonAsync<ApiResponseDto<Product>>();

                if (response is null) return "Creating product didn't work ";

                if (response.IsSuccessStatusCode)
                    return "Product added succesfully";

                return "Creating product didn't work ";
            }
            catch (Exception e)
            {
                return "Creating product didn't work " + e.Message;
            }
        }

        internal async Task<string> DeleteProduct(int id)
        {
            try
            {
                var response = await _client.DeleteFromJsonAsync<ApiResponseDto<string>>($"https://localhost:7077/api/product/{id}");

                if (response is null) return "Deleting product didn't work";

                if (!response.RequestFailed)
                    return "Product deleted successfully!";

                return response.ErrorMessage;
            }
            catch (Exception e)
            {
                return "Deleting product didn't work " + e.Message;
            }
        }

        internal async Task<Product?> GetProductById(int id)
        {
            try
            {
                var response = await _client.GetFromJsonAsync<ApiResponseDto<Product>>($"https://localhost:7077/api/product/{id}");

                if (response is null) return null;
                return response.Data;

            }
            catch (Exception e)
            {
                Console.WriteLine("Getting product went wrong " + e.Message);
                return null;
            }
        }

        internal async Task<ApiResponseDto<List<Product>>?> GetProducts(int pageNumber, int pageSize)
        {
            try
            {

                return await _client.GetFromJsonAsync<ApiResponseDto<List<Product>>>($"https://localhost:7077/api/product?page_size={pageSize}&page_number={pageNumber}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Getting products went wrong " + e.Message);
                return null;
            }
        }
        internal async Task<ApiResponseDto<List<Product>>?> GetProducts()
        {
            try
            {

                return await _client.GetFromJsonAsync<ApiResponseDto<List<Product>>>($"https://localhost:7077/api/product");
            }
            catch (Exception e)
            {
                Console.WriteLine("Getting products went wrong " + e.Message);
                return null;
            }
        }
        //without pagination
        internal async Task<List<Product>?> GetProductsWithoutPagination()
        {
            try
            {
                var response =  await _client.GetFromJsonAsync<ApiResponseDto<List<Product>>>($"https://localhost:7077/api/product/all");
                if (response is null) return null;

                return response.Data;
            }
            catch (Exception e)
            {
                Console.WriteLine("Getting products went wrong " + e.Message);
                return null;
            }
        }
        

        internal async Task<string> UpdateProduct(int id, ProductUpdateDto product)
        {
            try
            {
                var response = await _client.PutAsJsonAsync<ProductUpdateDto>($"https://localhost:7077/api/product/{id}", product);

                if (response.IsSuccessStatusCode)
                    return "Product updated successfully!";

                return "Updating product didn't work";
            }
            catch (Exception e)
            {
                return "Updating product didn't work " + e.Message;
            }
        }
        internal async Task<ApiResponseDto<List<Product>>?> GetProductsOrderedByPrice(int pageNumber, int pageSize)
        {
            try
            {
                return await _client.GetFromJsonAsync<ApiResponseDto<List<Product>>>($"https://localhost:7077/api/product?page_size={pageSize}&page_number={pageNumber}&sort_by=price");
            }
            catch (Exception e)
            {
                Console.WriteLine("Getting products went wrong " + e.Message);
                return null;
            }
        }

        internal async Task<ApiResponseDto<List<Product>>?> GetProductsOrderedByStock(int pageNumber, int pageSize)
        {
            try
            {
                return await _client.GetFromJsonAsync<ApiResponseDto<List<Product>>>($"https://localhost:7077/api/product?page_size={pageSize}&page_number={pageNumber}&sort_by=stock");
            }
            catch (Exception e)
            {
                Console.WriteLine("Getting products went wrong " + e.Message);
                return null;
            }
        }

        internal async Task<List<Product>?> GetProductsWithName(string name)
        {
            try
            {
                var response = await _client.GetFromJsonAsync<ApiResponseDto<List<Product>>>($"https://localhost:7077/api/product?name={name.Trim()}");

                if (response is null) return null;
                return response.Data;

            }
            catch (Exception e)
            {
                Console.WriteLine("Getting product went wrong " + e.Message);
                return null;
            }
        }

        internal async Task<List<Product>?> GetProductsCheaperThenPrice(decimal price)
        {
            try
            {
                var response = await _client.GetFromJsonAsync<ApiResponseDto<List<Product>>>($"https://localhost:7077/api/product?price={price}");

                if (response is null) return null;
                return response.Data;

            }
            catch (Exception e)
            {
                Console.WriteLine("Getting product went wrong " + e.Message);
                return null;
            }
        }

        internal async Task<List<Product>?> ViewProductsLowerThenStock(int stock)
        {
            try
            {
                var response = await _client.GetFromJsonAsync<ApiResponseDto<List<Product>>>($"https://localhost:7077/api/product?stock={stock}");

                if (response is null) return null;
                return response.Data;

            }
            catch (Exception e)
            {
                Console.WriteLine("Getting product went wrong " + e.Message);
                return null;
            }
        }
    }
}
