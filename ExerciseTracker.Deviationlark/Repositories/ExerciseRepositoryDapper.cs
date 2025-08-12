using System.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ExerciseTracker;

public class ExerciseRepositoryDapper : IExerciseRepository
{
    public string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
    public void AddExercise(Exercise exercise)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string sql = @"INSERT INTO Exercise (StartDate, EndDate, Duration, Comments) 
            Values (@StartDate, @EndDate, @Duration, @Comments)";
            int rowsAffected = connection.Execute(sql, exercise);
            Console.WriteLine("rows affected: " + rowsAffected);
        }
        
    }

    public void DeleteExercise(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string sql = "DELETE FROM Exercise WHERE Id = @Id";
            connection.Execute(sql, new { Id = id});
        }
        
    }

    public Exercise GetExerciseById(int id)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string sql = $"SELECT * FROM Exercise WHERE Id = {id}";
            var exercise = connection.QuerySingle<Exercise>(sql);
            return exercise;
        }
    }

    public IEnumerable<Exercise> GetExercises()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string sql = $"SELECT * FROM Exercise";
            var exercises = connection.Query<Exercise>(sql).ToList();
            return exercises;
        }
    }

    public void Save()
    {
        // unused for dapper.
    }

    public void UpdateExercise(Exercise exercise)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string sql = $@"UPDATE Exercise SET 
            StartDate = @StartDate, EndDate = @EndDate, Duration = @Duration,
            Comments = @Comments WHERE Id = @Id";
            connection.Execute(sql, exercise);
        }
    }
}