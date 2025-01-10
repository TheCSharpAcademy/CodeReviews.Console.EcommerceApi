

namespace ExerciseTracker;

internal interface IExerciseRepository : IDisposable
{
	IEnumerable<ExerciseModel> GetExercises();
	ExerciseModel GetExerciseById(int id);
	void Insert(ExerciseModel model);
	void Update(ExerciseModel model);
	void Delete(int id);
	void Save();

}
