using Dapper;
using ExerciseTrackerAPI.Data;
using ExerciseTrackerAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ExerciseTrackerAPI.Repositories.ExerciseRepositories;

public class DapperExerciseRepository : IExerciseRepository
{
    public IEnumerable<Exercise> GetAll()
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        var query = @"
            SELECT * FROM Exercises ORDER BY StartTime";
        return connection.Query<Exercise>(query).ToList();
    }

    public Exercise? GetById(int id)
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        var exercise = connection.Query<Exercise>(@"
            SELECT * FROM Exercises WHERE Id = @Id", parameters).FirstOrDefault();
        return exercise;
    }

    public void Create(Exercise exercise)
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var parameters = new DynamicParameters();
        parameters.Add("@ExerciseTypeName", exercise.ExerciseTypeName);
        parameters.Add("@StartTime", exercise.StartTime);
        parameters.Add("@EndTime", exercise.EndTime);
        parameters.Add("@Duration", exercise.Duration);
        parameters.Add("@Comments", exercise.Comments);

        var result = connection.Query<int>(@"
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
            SELECT SCOPE_IDENTITY();", parameters);
        exercise.Id = result.FirstOrDefault();
    }

    public void Update(int id, Exercise exercise)
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);
        parameters.Add("@ExerciseTypeName", exercise.ExerciseTypeName);
        parameters.Add("@StartTime", exercise.StartTime);
        parameters.Add("@EndTime", exercise.EndTime);
        parameters.Add("@Duration", exercise.Duration);
        parameters.Add("@Comments", exercise.Comments);

        connection.Execute(@"
            UPDATE Exercises
            SET
                ExerciseTypeName = @ExerciseTypeName,
                StartTime = @StartTime,
                EndTime = @EndTime,
                Duration = @Duration,
                Comments = @Comments
            WHERE Id = @Id", parameters);
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        connection.Execute(@"
            DELETE FROM Exercises WHERE Id = @Id", parameters);
    }
}
