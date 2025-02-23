using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ExerciseEFRepository : EFRepository<Exercise>, IExerciseRepository 
    {
    public ExerciseEFRepository(ExerciseEFDbContext exerciseDbContext) : base(exerciseDbContext)
    {
    }

    public Task<List<Exercise>> GetAllExercisesAsync()
    {
       return GetAll().ToListAsync();
    }

    public Task<Exercise?> GetExerciseByIdAsync(int id)
    {
        return GetAll().FirstOrDefaultAsync(r => r.Id == id);
    }
}