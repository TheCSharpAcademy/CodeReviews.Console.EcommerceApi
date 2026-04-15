namespace Ecommerce.Core.Utilities;

public record PaginationParams
{
    private const int DefaultPageSize = 50;

    public int PageNumber
    {
        get;
        init => 
            field = value > 0 ? value : 1;
    } = 1;

    public int PageSize
    {
        get;
        init => 
            field = value > DefaultPageSize ? DefaultPageSize : value;
    } = 10;
}