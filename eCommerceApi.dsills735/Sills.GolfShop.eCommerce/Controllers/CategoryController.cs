using Microsoft.AspNetCore.Mvc;
using Sills.GolfShop.eCommerceAPI.Models;
using Sills.GolfShop.eCommerceAPI.Services;
using Sills.GolfShop.eCommerceAPI.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Sills.GolfShop.eCommerceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService categoryService) : ControllerBase

{
    private readonly ICategoryService _categoryService = categoryService;

        [HttpGet]
        public async Task<ActionResult<List<Categories>>> GetAllCategories([FromQuery] PaginationParameters param, [FromQuery] CategoryParameters categoryParams)
        {
            var query = _categoryService.GetAllCategoriesQuery();
            query = categoryParams.sortBy switch
            {
                "name_desc" => query.OrderByDescending(p => p.Name),
                _ => query.OrderBy(p => p.Name)
            };

            var pagedCategories = await _categoryService
                .GetPagedCategoriesAsync(param.PageNumber, param.PageSize);

        return Ok(pagedCategories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Categories>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<Categories>> CreateCategory(Categories category)
        {
            var createdCategory = await _categoryService.CreateCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Categories category)
        {
            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }
            await _categoryService.UpdateCategoryAsync(id, category);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }

}
