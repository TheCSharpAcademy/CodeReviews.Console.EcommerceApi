namespace ExerciseTrackerAPI.Services.ExerciseServices;

public class ExerciseService : IExerciseService
{
    private readonly DataContext _context;

    public ExerciseService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Exercise>> AddExercise(Exercise exercise)
    {
        _context.Exercises.Add(exercise);
        await _context.SaveChangesAsync();

        return await _context.Exercises.ToListAsync();
    }

    public async Task<List<Exercise>> Delete(int id)
    {
        var dbExercise = await _context.Exercises.FindAsync(id);
        if (dbExercise == null)
            return new List<Exercise>();

        _context.Exercises.Remove(dbExercise);
        await _context.SaveChangesAsync();

        return await _context.Exercises.ToListAsync();
    }

    public async Task<List<Exercise>> DeleteExercisesByCustomerId(int customerId)
    {
        var exercises = await _context.Exercises.Where(e => e.CustomerId == customerId).ToListAsync();

        if (exercises.Count == 0)
            return new List<Exercise>();

        _context.Exercises.RemoveRange(exercises);
        await _context.SaveChangesAsync();

        return await _context.Exercises.ToListAsync();
    }

    public async Task<Exercise> Get(int id)
    {
        var exercise = await _context.Exercises.FindAsync(id);
        if (exercise == null)
            return null;

        return exercise;
    }

    public async Task<List<Exercise>> GetAll()
    {
        return await _context.Exercises.ToListAsync();
    }

    public async Task<List<Exercise>> GetExercisesByCustomerId(int customerId)
    {
        var exercises = await _context.Exercises.Where(e => e.CustomerId == customerId).ToListAsync();
        if (exercises.Count == 0)
            return null;

        return exercises;
    }

    public async Task<List<Exercise>> UpdateExercise(int id, Exercise request
        )
    {
        var exercise = await _context.Exercises.FindAsync(id);
        if (exercise == null)
            return null;

        exercise.DateStart = request.DateStart;
        exercise.DateEnd = request.DateEnd;
        exercise.Repetitions = request.Repetitions;
        exercise.Comments = request.Comments;

        return await _context.Exercises.ToListAsync();
    }
}
