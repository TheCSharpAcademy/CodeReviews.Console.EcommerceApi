using ECommerce.Contracts.TerrenceLGee.Common.Pagination;

namespace ECommerce.Contracts.TerrenceLGee.Common.Extensions;

public static class EnumerableExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        public PagedList<T> ToPagedList(int count, int page, int pageSize)
        {
            if (count > 0)
            {
                var items = source.ToList();

                return new PagedList<T>(items, count, page, pageSize);
            }

            return new([], 0, 0, 0);
        }
    }
}