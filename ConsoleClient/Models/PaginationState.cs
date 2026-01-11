namespace ECommerceApp.ConsoleClient.Models;

/// <summary>
/// Represents the state of a paginated view for navigation purposes.
/// </summary>
public record PaginationState
{
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }
    public bool HasNextPage => CurrentPage * PageSize < TotalCount;
    public bool HasPreviousPage => CurrentPage > 1;
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
    public int IndexOffset => (CurrentPage - 1) * PageSize;

    /// <summary>
    /// Gets the navigation choices available for the current pagination state.
    /// </summary>
    public List<string> GetNavigationChoices()
    {
        var choices = new List<string>();

        if (HasPreviousPage)
            choices.Add("Previous Page");

        if (HasNextPage)
            choices.Add("Next Page");

        choices.Add("Jump to Page");
        choices.Add("Back to Menu");

        return choices;
    }
}
