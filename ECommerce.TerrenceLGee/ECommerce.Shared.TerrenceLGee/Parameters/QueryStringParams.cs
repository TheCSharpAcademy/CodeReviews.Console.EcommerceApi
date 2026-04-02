namespace ECommerce.Shared.TerrenceLGee.Parameters;

public abstract class QueryStringParams : BaseParams
{
    const int MaxPageSize = 50;
    public int Page { get; set; } = 1;
    private int _pageSize = 10;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
    public string? OrderBy { get; set; } = "Id";
}