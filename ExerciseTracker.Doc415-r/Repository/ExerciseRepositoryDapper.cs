using Dapper;
using exerciseTracker.doc415.context;
using exerciseTracker.doc415.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace exerciseTracker.doc415.Repository;

internal class ExerciseRepositoryDapper : IExerciseRepository
{
    private readonly string _connectionString;
    public ExerciseRepositoryDapper()
    {
        _connectionString = "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=Exercise; Integrated Security=true;";
    }

    public IEnumerable<Exercise> GetAll()
    {
        var task = TGetAll();
        task.Wait();
        return task.Result;
    }
    private async Task<IEnumerable<Exercise>> TGetAll()
    {
        using var connection = new SqlConnection(_connectionString);
        string getAllDataQuery = @"SELECT * FROM Exercies";
        using var dbReturns = await connection.QueryMultipleAsync(getAllDataQuery);
        var exerciseList = dbReturns.Read<Exercise>().ToList();
        return exerciseList;
    }

    public Exercise GetById(int id)
    {
        var task = TGetById(id);
        task.Wait();
        return task.Result;
    }

    private async Task<Exercise> TGetById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        string getByIdDataQuery = @"SELECT * FROM Exercies Where Id={0}";
        using var dbReturn = await connection.QuerySingleAsync(getByIdDataQuery, new { Id = id });
        var exercise = dbReturn.Read<Exercise>();
        return exercise;
    }

    public void Delete(int id)
    {
        var task = TDelete(id);
        task.Wait();
    }
    private async Task TDelete(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        string deleteDataQuery = @"DELETE FROM Exercies Where Id={0}";
        var affectedRows = await connection.ExecuteAsync(deleteDataQuery, new { Id = id });
        Console.Error.WriteLine(affectedRows);
    }

    public void Insert(Exercise exercise)
    {
        var task = TInsert(exercise);
        task.Wait();
    }
    private async Task TInsert(Exercise exercise)
    {
        using var connection = new SqlConnection(_connectionString);
        string insertDataQuery = "INSERT INTO Exercies(Type,DateStart,DateEnd,Duration,Comments) VALUES (@Type,@DateStart, @DateEnd, @Duration, @Comments )";
        var affectedRows = await connection.ExecuteAsync(insertDataQuery, new
        {
            DateStart = exercise.DateStart,
            DateEnd = exercise.DateEnd,
            Duration = exercise.Duration,
            Comments = exercise.Comments,
            Type = exercise.Type
        });
        Console.Error.WriteLine(affectedRows);
    }

    public void Update(Exercise exercise)
    {
        using var context = new ExerciseDbContext();
        var task = TUpdate(exercise, context);
        task.Wait();
        context.ChangeTracker.DetectChanges();
        context.SaveChanges();

    }
    private async Task TUpdate(Exercise exercise, ExerciseDbContext _context)
    {
        var updateDataQuery = "UPDATE Exercies SET Type=@Type,DateStart=@DateStart,DateEnd=@DateEnd,Duration=@Duration,Comments=@Comments WHERE Id = @Id";
        var connection = _context.Database.GetDbConnection();
        await connection.ExecuteAsync(updateDataQuery, new
        {
            Id = exercise.Id,
            DateStart = exercise.DateStart,
            DateEnd = exercise.DateEnd,
            Duration = exercise.Duration,
            Comments = exercise.Comments,
            Type = exercise.Type
        });

    }
}


