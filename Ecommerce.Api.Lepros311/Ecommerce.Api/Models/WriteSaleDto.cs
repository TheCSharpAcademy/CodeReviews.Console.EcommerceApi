namespace Ecommerce.Api.Models;

public class WriteSaleDto
{
    public List<WriteLineItemOnSaleDto> LineItems { get; set; } = new List<WriteLineItemOnSaleDto>();
}
