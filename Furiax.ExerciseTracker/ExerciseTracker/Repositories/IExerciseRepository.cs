using ExerciseTracker.Models;

namespace ExerciseTracker.Repositories
{
	public interface IExerciseRepository
	{
		public IEnumerable<ExerciseModel> GetAll();
		public void Add(ExerciseModel exercise);
		public void Update(ExerciseModel exercise);
		public void Delete(int id);
		public ExerciseModel GetExerciseById(int id);
	}
}
