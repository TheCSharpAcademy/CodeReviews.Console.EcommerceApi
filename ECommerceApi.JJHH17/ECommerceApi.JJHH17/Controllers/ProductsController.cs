using ECommerceApi.JJHH17.Models;
using ECommerceApi.JJHH17.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.JJHH17.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Example call: http://localhost:5609/api/product/
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult<PagedResponse<GetProductsDto>> GetAllProducts([FromQuery] Pagination pagination)
        {
            if (pagination.PageNumber < 1) pagination.PageNumber = 1;
            if (pagination.PageSize < 1) pagination.PageSize = 10;

            var allProducts = _productService.GetAllProducts();

            var totalRecords = allProducts.Count;
            var pagedData = allProducts
                .OrderBy(c => c.productId)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return Ok(pagedData);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProductById(int id)
        {
            var result = _productService.GetProductById(id);

            if (result == null) { return NotFound(); }

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<CreateProductDto> CreateProduct(CreateProductDto product)
        {
            return Ok(_productService.CreateProduct(product));
        }
    }
}
