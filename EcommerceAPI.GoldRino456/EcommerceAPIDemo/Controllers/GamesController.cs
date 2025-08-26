using EcommerceAPIDemo.Data.DTOs;
using EcommerceAPIDemo.Data.Models;
using EcommerceAPIDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPIDemo.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController :ControllerBase
{
    private readonly IGamesService _gamesService;
    public GamesController(IGamesService gamesService)
    {
        _gamesService = gamesService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<GameProductResponseDto>>> GetAllGamesAsync([FromQuery] PaginationParams pagination, [FromQuery] GameProductFilterParams filters)
    {
        var query = _gamesService.GetAllGames();

        if (query == null)
        {
            return NoContent();
        }

        if (filters.CategoryId.HasValue)
        {
            query = query.Where(p => p.Categories.Any(c => c.Id == filters.CategoryId.Value));
        }
        if(filters.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= filters.MinPrice.Value);
        }
        if (filters.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price < filters.MaxPrice.Value);
        }


        var totalRecords = await query.CountAsync();
        var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        var responseItems = items.Select(x => _gamesService.ConvertGameProductObjToResponseDto(x)).ToList();

        var pagedResponse = new PagedResponse<GameProductResponseDto>(responseItems, pagination.PageNumber, pagination.PageSize, totalRecords);

        return Ok(pagedResponse);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<PagedResponse<GameCategory>>> GetAllCategoriesAsync([FromQuery] PaginationParams paginationParams)
    {
        var query = _gamesService.GetAllCategories();

        if (query == null)
        {
            return NoContent();
        }

        var totalRecords = await query.CountAsync();
        var items = await query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        var pagedResponse = new PagedResponse<GameCategory>(items, paginationParams.PageNumber, paginationParams.PageSize, totalRecords);

        return Ok(pagedResponse);
    }

    [HttpGet("{gameId}")]
    public async Task<ActionResult<GameProductResponseDto>> GetGameAsync(int gameId)
    {
        var selectedGame = await _gamesService.GetGameAsync(gameId);

        if(selectedGame == null)
        {
            return NotFound($"Game with id {gameId} was not found.");
        }

        return Ok(_gamesService.ConvertGameProductObjToResponseDto(selectedGame));
    }

    [HttpGet("categories/{categoryId}")]
    public async Task<ActionResult<GameCategory>> GetCategoryAsync(int categoryId)
    {
        var selectedCategory = await _gamesService.GetCategoryAsync(categoryId);

        if (selectedCategory == null)
        {
            return NotFound($"Category with id {categoryId} was not found.");
        }

        return Ok(selectedCategory);
    }

    [HttpPost]
    public async Task<ActionResult<GameProductResponseDto>> CreateGameAsync(NewGameDto dto)
    {
        var newGame = await _gamesService.CreateGameAsync(dto);
        return Ok(_gamesService.ConvertGameProductObjToResponseDto(newGame));
    }

    [HttpPost("categories")]
    public async Task<ActionResult<GameCategory>> CreateCategoryAsync(CategoryDto dto)
    {
        return Ok(await _gamesService.CreateCategoryAsync(dto));
    }

    [HttpPut("{gameId}")]
    public async Task<ActionResult<GameProductResponseDto>> UpdateGameAsync(int gameId, ExistingGameDto dto)
    {
        var selectedGame = await _gamesService.GetGameAsync(gameId);

        if(selectedGame == null)
        {
            return NotFound($"Game with id {gameId} was not found.");
        }

        var updatedGame = await _gamesService.UpdateGameAsync(selectedGame.Id, dto);
        return Ok(_gamesService.ConvertGameProductObjToResponseDto(updatedGame));
    }

    [HttpPut("categories/{categoryId}")]
    public async Task<ActionResult<GameCategory>> UpdateCategoryAsync(int categoryId, CategoryDto dto)
    {
        var selectedCategory = await _gamesService.GetCategoryAsync(categoryId);

        if (selectedCategory == null)
        {
            return NotFound($"Category with id {categoryId} was not found.");
        }

        return Ok(await _gamesService.UpdateCategoryAsync(selectedCategory.Id, dto));
    }
}
