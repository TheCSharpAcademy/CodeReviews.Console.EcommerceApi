using ExerciseTracker.Controller;
using ExerciseTracker.Models;
using Microsoft.Data.Sqlite;
using System.Text.Json;

namespace ExerciseTracker.Repositories;

internal class ExerciseRepository : IExerciseRepository
{
    private readonly string _connectionString = Helpers.GetConnectionString();

    public ExerciseRepository()
    {

    }
    public void AddExercise(Exercise exercise)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO exercises (DateStart, DateEnd, Duration, Comment) 
            VALUES (@date_start, @date_end, @duration, @comment)";
            command.Parameters.AddWithValue("@date_start", exercise.DateStart);
            command.Parameters.AddWithValue("@date_end", exercise.DateEnd);
            command.Parameters.AddWithValue("@duration", exercise.Duration);
            command.Parameters.AddWithValue("@comment", exercise.Comment);
            command.ExecuteNonQuery();
        }
    }

    public void DeleteExercise(int id)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM exercises WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }

    public Exercise GetExercise(int id)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM exercises WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Exercise
                    {
                        Id = reader.GetInt32(0),
                        DateStart = reader.GetDateTime(1),
                        DateEnd = reader.GetDateTime(2),
                        Duration = reader.GetTimeSpan(3),
                        Comment = reader.GetString(4)
                    };
                }
            }
        }

        return null;
    }

    public List<Exercise> GetExercises()
    {
        using (var connection  = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM exercises";
            using (var reader = command.ExecuteReader())
            {
                var exercises = new List<Exercise>();
                while (reader.Read())
                {
                    exercises.Add(new Exercise
                    {
                        Id = reader.GetInt32(0),
                        DateStart = reader.GetDateTime(1),
                        DateEnd = reader.GetDateTime(2),
                        Duration = reader.GetTimeSpan(3),
                        Comment = reader.GetString(4)
                    });
                }
                return exercises;
            }
        }
    }

    public void UpdateExercise(int id, Exercise exercise)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"UPDATE exercises SET DateStart = @date_start, DateEnd = @date_end, 
            Duration = @duration, Comment = @comment WHERE id = @id";
            command.Parameters.AddWithValue("@date_start", exercise.DateStart);
            command.Parameters.AddWithValue("@date_end", exercise.DateEnd);
            command.Parameters.AddWithValue("@duration", exercise.Duration);
            command.Parameters.AddWithValue("@comment", exercise.Comment);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}
