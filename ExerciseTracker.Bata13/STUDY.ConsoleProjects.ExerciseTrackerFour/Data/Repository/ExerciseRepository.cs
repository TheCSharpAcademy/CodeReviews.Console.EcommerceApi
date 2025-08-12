using STUDY.ConsoleProjects.ExerciseTrackerFour.Models;

namespace STUDY.ConsoleProjects.ExerciseTrackerFour.Data.Repository;
public class ExerciseRepository : IExerciseRepository
{
    private readonly ExerciseDbContext _context;
    public ExerciseRepository(ExerciseDbContext context)
    {
        _context = context;
    }
    public void AddExerciseEntry(Exercise exercise)
    {
        _context.Exercises.Add(exercise);
        _context.SaveChanges();
    }
    public void DeleteExerciseEntry(int exerciseId)
    {
        var exercise = _context.Exercises.Find(exerciseId);        
        _context.Exercises.Remove(exercise);
        _context.SaveChanges();        
    }
    public void UpdateExerciseEntry(int exerciseId, Exercise newExercise)
    {
        var oldExercise = _context.Exercises.Find(exerciseId);
       
        oldExercise.StarTime = newExercise.StarTime;
        oldExercise.EndTime = newExercise.EndTime;
        oldExercise.Duration = newExercise.Duration;
        oldExercise.Comments = newExercise.Comments;       

        _context.SaveChanges();
    }
    public Exercise ViewSpecificExerciseEntry(int exerciseId)
    {
        var exercise = _context.Exercises.FirstOrDefault(e => e.Id == exerciseId);

        return exercise;
    }
    public Exercise GetExerciseEntryById(int exerciseId)
    {
        return _context.Exercises.Find(exerciseId)!;
    }
    public List<Exercise> GetExercises()
    {
        return _context.Exercises.ToList();
    }
}
