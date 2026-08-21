namespace silvermax.ecommerceapi.Dtos;

public record ProductResponseDto(
    Guid ProductId,
    string Name,
    string? Description,
    double Price,
    string? ImgUrl,
    Guid CategoryId,
    string CategoryName
    );
