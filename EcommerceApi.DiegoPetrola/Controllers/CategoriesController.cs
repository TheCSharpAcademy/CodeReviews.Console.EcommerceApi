using EcommerceApi.Models.DTOs;
using EcommerceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(CategoriesService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var res = await service.GetCategories();
        return this.ToActionResult(res);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
        var res = await service.GetCategory(id);
        return this.ToActionResult(res);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto dto)
    {
        var res = await service.CreateCategory(dto);
        return this.ToActionResult(res);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCategory(int id, CategoryDto dto)
    {
        if (dto.Id != id)
            return BadRequest();
        var res = await service.UpdateCategory(dto);
        return this.ToActionResult(res);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        var res = await service.DeleteCategory(id);
        return this.ToActionResult(res);
    }
}
