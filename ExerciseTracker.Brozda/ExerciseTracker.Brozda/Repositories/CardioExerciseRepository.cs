using Dapper;
using ExerciseTracker.Brozda.Models;
using ExerciseTracker.Brozda.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using static Dapper.SqlMapper;

namespace ExerciseTracker.Brozda.Repositories
{
    internal class CardioExerciseRepository : IExerciseRepository
    {
        private string _connectionString;
        public CardioExerciseRepository(string connectionString)
        {
            _connectionString = connectionString;
            InitializeDb();
        }

        private void InitializeDb()
        {
            if (!DoesTableExist())
            {
                CreateTable();
                InitialAutoSeed();
            }
        }

        private bool DoesTableExist()
        {
            var connection = new SqlConnection(_connectionString);
            string sql = @"SELECT COUNT(*)
                          FROM INFORMATION_SCHEMA.TABLES
                          WHERE TABLE_NAME = 'ExercisesCardio';";

            var result = connection.QuerySingle<int>(sql);

            return result != 0;
        }

        private async void CreateTable()
        {
            var connection = new SqlConnection(_connectionString);

            string sql = @"CREATE TABLE [ExercisesCardio] (
                        [Id] INT IDENTITY(1, 1) NOT NULL,
                        [Name] NVARCHAR(MAX) NOT NULL,
                        [Volume] FLOAT(53) NOT NULL,
                        [DateStart] DATETIME2(7) NOT NULL,
                        [DateEnd] DATETIME2(7) NOT NULL,
                        [Comments] NVARCHAR(MAX) NULL,
                        [Duration] BIGINT NULL,
                        [TypeId] INT DEFAULT((0)) NOT NULL,
                        CONSTRAINT [PK_ExercisesCardio] PRIMARY KEY CLUSTERED ([Id] ASC),
                        CONSTRAINT [FK_ExercisesCardio_ExerciseTypes_TypeId] FOREIGN KEY ([TypeId])
                        REFERENCES [dbo].[ExerciseTypes]([Id]) ON DELETE CASCADE
                        );";

            await connection.ExecuteAsync(sql);
        }

        private async void InitialAutoSeed()
        {
            var projectRoot = Environment.GetEnvironmentVariable("PROJECT_ROOT");

            var path = Path.Combine(projectRoot ?? Directory.GetCurrentDirectory(), "Resources", "SeedData.json");

            if (File.Exists(path))
            {
                var rawData = File.ReadAllText(path);
                var seedData = JsonSerializer.Deserialize<SeedDataEf>(rawData);

                if (seedData is not null)
                {
                    foreach (var excercise in seedData.ExercisesCardio)
                    {
                        excercise.Duration = (long)(excercise.DateEnd - excercise.DateStart).TotalSeconds;
                    }
                    await BulkInsert(seedData.ExercisesCardio);
                }
            }
        }

        private async Task BulkInsert(List<Exercise> exercises)
        {
            var connection = new SqlConnection(_connectionString);
            var sql = "INSERT INTO [ExercisesCardio] " +
                      "(Name, " +
                      "Volume, " +
                      "DateStart, " +
                      "DateEnd, " +
                      "Comments, " +
                      "Duration, " +
                      "TypeId) " +

                      "VALUES " +

                      "(@Name, " +
                      "@Volume, " +
                      "@DateStart, " +
                      "@DateEnd, " +
                      "@Comments, " +
                      "@Duration, " +
                      "@TypeId);";

            foreach (var exercise in exercises)
            {
                await connection.ExecuteAsync(sql, new
                {
                    Name = exercise.Name,
                    TypeId = exercise.TypeId,
                    Volume = exercise.Volume,
                    DateStart = exercise.DateStart,
                    DateEnd = exercise.DateEnd,
                    Duration = exercise.Duration,
                    Comments = exercise.Comments,
                });
            }
        }

        public async Task<List<Exercise>> GetAll()
        {
            var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT
                        ex.Id, ex.Name, ex.Volume, ex.DateStart, ex.DateEnd, ex.Duration, ex.Comments, ex.TypeId,
                        type.Id, type.Name, type.Unit
                     FROM [ExercisesCardio] AS ex
                     INNER JOIN [ExerciseTypes] AS type ON ex.TypeId = type.Id";

            var exercises = await connection.QueryAsync<Exercise, ExerciseType, Exercise>(sql, (exercise, type) =>
            {
                exercise.Type = type;
                return exercise;
            },
            splitOn: "Id");

            return exercises.ToList();
        }

        public async Task<Exercise?> GetById(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = @"SELECT
                        ex.Id, ex.Name, ex.Volume, ex.DateStart, ex.DateEnd, ex.Duration, ex.Comments, ex.TypeId,
                        type.Id, type.Name, type.Unit
                     FROM [ExercisesCardio] AS ex
                     INNER JOIN [ExerciseTypes] AS type ON ex.TypeId = type.Id
                     WHERE ex.Id = @Id;";

            var result = await connection.QueryAsync<Exercise, ExerciseType, Exercise>(
                sql,
                (exercise, type) =>
                {
                    exercise.Type = type;
                    return exercise;
                },
                new { Id = id },
                splitOn: "Id"
            );

            return result.FirstOrDefault();
        }

        public async Task<Exercise> Create(Exercise entity)
        {
            var connection = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO [ExercisesCardio]
                        (
                            Name,
                            Volume,
                            DateStart,
                            DateEnd,
                            Comments,
                            Duration,
                            TypeId
                        )
                        OUTPUT INSERTED.Id
                        VALUES
                        (
                            @Name,
                            @Volume,
                            @DateStart,
                            @DateEnd,
                            @Comments,
                            @Duration,
                            @TypeId
                        );";

            var id = await connection.QuerySingleAsync<int>(sql, new
            {
                Name = entity.Name,
                Volume = entity.Volume,
                DateStart = entity.DateStart,
                DateEnd = entity.DateEnd,
                Comments = entity.Comments,
                Duration = entity.Duration,
                TypeId = entity.TypeId,
            });

            return await GetById(id) ?? new Exercise() { };
        }

        public async Task<bool> DeleteById(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var sql = @"DELETE FROM [ExercisesCardio]
                        WHERE Id=@Id;";

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });

            return affectedRows > 0;
        }

        public async Task<List<ExerciseType>> GetExerciseTypes()
        {
            var connection = new SqlConnection(_connectionString);
            var sql = @"SELECT
                       Id,Name,Unit
                       FROM [ExerciseTypes];";

            var exTypes = await connection.QueryAsync<ExerciseType>(sql);

            return exTypes.ToList();
        }

        public async Task<Exercise?> Edit(Exercise updatedEntity)
        {
            var connection = new SqlConnection(_connectionString);
            var sql = @"UPDATE [ExercisesCardio] SET
                        Name = @Name,
                        TypeId = @TypeId,
                        Volume = @Volume,
                        DateStart = @DateStart,
                        DateEnd = @DateEnd,
                        Duration = @Duration,
                        Comments = @Comments
                    WHERE Id = @Id;";

            var affectedRows = await connection.ExecuteAsync(sql, new
            {
                Id = updatedEntity.Id,
                Name = updatedEntity.Name,
                TypeId = updatedEntity.TypeId,
                Volume = updatedEntity.Volume,
                DateStart = updatedEntity.DateStart,
                DateEnd = updatedEntity.DateEnd,
                Duration = updatedEntity.Duration,
                Comments = updatedEntity.Comments,
            });

            return affectedRows > 0
                ? await GetById(updatedEntity.Id)
                : null;
        }
    }
}