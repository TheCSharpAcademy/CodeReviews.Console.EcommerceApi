using ECommerce.Shared.Models;
using ECommerce.UI.Enums;
using ECommerce.UI.Helpers;
using ECommerce.UI.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;
using static ECommerce.UI.Helpers.DisplayHelper;

namespace ECommerce.UI.UserInterface.TestingUi;

internal class CheckoutUi(ICartService cartService)
{
    internal async Task AddItemToCartAsync(PagedResponse<ItemDto> response)
    {
        Console.Clear();

        Dictionary<string, ItemDto> items = new();
        foreach (var item in response.Data) items.Add($"{item.Name} - {item.Artist} - ${item.Price}", item);

        var options = DisplayMultiPrompt(items.Keys.ToList(), requireChoice: false);

        if (options.Count > 0)
            foreach (var option in options)
                await cartService.AddToCartAsync(items[option]);

        DisplaySuccess("Successfully added items to cart.");
        UiHelper.WaitForUser();
    }

    internal async Task CheckoutMenu()
    {
        while (true)
        {
            Console.Clear();

            var cart = await cartService.GetCartAsync();

            DisplayMessage("Current cart:");

            if (cart.Count == 0)
            {
                DisplayWarning("There are no items in your cart, please add some and try again.");
                UiHelper.WaitForUser();
                return;
            }

            DisplayRows(BuildItemDtoRenderable(cart));

            var option = DisplayMenu<CheckoutMenu>();

            switch (option)
            {
                case Enums.CheckoutMenu.CheckoutItems:
                    await CheckOut();
                    break;
                case Enums.CheckoutMenu.RemoveItem:
                    await RemoveItemsAsync(cart);
                    break;
                case Enums.CheckoutMenu.ClearAllItems:
                    await ClearCartAsync();
                    break;
                case Enums.CheckoutMenu.Back:
                    return;
            }
        }
    }

    private async Task RemoveItemsAsync(List<ItemDto> items)
    {
        var itemList = BuildItemDtoStringList(items);
        Dictionary<string, ItemDto> itemDictionary = new();

        for (var i = 0; i < items.Count; i++) itemDictionary.Add(itemList[i], items[i]);

        var options = DisplayMultiPrompt(itemDictionary.Keys.ToList(), "Please choose the item(s) to remove:", false);

        foreach (var option in options) await cartService.RemoveFromCartAsync(itemDictionary[option].ItemId);

        DisplaySuccess("Successfully removed items from cart.");
        UiHelper.WaitForUser();
    }

    private async Task ClearCartAsync()
    {
        if (await AnsiConsole.ConfirmAsync("Are you sure you want to delete all items in your cart?"))
        {
            cartService.ClearCart();
            DisplaySuccess("Successfully deleted the contents of your cart.");
        }
        else
        {
            DisplaySuccess("Deletion was cancelled.");
        }

        UiHelper.WaitForUser();
    }

    private async Task CheckOut()
    {
        const string fakeCardDetails = "************1112, expires: 12/2027";
        const string fakeEmailAddress = "******@gmail.com";

        Console.Clear();
        DisplayWarning(
            "No checkout process has currently been built, this is only a replication of what a checkout process might look like after development. Checking out will clear your cart afterwards.");

        if (await AnsiConsole.ConfirmAsync($"Would you like to checkout with the card: {fakeCardDetails}?"))
        {
            //await cartService.CheckoutCart(CardDetails card)

            cartService.ClearCart();

            DisplaySuccess(
                $"Cart was successfully checked out, please check for an order confirmation email sent to your email: {fakeEmailAddress}");
        }

        UiHelper.WaitForUser();
    }

    //------- Helper Methods -------
    private List<IRenderable> BuildItemDtoRenderable(List<ItemDto> items)
    {
        List<IRenderable> iRenderables = [];

        foreach (var item in items)
            iRenderables.Add(new Markup($"[{White}]{item.Name} - {item.Artist}[/][{Grey}] - {item.Price}[/]"));

        return iRenderables;
    }

    private List<string> BuildItemDtoStringList(List<ItemDto> items)
    {
        List<string> itemsAsStrings = [];

        foreach (var item in items)
            itemsAsStrings.Add($"[{White}]{item.Name} - {item.Artist}[/][{Grey}] - {item.Price}[/]");

        return itemsAsStrings;
    }
}