using Ecommerce.API.davetn657.Data;
using Ecommerce.API.davetn657.Data.Dtos;
using Ecommerce.API.davetn657.Services;
using Ecommerce.API.davetn657.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.davetn657.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryServices)
    {
        _categoryService = categoryServices;
    }

    [HttpGet]
    public ActionResult<PagedResponse<CategoriesDto>> AllCategories()
    {
        return Ok(_categoryService.AllCategories(new PaginationParams()));
    }
    [HttpGet("id/{id}")]
    public ActionResult<CategoriesDto> CategoryById(int id)
    {
        var result = _categoryService.CategoryById(id);

        if (result == null) return NotFound();

        return Ok(result);
    }
    [HttpPost]
    public ActionResult<CategoriesDto> CreateCategory(CategoriesDto category)
    {
        return Ok(_categoryService.CreateCategory(category));
    }
    [HttpDelete("{id}")]
    public ActionResult<string> DeleteCategory(int id)
    {
        var result = _categoryService.DeleteCategory(id);

        if (result == null) return NotFound();

        return Ok(result);
    }
}