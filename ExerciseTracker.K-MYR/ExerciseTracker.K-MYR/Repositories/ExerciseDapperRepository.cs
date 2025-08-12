using Dapper;
using System.Data;

namespace ExerciseTracker.K_MYR;

internal class ExerciseDapperRepository : IExerciseRepository
{
    private readonly DapperContext _DapperContext;

    public ExerciseDapperRepository(DapperContext dapperContext)
    {
        _DapperContext = dapperContext;
    }

    public IEnumerable<Exercise> GetAll()
    {
        try
        {
            var sql = "SELECT * FROM Exercises";

            using var connection = _DapperContext.CreateConnection();


            var exercises = connection.Query<Exercise>(sql);

            return exercises;
        }
        catch (Exception ex)
        {
            throw new Exception($"Couldn't retrieve Entities: {ex.Message}");
        }
    }

    public async Task<Exercise> AddAsync(ExerciseInsertModel exerciseEntity)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(exerciseEntity);

            var sql = "INSERT INTO Exercises (Type, StartTime,EndTime,Duration,Comments) VALUES (@Type, @StartTime,@EndTime,@Duration,@Comments); SELECT last_insert_rowid()";

            using var connection = _DapperContext.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("Type", exerciseEntity.Type, DbType.String);
            parameters.Add("StartTime", exerciseEntity.StartTime.ToString(), DbType.String);
            parameters.Add("EndTime", exerciseEntity.EndTime.ToString(), DbType.String);
            parameters.Add("Duration", (exerciseEntity.EndTime - exerciseEntity.StartTime).Ticks, DbType.Int64);
            parameters.Add("Comments", exerciseEntity.Comments, DbType.String);

            var id = await connection.QuerySingleAsync<int>(sql, parameters);

            var exercise = new Exercise()
            {
                Id = id,
                Type = exerciseEntity.Type,
                StartTime = exerciseEntity.StartTime,
                EndTime = exerciseEntity.EndTime,
                Duration = (exerciseEntity.EndTime - exerciseEntity.StartTime).Ticks,
                Comments = exerciseEntity.Comments
            };

            return exercise;
        }
        catch (Exception ex)
        {
            throw new Exception($"{nameof(exerciseEntity)} couldn't be saved: {ex.Message}");
        }

    }

    public async Task<Exercise> UpdateAsync(Exercise exerciseEntity)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(exerciseEntity);

            var sql = "UPDATE Exercises SET Type=@Type,StartTime=@StartTime,EndTime=@EndTime,Duration=@Duration,Comments=@Comments WHERE Id=@Id";

            using var connection = _DapperContext.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("Type", exerciseEntity.Type, DbType.String);
            parameters.Add("StartTime", exerciseEntity.StartTime.ToString(), DbType.String);
            parameters.Add("EndTime", exerciseEntity.EndTime.ToString(), DbType.String);
            parameters.Add("Duration", exerciseEntity.Duration, DbType.Int64);
            parameters.Add("Comments", exerciseEntity.Comments, DbType.String);
            parameters.Add("Id", exerciseEntity.Id, DbType.Int64);

            await connection.ExecuteAsync(sql, parameters);

            return exerciseEntity;
        }
        catch (Exception ex)
        {
            throw new Exception($"{nameof(exerciseEntity)} couldn't be updated: {ex.Message}");
        }
    }

    public async Task DeleteAsync(Exercise exerciseEntity)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(exerciseEntity);

            var sql = "DELETE FROM Exercises WHERE Id=@Id";

            using var connection = _DapperContext.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("Id", exerciseEntity.Id, DbType.Int64);

            await connection.ExecuteAsync(sql, parameters);

            return;
        }
        catch (Exception ex)
        {
            throw new Exception($"{nameof(exerciseEntity)} couldn't be updated: {ex.Message}");
        }
    }
}
