using Dapper;
using Exercisetacker.Entities;
using Exercisetacker.Services;
using Microsoft.Data.SqlClient;

namespace Exercisetacker.Data;

public class JoggingDapperDbContext
{
    private string ConnectionString { get; init; }

    public JoggingDapperDbContext(string connectionString)
    {
        ConnectionString = connectionString;
        CreateTable();
    }

    private SqlConnection? GetConnection()
    {
        return new SqlConnection(ConnectionString);
    }

    private void CreateTable()
    {
        var sql = @"IF OBJECT_ID(N'[dbo].[Joggings]', 'U') IS NULL
                Create Table Joggings (
                    Id INT IDENTITY (1,1) NOT NULL,
                    DateStart DateTime2 NOT NULL,
                    DateEnd DateTime2 NOT NULL,
                    Duration TIME NOT NULL,
                    Comments VARCHAR(255) NOT NULL,
                    CONSTRAINT [PK_Joggings] Primary Key CLUSTERED ([Id] Asc)
                )";
        using (var connection = GetConnection())
        {
            connection.Open();
            connection.Execute(sql);
        }
    }

    public async Task<List<Jogging>> GetAllAsync()
    {
        var sql = @$"Select * from Joggings ORDER BY Id Asc;";
        using (var connection = GetConnection())
        {
            connection.Open();
            var result = await connection.QueryAsync<Jogging>(sql);
            return result.ToList();
        }
    }

    public async Task<Jogging> FirstOrDefaultAsync(int id)
    {
        var sql = @$"Select * from Joggings WHERE Id = @Id;";
        using (var connection = GetConnection())
        {
            connection.Open();
            var result = await connection.QueryFirstOrDefaultAsync<Jogging>(sql, new { Id = id });
            return result;
        }
    }

    internal async Task<Jogging> Add(Jogging parameters)
    {
        var sql = @$"INSERT INTO Joggings 
                        (DateStart, DateEnd, Duration, Comments) 
                        VALUES(@DateStart, @DateEnd, @Duration, @Comments);
                        SELECT CAST(SCOPE_IDENTITY() as int);";
        using (var connection = GetConnection())
        {
            connection.Open();
            var newId = await connection.ExecuteScalarAsync<int>(sql, parameters);
            var addedJogging = await FirstOrDefaultAsync(newId);
            return addedJogging;
        }
    }

    internal async Task<int> Update(Jogging parameters)
    {
        var sql = @$"UPDATE Joggings 
                    SET 
                        DateStart = @DateStart, 
                        DateEnd   = @DateEnd, 
                        Duration  = @Duration,
                        Comments  = @Comments
                    WHERE Id = @Id
                    ";
        using (var connection = GetConnection())
        {
            connection.Open();
            return await connection.ExecuteAsync(sql, parameters);
        }
    }

    internal async Task<int> Remove(Jogging parameters)
    {
        var sql = @$"DELETE FROM Joggings 
                    WHERE Id = @Id
                    ";
        using (var connection = GetConnection())
        {
            connection.Open();
            return await connection.ExecuteAsync(sql, parameters);
        }
    }



}