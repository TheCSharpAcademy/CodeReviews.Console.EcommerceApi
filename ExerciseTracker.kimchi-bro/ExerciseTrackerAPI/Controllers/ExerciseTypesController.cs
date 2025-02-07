using ExerciseTrackerAPI.Models;
using ExerciseTrackerAPI.Repositories.ExerciseTypeRepositories;
using Microsoft.AspNetCore.Mvc;

namespace ExerciseTrackerAPI.Controllers;

[ApiController]
[Route("api/exercise-types")]
[Produces("application/json")]
public class ExerciseTypesController(IExerciseTypeRepository repository) : ControllerBase
{
    private readonly IExerciseTypeRepository _repository = repository;

    [HttpGet]
    public IActionResult GetAllExerciseTypes()
    {
        var exerciseTypes = _repository.GetAll();
        return Ok(new { exerciseTypes });
    }

    [HttpGet("{id}")]
    public IActionResult GetExerciseTypeById(int id)
    {
        var exerciseType = _repository.GetById(id);
        if (exerciseType is null) return NotFound();

        return Ok(exerciseType);
    }

    [HttpPost]
    public ActionResult AddExerciseType([FromBody] ExerciseType exerciseType)
    {
        _repository.Create(exerciseType);

        return CreatedAtAction(nameof(GetExerciseTypeById), new { id = exerciseType.Id }, exerciseType);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateExerciseType(int id, [FromBody] ExerciseType updateExerciseType)
    {
        var exerciseType = _repository.GetById(id);
        if (exerciseType is null) return NotFound();

        _repository.Update(id, updateExerciseType);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteExerciseTypeById(int id)
    {
        var exerciseType = _repository.GetById(id);
        if (exerciseType is null) return NotFound();

        _repository.Delete(id);

        return NoContent();
    }
}
