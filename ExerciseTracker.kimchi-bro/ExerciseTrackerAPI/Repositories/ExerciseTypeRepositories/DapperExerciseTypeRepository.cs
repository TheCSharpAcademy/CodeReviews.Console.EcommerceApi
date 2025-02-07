using Dapper;
using ExerciseTrackerAPI.Data;
using ExerciseTrackerAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ExerciseTrackerAPI.Repositories.ExerciseTypeRepositories;

public class DapperExerciseTypeRepository : IExerciseTypeRepository
{
    public IEnumerable<ExerciseType> GetAll()
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        var query = @"
            SELECT * FROM ExerciseTypes ORDER BY Name";
        return connection.Query<ExerciseType>(query).ToList();
    }

    public ExerciseType? GetById(int id)
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        var exercise = connection.Query<ExerciseType>(@"
            SELECT * FROM ExerciseTypes WHERE Id = @Id", parameters).FirstOrDefault();
        return exercise;
    }

    public void Create(ExerciseType exerciseType)
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var parameters = new DynamicParameters();
        parameters.Add("@Name", exerciseType.Name);
        var result = connection.Query<int>(@"
            INSERT INTO ExerciseTypes (Name)
            VALUES (@Name);
            SELECT SCOPE_IDENTITY()", parameters);
        exerciseType.Id = result.FirstOrDefault();
    }

    public void Update(int id, ExerciseType exerciseType)
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);
        parameters.Add("@Name", exerciseType.Name, DbType.String);

        connection.Execute(@"
            UPDATE ExerciseTypes
            SET Name = @Name
            WHERE Id = @Id", parameters);
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        connection.Execute(@"
            DELETE FROM ExerciseTypes WHERE Id = @Id", parameters);
    }
}
