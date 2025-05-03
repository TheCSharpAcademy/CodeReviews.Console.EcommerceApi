public interface IService
{
    public Task<Exercise> AddAsync(Exercise log);
    public List<Exercise>? GetAll();
    public Task<Exercise?> GetByIdAsync(int id);
    public Task<Exercise> UpdateAsync(Exercise log);
    public Task<Exercise> DeleteAsync(Exercise log);
}

public class ExerciseService(IRepository repo) : IService
{
    public async Task<Exercise> AddAsync(Exercise log)
    {
        Exercise exerciseAdded = await repo.AddAsync(log);
        return exerciseAdded;
    }

    public List<Exercise>? GetAll()
    {
        return repo.GetAll();
    }

    public async Task<Exercise?> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id);
    }

    public async Task<Exercise> UpdateAsync(Exercise log)
    {
        Exercise updatedExercise = await repo.UpdateAsync(log);
        return updatedExercise;
    }

    public async Task<Exercise> DeleteAsync(Exercise log)
    {
        Exercise deletedExercise = await repo.DeleteAsync(log);
        return deletedExercise;
    }
}
