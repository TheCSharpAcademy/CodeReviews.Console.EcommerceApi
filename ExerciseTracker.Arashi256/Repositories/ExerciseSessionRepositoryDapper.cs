using Dapper;
using ExerciseTracker.Arashi256.Classes;
using ExerciseTracker.Arashi256.Config;
using ExerciseTracker.Arashi256.Interfaces;
using ExerciseTracker.Arashi256.Models;
using Microsoft.Data.SqlClient;

namespace ExerciseTracker.Arashi256.Repositories
{
    public class ExerciseSessionRepositoryDapper : IExerciseSessionRepository
    {
        private readonly string _connectionString;

        public ExerciseSessionRepositoryDapper(AppSettings appSettings)
        {
            _connectionString = appSettings.DatabaseConnectionString ?? throw new ArgumentNullException(nameof(appSettings.DatabaseConnectionString), "HALT ERROR: Connection string cannot be null.");
            EnsureModelTableCreated();
        }

        private void EnsureModelTableCreated()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    // Check if the table for the ExerciseSession exists.
                    var tableExists = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ExerciseSessions'");
                    // If the table does not exist, create it.
                    if (tableExists == 0)
                    {
                        connection.Execute(@"CREATE TABLE dbo.ExerciseSessions (
                                        Id INT IDENTITY(1,1) PRIMARY KEY,
                                        Type NVARCHAR(25) NOT NULL,
                                        DateTimeStart DATETIME2 NOT NULL,
                                        DateTimeEnd DATETIME2 NOT NULL,
                                        Duration TIME NOT NULL,
                                        Comments NVARCHAR(255));");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"HALT ERROR: ExerciseSession table does not exist and could not create it: '{ex.Message}'");
                }
            }
        }

        public ServiceResponse AddExerciseSession(ExerciseSession exercise)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string insertQuery = "INSERT INTO dbo.ExerciseSessions(Type, DateTimeStart, DateTimeEnd, Duration, Comments) VALUES (@Type, @DateTimeStart, @DateTimeEnd, @Duration, @Comments)";
                    int rows = connection.Execute(insertQuery, exercise);
                    if (rows > 0)
                        return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", exercise);
                    else
                        return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "Could not add exercise session", null);
                }
            }
            catch (Exception ex)
            {
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, ex.Message, null);
            }
        }

        public ServiceResponse DeleteExerciseSession(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string deleteQuery = "DELETE FROM dbo.ExerciseSessions WHERE Id = @Id";
                    int rows = connection.Execute(deleteQuery, new { Id = id });
                    if (rows > 0)
                        return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", null);
                    else
                        return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "Could not dete exercise session", null);
                }
            }
            catch (Exception ex)
            {
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, ex.Message, null);
            }
        }

        public ServiceResponse GetExerciseSessionById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string selectQuery = "SELECT * FROM ExerciseSessions WHERE Id = @id";
                    var session = connection.QuerySingleOrDefault<ExerciseSession>(selectQuery, new { id });
                    if (session != null)
                        return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", session);
                    else
                        return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "Exercise session not found", null);
                }
            }
            catch (Exception ex)
            {
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, ex.Message, null);
            }
        }

        public ServiceResponse GetExerciseSessions()
        {
            List<ExerciseSession> results = new List<ExerciseSession>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    string selectQuery = "SELECT * from dbo.ExerciseSessions";
                    results = connection.Query<ExerciseSession>(selectQuery).AsList();
                    if (results == null || results.Count == 0)
                        ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "No exercise sessions found", results);
                }
                catch (Exception ex)
                {
                    ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, ex.Message, null);
                }
            }
            return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", results);
        }

        public ServiceResponse UpdateExerciseSession(int id, ExerciseSession exercise)
        {
            if (id != exercise.Id)
            {
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "Update exercise session and id mismatch", null);
            }
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    // Check if the session exists in the database
                    string selectQuery = "SELECT * FROM ExerciseSessions WHERE Id = @id";
                    var existingSession = connection.QuerySingleOrDefault<ExerciseSession>(selectQuery, new { id });
                    if (existingSession == null)
                        return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "Exercise session not found", null);
                    string updateQuery = "UPDATE ExerciseSessions SET Type = @Type, DateTimeStart = @DateTimeStart, DateTimeEnd = @DateTimeEnd, Duration = @Duration, Comments = @Comments WHERE Id = @Id";
                    int rows = connection.Execute(updateQuery, exercise);
                    if (rows > 0)
                        return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", exercise);
                    else
                        return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "Could not update exercise session", null);
                }
            }
            catch (Exception ex)
            {
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, ex.Message, null);
            }
        }

        public ServiceResponse ExerciseSessionExistsInRange(DateTime startDate, DateTime endDate, int? sessionIdToExclude = null)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string existsQuery = sessionIdToExclude.HasValue
                        ? "SELECT CASE WHEN EXISTS (SELECT 1 FROM ExerciseSessions WHERE Id != @sessionIdToExclude AND @startDate < DateTimeEnd AND @endDate > DateTimeStart) THEN 1 ELSE 0 END"
                        : "SELECT CASE WHEN EXISTS (SELECT 1 FROM ExerciseSessions WHERE @startDate < DateTimeEnd AND @endDate > DateTimeStart) THEN 1 ELSE 0 END";
                    bool isOverlapping = connection.ExecuteScalar<int>(existsQuery, new { startDate, endDate, sessionIdToExclude }) == 1;
                    return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", isOverlapping);
                }
            }
            catch (Exception ex)
            {
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, ex.Message, null);
            }
        }
    }
}
