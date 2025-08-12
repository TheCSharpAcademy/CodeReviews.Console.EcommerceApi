using ExerciseTracker.StanimalTheMan.Models;
using ExerciseTracker.StanimalTheMan.Services;

namespace ExerciseTracker.StanimalTheMan.Controllers;

public class ExerciseController
{
	private readonly ExerciseService _exerciseService;

	public ExerciseController(ExerciseService exerciseService)
	{
		_exerciseService = exerciseService;
	}

	public void AddExercise(Run run)
	{
		_exerciseService.AddExercise(run);
	}

	public Run GetExerciseById(int id)
	{
		return _exerciseService.GetExerciseById(id);
	}

	public IEnumerable<Run> GetAllExercises()
	{
		return _exerciseService.GetAllExercises();
	}

	public void UpdateExercise(Run run)
	{
		_exerciseService.UpdateExercise(run);
	}

	public void DeleteExercise(int id)
	{
		_exerciseService.DeleteExercise(id);
	}
}
