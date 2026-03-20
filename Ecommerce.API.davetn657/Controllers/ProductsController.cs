using Ecommerce.API.davetn657.Data;
using Ecommerce.API.davetn657.Data.Dtos;
using Ecommerce.API.davetn657.Services;
using Ecommerce.API.davetn657.Models;
using Microsoft.AspNetCore.Mvc;


namespace Ecommerce.API.davetn657.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    [HttpGet]
    public ActionResult<PagedResponse<ProductsDto>> AllProducts()
    {
        return Ok(_productService.AllProducts(new PaginationParams()));
    }
    [HttpGet("category/{category}")]
    public ActionResult<PagedResponse<ProductsDto>> ProductsByCategory(string category)
    {
        return Ok(_productService.ProductsByCategory( category, new PaginationParams()));
    }
    [HttpGet("id/{id}")]
    public ActionResult<ProductsDto> ProductById(int id)
    {
        return Ok(_productService.ProductById(id));
    }
    [HttpPost]
    public ActionResult<ProductsDto> CreateProduct(ProductsDto product)
    {
        return Ok(_productService.CreateProduct(product));
    }

}