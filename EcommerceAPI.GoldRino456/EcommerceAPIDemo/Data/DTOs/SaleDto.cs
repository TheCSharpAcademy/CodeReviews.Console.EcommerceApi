using EcommerceAPIDemo.Data.Models;

namespace EcommerceAPIDemo.Data.DTOs;

public class SaleDto
{
    public List<int>? PurchasedGameIds { get; set; }
    public CreditCardTypes CreditCardType { get; set; }
    public int LastFourDigitsOfPaymentCard { get; set; }
    public double SubTotal { get; set; }
    public double SalesTax { get; set; }
    public double Total { get; set; }
}
