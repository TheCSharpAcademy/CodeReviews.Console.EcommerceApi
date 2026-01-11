using ECommerceApp.ConsoleClient.Utilities;
using Spectre.Console;
using Spectre.Console.Testing;
using Xunit;

namespace ECommerceApp.UnitTests.ConsoleClient;

[Collection(SpectreConsoleCollection.Name)]
public class TableRendererDynamicPagingTheoryTests
{
    private sealed record Item(int Id, string Name);

    private static TestConsole CreateInteractiveTestConsole()
    {
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        return console;
    }

    private static int TotalPages(int totalCount, int pageSize) =>
        (totalCount + pageSize - 1) / pageSize;

    private static int ItemsOnPage(int totalCount, int pageSize, int pageIndex) =>
        Math.Max(0, Math.Min(pageSize, totalCount - (pageIndex * pageSize)));

    private static int CancelIndex(int itemCountOnPage, bool hasPrev, bool hasNext, bool showPaging)
    {
        var extras = showPaging ? 1 : 0; // "---"
        if (showPaging && hasPrev)
            extras++;
        if (showPaging && hasNext)
            extras++;
        return itemCountOnPage + extras;
    }

    private static int NextIndex(int itemCountOnPage, bool hasPrev, bool showPaging) =>
        itemCountOnPage + 1 + (showPaging && hasPrev ? 1 : 0);

    private static int PreviousIndex(int itemCountOnPage) => itemCountOnPage + 1; // after "---"

    private static async Task<(Item? Selected, List<int> PagesFetched)> RunSelectionAsync(
        int totalCount,
        int pageSize,
        Action<TestConsole> pushKeys,
        Func<int, List<Item>>? pageProvider = null
    )
    {
        var items = Enumerable.Range(1, totalCount).Select(i => new Item(i, $"Item {i}")).ToList();
        var fetched = new List<int>();

        Task<List<Item>> FetchPageAsync(int pageNum)
        {
            fetched.Add(pageNum);
            if (pageProvider != null)
                return Task.FromResult(pageProvider(pageNum));

            return Task.FromResult(items.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList());
        }

        var originalConsole = AnsiConsole.Console;
        var testConsole = CreateInteractiveTestConsole();
        AnsiConsole.Console = testConsole;

        try
        {
            pushKeys(testConsole);

            var selected = await TableRenderer.SelectFromPromptAsync(
                FetchPageAsync,
                totalCount: totalCount,
                pageSize: pageSize,
                title: "Select an Item",
                displayFormatter: x => x.Name
            );

            return (selected, fetched);
        }
        finally
        {
            AnsiConsole.Console = originalConsole;
        }
    }

    // 15 test cases
    [Theory]
    [InlineData(1, 32)]
    [InlineData(2, 32)]
    [InlineData(10, 32)]
    [InlineData(31, 32)]
    [InlineData(32, 32)]
    [InlineData(33, 32)]
    [InlineData(40, 32)]
    [InlineData(64, 32)]
    [InlineData(65, 32)]
    [InlineData(5, 5)]
    [InlineData(6, 5)]
    [InlineData(9, 5)]
    [InlineData(10, 5)]
    [InlineData(11, 5)]
    [InlineData(100, 10)]
    public async Task SelectFromPromptAsync_CancelAlways_ReturnsNull(int totalCount, int pageSize)
    {
        var totalPages = TotalPages(totalCount, pageSize);
        var showPaging = totalPages > 1;
        var itemCount = ItemsOnPage(totalCount, pageSize, pageIndex: 0);

        var cancelIndex = CancelIndex(
            itemCount,
            hasPrev: false,
            hasNext: showPaging,
            showPaging: showPaging
        );

        var (selected, pages) = await RunSelectionAsync(
            totalCount,
            pageSize,
            console =>
            {
                for (var i = 0; i < cancelIndex; i++)
                    console.Input.PushKey(ConsoleKey.DownArrow);
                console.Input.PushKey(ConsoleKey.Enter);
            }
        );

        Assert.Null(selected);
        Assert.Equal(new[] { 1 }, pages);
    }

