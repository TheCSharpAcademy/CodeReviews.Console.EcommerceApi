using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker;
public class ExerciseRepository : IExerciseRepository
{
    private readonly ExerciseDatabase _dbContext;
    private readonly DbSet<Exercise> _dbSet;
    public ExerciseRepository(ExerciseDatabase dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<Exercise>();
    }
    public void AddExercise(Exercise exercise)
    {
        _dbSet.Add(exercise);
    }

    public void DeleteExercise(int id)
    {
        var exercise = _dbSet.Find(id);
        if (exercise != null)
            _dbSet.Remove(exercise);
    }

    public Exercise GetExerciseById(int id)
    {
        return _dbSet.Find(id);
    }

    public IEnumerable<Exercise> GetExercises()
    {
        return _dbSet.ToList();
    }

    public void UpdateExercise(Exercise exercise)
    {
        var existingRecord = _dbSet.Local.FirstOrDefault(x => x.Id == exercise.Id);
        if (existingRecord != null)
        {
            _dbSet.Entry(existingRecord).State = EntityState.Detached;
        }

        _dbSet.Attach(exercise);
        _dbSet.Entry(exercise).State = EntityState.Modified;
    }
    public void Save()
    {
        _dbContext.SaveChanges();
    }
}

