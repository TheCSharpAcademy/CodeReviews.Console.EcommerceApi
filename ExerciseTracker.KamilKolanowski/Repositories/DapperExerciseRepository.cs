using Dapper;
using ExerciseTracker.KamilKolanowski.Interfaces;
using ExerciseTracker.KamilKolanowski.Models;
using ExerciseTracker.KamilKolanowski.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.KamilKolanowski.Repositories;

public class DapperExerciseRepository : IExerciseRepository
{
    private readonly ExerciseTrackerDbContext _context;
    private readonly string _tableName = "ExerciseTracker.TCSA.Exercises";
    
    public DapperExerciseRepository(ExerciseTrackerDbContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Exercise?> GetExercises(string type)
    {
        var connection = _context.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
            connection.Open();
        
        string query = $"SELECT * FROM {_tableName} WHERE ExerciseType = @type";
        return connection.Query<Exercise>(query, new { type });
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
        
        string query = $"INSERT INTO {_tableName} (Name, DateStart, DateEnd, Comment, ExerciseType) VALUES (@Name, @DateStart, @DateEnd, @Comment, @ExerciseType)";
        connection.Execute(query, new { exercise.Name, exercise.DateStart, exercise.DateEnd, exercise.Comment, exercise.ExerciseType });
    }

    public void Update(Exercise exercise)
    {
        var connection = _context.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
            connection.Open();
        
        string query = $"UPDATE {_tableName} " +
                       "SET Name = @Name, DateStart = @DateStart, DateEnd = @DateEnd, Comment = @Comment, ExerciseType = @ExerciseType " +
                       "WHERE Id = @Id";
        connection.Execute(query,
            new { exercise.Id, exercise.Name, exercise.DateStart, exercise.DateEnd, exercise.Comment, exercise.ExerciseType });
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
        throw new System.NotImplementedException();
    }
}