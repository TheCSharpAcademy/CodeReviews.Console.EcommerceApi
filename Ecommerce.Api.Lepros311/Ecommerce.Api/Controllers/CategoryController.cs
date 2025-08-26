using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;
using Ecommerce.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetPagedCategories([FromQuery] PaginationParams paginationParams)
        {
            var responseWithDtos = await _categoryService.GetPagedCategories(paginationParams);

            if (responseWithDtos.Status == ResponseStatus.Fail)
            {
                return BadRequest(responseWithDtos.Message);
            }

            return Ok(responseWithDtos.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            var response = await _categoryService.GetCategoryById(id);

            if (response.Status == ResponseStatus.Fail)
            {
                return NotFound(response.Message);
            }

            var returnedCategory = response.Data;

            var categoryDto = new CategoryDto
            {
                CategoryId = returnedCategory.CategoryId,
                CategoryName = returnedCategory.CategoryName,
                Products = returnedCategory.Products.Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Category = p.Category.CategoryName
                }).ToList()
            };

            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] WriteCategoryDto writeCategoryDto)
        {
            var responseWithDataDto = await _categoryService.CreateCategory(writeCategoryDto);

            if (responseWithDataDto.Message == "Category not found.")
            {
                return NotFound(responseWithDataDto.Message);
            }

            if (responseWithDataDto.Status == ResponseStatus.Fail)
            {
                return BadRequest(responseWithDataDto.Message);
            }

            return CreatedAtAction(nameof(GetCategoryById),
                new { id = responseWithDataDto.Data.CategoryId }, responseWithDataDto.Data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] WriteCategoryDto writeCategoryDto)
        {
            var response = await _categoryService.UpdateCategory(id, writeCategoryDto);

            if (response.Message == "Category not found.")
            {
                return NotFound(response.Message);
            }

            if (response.Status == ResponseStatus.Fail)
            {
                return BadRequest(response.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var response = await _categoryService.DeleteCategory(id);

            if (response.Message == "Category not found.")
            {
                return NotFound(response.Message);
            }
            else if (response.Message == "Cannot delete categories that contain products.")
            {
                return Conflict(response.Message);
            }
            else if (response.Status == ResponseStatus.Fail)
            {
                return BadRequest(response.Message);
            }

            return NoContent();
        }
    }
}

