using ExerciseTrackerCLI.Controllers;
using ExerciseTrackerCLI.Data;
using ExerciseTrackerCLI.Models;
using ExerciseTrackerCLI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddScoped<ExerciseController>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IExerciseRepository<TreadmillRun>, ExerciseRepository>();
builder.Services.AddScoped<Notifications>();
builder.Services.AddDbContext<ExerciseDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

using var scope = builder.Build().Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ExerciseDbContext>();
dbContext.Database.Migrate();

var controller = scope.ServiceProvider.GetRequiredService<ExerciseController>();
controller.Run();