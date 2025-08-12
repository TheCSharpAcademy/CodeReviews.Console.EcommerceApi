using ExerciseTracker.kjanos89.Models;
using ExerciseTracker.kjanos89.Repository;

namespace ExerciseTracker.kjanos89.Services;

public class Service
{
    IExerciseRepository repo;

    public Service(IExerciseRepository repository)
    {
        repo = repository;
    }

    public IEnumerable<Exercise> ListAll()
    {
        return repo.ListAll();
    }

    public void AddExercise(DateTime start, DateTime end, TimeSpan duration, string comment)
    {
        Exercise exercise = new Exercise
        {
            Start = start,
            End = end,
            Duration = duration,
            Comments = comment
        };
        repo.Create(exercise);
    }

    public Exercise ReadExercise(int id)
    {
        return repo.Read(id);
    }

    public bool IdExists(int id)
    {
        return repo.Exists(id);
    }

    public void UpdateExercise(int id, DateTime start, DateTime end, TimeSpan duration, string comment)
    {
        Exercise newExercise = new Exercise
        {
            Id = id,
            Start = start,
            End = end,
            Duration = duration,
            Comments = comment
        };
        repo.Update(newExercise);
    }

    public void DeleteExercise(int id)
    {
        repo.Delete(id);
    }
}