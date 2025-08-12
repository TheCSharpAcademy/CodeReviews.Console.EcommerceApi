using edvaudin.ExerciseTracker.Input;
using edvaudin.ExerciseTracker.Services;
using edvaudin.ExerciseTracker.Visulisation;

namespace edvaudin.ExerciseTracker.Controllers;

internal class ExerciseController : IExerciseController
{
    private static bool endApp;
    private readonly IExerciseService exerciseService;
    private readonly IUserInput userInput;
    public ExerciseController(IUserInput userInput, IExerciseService exerciseService)
    {
        this.userInput = userInput;
        this.exerciseService = exerciseService;
    }

    public void Run()
    {
        if (exerciseService == null)
        {
            throw new ArgumentNullException();
        }
        while (!endApp)
        {
            Viewer.DisplayOptionsMenu();
            string input = userInput.GetOption();
            ProcessOptions(input);
        }
        Exit();
    }

    private void ProcessOptions(string input)
    {
        switch (input)
        {
            case "v":
                exerciseService.ViewExercises();
                break;
            case "a":
                AddExercise();
                break;
            case "d":
                DeleteExercise();
                break;
            case "u":
                UpdateExercise();
                break;
            case "0":
                Exit();
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private static void Exit()
    {
        Environment.Exit(0);
    }

    private void DeleteExercise()
    {
        exerciseService.ViewExercises();
        Console.WriteLine("Which exercise would you like to delete?");
        int id = userInput.GetId();
        if (id == -1) { return; }
        exerciseService.DeleteExercise(id);
    }

    private void UpdateExercise()
    {
        exerciseService.ViewExercises();
        Console.WriteLine("Which exercise would you like to update?");
        int id = userInput.GetId();
        if (id == -1) { return; }
        GetNewExerciseData(out DateTime start, out DateTime end, out string? comments);
        exerciseService.UpdateExercise(id, start, end, comments);
    }

    private void AddExercise()
    {
        GetNewExerciseData(out DateTime start, out DateTime end, out string? comments);
        exerciseService.AddExercise(start, end, comments);
    }

    private void GetNewExerciseData(out DateTime start, out DateTime end, out string? comments)
    {
        Console.WriteLine("When did you start this exercise? Use the format: dd/MM/yyyy HH:mm:ss.");
        start = userInput.GetStartTime();
        Console.WriteLine("When did you finish this exercise? Use the format: dd/MM/yyyy HH:mm:ss.");
        end = userInput.GetEndTime(start);
        Console.WriteLine("Do you have any comments about this exercise? If not, just press enter.");
        comments = Console.ReadLine();
    }
}
