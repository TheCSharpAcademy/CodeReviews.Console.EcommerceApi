using ExerciseTracker.Controller;
using ExerciseTracker.Models;
using Spectre.Console;

namespace ExerciseTracker.Services;

internal class ExerciseServices
{
	private readonly ExerciseController _controller;
	private readonly UserInput input;

	// Constructor injection of both controller and input
	public ExerciseServices(ExerciseController controller, UserInput input)
	{
		_controller = controller;
		_input = input;
	}

	// Example of Add with validation or business logic
	internal void Add()
	{
		// Validate or add business logic here
		var exercise = _input.AddInput();
		if (exercise != null)
		{
			_controller.Add(exercise);
			AnsiConsole.MarkupLine("[green]Exercise added successfully![/]");
		}
		else
		{
			AnsiConsole.MarkupLine("[red]Exercise addition failed![/]");
		}
	}

	internal void GetAll()
	{
		var exercises = _controller.GetAll();
		UI.ShowTable(exercises);
	}

	internal void GetById()
	{
		var exercise = input.GetSingleInput();
		if (exercise != null)
		{
			UI.ShowExercise(exercise);
		}
		else
		{
			AnsiConsole.MarkupLine("[red]Exercise not found![/]");
		}
	}

	internal void Remove()
	{
		var exercise = input.GetSingleInput();
		if (exercise != null)
		{
			_controller.Delete(exercise);
			AnsiConsole.MarkupLine("[green]Exercise removed successfully![/]");
		}
		else
		{
			AnsiConsole.MarkupLine("[red]Exercise removal failed![/]");
		}
	}

	internal void Update()
	{
		var exercise = input.GetSingleInput();
		if (exercise != null)
		{
			// Example of updating with validation
			var updatedExercise = input.CollectUpdatedExerciseInput(exercise);
			if (updatedExercise != null)
			{
				_controller.Update(updatedExercise);
				AnsiConsole.MarkupLine("[green]Exercise updated successfully![/]");
			}
			else
			{
				AnsiConsole.MarkupLine("[red]Exercise update failed![/]");
			}
		}
		else
		{
			AnsiConsole.MarkupLine("[red]Exercise not found for update![/]");
		}
	}
}
