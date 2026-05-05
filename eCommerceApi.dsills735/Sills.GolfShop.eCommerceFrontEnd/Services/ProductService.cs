
using Newtonsoft.Json;
using Sills.GolfShop.eCommerceFrontEnd.Models;
using Sills.GolfShop.eCommerceFrontEnd.Visualizations;


namespace Sills.GolfShop.eCommerceFrontEnd.Services;

internal class ProductService
{
    private static readonly HttpClient client = new HttpClient();

    static ProductService()
    {
        client.BaseAddress = new Uri("http://localhost:8080/");
        client.Timeout = TimeSpan.FromSeconds(30);
    }

    

    internal async Task GetAllProductsAsync()
    {
        //TODO finish once admin area is done.
        try
        {
            var response = await client.GetStringAsync("api/Product?PageNumber=1&PageSize=20");

            if (response == null)
            {
                Console.WriteLine("No products found.");
                return;
            }
            var deserializedProducts = JsonConvert.DeserializeObject<List<Product>>(response);


            if (deserializedProducts != null && deserializedProducts.Count > 0)
            {              
                ProductVisualizations.DisplayProductsTable(deserializedProducts);
            }
            else
            {
                Console.WriteLine("No products found.");

            }
        }
        
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching products: {ex.Message}");
            return;

        }
        catch(JsonException ex)
        {
            Console.WriteLine($"Error parsing product data: {ex.Message}");
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return;
        }
    }
    internal static async Task AddProduct(Product product)
    {
        throw new NotImplementedException();
    }
}