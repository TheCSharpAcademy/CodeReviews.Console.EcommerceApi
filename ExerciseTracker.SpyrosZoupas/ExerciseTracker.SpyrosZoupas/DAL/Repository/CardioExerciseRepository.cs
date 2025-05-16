
using Dapper;
using Microsoft.Data.SqlClient;

namespace ExerciseTracker.SpyrosZoupas.DAL.Repository;

public class CardioExerciseRepository<TEntity> : IRepository<TEntity>, IRepositoryDapper<TEntity> where TEntity : class, new()
{
    private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["exerciseTracker"].ConnectionString;
    public CardioExerciseRepository()
    {
    }

    public void CreateTables()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"IF NOT EXISTS (
                    SELECT * FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_NAME = 'CardioExercises' AND TABLE_SCHEMA = 'dbo'
                )
                BEGIN
                    CREATE TABLE dbo.CardioExercises (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        DateStart DATETIME2(7),
                        DateEnd DATETIME2(7),
                        Comments NVARCHAR(MAX),
                        Speed FLOAT
                    );
                END;";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    public IEnumerable<TEntity> GetAll()
    {
        List<TEntity> result = new List<TEntity>();
        string sql = $"SELECT * FROM CardioExercises";
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            result = connection.Query<TEntity>(sql).ToList();
            connection.Close();
        }
        return result;
    }

    public TEntity GetById(int id)
    {
        TEntity result;
        string sql = $"SELECT * FROM CardioExercises WHERE Id = @Id";
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            result = connection.QuerySingleOrDefault<TEntity>(sql, new { Id = id });
            connection.Close();
        }

        return result;
    }

    public void Insert(TEntity entity)
    {
        var properties = typeof(TEntity).GetProperties()
            .Where(p => p.Name != "Id" && p.Name != "Duration")
            .ToArray();

        var columns = string.Join(", ", properties.Select(p => $"{p.Name}"));
        var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));

        string sql = $"INSERT INTO CardioExercises ({columns}) VALUES ({values})";
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            connection.Execute(sql, entity);
            connection.Close();
        }
    }

    public void Update(TEntity entity)
    {
        var properties = typeof(TEntity).GetProperties()
            .Where(p => p.Name != "Id" && p.Name != "Duration")
            .ToArray();

        var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
        var id = typeof(TEntity).GetProperty("Id")?.GetValue(entity);

        string sql = $"UPDATE CardioExercises SET {setClause} WHERE Id = @Id";
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            connection.Execute(sql, new { Id = id });
            connection.Close();
        }
    }

    public void Delete(TEntity entity)
    {
        string sql = $"DELETE FROM CardioExercises WHERE Id = @Id";
        var id = typeof(TEntity).GetProperty("ExerciseId")?.GetValue(entity);
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            connection.Execute(sql, new { Id = id });
            connection.Close();
        }
    }
}

