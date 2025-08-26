namespace Ecommerce.Api.Models;

public class UpdateLineItemOnSaleDto
{
    public int? LineItemId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }
}
