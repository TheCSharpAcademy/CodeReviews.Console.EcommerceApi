using Microsoft.EntityFrameworkCore;
using ExerciseTrackerApi.Data;
using ExerciseTrackerApi.Interfaces;
using ExerciseTrackerApi.Repository;

var builder = WebApplication.CreateBuilder(args);
var ConnectionString = builder.Configuration.GetConnectionString("dbConnectionString");

builder.Services.AddDbContext<ExerciseContext>(options => options.UseSqlServer(ConnectionString));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();

Console.WriteLine("Connecting to database...");

var app = builder.Build();

app.MapControllers();

app.Run();
