namespace ECommerce.Shared.TerrenceLGee.Parameters.CustomerParameters;

public class CustomerQueryParams : QueryStringParams
{
    public int? MinSaleCount { get; set; }
    public int? MaxSaleCount { get; set; }
    public decimal? MinTotalSpent { get; set; }
    public decimal? MaxTotalSpent { get; set; }

    public bool IsValidSaleCountRange => MinSaleCount <= MaxSaleCount;
    public bool IsValidTotalSpentRange => MinTotalSpent <= MaxTotalSpent;
}