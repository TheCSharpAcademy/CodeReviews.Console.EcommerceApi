using EcommerceAPI.Ledana.Models;
using System.Globalization;

namespace ECommerceUI.Ledana
{
    internal class Helper
    {
        internal static int GetIntInput(string message)
        {
            Console.WriteLine(message);
            string? input = Console.ReadLine();
            int id;
            while (!int.TryParse(input, out id) || input is null)
            {
                Console.WriteLine("Invalid input, try again! Or type '0' to go back");
                input = Console.ReadLine();
            }
            return id;
        }

        internal static bool IsProductIdCorrect(int id, List<Product>? products)
        {
            if (products is null) return false;

            return products.Any(p => p.Id == id);
        }
        internal static decimal GetDecimalInput(string message)
        {
            Console.WriteLine(message);
            string? input = Console.ReadLine();
            decimal num;
            while(!decimal.TryParse(input, out num) || input is null)
            {
                Console.WriteLine("Invalid input, try again! Or type '0' to go back");
                input = Console.ReadLine();
            }
            return num;
        }

        internal static bool ValidateStock(int stock)
        {
            return stock > 0;
        }

        internal static bool ValidatePrice(decimal price)
        {
            return price > 0;
        }
    }
}
