using EcommerceAPI.Ledana.DTOs;
using ECommerceUI.Ledana.Clients;
using ECommerceUI.Ledana.Services;
using ECommerceUI.Ledana.UI;
using Spectre.Console;

namespace ECommerceUI.Ledana.Controllers
{
    internal class SaleUIController
    {
        static SaleApiClient saleApiClient = new();
        internal static async Task AddNewSale()
        {
            DateTime date = DateTime.UtcNow;

            var products = await SaleUIService.GetProducts();
            if (products is null || products.Count == 0)
            {
                Console.WriteLine("Your products can not be added to a new sale");
                return;
            }

            SaleDto saleDto = new()
            {
                Date = date,
                SaleProducts = products
            };
            Console.WriteLine(await saleApiClient.CreateSale(saleDto));
        }

        internal static void ViewAllSales(ref ApiResponseDto<List<SaleProductViewDto>> response, ref int pageNumber, ref int pageSize, ref bool keepRunning)
        {
            if (response?.Data == null || response.Data.Count == 0) { Console.WriteLine("No sales found!"); return; }

            Console.Clear();

            TableVisualisation.ShowSales(response.Data);
            Console.WriteLine($"Total count: {response.TotalCount}");
            Console.WriteLine($"Current page: {response.CurrentPage}");
            Console.WriteLine($"Page size: {response.PageSize}");
            Console.WriteLine($"Has next: {response.HasNext}");
            Console.WriteLine($"Has previous: {response.HasPrevious}");

            var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .AddChoices("Next", "Previous", "First", "Last", "Go Back"));

            switch (choice)
            {
                case "Next":
                    if (!response.HasNext) return;
                    pageNumber++;
                    break;
                case "Previous":
                    if (!response.HasPrevious) return;
                    if (pageNumber > 1) pageNumber--;
                    break;
                case "First":
                    pageNumber = 1;
                    break;
                case "Last":
                    pageNumber = response.TotalCount % pageSize == 0 ? response.TotalCount / pageSize : response.TotalCount / pageSize + 1;
                    break;
                case "Go Back":
                    keepRunning = false;
                    break;
            }
        }

        internal static async Task ViewSalesCheaperThenPrice()
        {
            decimal price = Helper.GetDecimalInput("Please put the price you want to view");
            var sales = await saleApiClient.GetSalesCheaperThenPrice(price);
            if (sales is null || sales.Count == 0)
            {
                Console.WriteLine("No sales found");
                return;
            }

            TableVisualisation.ShowSales(sales);
        }


        internal static async Task ViewSalesOrderedByDate()
        {
            int pageNumber = 1;
            int pageSize = 5;
            bool keepRunning = true;

            while (keepRunning)
            {
                var response = await saleApiClient.GetSalesSortedByDate(pageNumber, pageSize);
                if (response is null)
                {
                    Console.WriteLine("Couldn't get sales");
                    return;
                }

                ViewAllSales(ref response, ref pageNumber, ref pageSize, ref keepRunning);
            }
        }

        internal static async Task ViewSalesOrderedByTotalPrice()
        {
            int pageNumber = 1;
            int pageSize = 5;
            bool keepRunning = true;

            while (keepRunning)
            {
                var response = await saleApiClient.GetSalesSortedByTotalPrice(pageNumber, pageSize);
                if (response is null)
                {
                    Console.WriteLine("Couldn't get sales");
                    return;
                }

                ViewAllSales(ref response, ref pageNumber, ref pageSize, ref keepRunning);
            }
        }

        internal static async Task ViewSalesWithCategoryName()
        {
            string name = ProductUIService.GetProductName("Please put the name of category you want to view sales on");
            var sales = await saleApiClient.GetSalesWithCategoryName(name);
            if (sales is null || sales.Count == 0)
            {
                Console.WriteLine("No sales found");
                return;
            }

            TableVisualisation.ShowSales(sales);
        }


        internal static async Task ViewSalesWithProductName()
        {
            string name = ProductUIService.GetProductName("Please put the name of product you want to view sales on");
            var sales = await saleApiClient.GetSalesWithProductName(name);
            if (sales is null || sales.Count == 0)
            {
                Console.WriteLine("No sales found");
                return;
            }

            TableVisualisation.ShowSales(sales);
        }


        internal static async Task ViewSaleWithId()
        {
            await ViewSalesOrderedByDate();
            int categoryId = Helper.GetIntInput("Please put the id of the sale you want to view");
            if (categoryId == 0) return;

            var sale = await saleApiClient.GetSaleById(categoryId);
            if (sale is null)
            {
                Console.WriteLine("No sale found");
                return;
            }

            TableVisualisation.ShowSale(sale);
        }
    }
}
