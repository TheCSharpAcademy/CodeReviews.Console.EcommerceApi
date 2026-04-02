namespace ECommerce.Shared.TerrenceLGee.Parameters.SaleParameters;

public class SaleQueryParams : QueryStringParams
{
    public decimal? MinTotalAmount { get; set; }
    public decimal? MaxTotalAmount { get; set; }
    public string? Status { get; set; }
    public bool IsValidAmountRange => MaxTotalAmount >= MinTotalAmount;
}