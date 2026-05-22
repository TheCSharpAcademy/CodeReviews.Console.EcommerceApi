using EcommerceAPI.Ledana.DTOs;
using EcommerceAPI.Ledana.Interfaces;
using EcommerceAPI.Ledana.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EcommerceAPI.Ledana.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        
        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<List<Category>>>> Get()
        {
            ApiResponseDto<List<Category>> response = await _categoryService.GetAllCategories();

            if (response is null) return BadRequest();

            return Ok(response);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<Category>>> Get(int id)
        {
            ApiResponseDto<Category> response = await _categoryService.GetCategoryById(id);

            if (response is null) return NotFound();

            return Ok(response);
        }

        
        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<Category>>> Post([FromBody] CategoryDto category)
        {
            if (!ModelState.IsValid) return BadRequest();

            ApiResponseDto<Category> response = await _categoryService.CreateCategory(category);

            if (response is null) return NotFound();

            return Ok(response);
        }

        
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<Category>>> Put(int id, [FromBody] CategoryDto category)
        {
            if (!ModelState.IsValid) return BadRequest();

            ApiResponseDto<Category> response = await _categoryService.UpdateCategory(id, category);

            if (response is null) return NotFound();

            return Ok(response);
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<string>>> Delete(int id)
        {
            var response = await _categoryService.SoftDeleteCategory(id);

            if (response is null) return NotFound();

            return Ok(response);
        }
    }
}
