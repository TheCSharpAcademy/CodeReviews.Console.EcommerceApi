using silvermax.ecommerceapi.Dtos;
using silvermax.ecommerceapi.Models;
using silvermax.ecommerceapi.Pagination;

namespace silvermax.ecommerceapi.Service;

public interface IEcommerceService
{
    public Task<CategoryResponse?> AddCategory(AddCategoryDto dto, CancellationToken ct);
    public Task<AddProductResponseDto> AddProduct(AddProductDto dto, CancellationToken ct);
    public Task<Client?> CreateClient(CreateClientDto dto, CancellationToken ct);
    public Task<PlaceOrderResponseDto> PlaceOrder(PlaceOrderDto dto, CancellationToken ct);
    public Task<PagedList<OrderResponse>> GetOrders(PageParameters pageParameters, CancellationToken ct);
    public Task<PagedList<ProductResponseDto>> GetProducts(PageParameters pageParameters, CancellationToken ct);
}
