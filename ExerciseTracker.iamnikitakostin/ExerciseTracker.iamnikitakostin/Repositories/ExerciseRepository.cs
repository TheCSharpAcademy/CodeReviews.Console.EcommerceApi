using ExerciseTracker.iamnikitakostin.Data;
using ExerciseTracker.iamnikitakostin.Models;

namespace ExerciseTracker.iamnikitakostin.Repositories;
internal class ExerciseRepository : IExerciseRepository
{
    private readonly DataContext _context;
    public ExerciseRepository(DataContext context)
    {
        _context = context;
    }

    public List<Exercise> GetAll()
    {
        List<Exercise> exercises = _context.Exercises.ToList();
        return exercises;
    }

    public Dictionary<int, string> GetAllAsDictionary()
    {
        Dictionary<int, string> exercises = _context.Exercises.ToDictionary(e => e.Id, e => $"{e.DateStart} - {e.DateStart}");
        return exercises;
    }

    public Exercise GetById(int id)
    {
        Exercise exercise = _context.Exercises.FirstOrDefault(e => e.Id == id);
        return exercise;
    }
    public bool Add(Exercise exercise) { 
        _context.Exercises.Add(exercise);
        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id) {
        Exercise exercise = GetById(id);

        if (exercise == null) {
            return false;
        }

        _context.Exercises.Remove(exercise);
        _context.SaveChanges();

        return true;
    }

    public bool Update(Exercise exercise) {
        Exercise savedExercise = GetById(exercise.Id);

        if (savedExercise == null)
        {
            return false;
        }

        savedExercise.DateStart = exercise.DateStart;
        savedExercise.DateEnd = exercise.DateEnd;
        savedExercise.Duration = exercise.Duration;
        savedExercise.Comments = exercise.Comments;

        _context.SaveChanges();
        return true;
    }
}
