using Microsoft.Extensions.Hosting;
using ExerciseTracker.frockett.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ExerciseTracker.frockett.Repositories;
using ExerciseTracker.frockett.Services;
using ExerciseTracker.frockett.Controllers;
using ExerciseTracker.frockett.UI;

var builder = Host.CreateApplicationBuilder();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

builder.Services.AddDbContext<ExerciseTrackerContext>(options =>
{
    options.UseSqlite(configuration.GetConnectionString("ExerciseTrackerConnection"));
});

builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<ExerciseController>();
builder.Services.AddScoped<UserInput>();
builder.Services.AddScoped<TableEngine>();

var app = builder.Build();

var scope = app.Services.CreateScope();
var exerciseServices = scope.ServiceProvider;
var exerciseController = exerciseServices.GetRequiredService<ExerciseController>();
var userInput = exerciseServices.GetRequiredService<UserInput>();
var tableEngine = exerciseServices.GetRequiredService<TableEngine>();

MenuHandler menuHandler = new(exerciseController, userInput, tableEngine);
menuHandler.ShowMainMenu();