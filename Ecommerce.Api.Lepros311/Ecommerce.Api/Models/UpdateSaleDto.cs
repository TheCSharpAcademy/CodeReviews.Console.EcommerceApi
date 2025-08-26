namespace Ecommerce.Api.Models;

public class UpdateSaleDto
{
    public List<UpdateLineItemOnSaleDto> LineItems { get; set; } = new List<UpdateLineItemOnSaleDto>();
}
