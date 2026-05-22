using ECommerceUI.Ledana.Controllers;
using Spectre.Console;
using static ECommerceUI.Ledana.Enums;

namespace ECommerceUI.Ledana.UI
{
    internal class UserInterface
    {
        internal async Task MainMenu()
        {
            Console.WriteLine("Welcome to our app!");

            bool isRunning = true;
            while(isRunning)
            {
                Console.Clear();
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<MainMenuOptions>()
                    .Title("What do you want to do?")
                    .AddChoices(
                        MainMenuOptions.ProductsMenu,
                        MainMenuOptions.CategoriesMenu,
                        MainMenuOptions.SalesMenu,
                        MainMenuOptions.Quit
                        ));

                switch(option)
                {
                    case MainMenuOptions.ProductsMenu:
                        await ProductsMenu();
                        break;
                    case MainMenuOptions.CategoriesMenu:
                        await CategoriesMenu();
                        break;
                    case MainMenuOptions.SalesMenu:
                        await SalesMenu();
                        break;
                    case MainMenuOptions.Quit:
                        Console.WriteLine("Good bye");
                        isRunning = false;
                        break;
                }               
            }
        }

        private async Task SalesMenu()
        {
            bool isRunning = true;
            while (isRunning)
            {
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<SalesMenuOptions>()
                    .Title("What do you want to do?")
                    .AddChoices(
                        SalesMenuOptions.ViewSalesMenu,
                        SalesMenuOptions.AddNewSale,
                        SalesMenuOptions.GoBack
                    ));

                switch (option)
                {
                    case SalesMenuOptions.ViewSalesMenu:
                        await ViewSalesMenu();
                        break;
                    case SalesMenuOptions.AddNewSale:
                        await SaleUIController.AddNewSale();
                        break;;
                    case SalesMenuOptions.GoBack:
                        isRunning = false;
                        break;
                }
            }
        }

        private async Task ViewSalesMenu()
        {
            bool isRunning = true;
            while (isRunning)
            {
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<ViewSalesMenuOptions>()
                    .Title("What do you want to do?")
                    .AddChoices(
                        ViewSalesMenuOptions.ViewSalesOrderedByDate,
                        ViewSalesMenuOptions.ViewSalesOrderedByTotalPrice,
                        ViewSalesMenuOptions.ViewSaleWithId,
                        ViewSalesMenuOptions.ViewSalesWithProductName,
                        ViewSalesMenuOptions.ViewSalesWithCategoryName,
                        ViewSalesMenuOptions.ViewSalesCheaperThenPrice,
                        ViewSalesMenuOptions.GoBack
                    ));

                switch (option)
                {
                    case ViewSalesMenuOptions.ViewSalesOrderedByDate:
                        await SaleUIController.ViewSalesOrderedByDate();
                        break;
                    case ViewSalesMenuOptions.ViewSalesOrderedByTotalPrice:
                        await SaleUIController.ViewSalesOrderedByTotalPrice();
                        break;
                    case ViewSalesMenuOptions.ViewSaleWithId:
                        await SaleUIController.ViewSaleWithId();
                        break;
                    case ViewSalesMenuOptions.ViewSalesWithProductName:
                        await SaleUIController.ViewSalesWithProductName();
                        break;
                    case ViewSalesMenuOptions.ViewSalesWithCategoryName:
                        await SaleUIController.ViewSalesWithCategoryName();
                        break;
                    
                    case ViewSalesMenuOptions.ViewSalesCheaperThenPrice:
                        await SaleUIController.ViewSalesCheaperThenPrice();
                        break;
                    
                    case ViewSalesMenuOptions.GoBack:
                        isRunning = false;
                        break;
                }
            }
        }

        private async Task CategoriesMenu()
        {
            bool isRunning = true;
            while (isRunning)
            {
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<CategoriesMenuOptions>()
                    .Title("What do you want to do?")
                    .AddChoices(
                        CategoriesMenuOptions.ViewCategories,
                        CategoriesMenuOptions.ViewCategoryById,
                        CategoriesMenuOptions.AddNewCategory,
                        CategoriesMenuOptions.UpdateCategory,
                        CategoriesMenuOptions.DeleteCategory,
                        CategoriesMenuOptions.GoBack
                    ));

                switch (option)
                {
                    case CategoriesMenuOptions.ViewCategories:
                        await CategoryUIController.ViewCategories();
                        break;
                    case CategoriesMenuOptions.ViewCategoryById:
                        await CategoryUIController.ViewCategoryById();
                        break;
                    case CategoriesMenuOptions.AddNewCategory:
                        await CategoryUIController.AddNewCategory();
                        break;
                    case CategoriesMenuOptions.UpdateCategory:
                        await CategoryUIController.UpdateCategory();
                        break;
                    case CategoriesMenuOptions.DeleteCategory:
                        await CategoryUIController.DeleteCategory();
                        break;
                    case CategoriesMenuOptions.GoBack:
                        isRunning = false;
                        break;
                }
            }
        }

        private async Task ProductsMenu()
        {
            bool isRunning = true;
            while (isRunning)
            {
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<ProductsMenuOptions>()
                    .Title("What do you want to do?")
                    .AddChoices(
                        ProductsMenuOptions.ViewProductsMenu,
                        ProductsMenuOptions.AddNewProduct,
                        ProductsMenuOptions.UpdateProduct,
                        ProductsMenuOptions.DeleteProduct,
                        ProductsMenuOptions.GoBack
                    ));

                switch (option)
                {
                    case ProductsMenuOptions.ViewProductsMenu:
                        await ViewProductsMenu();
                        break;
                    case ProductsMenuOptions.AddNewProduct:
                        await ProductUIController.AddNewProduct();
                        break;
                    case ProductsMenuOptions.UpdateProduct:
                        await ProductUIController.UpdateProduct();
                        break;
                    case ProductsMenuOptions.DeleteProduct:
                        await ProductUIController.DeleteProduct();
                        break;
                    case ProductsMenuOptions.GoBack:
                        isRunning = false;
                        break;
                }
            }
        }

        private async Task ViewProductsMenu()
        {
            bool isRunning = true;
            while (isRunning)
            {
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<ViewProductsMenuOptions>()
                    .Title("What do you want to do?")
                    .AddChoices(
                        ViewProductsMenuOptions.ViewAllProductsOrderedById,
                        ViewProductsMenuOptions.ViewAllProductsOrderedByPrice,
                        ViewProductsMenuOptions.ViewAllProductsOrderedByStock,
                        ViewProductsMenuOptions.ViewProductById,
                        ViewProductsMenuOptions.ViewProductsByName,
                        ViewProductsMenuOptions.ViewProductsCheaperThenPrice,
                        ViewProductsMenuOptions.ViewProductsLowerThenStock,
                        ViewProductsMenuOptions.GoBack
                    ));

                switch (option)
                {
                    case ViewProductsMenuOptions.ViewAllProductsOrderedById:
                        await ProductUIController.ViewAllProductsOrderedById();
                        break;
                    case ViewProductsMenuOptions.ViewAllProductsOrderedByPrice:
                        await ProductUIController.ViewAllProductsOrderedByPrice();
                        break;
                    case ViewProductsMenuOptions.ViewAllProductsOrderedByStock:
                        await ProductUIController.ViewAllProductsOrderedByStock();
                        break;
                    case ViewProductsMenuOptions.ViewProductById:
                        await ProductUIController.ViewProductById();
                        break;
                    case ViewProductsMenuOptions.ViewProductsByName:
                        await ProductUIController.ViewProductsByName();
                        break;
                    case ViewProductsMenuOptions.ViewProductsCheaperThenPrice:
                        await ProductUIController.ViewProductsCheaperThenPrice();
                        break;
                    case ViewProductsMenuOptions.ViewProductsLowerThenStock:
                        await ProductUIController.ViewProductsLowerThenStock();
                        break;
                    case ViewProductsMenuOptions.GoBack:
                        isRunning = false;
                        break;
                }
            }
        }
    }
}
