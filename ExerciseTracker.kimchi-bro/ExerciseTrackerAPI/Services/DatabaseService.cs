using ExerciseTrackerAPI.Data;
using ExerciseTrackerAPI.ErrorHandlers;
using Microsoft.Data.SqlClient;

namespace ExerciseTrackerAPI.Services;

public static class DatabaseService
{
    public static void Initialize()
    {
        try
        {
            using var serverConnection = new SqlConnection(ApplicationDbConnection.ServerConnection);
            serverConnection.Open();
            var command = serverConnection.CreateCommand();
            command.CommandText = $@"
                IF NOT EXISTS (
                    SELECT * FROM sys.databases WHERE name = '{ApplicationDbConnection.DatabaseName}')
                CREATE DATABASE {ApplicationDbConnection.DatabaseName}";
            command.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            ErrorHandler.SqlError(ex);
            ErrorHandler.EnvExit();
        }
        catch (Exception ex)
        {
            ErrorHandler.GeneralError(ex);
            ErrorHandler.EnvExit();
        }
    }
}
