using Data.Entities;
using Data.Repositories;

namespace ExerciseTracker.Services;

public class ExerciseService {
    private readonly IExerciseRepository Repository;
    public ExerciseService(IExerciseRepository repository)
    {
        Repository = repository; 
    }

    public async Task<List<Exercise>> GetAllExercisesAsync()
    {
        return await Repository.GetAllExercisesAsync();
    }

    public async Task CreateExerciseAsync(DateTime dateStart, DateTime dateEnd, TimeSpan duration, string comments)
    {
        await Repository.AddAsync(new Exercise{
            DateStart = dateStart,
            DateEnd = dateEnd,
            Duration = duration,
            Comments = comments
        });
    } 

    public async Task UpdateExerciseAsync(int id, DateTime dateStart, DateTime dateEnd, TimeSpan duration, string comments)
    {
        var exercise = await Repository.GetExerciseByIdAsync(id);
        exercise!.DateStart = dateStart;
        exercise.DateEnd = dateEnd;
        exercise.Duration = duration;
        exercise.Comments = comments;
        
        await Repository.UpdateAsync(exercise);
    }

    public async Task<Exercise?> GetExerciseByIdAsync(int id)
    {
        return await Repository.GetExerciseByIdAsync(id);
    }

    public async Task DeleteExerciseAsync(Exercise exercise)
    {
        await Repository.DeleteAsync(exercise);
    } 
}