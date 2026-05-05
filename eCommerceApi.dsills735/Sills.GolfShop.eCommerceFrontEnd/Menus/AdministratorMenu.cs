using Sills.GolfShop.eCommerceFrontEnd.Controllers;
using Spectre.Console;

namespace Sills.GolfShop.eCommerceFrontEnd.Menus;

internal class AdministratorMenu
{
    internal static async Task AdminMenuAsync()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();

            AnsiConsole.Write(
                new FigletText("Admin Menu")
                    .LeftJustified()
                    .Color(Color.Green));

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(new[] {
                        "Add a product",
                        "Update a product",
                        "Delete a product",
                        "Add a category",
                        "Update a category",
                        "Delete a category",
                        "Exit"
                    }));

            switch (choice)
            {
                case "Add a product":
                    ProductsController productsController = new ProductsController();
                    await productsController.AddProduct();
                    break;

                case "Update a product":
                    //UpdateProductMenu.DisplayUpdateProductMenu();
                    break;

                case "Delete a product":
                    //DeleteProductMenu.DisplayDeleteProductMenu();
                    break;

                case "Exit":
                    running = false;
                    break;
            }
        }
    }
}
