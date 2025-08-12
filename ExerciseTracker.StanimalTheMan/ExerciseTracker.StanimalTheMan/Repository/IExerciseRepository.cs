using ExerciseTracker.StanimalTheMan.Models;

namespace ExerciseTracker.StanimalTheMan.Repository;

public interface IExerciseRepository
{
	void AddExercise(Run run);
	Run GetExerciseById(int id);
	IEnumerable<Run> GetAllExercises();
	void UpdateExercise(Run run);
	void DeleteExercise(int id);
}
