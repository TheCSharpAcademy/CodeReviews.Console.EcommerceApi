public interface IRepository
{
    public Task<Exercise> AddAsync(Exercise log);
    public List<Exercise>? GetAll();
    public Task<Exercise?> GetByIdAsync(int id);
    public Task<Exercise> UpdateAsync(Exercise log);
    public Task<Exercise> DeleteAsync(Exercise log);
}

public class ExerciseRepository : IRepository
{ 
    private readonly ExerciseContext _exerciseContext;

    public ExerciseRepository(ExerciseContext exerciseContext)
    {
        _exerciseContext = exerciseContext;
    }

    public async Task<Exercise> AddAsync(Exercise log)
    {
        var savedResult = await _exerciseContext.Exercises.AddAsync(log);
        await _exerciseContext.SaveChangesAsync();
        return savedResult.Entity;
    }

    public List<Exercise>? GetAll()
    {
        return _exerciseContext.Exercises.ToList();
    }

    public async Task<Exercise?> GetByIdAsync(int id)
    {
        Exercise? result = await _exerciseContext.Exercises.FindAsync(id);
        return result;
    }

    public async Task<Exercise> UpdateAsync(Exercise log)
    {
        Exercise logInDb = await _exerciseContext.Exercises.FindAsync(log.Id)
        ?? throw new Exception("Could not find log to update");

        _exerciseContext.Entry(logInDb).CurrentValues.SetValues(log);

        await _exerciseContext.SaveChangesAsync();
        return _exerciseContext.Entry(logInDb).Entity;
    }

    public async Task<Exercise> DeleteAsync(Exercise log)
    {
        var savedResult = _exerciseContext.Exercises.Remove(log);
        await _exerciseContext.SaveChangesAsync();
        return savedResult.Entity;
    }
}