using ExerciseTracker.Services;
using ExerciseTracker.UserInput;

namespace ExerciseTracker.Controller;

public class ExerciseController : IExerciseController
{
    private readonly IInput _input;
    private readonly IExerciseService _exerciseService;

    public ExerciseController(IInput input, IExerciseService exerciseService)
    {
        _input = input;
        _exerciseService = exerciseService;
    }

    private void DisplayStartMenu()
    {
        Console.Clear();
        Console.WriteLine("1 to Start A New Exercise");
        Console.WriteLine("2 to Retrieve All Recorded Exercises");
        Console.WriteLine("3 to Retrieve A Recorded Exercise");
        Console.WriteLine("4 to End An Existing Exercise");
        Console.WriteLine("5 to Delete An Existing Exercise");
        Console.WriteLine("0 to Exit The Application");
        Console.WriteLine("\nYour choice?");
    }

    public void Run()
    {
        DisplayStartMenu();
        var choice = _input.GetChoice();

        while(choice != "0")
        {
            switch (choice)
            {
                case "1":
                    _exerciseService.RecordNewExercise();
                    break;
                case "2":
                    _exerciseService.GetAllExercises();
                    break;
                case "3":
                    _exerciseService.GetExerciseById();
                    break;
                case "4":
                    _exerciseService.UpdateExistingExercise();
                    break;
                case "5":
                    _exerciseService.DeleteExercise();
                    break;
                default:
                    Console.WriteLine("Invalid choice!");
                    Console.Write("Hit Enter to continue...");
                    Console.ReadLine();
                    break;
            }
            
            DisplayStartMenu();
            choice = _input.GetChoice();
        }
    }
}