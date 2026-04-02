using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Common.Results;
using ECommerce.Shared.TerrenceLGee.DTOs.ProductDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;

namespace ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;

public interface IProductService
{
    Task<Result<RetrievedProductForAdminDto?>> AddProductAsync(CreateProductDto product);
    Task<Result<RetrievedProductForAdminDto?>> UpdateProductAsync(UpdateProductDto product);
    Task<Result> DeleteProductAsync(ProductParams productParams);
    Task<Result> RestoreProductAsync(ProductParams productParams);
    Task<Result<RetrievedProductDto?>> GetProductAsync(ProductParams productParams);
    Task<Result<RetrievedProductForAdminDto?>> GetProductForAdminAsync(ProductParams productParams);
    Task<Result<PagedList<RetrievedProductDto>>> GetProductsAsync(ProductQueryParams productQueryParams);
    Task<Result<PagedList<RetrievedProductForAdminDto>>> GetProductsForAdminAsync(ProductQueryParams productQueryParams);
}