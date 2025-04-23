using ExerciseTracker.Data;
using ExerciseTracker.Interfaces;
using ExerciseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExerciseTracker.Repository;

public class ExerciseRepository : IExerciseRepository
{
    private readonly ExerciseTrackerDbContext _context;
    private readonly ILogger<ExerciseRepository> _logger;

    public ExerciseRepository(ExerciseTrackerDbContext context, ILogger<ExerciseRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(Pushup pushup)
    {
        if (pushup == null)
        {
            _logger.LogError("Attempted to add a null entity of type {EntityType}", typeof(Pushup).Name);
            throw new ArgumentNullException(nameof(pushup), "Entity cannot be null");
        }
        try
        {
            await _context.Exercises.AddAsync(pushup);
            await _context.SaveChangesAsync();
        }
        catch
        {
            _logger.LogError("Error adding entity of type {EntityType}", typeof(Pushup).Name);
            throw;
        }
    }

    public async Task DeleteAsync(Pushup pushup)
    {
        var pushupById = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == pushup.Id);
        if (pushupById == null)
        {
            _logger.LogError("Attempted to delete a non-existing entity of type {EntityType}", typeof(Pushup).Name);
            throw new ArgumentException("Entity not found");
        }

        _context.Exercises.Remove(pushupById);
        await _context.SaveChangesAsync();
    }


    public async Task<IEnumerable<Pushup>> GetAllAsync()
    {
        try
        {
            return await _context.Exercises.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all entities of type {EntityType}", typeof(Pushup).Name);
            throw;
        }
    }

    public async Task<Pushup> GetByIdAsync(int id)
    {
        try
        {
            return await _context.Exercises.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }
        catch
        {
            _logger.LogError("Error retrieving entity of type {EntityType} with ID {Id}", typeof(Pushup).Name, id);
            throw;
        }
    }

    public async Task UpdateAsync(Pushup updatedPushup)
    {
        if (updatedPushup == null)
        {
            _logger.LogError("Attempted to update a null entity of type {EntityType}", typeof(Pushup).Name);
            throw new ArgumentNullException(nameof(updatedPushup), "Entity cannot be null");
        }

        var existingPushup = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == updatedPushup.Id);
        if (existingPushup == null)
        {
            _logger.LogError("Entity of type {EntityType} with ID {Id} not found for update", typeof(Pushup).Name, updatedPushup.Id);
            throw new InvalidOperationException("Entity not found.");
        }

        existingPushup.Date = updatedPushup.Date;
        existingPushup.Reps = updatedPushup.Reps;
        existingPushup.Comments = updatedPushup.Comments;

        await _context.SaveChangesAsync();
    }
}