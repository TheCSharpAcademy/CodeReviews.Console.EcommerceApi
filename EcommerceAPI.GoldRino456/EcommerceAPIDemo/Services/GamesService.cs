using EcommerceAPIDemo.Data;
using EcommerceAPIDemo.Data.DTOs;
using EcommerceAPIDemo.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPIDemo.Services;

public interface IGamesService
{
    //READ Operations
    public IQueryable<GameProduct>? GetAllGames();
    public IQueryable<GameCategory>? GetAllCategories();
    public Task<GameProduct?> GetGameAsync(int gameProductId);
    public Task<GameCategory?> GetCategoryAsync(int categoryId);

    //CREATE Operations
    public Task<GameProduct> CreateGameAsync(NewGameDto dto);
    public Task<GameCategory> CreateCategoryAsync(CategoryDto dto);

    //UPDATE Operations
    public Task<GameProduct> UpdateGameAsync(int gameProductId, ExistingGameDto dto);
    public Task<GameCategory> UpdateCategoryAsync(int gameCategoryId, CategoryDto dto);

    //Utilities
    public GameProductResponseDto ConvertGameProductObjToResponseDto(GameProduct gameProduct);
}

public class GamesService : IGamesService
{
    private readonly SalesDbContext _salesDbContext;

    public GamesService(SalesDbContext salesDbContext)
    {
        _salesDbContext = salesDbContext;
    }

    public async Task<GameCategory> CreateCategoryAsync(CategoryDto dto)
    {
        GameCategory newCategory = ConvertDtoToGameCategory(dto);

        var savedCategory = _salesDbContext.GameCategories.Add(newCategory);
        await _salesDbContext.SaveChangesAsync();

        return savedCategory.Entity;
    }

    public async Task<GameProduct> CreateGameAsync(NewGameDto dto)
    {
        GameProduct newGame = await ConvertDtoToGameProductAsync(dto);

        var savedGame = _salesDbContext.GameProducts.Add(newGame);
        await _salesDbContext.SaveChangesAsync();

        return savedGame.Entity;
    }

    public IQueryable<GameCategory>? GetAllCategories()
    {
        var categories = _salesDbContext.GameCategories
            .OrderBy(c => c.Id);

        if(!categories.Any())
        {
            return null;
        }

        return categories;
    }

    public IQueryable<GameProduct>? GetAllGames()
    {
        var games = _salesDbContext.GameProducts
            .Include(p => p.Categories)
            .Include(p => p.Sales)
            .OrderBy(p => p.Id);

        if (!games.Any())
        {
            return null;
        }

        return games;
    }

    public async Task<GameProduct?> GetGameAsync(int gameProductId)
    {
        return await _salesDbContext.GameProducts
            .Include(p => p.Categories)
            .Include(p => p.Sales)
            .FirstOrDefaultAsync(p => p.Id == gameProductId);
    }

    public async Task<GameCategory?> GetCategoryAsync(int categoryId)
    {
        return await _salesDbContext.GameCategories.FirstOrDefaultAsync(c => c.Id == categoryId);
    }

    public async Task<GameCategory> UpdateCategoryAsync(int gameCategoryId, CategoryDto dto)
    {
        var existingCategory = await _salesDbContext.GameCategories
            .FirstOrDefaultAsync(c => c.Id == gameCategoryId);


        GameCategory newCategory = ConvertDtoToGameCategory(dto);
        newCategory.Id = existingCategory.Id;

        _salesDbContext.GameCategories.Entry(existingCategory).CurrentValues.SetValues(newCategory);
        await _salesDbContext.SaveChangesAsync();

        return existingCategory;
    }

    public async Task<GameProduct> UpdateGameAsync(int gameProductId, ExistingGameDto dto)
    {
        var existingGame = await _salesDbContext.GameProducts
            .Include(p => p.Categories)
            .Include(p => p.Sales)
            .FirstOrDefaultAsync(p => p.Id == gameProductId);

        GameProduct newGame = await ConvertDtoToGameProductAsync(dto);
        newGame.Id = existingGame.Id;
        newGame.Price = existingGame.Price;

        _salesDbContext.GameProducts.Entry(existingGame).CurrentValues.SetValues(newGame);
        existingGame.Categories.Clear();
        existingGame.Categories.AddRange(newGame.Categories);
        await _salesDbContext.SaveChangesAsync();

        return existingGame;
    }

    public GameProductResponseDto ConvertGameProductObjToResponseDto(GameProduct gameProduct)
    {
        var response = new GameProductResponseDto()
        {
            Id = gameProduct.Id,
            Title = gameProduct.Title,
            Description = gameProduct.Description,
            Developer = gameProduct.Developer,
            Publisher = gameProduct.Publisher,
            ReleaseDate = gameProduct.ReleaseDate,
            Price = gameProduct.Price,
            FileSize = gameProduct.FileSize,
            SystemRequirements = gameProduct.SystemRequirements
        };

        if(gameProduct.Categories.Any())
        {
            foreach(var category in gameProduct.Categories)
            {
                response.CategoryIds.Add(category.Id);
            }
        }

        if(gameProduct.Sales.Any())
        {
            foreach(var sales in gameProduct.Sales)
            {
                response.SalesIds.Add(sales.Id);
            }
        }

        return response;
    }

    private GameCategory ConvertDtoToGameCategory(CategoryDto dto)
    {
        GameCategory gameCategory = new()
        {
            Name = dto.Name  
        };

        return gameCategory;
    }

    private async Task<GameProduct> ConvertDtoToGameProductAsync(NewGameDto dto)
    {
        GameProduct gameProduct = new()
        {
            Title = dto.Title,
            Description = dto.Description,
            Developer = dto.Developer,
            Publisher = dto.Publisher,
            ReleaseDate = dto.ReleaseDate,
            Price = dto.Price,
            FileSize = dto.FileSize,
            SystemRequirements = dto.SystemRequirements
        };

        if (dto.GameCategoryIds != null)
        {
            foreach (var id in dto.GameCategoryIds)
            {
                var selectedCategory = await _salesDbContext.GameCategories
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (selectedCategory != null)
                {
                    _salesDbContext.Attach(selectedCategory);
                    gameProduct.Categories.Add(selectedCategory);
                }
            }
        }

        return gameProduct;
    }

    private async Task<GameProduct> ConvertDtoToGameProductAsync(ExistingGameDto dto)
    {
        GameProduct gameProduct = new()
        {
            Title = dto.Title,
            Description = dto.Description,
            Developer = dto.Developer,
            Publisher = dto.Publisher,
            ReleaseDate = dto.ReleaseDate,
            FileSize = dto.FileSize,
            SystemRequirements = dto.SystemRequirements
        };

        if (dto.GameCategoryIds != null)
        {
            foreach (var id in dto.GameCategoryIds)
            {
                var selectedCategory = await _salesDbContext.GameCategories
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (selectedCategory != null)
                {
                    _salesDbContext.Attach(selectedCategory);
                    gameProduct.Categories.Add(selectedCategory);
                }
            }
        }

        return gameProduct;
    }
}