    // 10 test cases
    [Theory]
    [InlineData(33, 32)]
    [InlineData(40, 32)]
    [InlineData(63, 32)]
    [InlineData(64, 32)]
    [InlineData(65, 32)]
    [InlineData(6, 5)]
    [InlineData(9, 5)]
    [InlineData(11, 5)]
    [InlineData(21, 10)]
    [InlineData(101, 10)]
    public async Task SelectFromPromptAsync_NextPageThenCancel_FetchesSecondPage(
        int totalCount,
        int pageSize
    )
    {
        var totalPages = TotalPages(totalCount, pageSize);
        Assert.True(totalPages >= 2);

        var page1Items = ItemsOnPage(totalCount, pageSize, pageIndex: 0);
        var nextIndex = NextIndex(page1Items, hasPrev: false, showPaging: true);

        var page2Items = ItemsOnPage(totalCount, pageSize, pageIndex: 1);
        var page2CancelIndex = CancelIndex(
            page2Items,
            hasPrev: true,
            hasNext: totalPages > 2,
            showPaging: true
        );

        var (selected, pages) = await RunSelectionAsync(
            totalCount,
            pageSize,
            console =>
            {
                // select "Next Page >"
                for (var i = 0; i < nextIndex; i++)
                    console.Input.PushKey(ConsoleKey.DownArrow);
                console.Input.PushKey(ConsoleKey.Enter);

                // then cancel on page 2
                for (var i = 0; i < page2CancelIndex; i++)
                    console.Input.PushKey(ConsoleKey.DownArrow);
                console.Input.PushKey(ConsoleKey.Enter);
            }
        );

        Assert.Null(selected);
        Assert.Equal(new[] { 1, 2 }, pages);
    }

    // 15 test cases
    [Theory]
    // totalCount, pageSize, targetPageIndex (0-based), localIndexOnPage (0-based), expectedId
    [InlineData(40, 32, 1, 0, 33)]
    [InlineData(40, 32, 1, 7, 40)]
    [InlineData(65, 32, 1, 0, 33)]
    [InlineData(65, 32, 1, 31, 64)]
    [InlineData(65, 32, 2, 0, 65)]
    [InlineData(21, 10, 0, 0, 1)]
    [InlineData(21, 10, 0, 9, 10)]
    [InlineData(21, 10, 1, 0, 11)]
    [InlineData(21, 10, 1, 9, 20)]
    [InlineData(21, 10, 2, 0, 21)]
    [InlineData(11, 5, 0, 4, 5)]
    [InlineData(11, 5, 1, 0, 6)]
    [InlineData(11, 5, 1, 4, 10)]
    [InlineData(11, 5, 2, 0, 11)]
    [InlineData(100, 10, 9, 9, 100)]
    public async Task SelectFromPromptAsync_NavigateToPageAndSelect_ReturnsCorrectItem(
        int totalCount,
        int pageSize,
        int targetPageIndex,
        int localIndexOnPage,
        int expectedId
    )
    {
        var totalPages = TotalPages(totalCount, pageSize);
        Assert.InRange(targetPageIndex, 0, totalPages - 1);

        var (selected, pages) = await RunSelectionAsync(
            totalCount,
            pageSize,
            console =>
            {
                // Navigate to target page by selecting "Next Page >" repeatedly.
                for (
                    var currentPageIndex = 0;
                    currentPageIndex < targetPageIndex;
                    currentPageIndex++
                )
                {
                    var count = ItemsOnPage(totalCount, pageSize, currentPageIndex);
                    var nextIndex = NextIndex(
                        count,
                        hasPrev: currentPageIndex > 0,
                        showPaging: totalPages > 1
                    );

                    for (var i = 0; i < nextIndex; i++)
                        console.Input.PushKey(ConsoleKey.DownArrow);
                    console.Input.PushKey(ConsoleKey.Enter);
                }

                // Select the desired item on the target page.
                for (var i = 0; i < localIndexOnPage; i++)
                    console.Input.PushKey(ConsoleKey.DownArrow);
                console.Input.PushKey(ConsoleKey.Enter);
            }
        );

        Assert.NotNull(selected);
        Assert.Equal(expectedId, selected!.Id);

        // Sanity: pages fetched should start at 1 and include target page.
        Assert.True(pages.Count >= 1);
        Assert.Equal(1, pages[0]);
        Assert.Contains(targetPageIndex + 1, pages);
    }

