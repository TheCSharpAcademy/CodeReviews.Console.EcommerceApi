using System.Configuration;
using Dapper;
using ExerciseTracker.wkktoria.Data.Models;
using Microsoft.Data.SqlClient;

namespace ExerciseTracker.wkktoria.Data.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private readonly AppDbContext _context;

    public ExerciseRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Exercise> GetAllExercises()
    {
        var exercises = new List<Exercise>();

        try
        {
            using var connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var reader = connection.ExecuteReader("SELECT Id, StartDate, EndDate, Comment FROM Exercises");

            while (reader.Read())
                exercises.Add(new Exercise
                {
                    Id = reader.GetInt32(0),
                    StartDate = reader.GetDateTime(1),
                    EndDate = reader.GetDateTime(2),
                    Duration = reader.GetDateTime(2) - reader.GetDateTime(1),
                    Comment = reader.GetString(3)
                });

            return exercises;
        }
        catch (Exception e)
        {
            throw new Exception($"Couldn't retrieve entities: {e.Message}");
        }
    }

    public Exercise? GetExercise(int id)
    {
        try
        {
            return _context.Exercises.Find(id);
        }
        catch (Exception e)
        {
            throw new Exception($"Couldn't retrieve entity: {e.Message}");
        }
    }

    public Exercise AddExercise(Exercise exercise)
    {
        if (exercise == null) throw new ArgumentNullException(nameof(exercise));

        try
        {
            _context.Add(exercise);
            _context.SaveChanges();

            return exercise;
        }
        catch (Exception e)
        {
            throw new Exception($"{nameof(exercise)} could not be saved: {e.Message}");
        }
    }

    public Exercise UpdateExercise(Exercise exercise)
    {
        if (exercise == null)
            throw new ArgumentNullException(nameof(exercise));

        try
        {
            _context.Update(exercise);
            _context.SaveChanges();

            return exercise;
        }
        catch (Exception e)
        {
            throw new Exception($"{nameof(exercise)} could not be updated: {e.Message}");
        }
    }

    public void DeleteExercise(int id)
    {
        try
        {
            var exercise = _context.Exercises.Find(id);

            if (exercise == null) throw new ArgumentNullException(nameof(exercise));

            _context.Remove(exercise);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new Exception($"Entity with id '{id}' could not be deleted: {e.Message}");
        }
    }
}