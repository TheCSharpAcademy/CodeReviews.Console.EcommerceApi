using ExerciseTracker.SpyrosZoupas;
using ExerciseTracker.SpyrosZoupas.Controller;
using ExerciseTracker.SpyrosZoupas.DAL;
using ExerciseTracker.SpyrosZoupas.DAL.Model;
using ExerciseTracker.SpyrosZoupas.DAL.Repository;
using ExerciseTracker.SpyrosZoupas.Services;
using Microsoft.Extensions.DependencyInjection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

var services = new ServiceCollection();

services.AddDbContext<ExerciseTrackerDbContext>();
services.AddSingleton<Validation>();
services.AddScoped(typeof(IRepository<WeightExercise>), typeof(WeightExerciseRepository<WeightExercise>));
services.AddScoped<WeightExerciseRepository<WeightExercise>>();
services.AddScoped(typeof(IRepositoryDapper<CardioExercise>), typeof(CardioExerciseRepository<CardioExercise>));
services.AddScoped<CardioExerciseRepository<CardioExercise>>();
services.AddScoped<WeightExerciseController>();
services.AddScoped<CardioExerciseController>();
services.AddScoped<WeightExerciseService>();
services.AddScoped<CardioExerciseService>();
services.AddScoped<UserInput>();

var serviceProvider = services.BuildServiceProvider();

var dbContext = serviceProvider.GetRequiredService<ExerciseTrackerDbContext>();
dbContext.Database.EnsureDeleted();
dbContext.Database.EnsureCreated();

var cardioExerciseRepository = serviceProvider.GetRequiredService<CardioExerciseRepository<CardioExercise>>();
cardioExerciseRepository.CreateTables();

var userInterface = serviceProvider.GetRequiredService<UserInput>();
userInterface.MainMenu();