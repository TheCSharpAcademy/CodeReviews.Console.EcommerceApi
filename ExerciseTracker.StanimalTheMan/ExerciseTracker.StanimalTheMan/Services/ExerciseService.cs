using ExerciseTracker.StanimalTheMan.Models;
using ExerciseTracker.StanimalTheMan.Repository;

namespace ExerciseTracker.StanimalTheMan.Services;

public class ExerciseService
{
	private readonly IExerciseRepository _exerciseRepository;

	public ExerciseService(IExerciseRepository exerciseRepository)
	{
		_exerciseRepository = exerciseRepository;
	}

	public void AddExercise(Run run)
	{
		_exerciseRepository.AddExercise(run);
	}

	public Run GetExerciseById(int id)
	{
		return _exerciseRepository.GetExerciseById(id);
	}

	public IEnumerable<Run> GetAllExercises()
	{
		return _exerciseRepository.GetAllExercises();
	}

	public void UpdateExercise(Run run)
	{
		_exerciseRepository.UpdateExercise(run);
	}

	public void DeleteExercise(int id)
	{
		_exerciseRepository.DeleteExercise(id);
	}
}
