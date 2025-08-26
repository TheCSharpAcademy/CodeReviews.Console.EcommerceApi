namespace EcommerceAPIDemo.Data.Models;

public class Sale
{
    public int Id { get; set; }
    public DateTime TransactionDate { get; set; }
    public DateTime TransactionLastUpdatedDate { get; set; }
    public bool IsRefund { get; set; }
    public bool IsPartialRefund { get; set; }
    public List<GameProduct> GamesPurchased { get; } = [];
    public CreditCardTypes CreditCardType { get; set; }
    public int LastFourDigitsOfPaymentCard { get; set; }
    public double SubTotal { get; set; }
    public double SalesTax { get; set; }
    public double Total { get; set; }
    public double ActualTransactionValue { get; set; }
}

public enum CreditCardTypes
{
    AmericanExpress,
    Visa,
    MasterCard,
    Discover
}
