using ExerciseTracker.Models;
using ExerciseTracker.Services;
using Spectre.Console;

namespace ExerciseTracker;
internal class UserInput
{
	private readonly ExerciseServices _services;

	// Constructor injection of service
	public UserInput(ExerciseServices services)
	{
		_services = services;
	}

	internal ExerciseModel AddInput()
	{
		var exercise = new ExerciseModel
		{
			DateStart = AnsiConsole.Ask<DateTime>("Enter start time (yyyy-MM-dd HH:mm): "),
			DateEnd = AnsiConsole.Ask<DateTime>("Enter end time (yyyy-MM-dd HH:mm): "),
			Comments = AnsiConsole.Ask<string>("Enter comments: ")

		};

		if (ValidateExercise(exercise))
		{
			return exercise;
		}

		AwaitKeyPress();
		return null;
	}

	internal void DeleteInput()
	{
		var exercise = GetOptionInput();
		if (exercise == null)
		{
			AnsiConsole.MarkupLine("[red]The exercise to be removed does not exist.[/]");
			AwaitKeyPress();
			return;
		}

		_services.Delete(exercise);  // Call the service to delete the exercise
		AnsiConsole.MarkupLine("[green]Exercise removed successfully![/]");
	}

	private ExerciseModel? GetOptionInput()
	{
		var exercises = _services.GetAll();  // Use the service to get exercises
		if (!exercises.Any())
		{
			AnsiConsole.MarkupLine("[red]No exercises available.[/]");
			return null;
		}

		var exerciseList = exercises.Select(x => x.Id.ToString()).ToList();
		var option = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("Select an exercise: ")
				.AddChoices(exerciseList)
		);

		var id = int.Parse(option);
		return exercises.FirstOrDefault(x => x.Id == id);
	}

	internal ExerciseModel? GetSingleInput()
	{
		var data = GetOptionInput();
		if (data == null)
		{
			AnsiConsole.MarkupLine("[red]The exercise does not exist.[/]");
			AwaitKeyPress();
			return null;
		}
		UI.ShowExercise(data);
		return data;
	}

	internal ExerciseModel CollectUpdatedExerciseInput()
	{
		var existingExercise = GetOptionInput();
		var updatedExercise = new ExerciseModel
		{
			Id = existingExercise.Id,
			DateStart = AnsiConsole.Confirm("Update start time?")
				? AnsiConsole.Ask<DateTime>("Enter start time: ")
				: existingExercise.DateStart,
			DateEnd = AnsiConsole.Confirm("Update end time?")
				? AnsiConsole.Ask<DateTime>("Enter end time: ")
				: existingExercise.DateEnd,
			Comments = AnsiConsole.Confirm("Update comments?")
				? AnsiConsole.Ask<string>("Enter comments: ")
				: existingExercise.Comments
		};

		return ValidateExercise(updatedExercise) ? updatedExercise : null;
	}

	private bool ValidateExercise(ExerciseModel exercise)
	{
		if (exercise.DateEnd < exercise.DateStart)
		{
			AnsiConsole.MarkupLine("[red]End time cannot be earlier than start time![/]");
			return false;
		}

		if (string.IsNullOrEmpty(exercise.Comments))
		{
			AnsiConsole.MarkupLine("[red]Comments cannot be empty![/]");
			return false;
		}

		return true;
	}

	static internal void AwaitKeyPress()
	{
		AnsiConsole.MarkupLine("\nPress any key to return to the menu...");
		Console.ReadKey();
	}
}

