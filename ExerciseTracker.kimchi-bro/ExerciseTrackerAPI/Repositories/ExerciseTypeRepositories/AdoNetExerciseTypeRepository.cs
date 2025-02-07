using ExerciseTrackerAPI.Data;
using ExerciseTrackerAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ExerciseTrackerAPI.Repositories.ExerciseTypeRepositories;

public class AdoNetExerciseTypeRepository : IExerciseTypeRepository
{
    public IEnumerable<ExerciseType> GetAll()
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM ExerciseTypes ORDER BY Name";

        SqlDataReader reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            var list = new List<ExerciseType>();

            while (reader.Read())
            {
                list.Add(
                    new ExerciseType
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
            }
            return list;
        }
        else
        {
            return [];
        }
    }

    public ExerciseType? GetById(int id)
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM ExerciseTypes WHERE Id = @Id";
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new ExerciseType
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            };
        }
        else
        {
            return null;
        }
    }

    public void Create(ExerciseType exerciseType)
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO ExerciseTypes (Name)
            VALUES (@Name);
            SELECT SCOPE_IDENTITY()";
        command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = exerciseType.Name;

        var result = command.ExecuteScalar();
        if (result != DBNull.Value)
        {
            exerciseType.Id = (int)(decimal)result;
        }
    }

    public void Update(int id, ExerciseType exerciseType)
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE ExerciseTypes
            SET Name = @Name
            WHERE Id = @Id";
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = exerciseType.Name;
        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(ApplicationDbConnection.ConnectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"
            DELETE FROM ExerciseTypes WHERE Id = @Id";
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
        command.ExecuteNonQuery();
    }
}
