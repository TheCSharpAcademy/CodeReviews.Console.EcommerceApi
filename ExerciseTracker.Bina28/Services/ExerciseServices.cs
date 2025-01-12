using ExerciseTracker.Models;
using ExerciseTracker.Repositories;

namespace ExerciseTracker.Services;

internal class ExerciseServices
{
	private readonly IExerciseRepository<ExerciseModel> _exerciseRepository;

	public ExerciseServices(IExerciseRepository<ExerciseModel> exerciseRepository)
	{
		_exerciseRepository = exerciseRepository;
	}

	public ExerciseModel GetById(int id)
	{
		try
		{
			return _exerciseRepository.GetById(id);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error in GetById: {ex.Message}");
			return null;
		}
	}

	public IEnumerable<ExerciseModel> GetAll()
	{
		try
		{
			return _exerciseRepository.GetAll();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error in GetAll: {ex.Message}");
			return Enumerable.Empty<ExerciseModel>();
		}
	}

	public void Add(ExerciseModel model)
	{
		try
		{
			_exerciseRepository.Add(model);
		}

		catch (Exception ex)
		{
			Console.WriteLine($"Error in Add: {ex.Message}");
		}
	}
	public void Update(ExerciseModel model)
	{
		try
		{
			_exerciseRepository.Update(model);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error in Update: {ex.Message}");
		}
	}

	public void Delete(ExerciseModel model)
	{
		try
		{
			_exerciseRepository.Delete(model);
		}
		catch (Exception ex)
		{

			Console.WriteLine($"Error in Delete: {ex.Message}");
		}
	}
}


