using Newtonsoft.Json;
using ExerciseTrackerUI.Services;
using ExerciseTrackerUI.Models;

namespace ExerciseTrackerUI;

public class UserInput
{
  private static ExerciseService _exerciseService = new ExerciseService();
  public async Task Main()
  {
    bool isRunning = true;

    while (isRunning)
    {
      Console.WriteLine("Shift menu\n");
      Console.WriteLine("----------------------------------");
      Console.WriteLine("Press '0' to exit the application");
      Console.WriteLine("Press '1' to view all Exercises");
      Console.WriteLine("Press '2' to add a exercise");
      Console.WriteLine("Press '3' to update a exercise");
      Console.WriteLine("Press '4' to delete a exercise");
      Console.WriteLine("----------------------------------");
      var input = Console.ReadLine();

      switch (input)
      {
        case "0":
          isRunning = false;
          Environment.Exit(0);
          break;

        case "1":
          await ShowExercises();
          break;

        case "2":
          await AddExercise();
          break;

        case "3":
          await UpdateExercise();
          break;

        case "4":
          await DeleteExercise();
          break;

        default:
          Console.WriteLine("Invalid input");
          break;
      }

      if (isRunning)
      {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        Console.Clear();
      }

    }

    _exerciseService.Dispose();
    Console.WriteLine("Thanks for using the Tracker!");

  }

  public async Task ShowExercises()
  {
    var exercises = await _exerciseService.GetAllExercisesAsync();
    if (exercises.Any())
    {
      foreach (var exercise in exercises)
      {
        Console.WriteLine($"ID: {exercise.Id}, Duration: {exercise.Duration} minutes, Comments: {exercise.Comments}");
      }
    }
    else
    {
      Console.WriteLine("No exercises found.");
    }
  }

  public async Task ShowExerciseById(int id)
  {
    var exercise = await _exerciseService.GetExerciseByIdAsync(id);
    if (exercise != null)
    {
      Console.WriteLine($"ID: {exercise.Id}, Duration: {exercise.Duration} minutes");
    }
    else
    {
      Console.WriteLine("Exercise not found.");
    }
  }

  public async Task AddExercise()
  {
    Console.Clear();
    var exercise = new Exercise();

    Console.WriteLine("Enter exercise start date (MM/dd/yyyy HH:mm):");
    if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
    {
      Console.WriteLine("Invalid date format.");
      return;
    }
    exercise.DateStart = startDate;

    Console.WriteLine("Enter exercise end date (MM/dd/yyyy HH:mm):");
    if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateEnd))
    {
      Console.WriteLine("Invalid date format.");
      return;
    }
    if (dateEnd <= startDate)
    {
      Console.WriteLine("End date must be after start date.");
      return;
    }
    exercise.DateEnd = dateEnd;

    Console.WriteLine("Enter Comments (optional):");
    exercise.Comments = Console.ReadLine() ?? string.Empty;
    if (string.IsNullOrWhiteSpace(exercise.Comments))
    {
      exercise.Comments = "No comments provided.";
    }
    
    var success = await _exerciseService.CreateExerciseAsync(exercise);
    if (success)
    {
      Console.WriteLine("Exercise added successfully.");
    }
    else
    {
      Console.WriteLine("Failed to add exercise.");
    }
  }

  public async Task UpdateExercise()
  {
    Console.WriteLine("Enter the ID of the exercise to update:");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
      Console.WriteLine("Invalid ID format.");
      return;
    }

    var exercise = await _exerciseService.GetExerciseByIdAsync(id);
    if (exercise == null)
    {
      Console.WriteLine("Exercise not found.");
      return;
    }

    Console.WriteLine("Enter Start Date (MM/dd/yyyy HH:mm):");
    if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
    {
      Console.WriteLine("Invalid date format.");
      return;
    }
    exercise.DateStart = startDate;

    Console.WriteLine("Enter End Date (MM/dd/yyyy HH:mm):");
    if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateEnd))
    {
      Console.WriteLine("Invalid date format.");
      return;
    }
    if (dateEnd <= startDate)
    {
      Console.WriteLine("End date must be after start date.");
      return;
    }
    exercise.DateEnd = dateEnd;

    Console.WriteLine("Enter Comments (optional):");
    exercise.Comments = Console.ReadLine() ?? string.Empty;
    if (string.IsNullOrWhiteSpace(exercise.Comments))
    {
      exercise.Comments = "No comments provided.";
    }
    Console.WriteLine("Updating exercise...");
    Console.WriteLine($"ID: {exercise.Id}, Start: {exercise.DateStart}, End: {exercise.DateEnd}, Duration: {exercise.Duration} minutes, Comments: {exercise.Comments}");

    var success = await _exerciseService.UpdateExerciseAsync(exercise);
    if (success)
    {
      Console.WriteLine("Exercise updated successfully.");
    }
    else
    {
      Console.WriteLine("Failed to update exercise.");
    }
  }

  public async Task DeleteExercise()
  {
    Console.WriteLine("Enter the ID of the exercise to delete:");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
      Console.WriteLine("Invalid ID format.");
      return;
    }

    var success = await _exerciseService.DeleteExerciseAsync(id);
    if (success)
    {
      Console.WriteLine("Exercise deleted successfully.");
    }
    else
    {
      Console.WriteLine("Failed to delete exercise.");
    }
  }

  public async Task Run()
  {
    await Main();
  }
}
