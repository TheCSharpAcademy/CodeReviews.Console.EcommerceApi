using EcommerceAPI.Ledana.Models;
using ECommerceUI.Ledana.Clients;

namespace ECommerceUI.Ledana.Services
{
    internal class CategoryUIService
    {
        static CategoryApiClient categoryApiClient = new();

        internal static async Task<int> GetCategoryId(string message)
        {
            int id = Helper.GetIntInput(message);
            if (id == 0) return 0;

            var categories = await categoryApiClient.GetCategories();
            if (categories is null) return 0;

            if (categories.Any(c => c.Id == id))
                return id;

            Console.WriteLine("Id is not correct");
            return 0;
        }

        internal static string GetCategoryName(string message)
        {
            Console.WriteLine(message);
            var input =  Console.ReadLine();
            while(string.IsNullOrEmpty(input))
            {
                Console.WriteLine(message);
                input = Console.ReadLine();
            }
            return input;
        }
    }
}
