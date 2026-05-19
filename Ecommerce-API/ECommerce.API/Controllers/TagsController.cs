using ECommerce.API.Interfaces;
using ECommerce.API.Models;
using ECommerce.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController(ITagService tagService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTags([FromQuery] PaginationParams paginationParams)
    {
        var pagedResponse = await tagService.GetTagsAsync(paginationParams);
        if (pagedResponse.TotalRecords == 0)
            return NotFound();

        return Ok(pagedResponse);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<int>> GetTagId(string name)
    {
        var tag = await tagService.GetTagIdByName(name);
        return tag switch
        {
            null => NotFound(),
            _ => Ok(tag)
        };
    }

    [HttpPost]
    public async Task<IActionResult> PostTag([FromBody] CreateTagDto tagDto)
    {
        await tagService.PostTagAsync(tagDto);
        return Created();
    }

    [HttpDelete("{tagId:int}")]
    public async Task<IActionResult> DeleteTag(int tagId)
    {
        var success = await tagService.DeleteTagByIdAsync(tagId);

        return success switch
        {
            null => NotFound(),
            true => NoContent(),
            false => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}