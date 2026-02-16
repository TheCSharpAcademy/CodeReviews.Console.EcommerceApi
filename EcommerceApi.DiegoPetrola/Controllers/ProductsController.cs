using EcommerceApi.Models.DTOs;
using EcommerceApi.Services;
using EcommerceApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EcommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(ProductsService service, IOptions<PaginationSettings> options) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetProducts([FromQuery] int? pageSize, [FromQuery] int pageNumber = 0)
    {
        int finalSize = Math.Min(pageSize ?? options.Value.DefaultPageSize, options.Value.MaxPageSize);
        var res = await service.GetProducts(pageNumber, finalSize);
        return this.ToActionResult(res);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        var res = await service.GetProduct(id);
        return this.ToActionResult(res);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto dto)
    {
        var res = await service.CreateProduct(dto);
        return this.ToActionResult(res);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ProductDto>> DeleteProduct(int id)
    {
        var res = await service.SoftDeleteProduct(id);
        return this.ToActionResult(res);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(int id, ProductDto dto)
    {
        if (id != dto.Id)
            return BadRequest();
        var res = await service.UpdateProduct(dto);
        return this.ToActionResult(res);
    }
}
