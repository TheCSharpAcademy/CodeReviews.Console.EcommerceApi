using ExerciseTrackerAPI.Data;
using ExerciseTrackerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTrackerAPI.Controllers;

[ApiController]
[Route("api/mock")]
[Produces("application/json")]
public class MockControllers(ApplicationDbContext context) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;

    [HttpPost("add")]
    public IActionResult AddExercises([FromBody] List<Exercise> exercises)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            _context.Exercises.AddRange(exercises);
            _context.SaveChanges();
            transaction.Commit();
            return Ok();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("extadd")]
    public IActionResult AddExerciseTypes([FromBody] List<ExerciseType> exerciseTypes)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            _context.ExerciseTypes.AddRange(exerciseTypes);
            _context.SaveChanges();
            transaction.Commit();
            return Ok();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("del")]
    public async Task<IActionResult> DeleteExercise()
    {
        _context.Exercises.RemoveRange(_context.Exercises);
        await _context.SaveChangesAsync();
        _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Exercises', RESEED, 0)");

        return NoContent();
    }

    [HttpDelete("extdel")]
    public async Task<IActionResult> DeleteExerciseTypes()
    {
        _context.ExerciseTypes.RemoveRange(_context.ExerciseTypes);
        await _context.SaveChangesAsync();
        _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('ExerciseTypes', RESEED, 0)");

        return NoContent();
    }
}
