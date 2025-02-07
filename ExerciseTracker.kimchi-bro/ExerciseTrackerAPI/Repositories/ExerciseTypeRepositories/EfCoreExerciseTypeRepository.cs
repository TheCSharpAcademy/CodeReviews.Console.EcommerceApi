using ExerciseTrackerAPI.Data;
using ExerciseTrackerAPI.Models;

namespace ExerciseTrackerAPI.Repositories.ExerciseTypeRepositories;

public class EfCoreExerciseTypeRepository(ApplicationDbContext context) : IExerciseTypeRepository
{
    private readonly ApplicationDbContext _context = context;

    public IEnumerable<ExerciseType> GetAll()
    {
        return _context.ExerciseTypes.ToList();
    }

    public ExerciseType? GetById(int id)
    {
        return _context.ExerciseTypes.Find(id);
    }

    public void Create(ExerciseType exerciseType)
    {
        _context.ExerciseTypes.Add(exerciseType);
        _context.SaveChanges();
    }

    public void Update(int id, ExerciseType exerciseType)
    {
        var toUpdate = _context.ExerciseTypes.Find(id) ?? new ExerciseType { Name = ""};
        toUpdate.Name = exerciseType.Name;
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var toDelete = _context.ExerciseTypes.Find(id) ?? new ExerciseType { Name = "" };
        _context.ExerciseTypes.Remove(toDelete);
        _context.SaveChanges();
    }
}
