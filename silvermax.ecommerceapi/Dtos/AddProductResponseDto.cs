using silvermax.ecommerceapi.Models;

namespace silvermax.ecommerceapi.Dtos;

public record AddProductResponseDto(ProductResponseDto? Product, AddProductError? Error);
