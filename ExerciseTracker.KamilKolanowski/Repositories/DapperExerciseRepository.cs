using Dapper;
using ExerciseTracker.KamilKolanowski.Models;
using ExerciseTracker.KamilKolanowski.Models.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.KamilKolanowski.Repositories;

internal class DapperExerciseRepository : IExerciseRepository
{
    private readonly ExerciseTrackerDbContext _context;
    private readonly string _tableName = "ExerciseTracker.TCSA.Exercises";
    
    public DapperExerciseRepository(ExerciseTrackerDbContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Exercise?> GetExercises()
    {
        var connection = _context.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
            connection.Open();
        
        string query = $"SELECT * FROM {_tableName}";
        return connection.Query<Exercise>(query);
    }

    public Exercise? GetExercise(int id)
    {
        var connection = _context.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
            connection.Open();
        
        string query = $"SELECT * FROM {_tableName} WHERE Id = @Id";
        return connection.QuerySingleOrDefault<Exercise>(query, new { id });
    }

    public void Insert(Exercise exercise)
    {
        var connection = _context.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
            connection.Open();
        
        string query = $"INSERT INTO {_tableName} (Name, DateStart, DateEnd, Comment) VALUES (@Name, @DateStart, @DateEnd, @Comment)";
        connection.Execute(query, new { exercise.Name, exercise.DateStart, exercise.DateEnd, exercise.Comment });
    }

    public void Update(Exercise exercise)
    {
        var connection = _context.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
            connection.Open();
        
        string query = $"UPDATE {_tableName} " +
                       "SET Name = @Name, DateStart = @DateStart, DateEnd = @DateEnd, Comment = @Comment " +
                       "WHERE Id = @Id";
        connection.Execute(query,
            new { exercise.Id, exercise.Name, exercise.DateStart, exercise.DateEnd, exercise.Comment });
    }

    public void Delete(int id)
    {
        var connection = _context.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
            connection.Open();
        
        string query = $"DELETE FROM {_tableName} WHERE Id = @Id";
        connection.Execute(query, new { id });
    }

    public void Save()
    {
        throw new NotImplementedException();
    }
}