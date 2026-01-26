namespace kilozdazolik.Ecommerce.API.Features.Sales
{

    public record CreateSaleDto (
        List<CreateSaleItemDto> Items
        );

    public record CreateSaleItemDto(
        Guid ProductId,
        byte Quantity
        );


    public record SaleDto(
        Guid Id,
        DateTime Date,
        decimal TotalAmount,
        List<SaleDetailsDto> Items
        );

    public record SaleDetailsDto(
        Guid ProductId,
        string ProductName, 
        byte Quantity,
        decimal UnitPrice,     
        decimal LineTotal
        );
}
