using JafnaEcommerceApi.Models.DTOs.CategoryDTOs;
using JafnaEcommerceApi.Services;
using Microsoft.AspNetCore.Mvc;
using JafnaEcommerceApi.Models.DTOs.APIResponse;
using JafnaEcommerceApi.Models.DTOs.PaginationDTOs;

namespace JafnaEcommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] CategoryCreateDto categoryCreateDto)
    {
        await _categoryService.Create(categoryCreateDto);
         return Ok(ApiResponse<object>.Success(null, "Category created successfully")); 
    }

    [HttpPut("{categoryId}")]
    public async Task<ActionResult> UpdateAsync([FromRoute] int categoryId, [FromBody] CategoryUpdateDto categoryUpdateDto)
    {
        await _categoryService.Update(categoryId, categoryUpdateDto);
        return Ok(ApiResponse<object>.Success(null, "Category updated successfully"));

    }

    [HttpDelete("{categoryId}")]
    public async Task<ActionResult> DeleteAsync([FromRoute]int categoryId)
    {
        await _categoryService.Delete(categoryId);
        return Ok(ApiResponse<object>.Success(null, "Category deleted successfully"));
    }

    [HttpGet]
    public async Task<ActionResult> GetAllAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var categoryList = await _categoryService.GetAll(pageNumber, pageSize);
        return Ok(ApiResponse<PagedResponse<CategoryDto>>.Success(categoryList, "Categories obtained successfully"));
    }   


}
