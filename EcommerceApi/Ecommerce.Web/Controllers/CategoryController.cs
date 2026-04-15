using Asp.Versioning;
using Ecommerce.Core.DTOs.Category;
using Ecommerce.Core.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Controllers;

[ApiController]
[Route("api/v{v:apiVersion}/[controller]")]
public class CategoryController(ICategoryService categoryService, IValidator<CreateCategoryDto> createValidator, IValidator<UpdateCategoryDto> updateValidator) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;
    private readonly IValidator<CreateCategoryDto> _createValidator = createValidator;
    private readonly IValidator<UpdateCategoryDto> _updateValidator = updateValidator;
        

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories.Data);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _categoryService.GetCategoryAsync(id);
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.Message);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto category)
    {
        var validationResult = await _createValidator.ValidateAsync(category);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => $"{x.PropertyName} => {x.ErrorMessage}").ToList();
            return BadRequest(new {Errors = errors});
        }
        var result = await _categoryService.CreateCategoryAsync(category);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Message);
    }

    [Authorize(Roles = "Admin")]   
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateCategoryDto category)
    {
        var validationResult = await _updateValidator.ValidateAsync(category);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => $"{x.PropertyName} => {x.ErrorMessage}").ToList();
            return BadRequest(new {Errors = errors});
        }
        var result = await _categoryService.UpdateCategoryAsync(id, category);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Message);
    }
    
    [Authorize(Roles = "Admin")]  
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _categoryService.DeleteCategoryAsync(id);
        return result.IsSuccess ? NoContent() : BadRequest(result.Message);
    }
}