    // 10 test cases
    [Theory]
    // totalCount, pageSize, selectLocalIndexOnPage1, expectedId
    [InlineData(40, 32, 0, 1)]
    [InlineData(40, 32, 5, 6)]
    [InlineData(65, 32, 10, 11)]
    [InlineData(65, 32, 31, 32)]
    [InlineData(21, 10, 0, 1)]
    [InlineData(21, 10, 9, 10)]
    [InlineData(11, 5, 0, 1)]
    [InlineData(11, 5, 4, 5)]
    [InlineData(101, 10, 0, 1)]
    [InlineData(101, 10, 9, 10)]
    public async Task SelectFromPromptAsync_GoToPage2ThenPrevious_ThenSelectOnPage1_ReturnsCorrectItem(
        int totalCount,
        int pageSize,
        int selectLocalIndexOnPage1,
        int expectedId
    )
    {
        var totalPages = TotalPages(totalCount, pageSize);
        Assert.True(totalPages >= 2);

        var (selected, pages) = await RunSelectionAsync(
            totalCount,
            pageSize,
            console =>
            {
                // Go to page 2
                var page1Items = ItemsOnPage(totalCount, pageSize, 0);
                var nextIndex = NextIndex(page1Items, hasPrev: false, showPaging: true);
                for (var i = 0; i < nextIndex; i++)
                    console.Input.PushKey(ConsoleKey.DownArrow);
                console.Input.PushKey(ConsoleKey.Enter);

                // Back to page 1 via "< Previous Page"
                var page2Items = ItemsOnPage(totalCount, pageSize, 1);
                var prevIndex = PreviousIndex(page2Items);
                for (var i = 0; i < prevIndex; i++)
                    console.Input.PushKey(ConsoleKey.DownArrow);
                console.Input.PushKey(ConsoleKey.Enter);

                // Select an item on page 1
                for (var i = 0; i < selectLocalIndexOnPage1; i++)
                    console.Input.PushKey(ConsoleKey.DownArrow);
                console.Input.PushKey(ConsoleKey.Enter);
            }
        );

        Assert.NotNull(selected);
        Assert.Equal(expectedId, selected!.Id);

        // Should have fetched page 1, then 2, then 1 again.
        Assert.Contains(1, pages);
        Assert.Contains(2, pages);
        Assert.True(pages.Count >= 3);
    }

    // Bonus correctness checks that also count as additional cases, if totalCount==0.
    // (Kept as a single Fact to avoid altering the 50-case accounting above.)
    [Fact]
    public async Task SelectFromPromptAsync_TotalCountZero_ReturnsNullWithoutFetching()
    {
        var (selected, pages) = await RunSelectionAsync(
            totalCount: 0,
            pageSize: 10,
            pushKeys: _ => { },
            pageProvider: _ => throw new InvalidOperationException("Should not fetch")
        );

        Assert.Null(selected);
        Assert.Empty(pages);
    }

    [Fact]
    public async Task SelectFromPromptAsync_WhenProviderReturnsEmptyForCurrentPage_ReturnsNull()
    {
        var (selected, pages) = await RunSelectionAsync(
            totalCount: 50,
            pageSize: 10,
            pushKeys: _ => { },
            pageProvider: _ => new List<Item>()
        );

        Assert.Null(selected);
        Assert.Equal(new[] { 1 }, pages);
    }
}
