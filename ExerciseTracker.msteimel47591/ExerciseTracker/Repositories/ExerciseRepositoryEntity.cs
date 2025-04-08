using ExerciseTracker.Data;
using ExerciseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Repositories;


internal class ExerciseRepositoryEntity : IExerciseRepository
{
    private readonly ExerciseContext _context;

    public ExerciseRepositoryEntity()
    {
        _context = new ExerciseContext();
    }

    public void AddExercise(Exercise exercise)
    {
        _context.Exercises.Add(exercise);
        _context.SaveChanges();
    }

    public void DeleteExercise(int id)
    {
        var exercise = _context.Exercises.Find(id);
        if (exercise != null)
        {
            _context.Exercises.Remove(exercise);
            _context.SaveChanges();
        }
    }

    public Exercise GetExercise(int id)
    {
        return _context.Exercises.Find(id);
    }

    public List<Exercise> GetExercises()
    {
        return _context.Exercises.ToList();
    }

    public void UpdateExercise(int id, Exercise exercise)
    {
        var existingExercise = _context.Exercises.Find(id);

        exercise.Id = id;
        _context.Entry(existingExercise).CurrentValues.SetValues(exercise);
        _context.SaveChanges();
    }   
}

