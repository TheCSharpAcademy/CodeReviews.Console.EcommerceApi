using ExerciseTracker.Models;

namespace ExerciseTracker.Repositories
{
	public class ExerciseRepository : IExerciseRepository
	{
		private readonly ExerciseTrackerContext _context;
		public ExerciseRepository(ExerciseTrackerContext context)
		{
			_context = context;
		}

		public void Add(ExerciseModel exercise)
		{
			_context.Add(exercise);
			_context.SaveChanges();
		}

		public void Delete(int id)
		{
			var exerciseInDb = _context.Exercises.Find(id);
			if (exerciseInDb != null)
			{
				_context.Remove(exerciseInDb);
				_context.SaveChanges();
			}
		}

		public IEnumerable<ExerciseModel> GetAll()
		{
			return _context.Set<ExerciseModel>().ToList();
		}

		public ExerciseModel GetExerciseById(int id)
		{
			return _context.Find<ExerciseModel>(id);
		}

		public void Update(ExerciseModel exercise)
		{
			_context.Update(exercise);
			_context.SaveChanges();
		}
	}
}
