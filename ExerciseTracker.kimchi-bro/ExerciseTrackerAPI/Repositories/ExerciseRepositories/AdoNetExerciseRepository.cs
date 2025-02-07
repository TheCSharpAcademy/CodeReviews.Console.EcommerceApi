using ExerciseTrackerAPI.Data;
using ExerciseTrackerAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ExerciseTrackerAPI.Repositories.ExerciseRepositories;

public class AdoNetExerciseRepository : IExerciseRepository
{
    public IEnumerable<Exercise> GetAll()
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Exercises ORDER BY StartTime";

        SqlDataReader reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            var list = new List<Exercise>();

            while (reader.Read())
            {
                list.Add(
                    new Exercise
                    {
                        Id = reader.GetInt32(0),
                        ExerciseTypeName = reader.GetString(1),
                        StartTime = reader.GetDateTime(2),
                        EndTime = reader.GetDateTime(3),
                        Duration = reader.GetTimeSpan(4),
                        Comments = reader.GetString(5)
                    });
            }
            return list;
        }
        else
        {
            return [];
        }
    }
    public Exercise? GetById(int id)
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Exercises WHERE Id = @Id";
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Exercise
            {
                Id = reader.GetInt32(0),
                ExerciseTypeName = reader.GetString(1),
                StartTime = reader.GetDateTime(2),
                EndTime = reader.GetDateTime(3),
                Duration = reader.GetTimeSpan(4),
                Comments = reader.GetString(5)
            };
        }
        else
        {
            return null;
        }
    }

    public void Create(Exercise exercise)
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Exercises (
                ExerciseTypeName,
                StartTime,
                EndTime,
                Duration,
                Comments)
            VALUES (
                @ExerciseTypeName,
                @StartTime,
                @EndTime,
                @Duration,
                @Comments);
            SELECT SCOPE_IDENTITY()";
        command.Parameters.Add("@ExerciseTypeName", SqlDbType.NVarChar).Value = exercise.ExerciseTypeName;
        command.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = exercise.StartTime;
        command.Parameters.Add("@EndTime", SqlDbType.DateTime).Value = exercise.EndTime;
        command.Parameters.Add("@Duration", SqlDbType.Time).Value = exercise.Duration;
        command.Parameters.Add("@Comments", SqlDbType.NVarChar).Value = exercise.Comments;

        var result = command.ExecuteScalar();
        if (result != DBNull.Value)
        {
            exercise.Id = (int)(decimal)result;
        }
    }

    public void Update(int id, Exercise exercise)
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Exercises
            SET
                ExerciseTypeName = @ExerciseTypeName,
                StartTime = @StartTime,
                EndTime = @EndTime,
                Duration = @Duration,
                Comments = @Comments
            WHERE Id = @Id";
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        command.Parameters.Add("@ExerciseTypeName", SqlDbType.NVarChar).Value = exercise.ExerciseTypeName;
        command.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = exercise.StartTime;
        command.Parameters.Add("@EndTime", SqlDbType.DateTime).Value = exercise.EndTime;
        command.Parameters.Add("@Duration", SqlDbType.Time).Value = exercise.Duration;
        command.Parameters.Add("@Comments", SqlDbType.NVarChar).Value = exercise.Comments;
        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"
            DELETE FROM Exercises WHERE Id = @Id";
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        command.ExecuteNonQuery();
    }
}
