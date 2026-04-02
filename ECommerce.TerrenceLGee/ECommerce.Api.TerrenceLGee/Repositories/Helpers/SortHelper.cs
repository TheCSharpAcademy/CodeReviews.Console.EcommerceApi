using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace ECommerce.Api.TerrenceLGee.Repositories.Helpers;

public static class SortHelper<T>
{
    public static IQueryable<T> ApplySorting(IQueryable<T> entities, string? orderByQueryString)
    {
        if (!entities.Any()) return entities;

        if (string.IsNullOrWhiteSpace(orderByQueryString)) return entities;

        var orderParams = orderByQueryString.Trim().Split(',');
        var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var orderQueryBuilder = new StringBuilder();

        foreach (var param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param)) continue;

            var propertyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertyInfos
                .FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName,
                StringComparison.InvariantCultureIgnoreCase));

            if (objectProperty is null) continue;

            var sortingOrder = param.EndsWith(" desc")
                ? "descending"
                : "ascending";

            orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
        }

        var orderQuery = orderQueryBuilder
            .ToString()
            .TrimEnd(',', ' ');

        return entities.OrderBy(orderQuery);
    }
}
