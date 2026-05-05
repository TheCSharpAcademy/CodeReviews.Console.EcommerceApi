

namespace Sills.GolfShop.eCommerceFrontEnd.Helpers;

internal class Pagination
{
    internal static string GetPaginationQuery(int pageNumber, int pageSize)
    {
        return $"?pageNumber={pageNumber}&pageSize={pageSize}";
    }
}
