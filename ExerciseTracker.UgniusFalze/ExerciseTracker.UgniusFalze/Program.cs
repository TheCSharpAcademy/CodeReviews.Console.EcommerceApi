using ExerciseTracker.UgniusFalze;
using ExerciseTracker.UgniusFalze.Controllers;
using ExerciseTracker.UgniusFalze.Models;
using ExerciseTracker.UgniusFalze.Repositories;
using ExerciseTracker.UgniusFalze.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddDbContext<PullUpContext>(opt =>
        opt.UseSqlServer(
            "Server=(LocalDb)\\MSSQLLocalDB;Initial Catalog=ExerciseTracker;Integrated Security=SSPI;Trusted_Connection=yes"))
    .AddScoped<IExerciseService, ExerciseService>()
    .AddScoped<IExerciseRepository, ExerciseRepository>()
    .AddTransient<ExerciseController>();
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
var app = builder.Build();

using var scope = app.Services.CreateScope();
var controller = scope.ServiceProvider.GetRequiredService<ExerciseController>();
var menu = new Menu(controller);
menu.Start();