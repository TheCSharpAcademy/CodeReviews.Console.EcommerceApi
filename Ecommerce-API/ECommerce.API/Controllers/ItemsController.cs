using ECommerce.API.Interfaces;
using ECommerce.API.Models;
using ECommerce.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController(IItemService itemService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PostItem([FromBody] CreateItemDto itemDto)
    {
        await itemService.PostItemAsync(itemDto);
        return Created();
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<Item>>> GetItemsAsync([FromQuery] PaginationParams paginationParams)
    {
        var pagedResponse = await itemService.GetItemsAsync(paginationParams);
        if (pagedResponse.TotalRecords == 0) return NotFound();
        return Ok(pagedResponse);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ItemDto>> GetItemByIdAsync(int id)
    {
        var itemDto = await itemService.GetItemByIdAsync(id);

        return itemDto is null ? NotFound() : Ok(itemDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteItemAsync(int id)
    {
        var success = await itemService.DeleteItemByIdAsync(id);
        if (success is null)
            return NotFound();
        if (success is false)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return NoContent();
    }
}