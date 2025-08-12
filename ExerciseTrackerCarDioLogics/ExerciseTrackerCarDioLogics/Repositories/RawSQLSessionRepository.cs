using ExerciseTrackerCarDioLogics.Models;
using Microsoft.Data.SqlClient;

namespace ExerciseTrackerCarDioLogics.Repositories;

public class RawSQLSessionRepository : ISessionRepository
{
    //implementation of the ExSession interface into RawSQL
    private readonly string _connectionString;

    public RawSQLSessionRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void CreateDatabase()
    {
        //Could not implement SQL to create database. Got error denying me access.
    }

    public Session GetSessionById(int id)
    {
        using ( var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT Id, DateStart, DateEnd, Duration, Comment FROM ExSessions WHERE Id = @Id"; //what is name where??

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using( var reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        return new Session
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            DateStart = Convert.ToDateTime(reader["DateStart"]),
                            DateEnd = Convert.ToDateTime(reader["DateEnd"]),
                            Duration = TimeSpan.Parse(reader["Duration"].ToString()),
                            Comment = reader["Comment"].ToString()
                        };
                    }
                }

                connection.Close();
            }
        }

        return null; //
    }

    public void AddSession(Session session)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string query = "INSERT INTO ExSessions (DateStart, DateEnd, Duration, Comment) VALUES (@DateStart, @DateEnd, @Duration, @Comment)";
           
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DateStart", session.DateStart);
                command.Parameters.AddWithValue("@DateEnd", session.DateEnd);
                command.Parameters.AddWithValue("@Duration", session.Duration.ToString());
                command.Parameters.AddWithValue("@Comment", session.Comment);
                command.ExecuteNonQuery();
            }
        }
    }

    public void RemoveSession(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string query = "DELETE FROM ExSessions WHERE Id = @Id";
            
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }

    public void UpdateSession(Session session)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string query = "UPDATE ExSessions SET DateStart = @DateStart, DateEnd = @DateEnd, Duration = @Duration, Comment = @Comment WHERE Id = @Id";
            
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", session.Id);
                command.Parameters.AddWithValue("@DateStart", session.DateStart);
                command.Parameters.AddWithValue("@DateEnd", session.DateEnd);
                command.Parameters.AddWithValue("@Duration", session.Duration.ToString());
                command.Parameters.AddWithValue("@Comment", session.Comment);
                command.ExecuteNonQuery();
            }
        }
    }

    public List<Session> GetAllSessions()
    {
        List<Session> sessions = new List<Session>();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string query = "SELECT Id, DateStart, DateEnd, Duration, Comment FROM ExSessions";
            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Session session = new Session
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            DateStart = Convert.ToDateTime(reader["DateStart"]),
                            DateEnd = Convert.ToDateTime(reader["DateEnd"]),
                            Duration = TimeSpan.Parse(reader["Duration"].ToString()),
                            Comment = reader["Comment"].ToString()
                        };
                        sessions.Add(session);
                    }
                }
            }
        }

        return sessions;
    }
}
