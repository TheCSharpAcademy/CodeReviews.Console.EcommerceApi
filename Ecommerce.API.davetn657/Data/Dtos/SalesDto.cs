namespace Ecommerce.API.davetn657.Data.Dtos;

public class SalesDto
{
    public int Id { get; set; }
    public List<int> ProductIds { get; set; } = [];
    public decimal Total { get; set; }
}

public class CreateSalesDto
{
    public List<int> ProductIds { get; set; } = [];
}

public class SalesResponseDto
{
    public int Id { get; set; }
    public decimal Total { get; set; }
    public List<ProductsSalesDto> Products { get; set; } = [];
}