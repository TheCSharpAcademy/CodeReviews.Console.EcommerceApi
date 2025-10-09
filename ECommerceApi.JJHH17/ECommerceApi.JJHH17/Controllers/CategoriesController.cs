using ECommerceApi.JJHH17.Models;
using ECommerceApi.JJHH17.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.JJHH17.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Example call: http://localhost:5609/api/category/
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public ActionResult<PagedResponse<CategoryWithProductsDto>> GetAllCategories([FromQuery] Pagination pagination)
        {
            if (pagination.PageNumber < 1) pagination.PageNumber = 1;
            if (pagination.PageSize < 1) pagination.PageSize = 10;

            var allCategories = _categoryService.GetAllCategories();

            var totalRecords = allCategories.Count;
            var pagedData = allCategories
                .OrderBy(c => c.CategoryId)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return Ok(pagedData);
        }

        [HttpGet("{id}")]
        public ActionResult<Category> GetCategoryById(int id)
        {
            var result = _categoryService.GetCategoryById(id);

            if (result == null) { return NotFound(); }

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<CreateCategoryDto> CreateCategory(CreateCategoryDto category)
        {
            return Ok(_categoryService.CreateCategory(category));
        }
    }
}
