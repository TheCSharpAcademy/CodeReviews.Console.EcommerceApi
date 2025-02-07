using ExerciseTrackerAPI.Models;
using ExerciseTrackerAPI.Repositories.ExerciseRepositories;
using Microsoft.AspNetCore.Mvc;

namespace ExerciseTrackerAPI.Controllers;

[ApiController]
[Route("api/exercises")]
[Produces("application/json")]
public class ExercisesController(IExerciseRepository repository) : ControllerBase
{
    private readonly IExerciseRepository _repository = repository;

    [HttpGet]
    public IActionResult GetAllExercises()
    {
        var exercises = _repository.GetAll();
        return Ok(new { exercises });
    }

    [HttpGet("{id}")]
    public IActionResult GetExerciseById(int id)
    {
        var exercise = _repository.GetById(id);
        if (exercise is null) return NotFound();

        return Ok(exercise);
    }

    [HttpPost]
    public ActionResult AddExercise([FromBody]Exercise exercise)
    {
        _repository.Create(exercise);

        return CreatedAtAction(nameof(GetExerciseById), new { id = exercise.Id }, exercise);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateExercise(int id, [FromBody] Exercise exercise)
    {
        _repository.Update(id, exercise);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteExerciseById(int id)
    {
        var exercise = _repository.GetById(id);
        if (exercise is null) return NotFound();

        _repository.Delete(id);

        return NoContent();
    }
}
