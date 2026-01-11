using ECommerceApp.ConsoleClient.Utilities;
using Spectre.Console;
using Spectre.Console.Testing;
using Xunit;

namespace ECommerceApp.UnitTests.ConsoleClient;

[Collection(SpectreConsoleCollection.Name)]
public class TableRendererDynamicPagingTests
{
    private sealed record Item(int Id, string Name);

    private static TestConsole CreateInteractiveTestConsole()
    {
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        return console;
    }

    [Fact]
    public async Task SelectFromPromptAsync_NextPage_ThenSelectItem_ReturnsItemFromSecondPage()
    {
        var items = Enumerable.Range(1, 40).Select(i => new Item(i, $"Item {i}")).ToList();
        const int pageSize = 32;

        Task<List<Item>> FetchPageAsync(int pageNum)
        {
            return Task.FromResult(items.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList());
        }

        var originalConsole = AnsiConsole.Console;
        var testConsole = CreateInteractiveTestConsole();
        AnsiConsole.Console = testConsole;

        try
        {
            // Page 1 choices:
            // 32 items + "---" + "Next Page >" + "Cancel".
            // Navigate to "Next Page >" (index 33) and select it.
            for (var i = 0; i < 33; i++)
                testConsole.Input.PushKey(ConsoleKey.DownArrow);
            testConsole.Input.PushKey(ConsoleKey.Enter);

            // Page 2: select the 3rd item on the page (global item 35).
            testConsole.Input.PushKey(ConsoleKey.DownArrow);
            testConsole.Input.PushKey(ConsoleKey.DownArrow);
            testConsole.Input.PushKey(ConsoleKey.Enter);

            var selected = await TableRenderer.SelectFromPromptAsync(
                FetchPageAsync,
                totalCount: items.Count,
                pageSize: pageSize,
                title: "Select an Item",
                displayFormatter: x => x.Name
            );

            Assert.NotNull(selected);
            Assert.Equal(35, selected!.Id);
        }
        finally
        {
            AnsiConsole.Console = originalConsole;
        }
    }

    [Fact]
    public async Task SelectFromPromptAsync_Cancel_ReturnsNull()
    {
        var items = Enumerable.Range(1, 10).Select(i => new Item(i, $"Item {i}")).ToList();

        Task<List<Item>> FetchPageAsync(int pageNum) => Task.FromResult(items);

        var originalConsole = AnsiConsole.Console;
        var testConsole = CreateInteractiveTestConsole();
        AnsiConsole.Console = testConsole;

        try
        {
            // With 10 items, choices are: 10 items + "Cancel".
            // Move down to "Cancel" and select it.
            for (var i = 0; i < 10; i++)
                testConsole.Input.PushKey(ConsoleKey.DownArrow);
            testConsole.Input.PushKey(ConsoleKey.Enter);

            var selected = await TableRenderer.SelectFromPromptAsync(
                FetchPageAsync,
                totalCount: items.Count,
                pageSize: 32,
                title: "Select an Item",
                displayFormatter: x => x.Name
            );

            Assert.Null(selected);
        }
        finally
        {
            AnsiConsole.Console = originalConsole;
        }
    }

    [Fact]
    public async Task SelectFromPromptAsync_PreviousPageNavigation_Works()
    {
        var items = Enumerable.Range(1, 40).Select(i => new Item(i, $"Item {i}")).ToList();
        const int pageSize = 32;

        Task<List<Item>> FetchPageAsync(int pageNum)
        {
            return Task.FromResult(items.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList());
        }

        var originalConsole = AnsiConsole.Console;
        var testConsole = CreateInteractiveTestConsole();
        AnsiConsole.Console = testConsole;

        try
        {
            // Go to page 2 first.
            for (var i = 0; i < 33; i++)
                testConsole.Input.PushKey(ConsoleKey.DownArrow);
            testConsole.Input.PushKey(ConsoleKey.Enter);

            // Page 2 choices:
            // 8 items + "---" + "< Previous Page" + "Cancel".
            // Navigate to "< Previous Page" (index 9) and select it.
            for (var i = 0; i < 9; i++)
                testConsole.Input.PushKey(ConsoleKey.DownArrow);
            testConsole.Input.PushKey(ConsoleKey.Enter);

            // Back on page 1: select first item.
            // Ensure we're at the first choice before selecting.
            for (var i = 0; i < 50; i++)
                testConsole.Input.PushKey(ConsoleKey.UpArrow);
            testConsole.Input.PushKey(ConsoleKey.Enter);

            var selected = await TableRenderer.SelectFromPromptAsync(
                FetchPageAsync,
                totalCount: items.Count,
                pageSize: pageSize,
                title: "Select an Item",
                displayFormatter: x => x.Name
            );

            Assert.NotNull(selected);
            Assert.Equal(1, selected!.Id);
        }
        finally
        {
            AnsiConsole.Console = originalConsole;
        }
    }

    [Fact]
    public async Task SelectFromPromptAsync_WhenFetchedPageIsEmpty_ReturnsNull()
    {
        Task<List<Item>> FetchPageAsync(int _) => Task.FromResult(new List<Item>());

        var originalConsole = AnsiConsole.Console;
        var testConsole = CreateInteractiveTestConsole();
        AnsiConsole.Console = testConsole;

        try
        {
            var selected = await TableRenderer.SelectFromPromptAsync(
                FetchPageAsync,
                totalCount: 10,
                pageSize: 32,
                title: "Select an Item",
                displayFormatter: x => x.Name
            );

            Assert.Null(selected);
        }
        finally
        {
            AnsiConsole.Console = originalConsole;
        }
    }
}
