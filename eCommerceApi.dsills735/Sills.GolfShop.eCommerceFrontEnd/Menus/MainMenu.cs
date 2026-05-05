using Sills.GolfShop.eCommerceFrontEnd.Services;
using Spectre.Console;

namespace Sills.GolfShop.eCommerceFrontEnd.Menus;

internal class MainMenu
{
    public static async Task MainDisplayAsync()
    {
        bool running = true;

        while (running)
        {
            //Console.Clear();

            AnsiConsole.Write(
                new FigletText("Sills Golf Shop")
                    .LeftJustified()
                    .Color(Color.Green));

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(new[] {
                     "Search for a product",
                    "Shop All Products",
                    "Shop from a category",
                    "View Cart",
                    "Checkout",
                    "View Order History",
                    "Administration Menu",
                    "Exit"
                    }));

            switch (choice)
            {
                case "Search for a product":
                    //SearchMenu.DisplaySearch();
                    break;

                case "Shop All Products":
                    Console.Clear();
                    ProductService productService = new ProductService();
                    await productService.GetAllProductsAsync();
                    break;

                case "Shop from a category":
                    //CategoryMenu.DisplayCategories();
                    break;
                case "View Cart":
                    //CartMenu.DisplayCart();
                    break;

                case "Checkout":
                    break;

                case "View Order History":
                    break;

                case "Administration Menu":
                    break; 

                case "Exit":
                    Environment.Exit(0);
                    break;
            }
        }
    }
}