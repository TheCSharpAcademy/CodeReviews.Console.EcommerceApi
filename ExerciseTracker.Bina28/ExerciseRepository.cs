
namespace ExerciseTracker;

internal class ExerciseRepository : IExerciseRepository, IDisposable
{
	private readonly ExerciseContext _context;

	public ExerciseRepository(ExerciseContext context)
	{
	   this._context = context;
	}

	public void Delete(int id)
	{
		throw new NotImplementedException();
	}

	public void Dispose()
	{
		throw new NotImplementedException();
	}

	public ExerciseModel GetExerciseById(int id)
	{
		throw new NotImplementedException();
	}

	public IEnumerable<ExerciseModel> GetExercises()
	{
		throw new NotImplementedException();
	}

	public void Insert(ExerciseModel model)
	{
		throw new NotImplementedException();
	}

	public void Save()
	{
		throw new NotImplementedException();
	}

	public void Update(ExerciseModel model)
	{
		throw new NotImplementedException();
	}
}
