using ExerciseTracker.Models;
using ExerciseTracker.Repositories;
using Spectre.Console;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace ExerciseTracker.Controller;

internal class ExerciseController
{
    private readonly IExerciseRepository<ExerciseModel> _exerciseRepository;

    public ExerciseController(IExerciseRepository<ExerciseModel> exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

	public ExerciseController()
	{
	}

	public ExerciseModel GetById(int id)
    {
        return _exerciseRepository.GetById(id);
    }

    public IEnumerable<ExerciseModel> GetAll() 
    {
        return _exerciseRepository.GetAll(); 
    }

    public void Add(ExerciseModel model)
    {
        _exerciseRepository.Add(model);

    }

    public void Update(ExerciseModel model)
    {
        _exerciseRepository.Update(model);

    }
    public void Delete(ExerciseModel model)
    {
        _exerciseRepository.Delete(model);

    }

}
