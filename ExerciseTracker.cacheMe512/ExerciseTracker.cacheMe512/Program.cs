
using ExerciseTracker.cacheMe512;

using (var context = new ExerciseContext())
{
    context.Database.EnsureCreated();
}

using (var context = new ExerciseContext())
{
    IExerciseRepository repository = new ExerciseRepository(context);
    var exerciseService = new ExerciseService(repository);
    var controller = new ExerciseController(exerciseService);

    var ui = new UserInterface(controller);
    ui.Run();
}