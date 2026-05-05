using Sills.GolfShop.eCommerceFrontEnd.Models;
using Spectre.Console;
namespace Sills.GolfShop.eCommerceFrontEnd.Visualizations;

internal class ProductVisualizations
{
    internal static void DisplayProductsTable(List<Product> products, bool stay = true)
    {
        var table = new Table()
            .RoundedBorder()
            .BorderColor(Color.Green)
            .AddColumn("Product Name")
            .AddColumn("Description")
            .AddColumn("Price");

        foreach (var product in products)
        {
            table.AddRow(product.Name, product.Description, product.Price.ToString("C"));
        }
        AnsiConsole.Write(table);

        if (stay)
        {
            Console.WriteLine("Press any key to return to the menu");
            Console.ReadKey();
            Console.Clear();
        }
    }
}