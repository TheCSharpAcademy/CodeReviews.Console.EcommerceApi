using EcommerceAPI.Ledana.DTOs;
using ECommerceUI.Ledana.Clients;
using ECommerceUI.Ledana.UI;
using Spectre.Console;

namespace ECommerceUI.Ledana.Services
{
    internal class SaleUIService
    {
        static ProductApiClient productApiClient = new();
        internal static async Task<List<SaleProductDto>?> GetProducts()
        {
            bool isRunning = AnsiConsole.Confirm("Do you want to add a product");
            List<SaleProductDto> saleProducts = [];
            int quantity;
            decimal discount;
            decimal totalPrice = 0m;
            var products = await productApiClient.GetProductsWithoutPagination();
            if(products is null)
            {
                Console.WriteLine("Validating products went wrong please close the app and try again");
                return null;
            }

            while (isRunning)
            {
                TableVisualisation.ShowAllProducts(products);
                int productId = Helper.GetIntInput("Please put the product id");
                var product = await productApiClient.GetProductById(productId);
                if(product is null)
                {
                    Console.WriteLine("Your chosen product is not found");
                    return null;
                }
                
                if (Helper.IsProductIdCorrect(productId, products))
                {
                    Console.WriteLine("\nPrice for product is " + product.Price);
                    quantity = Helper.GetIntInput("Please put the quantity");
                    while (quantity > product.Stock)
                    {
                        Console.WriteLine("Quantity is greater than stock of product");
                        quantity = Helper.GetIntInput("Please put the quantity");
                    }
                    if (quantity == 0) return [];

                    discount = Helper.GetDecimalInput("Please put the discount");
                }
                else
                {
                    Console.WriteLine("Input was not valid");
                    return null;
                }

                    SaleProductDto productDto = new()
                {
                    ProductsId = productId,
                    Quantity = quantity,
                    Discount = discount
                };
                saleProducts.Add(productDto);
                totalPrice += product.Price * (1 - discount);
                Console.WriteLine("Your total for now is " + totalPrice);
                isRunning = AnsiConsole.Confirm("Do you want to add a product?");
            }
            Console.WriteLine("\nTotal price is " + totalPrice);

            return saleProducts;
        }
    }
}
