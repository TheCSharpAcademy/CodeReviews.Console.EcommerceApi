namespace ExerciseTracker.Call911plz;

class Program
{
    static async Task Main(string[] args)
    {
        ExerciseContext context = new();
        ExerciseRepository exerciseRepository = new(context);
        ExerciseService exerciseService = new(exerciseRepository);
        ExerciseController exerciseController = new(exerciseService);
        
        await exerciseController.StartAsync();
    }
}
