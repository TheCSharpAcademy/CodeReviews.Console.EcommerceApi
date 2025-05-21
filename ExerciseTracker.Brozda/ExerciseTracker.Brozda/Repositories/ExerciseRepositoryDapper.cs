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

        public ExerciseRepositoryDapper()
        {
            InitializeDb();
        }
        private void InitializeDb()
        {
            if (!DoesTableExist())
            {
                CreateTable();
            }
        }
        private bool DoesTableExist()
        {
            var connection = new SqlConnection(_connectionString);
            string sql = "SELECT COUNT(*) " +
                "FROM INFORMATION_SCHEMA.TABLES " +
                "WHERE TABLE_NAME = 'ExercisesCardio';";

            var result = connection.QuerySingle<int>(sql);


            return result != 0;
        }
        private async void CreateTable()
        {
            var connection = new SqlConnection(_connectionString);

            string sql = "CREATE TABLE [dbo].[ExercisesCardio] ( " +
            "[Id] INT IDENTITY(1, 1) NOT NULL,"+
            "[Name] NVARCHAR(MAX) NOT NULL,"+
            "[Volume] FLOAT(53)     NOT NULL,"+
            "[DateStart] DATETIME2(7)  NOT NULL,"+
            "[DateEnd] DATETIME2(7)  NOT NULL,"+
            "[Comments] NVARCHAR(MAX) NULL,"+
            "[Duration] BIGINT NULL,"+
            "[TypeId] INT DEFAULT((0)) NOT NULL,"+
            "CONSTRAINT[PK_ExercisesWeight] PRIMARY KEY CLUSTERED([Id] ASC),"+
            "CONSTRAINT[FK_ExercisesWeight_ExerciseTypes_TypeId] FOREIGN KEY([TypeId]) REFERENCES[dbo].[ExerciseTypes]([Id]) ON DELETE CASCADE"+
            "); ";

            await connection.ExecuteAsync(sql);
        }
        public async Task<ExerciseDto> Create(ExerciseDto entity)
        {
            var connection = new SqlConnection(_connectionString);
            var sql = "INSERT INTO [ExercisesCardio] (" +
                "Name," +
                "TypeId," +
                "Volume," +
                "DateStart," +
                "DateEnd," +
                "Duration," +
                "Comments) " +

                "OUTPUT INSERTED.Id " +

                "VALUES (" +
                "@Name, " +
                "@TypeId, " +
                "@Volume, " +
                "@DateStart, " +
                "@DateEnd, " +
                "@Duration, " +
                "@Comments" +
                ");";

            var id = await connection.ExecuteAsync(sql, new {
                Name = entity.Name,
                TypeId = entity.TypeId,
                Volume = entity.Volume,
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
            var sql = "DELETE FROM [ExercisesCardio] WHERE Id=@Id;";

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });

            return affectedRows > 0;

        }

        public async Task<ExerciseDto?> Edit(ExerciseDto updatedEntity)
        {
            var connection = new SqlConnection(_connectionString);
            var sql = "UPDATE [ExercisesCardio] SET " +
                "Name=@Name, " +
                "TypeId=@TypeId, " +
                "Volume=@Volume, " +
                "DateStart=@DateStart, " +
                "DateEnd=@DateEnd," +
                "Duration=@Duration," +
                "Comments=@Comments " +
                "WHERE Id=@Id;";

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
                ? updatedEntity
                : null;
        }

        public async Task<List<ExerciseDto>> GetAll()
        {
            var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM [ExercisesCardio];";

            var exercises = await connection.QueryAsync<Exercise>(sql);

            return exercises.ToList();
        }

        public async Task<ExerciseDto?> GetById(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM [ExercisesCardio] WHERE Id=@Id;";

            return await connection.QuerySingleAsync<Exercise>(sql, new {Id = id});
        }
    }
}
