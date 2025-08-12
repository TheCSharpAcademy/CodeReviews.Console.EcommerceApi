using ExerciseTracker.StevieTV.Controllers;
using ExerciseTracker.StevieTV.Database;
using ExerciseTracker.StevieTV.Repositories;
using ExerciseTracker.StevieTV.Services;
using ExerciseTracker.StevieTV.UserInterface;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddDbContext<ExerciseContext>(options =>
{
    options.UseSqlServer("server=localhost;initial catalog=exercise;Trusted_Connection=True;Integrated Security=SSPI;TrustServerCertificate=True");
});

builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<ExerciseController>();
builder.Logging.ClearProviders();

var app = builder.Build();

var scope = app.Services.CreateScope();
var exerciseServices = scope.ServiceProvider;
var exerciseController = exerciseServices.GetRequiredService<ExerciseController>();

Menu menu = new Menu(exerciseController);
menu.MainMenu();
