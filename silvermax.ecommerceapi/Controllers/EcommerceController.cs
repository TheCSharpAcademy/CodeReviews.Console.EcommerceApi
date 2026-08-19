using Microsoft.AspNetCore.Mvc;
using silvermax.ecommerceapi.Dtos;
using silvermax.ecommerceapi.Models;
using silvermax.ecommerceapi.Service;

namespace silvermax.ecommerceapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EcommerceController(IEcommerceService ecommerceService) : ControllerBase
{
    [HttpPost("add-product")]
    public async Task<IActionResult> AddProduct(AddProductDto dto, CancellationToken ct)
    {
        var result = await ecommerceService.AddProduct(dto, ct);

        return result.Error switch
        {
            AddProductError.ProductAlreadyExists => Conflict("The product already exists"),
            AddProductError.CategoryNotFound => BadRequest("That category does not exist"),
            _ => Ok(result.Product)
        };
    }

    [HttpPost("add-category")]
    public async Task<IActionResult> AddCategory(AddCategoryDto dto, CancellationToken ct)
    {
        var result = await ecommerceService.AddCategory(dto, ct);

        if (result is null)
            return Conflict("The category already exists");

        return Ok(result);
    }

    [HttpPost("place-order")]
    public async Task<IActionResult> PlaceOrder(PlaceOrderDto dto, CancellationToken ct)
    {
        var result = await ecommerceService.PlaceOrder(dto, ct);

        return result.PlaceOrderError switch
        {
            PlaceOrderError.ClientNotFound => BadRequest("The client does not exist"),
            PlaceOrderError.EmptyOrder => BadRequest("The order is empty"),
            PlaceOrderError.ProductNotFound => NotFound("The product was not found"),
            _ => Ok(result.Order)
        };
    }

    [HttpPost("create-client")]
    public async Task<IActionResult> CreateClient(CreateClientDto dto, CancellationToken ct)
    {
        var result = await ecommerceService.CreateClient(dto, ct);

        if (result is null)
            return Conflict("This client already exists");

        return Ok(result);
    }
}
