using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace ExerciseTracker.K_MYR;

public class DapperContext
{
    private readonly string _connectionString;
    public DapperContext()
    {
        _connectionString = "Data Source = ExerciseTracker.db";
        CreateTables();
    }

    private async void CreateTables()
    {
        try
        {
            var sql = @"CREATE TABLE IF NOT EXISTS Exercises 
        ( 
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Type String,
            StartTime String,
            EndTime String,
            Duration INTEGER,
            Comments String 
        )";

            using var connection = CreateConnection();
            await connection.ExecuteAsync(sql);
        }
        catch (Exception ex)
        {
            throw new Exception($"Couldn't create database: {ex.Message}");
        }
    }

    public IDbConnection CreateConnection() => new SqliteConnection(_connectionString);
}
