using ExerciseTracker.K_MYR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

//Dapper Repository with SQLite
//builder.Services.AddSingleton<DapperContext>();
//builder.Services.AddScoped<IExerciseRepository, ExerciseDapperRepository>();

//EF Repository with SQL Server
builder.Services.AddDbContext<ExerciseDbContext>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();

builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IExerciseController, ExerciseController>();
builder.Services.AddScoped<UserInput>();

using IHost host = builder.Build();

//EF Repository with SQL Server
using var scope = host.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<ExerciseDbContext>();
db.Database.Migrate();

var ui = host.Services.GetRequiredService<UserInput>();
await ui.ShowMainMenu();
