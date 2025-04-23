using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ExerciseTracker.Data;

public class ExerciseTrackerDapperDbContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public ExerciseTrackerDapperDbContext(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
    }

    public IDbConnection CreateConnection()
         => new SqlConnection(_connectionString);
}
