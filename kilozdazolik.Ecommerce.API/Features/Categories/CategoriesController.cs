using Microsoft.AspNetCore.Mvc;

namespace kilozdazolik.Ecommerce.API.Features.Categories;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(ICategoryService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto category)
    {
        var createdCategory = await service.CreateCategoryAsync(category);
        return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories(
        [FromQuery] CategoryParameters parameters)
    {
        var categories = await service.GetCategoriesAsync(parameters);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategoryById(Guid id)
    {
        var category = await service.GetCategoryByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCategory(Guid id, UpdateCategoryDto updatedCategory)
    {
        await service.UpdateCategoryAsync(id, updatedCategory);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        await service.DeleteCategoryAsync(id);
        return NoContent();
    }
    
    [HttpPut("{id}/restore")]
    public async Task<ActionResult> RestoreCategory(Guid id)
    {
        await service.RestoreCategoryAsync(id);
        return NoContent(); 
    }
}