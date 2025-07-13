using Microsoft.AspNetCore.Mvc;
using ExerciseTrackerApi.Interfaces;
using ExerciseTrackerApi.Models;
namespace ExerciseTrackerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExerciseController : ControllerBase
{
  private readonly IExerciseRepository _exerciseRepository;

  public ExerciseController(IExerciseRepository exerciseRepository)
  {
    _exerciseRepository = exerciseRepository;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<Exercise>>> GetAllExercisesAsync()
  {
    try
    {
      var exercises = await _exerciseRepository.GetAllExercisesAsync();
      return Ok(exercises);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error fetching exercises: {ex.Message}");
      return StatusCode(500, "Internal server error");
    }
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Exercise>> GetExerciseByIdAsync(int id)
  {
    try
    {
      var exercise = await _exerciseRepository.GetExerciseByIdAsync(id);
      if (exercise == null)
      {
        return NotFound($"Exercise with ID {id} not found.");
      }
      return Ok(exercise);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error fetching exercise by ID: {ex.Message}");
      return StatusCode(500, "Internal server error");
    }
  }


  [HttpPost]
  public async Task<ActionResult<Exercise>> AddExerciseAsync(Exercise exercise)
  {
    try
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      await _exerciseRepository.AddExerciseAsync(exercise);
      return Ok(exercise);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error adding exercise: {ex.Message}");
      return BadRequest("An error occurred while adding the exercise.");
    }
  }


  [HttpPut("{id}")]
  public async Task<ActionResult<Exercise>> UpdateExerciseAsync(int id, Exercise exercise)
  {
    try
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      exercise.Id = id;
      
      await _exerciseRepository.UpdateExerciseAsync(exercise);
      return NoContent();
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error updating exercise: {ex.Message}");
      return BadRequest("An error occurred while updating the exercise.");
    }
  }


  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteExerciseAsync(int id)
  {
    try
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var existingExercise = await _exerciseRepository.GetExerciseByIdAsync(id);
      if (existingExercise == null)
      {
        return NotFound($"Exercise with ID {id} not found.");
      }
      await _exerciseRepository.DeleteExerciseAsync(id);
      return NoContent();
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error deleting exercise: {ex.Message}");
      return BadRequest("An error occurred while deleting the exercise.");
    }
  }
}