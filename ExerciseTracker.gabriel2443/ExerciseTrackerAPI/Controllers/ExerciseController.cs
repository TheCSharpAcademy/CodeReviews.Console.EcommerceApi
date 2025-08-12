using ExerciseTracker.Models;
using ExerciseTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExerciseTracker.Controllers;

[Route("api/exercises")]
[ApiController]
public class ExerciseController : ControllerBase
{
    private readonly IExerciseService _exerciseService;

    public ExerciseController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    [HttpPost]
    public async Task<IActionResult> PostExercise(Exercise exerciseToAdd)
    {
        await _exerciseService.CreateExercise(exerciseToAdd);

        return CreatedAtAction(nameof(GetExerciseById), new { id = exerciseToAdd.Id }, exerciseToAdd);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutExercise(int id, Exercise exerciseToUpdate)
    {
        await _exerciseService.UpdateExercise(id, exerciseToUpdate);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExercises()
    {
        var exercises = await _exerciseService.GetAllExercises();
        return Ok(exercises);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExerciseById(int id)
    {
        var exercise = await _exerciseService.GetExerciseById(id);
        return Ok(exercise);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExercise(int id)
    {
        if (id <= 0) return BadRequest();
        await _exerciseService.DeleteExercise(id);
        return NoContent();
    }
}