using System.Collections;

namespace ECommerce.Contracts.TerrenceLGee.Common.Pagination;

public class PagedList<T> : IReadOnlyList<T>
{
    private readonly IList<T> _subset;
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalEntities { get; }
    public bool IsFirstPage => PageNumber == 1;
    public bool IsLastPage => PageNumber == TotalPages;
    public int Count => _subset is null ? 0 : _subset.Count();
    public T this[int index] => _subset[index];
    public IEnumerator<T> GetEnumerator() => _subset.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _subset.GetEnumerator();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public PagedList() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public PagedList(
        IEnumerable<T> items,
        int count,
        int pageNumber,
        int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalEntities = count;
        _subset = items as IList<T> ?? new List<T>(items);
    }
}