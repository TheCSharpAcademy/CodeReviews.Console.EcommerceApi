using ExerciseTracker.API.Services;
using ExerciseTracker.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ExerciseTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExercisesController : ControllerBase
{
    private readonly IExerciseService _exerciseService;

    public ExercisesController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var exercises = _exerciseService.GetAllExercises();
        return Ok(exercises);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var exercise = _exerciseService.GetExerciseById(id);
        if (exercise == null)
        {
            return NotFound(new { Message = $"Exercise with ID {id} not found." });
        }
        return Ok(exercise);
    }

    [HttpPost]
    public IActionResult AddExercise([FromBody] ExerciseDTO exerciseDTO)
    {
        try
        {
            var createdExercise = _exerciseService.AddExercise(exerciseDTO);
            return CreatedAtAction(nameof(GetById), new { id = createdExercise.Id }, createdExercise);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateExersice(int id, [FromBody] ExerciseDTO exerciseDTO)
    {
        try
        {
            var updatedExercise = _exerciseService.UpdateExercise(id, exerciseDTO);
            if (updatedExercise == null)
            {
                return NotFound(new { Message = $"Exercise with ID {id} not found." });
            }
            return Ok(updatedExercise);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var deletedExercise = _exerciseService.DeleteExercise(id);
            if (deletedExercise == null)
            {
                return NotFound(new { Message = $"Exercise with ID {id} not found." });
            }
            return Ok(deletedExercise);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
        }
    }
}
