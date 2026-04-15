using Asp.Versioning;
using Ecommerce.Core.DTOs.Product;
using Ecommerce.Core.Interfaces.Services;
using Ecommerce.Core.Utilities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Controllers;

[ApiController]
[Route("api/v{v:apiVersion}/[controller]")]
public class ProductController(IProductService productService, IValidator<CreateProductDto> createValidator, IValidator<UpdateProductDto> updateValidator) : ControllerBase
{
    private readonly IProductService _productService = productService;
    private readonly IValidator<CreateProductDto> _createValidator = createValidator;
    private readonly IValidator<UpdateProductDto> _updateValidator = updateValidator;
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
    {
        var products = await _productService.GetAllProductsAsync(paginationParams);
        return Ok(products.Data);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductAsync(id);
        return product.IsSuccess ? Ok(product.Data) : NotFound(product.Message);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto product)
    {
        var validationResult = await _createValidator.ValidateAsync(product);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => $"{x.PropertyName} => {x.ErrorMessage}").ToList();
            return BadRequest(new {Errors = errors});
        }
        
        var result = await _productService.CreateProductAsync(product);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Message);
    }
    
    [Authorize(Roles = "Admin")] 
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteProductAsync(id);
        return result.IsSuccess ? NoContent() : BadRequest(result.Message);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto product)
    {
        var validationResult = await _updateValidator.ValidateAsync(product);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => $"{x.PropertyName} => {x.ErrorMessage}").ToList();
            return BadRequest(new {Errors = errors});
        }
        var result = await _productService.UpdateProductAsync(id, product);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Message);
    }
}