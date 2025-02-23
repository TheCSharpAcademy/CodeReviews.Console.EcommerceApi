using ExerciseTracker.Models;
using ExerciseTracker.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ExerciseTracker.Service;

internal class ExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;

    public ExerciseService()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IExerciseRepository, ExerciseRepository>();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _exerciseRepository = serviceProvider.GetService<IExerciseRepository>();
    }

    public void AddExercise(Exercise exercise)
    {
        _exerciseRepository.AddExercise(exercise);
    }

    public void DeleteExercise(int id)
    {
        _exerciseRepository.DeleteExercise(id);
    }

    public Exercise GetExercise(int id)
    {
        return _exerciseRepository.GetExercise(id);
    }

    public List<Exercise> GetExercises()
    {
        return _exerciseRepository.GetExercises();
    }

    public void UpdateExercise(int id, Exercise exercise)
    {
        _exerciseRepository.UpdateExercise(id, exercise);
    }
}



