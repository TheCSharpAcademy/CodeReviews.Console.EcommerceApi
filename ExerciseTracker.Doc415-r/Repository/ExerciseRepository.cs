using exerciseTracker.doc415.context;
using exerciseTracker.doc415.Models;
using Microsoft.EntityFrameworkCore;

namespace exerciseTracker.doc415.Repository;

internal class ExerciseRepository : IExerciseRepository
{



    public IEnumerable<Exercise> GetAll()
    {
        using var _context = new ExerciseDbContext();
        var exerciseList = _context.Exercies.ToList();
        return exerciseList;
    }
    public Exercise GetById(int id)
    {
        using var _context = new ExerciseDbContext();
        Exercise exercise = _context.Exercies.Find(id);
        return exercise;
    }
    public void Delete(int id)
    {
        using var _context = new ExerciseDbContext();
        var exerciseToDelete = _context.Exercies.Single(x => x.Id == id);
        if (exerciseToDelete != null)
            _context.Remove(exerciseToDelete);
        _context.SaveChanges();
    }
    public void Insert(Exercise exercise)
    {
        using var _context = new ExerciseDbContext();
        _context.Exercies.Add(exercise);
        _context.SaveChanges();
    }
    public void Update(Exercise exercise)
    {
        using var _context = new ExerciseDbContext();
        var exerciseToUpdate = _context.Exercies.Find(exercise.Id);
        if (exerciseToUpdate != null)
        {
            exerciseToUpdate.DateEnd = exercise.DateEnd;
            exerciseToUpdate.DateStart = exercise.DateStart;
            exerciseToUpdate.Duration = exercise.Duration;
            exerciseToUpdate.Comments = exercise.Comments;
        }
        _context.Exercies.Update(exerciseToUpdate);
        _context.Entry(exerciseToUpdate).State = EntityState.Modified;
        _context.SaveChanges();

    }

}
