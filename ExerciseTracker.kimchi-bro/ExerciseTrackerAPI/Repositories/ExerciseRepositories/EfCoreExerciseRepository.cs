using ExerciseTrackerAPI.Data;
using ExerciseTrackerAPI.Models;

namespace ExerciseTrackerAPI.Repositories.ExerciseRepositories;

public class EfCoreExerciseRepository(ApplicationDbContext context) : IExerciseRepository
{
    private readonly ApplicationDbContext _context = context;

    public IEnumerable<Exercise> GetAll()
    {
        return _context.Exercises.ToList();
    }

    public Exercise? GetById(int id)
    {
        return _context.Exercises.Find(id);
    }

    public void Create(Exercise exercise)
    {
        _context.Exercises.Add(exercise);
        _context.SaveChanges();
    }

    public void Update(int id, Exercise exercise)
    {
        var toUpdate = _context.Exercises.Find(id) ?? new Exercise();
        toUpdate.ExerciseTypeName = exercise.ExerciseTypeName;
        toUpdate.StartTime = exercise.StartTime;
        toUpdate.EndTime = exercise.EndTime;
        toUpdate.Duration = exercise.Duration;
        toUpdate.Comments = exercise.Comments;
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var toDelete = _context.Exercises.Find(id) ?? new Exercise();
        _context.Exercises.Remove(toDelete);
        _context.SaveChanges();
    }
}
