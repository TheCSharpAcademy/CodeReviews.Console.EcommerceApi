using System.Net;
using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;
using ECommerceApp.RyanW84.Interfaces;
using ECommerceApp.RyanW84.Interfaces.Helpers;

namespace ECommerceApp.RyanW84.Services
{
    /// <summary>
    /// Service layer for managing product operations.
    /// Handles product creation, retrieval, updates, and deletion.
    /// Delegates validation to helpers and persistence to repositories.
    /// </summary>
    public class ProductService(
        IProductRepository productRepository,
        IProductProcessingHelper productProcessingHelper,
        IProductQueryHelper productQueryHelper
    ) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IProductProcessingHelper _productProcessingHelper = productProcessingHelper;
        private readonly IProductQueryHelper _productQueryHelper = productQueryHelper;

        /// <summary>
        /// Creates a new product in the database.
        /// </summary>
        /// <param name="request">The product creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created product or error response</returns>
        public async Task<ApiResponseDto<Product>> CreateProductAsync(
            ApiRequestDto<Product> request,
            CancellationToken cancellationToken = default
        )
        {
            var validation = _productProcessingHelper.ValidateCreateRequest(request);
            if (validation != null) return validation;

            try
            {
                ApiResponseDto<Product> result = await _productRepository.AddAsync(
                    request.Payload!,
                    cancellationToken
                );
                return result.RequestFailed
                    ? result
                    : ApiResponseDto<Product>.Success(result.Data, HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<Product>.Failure(
                    HttpStatusCode.InternalServerError,
                    $"Failed to create product: {ex.Message}"
                );
            }
        }

        public async Task<PaginatedResponseDto<List<Product>>> GetProductsAsync(
            ProductQueryParameters parameters,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                _productQueryHelper.NormalizePriceRange(parameters);

                PaginatedResponseDto<List<Product>> result =
                    await _productRepository.GetAllProductsAsync(parameters, cancellationToken);
                if (result.RequestFailed)
                    return PaginatedResponseDto<List<Product>>.Failure(
                        result.ResponseCode,
                        result.ErrorMessage,
                        parameters.Page,
                        parameters.PageSize
                    );

                return PaginatedResponseDto<List<Product>>.Success(
                    result.Data,
                    result.CurrentPage,
                    result.PageSize,
                    result.TotalCount
                );
            }
            catch (Exception ex)
            {
                return PaginatedResponseDto<List<Product>>.Failure(
                    HttpStatusCode.InternalServerError,
                    $"Failed to retrieve products: {ex.Message}",
                    parameters.Page,
                    parameters.PageSize
                );
            }
        }

        public async Task<ApiResponseDto<Product?>> GetProductByIdAsync(
            int id,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                ApiResponseDto<Product?> result = await _productRepository.GetByIdAsync(
                    id,
                    cancellationToken
                );
                if (result.RequestFailed)
                    return result;
                if (result.Data is null)
                    return ApiResponseDto<Product?>.Failure(HttpStatusCode.NotFound, "Product not found.");

                return ApiResponseDto<Product?>.Success(result.Data);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<Product?>.Failure(
                    HttpStatusCode.InternalServerError,
                    $"Failed to retrieve product: {ex.Message}"
                );
            }
        }

        public async Task<ApiResponseDto<List<Product>>> GetProductsByCategoryIdAsync(
            int categoryId,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                ApiResponseDto<List<Product>> result =
                    await _productRepository.GetProductsByCategoryIdAsync(
                        categoryId,
                        cancellationToken
                    );
                if (result.RequestFailed)
                    return result;
                return ApiResponseDto<List<Product>>.Success(result.Data);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<List<Product>>.Failure(
                    HttpStatusCode.InternalServerError,
                    $"Failed to retrieve products: {ex.Message}"
                );
            }
        }

        public async Task<ApiResponseDto<Product>> UpdateProductAsync(
            int id,
            ApiRequestDto<Product> request,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                if (request.Payload == null)
                    return ApiResponseDto<Product>.Failure(
                        HttpStatusCode.BadRequest,
                        "Product data is required"
                    );

                ApiResponseDto<Product?> existingResult = await _productRepository.GetByIdAsync(
                    id,
                    cancellationToken
                );
                if (existingResult.RequestFailed || existingResult.Data == null)
                    return ApiResponseDto<Product>.Failure(
                        HttpStatusCode.NotFound,
                        "Product not found"
                    );

                var prepared = _productProcessingHelper.PrepareForUpdate(existingResult.Data!, request.Payload, id);

                ApiResponseDto<Product> result = await _productRepository.UpdateAsync(
                    prepared,
                    cancellationToken
                );
                return result.RequestFailed ? result : ApiResponseDto<Product>.Success(result.Data);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<Product>.Failure(
                    HttpStatusCode.InternalServerError,
                    $"Failed to update product: {ex.Message}"
                );
            }
        }

        public async Task<ApiResponseDto<bool>> DeleteProductAsync(
            int id,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                ApiResponseDto<bool> result = await _productRepository.DeleteAsync(
                    id,
                    cancellationToken
                );
                if (result.RequestFailed)
                    return result;
                return ApiResponseDto<bool>.Success(result.Data, HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.Failure(
                    HttpStatusCode.InternalServerError,
                    $"Failed to delete product: {ex.Message}"
                );
            }
        }

        public async Task<ApiResponseDto<List<Product>>> GetDeletedProductsAsync(
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                ApiResponseDto<List<Product>> result =
                    await _productRepository.GetDeletedProductsAsync(cancellationToken);
                if (result.RequestFailed)
                    return result;
                return ApiResponseDto<List<Product>>.Success(result.Data);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<List<Product>>.Failure(
                    HttpStatusCode.InternalServerError,
                    $"Failed to retrieve deleted products: {ex.Message}"
                );
            }
        }

        public async Task<ApiResponseDto<bool>> RestoreProductAsync(
            int id,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                ApiResponseDto<bool> result = await _productRepository.RestoreAsync(
                    id,
                    cancellationToken
                );
                if (result.RequestFailed)
                    return result;
                return ApiResponseDto<bool>.Success(result.Data);
            }
            catch (Exception ex)
            {
                return ApiResponseDto<bool>.Failure(
                    HttpStatusCode.InternalServerError,
                    $"Failed to restore product: {ex.Message}"
                );
            }
        }
    }
}
