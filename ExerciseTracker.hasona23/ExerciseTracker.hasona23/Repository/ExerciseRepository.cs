using ExerciseTracker.hasona23.Data;
using ExerciseTracker.hasona23.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.hasona23.Repository;

public class ExerciseRepository : IExerciseRepository
{ 
    private readonly ExerciseTrackerContext _context;

    public ExerciseRepository()
    {
        _context = new ExerciseTrackerContext();
        _context.Database.Migrate();
    }

    public List<Exercise> GetAllExercises()
    {
        return _context.Exercises.ToList();
    }

    public Exercise? GetExercise(int id)
    {
        return _context.Exercises.FirstOrDefault(e => e.Id == id);
    }

    public void AddExercise(ExerciseCreate exercise)
    {
        _context.Exercises.Add(new Exercise(exercise));
        _context.SaveChanges();
    }

    public bool UpdateExercise(ExerciseUpdate newExercise)
    {
        var exercise = _context.Exercises.FirstOrDefault(e => e.Id == newExercise.Id);
        if (exercise is null)
        {
            return false;
        }
        if (!string.IsNullOrEmpty(newExercise.Description))
            exercise.Description = newExercise.Description;
        if(newExercise.Start != DateTime.MinValue)
            exercise.Start = newExercise.Start;
        if(newExercise.End != DateTime.MinValue)
            exercise.End = newExercise.End;
        _context.SaveChanges();
        return true;
    }

    public bool DeleteExercise(int id)
    {
        var exercise = _context.Exercises.FirstOrDefault(e => e.Id == id);
        if(exercise is null)
            return false;
        _context.Exercises.Remove(exercise);
        _context.SaveChanges();
        return true;
    }
    
}