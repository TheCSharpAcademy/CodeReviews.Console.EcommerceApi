using ECommerce.UI.Enums;
using static ECommerce.UI.Helpers.DisplayHelper;

namespace ECommerce.UI.UserInterface.TestingUi;

internal class TestingUi(ProductTestUi testingUi, CheckoutUi checkoutUi)
{
    internal async Task TestingMenu()
    {
        while (true)
        {
            Console.Clear();

            var option = DisplayMenu<TestingMenuOption>();

            switch (option)
            {
                case TestingMenuOption.BrowseProducts:
                    await testingUi.ReviewProductsMenu();
                    break;
                case TestingMenuOption.SearchProducts:
                    await testingUi.SearchProducts();
                    break;
                case TestingMenuOption.Checkout:
                    await checkoutUi.CheckoutMenu();
                    break;
                case TestingMenuOption.ExitTestingEnvironment:
                    return;
            }
        }
    }
}