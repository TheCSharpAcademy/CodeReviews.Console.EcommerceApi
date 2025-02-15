using ExerciseTracker.Models;
using ExerciseTracker.Repositories;

namespace ExerciseTracker.Services;

public class ExerciseService : IService
{
    public IRepository _exerciseRepository;
    public ExerciseService(IRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public void Add(Exercise exercise)
    {
        _exerciseRepository.Add(exercise);
    }

    public void Update(Exercise exercise)
    {
        _exerciseRepository.Update(exercise);
    }

    public void Delete(Exercise exercise)
    {
        _exerciseRepository.Delete(exercise);
    }

    public Exercise GetById(int id)
    {
        return _exerciseRepository.GetById(id);
    }

    public IEnumerable<Exercise> GetAll()
    {

        return _exerciseRepository.GetAll();
    }

    public IEnumerable<Exercise> GetLast10()
    {
        return GetAll()
            .OrderByDescending(x => x.Id)
            .Take(10)
            .ToList();
    }
}