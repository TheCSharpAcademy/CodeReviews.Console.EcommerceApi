namespace EcommerceAPIDemo.Controllers;

public class GameProductFilterParams
{
    public int? CategoryId { get; set; }
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
}

public class SalesFilterParams
{
    public int? GameProductId { get; set; }
    public int? LastFourOfPaymentCard {  get; set; }
    public DateOnly? TransactionDate { get; set; }
    public double? MinTransactionValue { get; set; }
    public double? MaxTransactionValue { get; set; }
}

