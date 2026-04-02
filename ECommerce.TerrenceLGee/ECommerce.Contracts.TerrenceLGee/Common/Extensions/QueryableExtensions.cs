using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Contracts.TerrenceLGee.Common.Extensions;

public static class QueryableExtensions
{
    extension<T>(IQueryable<T> source)
    {
        public async Task<PagedList<T>> ToPagedListAsync(int count, int page, int pageSize)
        {
            if (count > 0)
            {
                var items = await source
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PagedList<T>(items, count, page, pageSize);
            }

            return new([], 0, 0, 0);
        }
    }
}