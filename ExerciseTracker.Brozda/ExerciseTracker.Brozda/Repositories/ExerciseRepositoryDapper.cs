using Dapper;
using ExerciseTracker.Brozda.Models;
using ExerciseTracker.Brozda.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using static Dapper.SqlMapper;


namespace ExerciseTracker.Brozda.Repositories
{
    internal class ExerciseRepositoryDapper : IExerciseRepository
    {
        private string _connectionString = @"Data Source=(localdb)\LOCALDB;Initial Catalog=ExcerciseTracker;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public async Task<Exercise> Create(Exercise entity)
        {
            var connection = new SqlConnection(_connectionString);
            var sql = "INSERT INTO [Exercises] " +
                "(Name,WeightLifted,DateStart,DateEnd,Duration,Comments) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@Name, @WeightLifted, @DateStart, @DateEnd, @Duration, @Comments);";

            var id = await connection.ExecuteAsync(sql, new {
                Name = entity.Name,
                WeightLifted = entity.WeightLifted,
                DateStart = entity.DateStart,
                DateEnd = entity.DateEnd,
                Duration = entity.Duration,
                Comments = entity.Comments,
            });

            entity.Id = id;

            return entity;
        }

        public async Task<bool> DeleteById(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM [Exercises] WHERE Id=@Id;";

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });

            return affectedRows > 0;

        }

        public async Task<Exercise?> Edit(Exercise updatedEntity)
        {
            var connection = new SqlConnection(_connectionString);
            var sql = "UPDATE [Exercises] " +
                "SET Name=@Name, WeightLifted=@WeightLifted, DateStart=@DateStart, DateEnd=@DateEnd,Duration=@Duration,Comments=@Comments " +
                "WHERE Id=@Id;";

            var affectedRows = await connection.ExecuteAsync(sql, new
            {
                Id = updatedEntity.Id,
                Name = updatedEntity.Name,
                WeightLifted = updatedEntity.WeightLifted,
                DateStart = updatedEntity.DateStart,
                DateEnd = updatedEntity.DateEnd,
                Duration = updatedEntity.Duration,
                Comments = updatedEntity.Comments,
            });

            return affectedRows > 0
                ? updatedEntity
                : null;
        }

        public async Task<List<Exercise>> GetAll()
        {
            var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM [Exercises];";

            var exercises = await connection.QueryAsync<Exercise>(sql);

            return exercises.ToList();
        }

        public async Task<Exercise?> GetById(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM [Exercises] WHERE Id=@Id;";

            return await connection.QuerySingleAsync<Exercise>(sql, new {Id = id});
        }
    }
}
