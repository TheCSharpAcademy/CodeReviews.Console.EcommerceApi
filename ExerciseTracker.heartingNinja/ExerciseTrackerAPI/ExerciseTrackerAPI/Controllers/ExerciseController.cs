using ExerciseTrackerAPI.Services.ExerciseServices;
using Microsoft.AspNetCore.Mvc;

namespace ExerciseTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExerciseController : ControllerBase
{
    private readonly IExerciseService _exercise;

    public ExerciseController(IExerciseService exercise)
    {
        _exercise = exercise;
    }

    [HttpGet]
    public async Task<ActionResult<List<Exercise>>> GetAll()
    {
        return await _exercise.GetAll();
    }

    [HttpPost]
    public async Task<ActionResult<List<Exercise>>> AddExercise(Exercise exercise)
    {
        var results = await _exercise.AddExercise(exercise);
        return results;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Exercise>> Get(int id)
    {
        var results = await _exercise.Get(id);
        if (results is null)
            return NotFound();

        return Ok(results);
    }

    [HttpGet("customer/{customerId}/exercises")]
    public async Task<ActionResult<List<Exercise>>> GetExercisesByCustomerId(int customerId)
    {
        var results = await _exercise.GetExercisesByCustomerId(customerId);
        if (results is null)
            return NotFound();

        return Ok(results);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<List<Exercise>>> UpdateExercise(int id, Exercise response)
    {
        var results = await _exercise.UpdateExercise(id, response);
        if (results is null)
            return NotFound();

        return Ok(results);
    }

    [HttpDelete("customer/{customerId}/exercises")]
    public async Task<ActionResult<List<Exercise>>> DeleteExercisesByCustomerId(int customerId)
    {
        var results = await _exercise.DeleteExercisesByCustomerId(customerId);
        if (results.Count == 0)
            return NotFound();

        return Ok(results);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<List<Exercise>>> Delete(int id)
    {
        var results = await _exercise.Delete(id);
        if (results is null)
            return NotFound();

        return Ok(results);
    }
}
