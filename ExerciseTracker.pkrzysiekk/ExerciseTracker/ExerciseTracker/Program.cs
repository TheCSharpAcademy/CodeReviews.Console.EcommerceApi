// See https://aka.ms/new-console-template for more information

using ExerciseTracker.Controllers;
using ExerciseTracker.Data;
using ExerciseTracker.Menu;
using ExerciseTracker.Models;
using ExerciseTracker.Repository;
using ExerciseTracker.Services;

ExerciseContext context = new ExerciseContext();
context.Database.EnsureCreated();
IRepository<Exercise> repository = new ExerciseRepository(context);
IExerciseService service = new ExerciseService(repository);
ExerciseController controller = new ExerciseController(service);
Menu menu = new Menu(controller);
await
    menu.ShowMenu();