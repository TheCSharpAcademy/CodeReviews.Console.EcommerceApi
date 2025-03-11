
namespace ExerciseTracker.cacheMe512;

internal interface IExerciseRepository
{
    IEnumerable<Exercise> GetAll();
    Exercise GetById(int id);
    void Add(Exercise exercise);
    void Update(Exercise excercise);
    void Delete(Exercise exercise);
}

internal class ExerciseRepository : IExerciseRepository
{
    private readonly ExerciseContext _context;

    public ExerciseRepository(ExerciseContext context)
    {
        _context = context;
    }

    public IEnumerable<Exercise> GetAll() =>
        _context.Exercises.ToList();

    public Exercise GetById(int id) =>
        _context.Exercises.SingleOrDefault(e => e.Id == id);

    public void Add(Exercise exercise)
    {
        _context.Exercises.Add(exercise);
        _context.SaveChanges();
    }

    public void Update(Exercise exercise)
    {
        var existingExercise = _context.Exercises.Find(exercise.Id);
        if (existingExercise == null)
        {
            throw new InvalidOperationException("Exercise not found.");
        }

        _context.Entry(existingExercise).CurrentValues.SetValues(exercise);
        _context.SaveChanges();
    }

    public void Delete(Exercise exercise)
    {
        _context.Exercises.Remove(exercise);
        _context.SaveChanges();
    }
}
