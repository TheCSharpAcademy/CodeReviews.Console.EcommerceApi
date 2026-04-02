using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Common.Results;
using ECommerce.Shared.TerrenceLGee.DTOs.CategoryDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.CategoryParameters;

namespace ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;

public interface ICategoryService
{
    Task<Result<RetrievedCategoryForAdminDto?>> AddCategoryAsync(CreateCategoryDto category);
    Task<Result<RetrievedCategoryForAdminDto?>> UpdateCategoryAsync(UpdateCategoryDto category);
    Task<Result<RetrievedCategoryDto?>> GetCategoryAsync(CategoryParams categoryParams);
    Task<Result<RetrievedCategoryForAdminDto?>> GetCategoryForAdminAsync(CategoryParams categoryParams);
    Task<Result<PagedList<RetrievedCategorySummaryDto>>> GetCategoriesAsync(CategoryQueryParams categoryQueryParams);
    Task<Result<PagedList<RetrievedCategorySummaryForAdminDto>>> GetCategoriesForAdminAsync(CategoryQueryParams categoryQueryParams);
}
