using ExerciseTracker.Services;
using Spectre.Console;

namespace ExerciseTracker.Controller;

internal class ExerciseController
{
	private readonly ExerciseServices _services;
	private readonly UserInput _input;

	// Constructor injection of both controller and input
	public ExerciseController(ExerciseServices services, UserInput input)
	{
		_services = services;
		_input = input;
	}

	internal void Add()
	{
		var exercise = _input.AddInput();
		if (exercise != null)
		{
			_services.Add(exercise);
			AnsiConsole.MarkupLine("[green]Exercise added successfully![/]");
		}
		else
		{
			AnsiConsole.MarkupLine("[red]Exercise addition failed due to invalid input![/]");
		}
		UserInput.AwaitKeyPress(); 
	}

	internal void GetAll()
	{
		var exercises = _services.GetAll();
		UI.ShowTable(exercises);
	}

	internal void GetById()
	{
		var exercise = _input.GetSingleInput();
		if (exercise != null)
		{
			UI.ShowExercise(exercise);
		}
		else
		{
			AnsiConsole.MarkupLine("[red]Exercise not found![/]");
		}
		UserInput.AwaitKeyPress();
	}

	internal void Remove()
	{
		var exercise = _input.GetSingleInput();
		if (exercise != null)
		{
			_services.Delete(exercise);
			AnsiConsole.MarkupLine("[green]Exercise removed successfully![/]");
		}
		else
		{
			AnsiConsole.MarkupLine("[red]Exercise removal failed![/]");
		}
		UserInput.AwaitKeyPress();
	}
	internal void Update()
	{
		var exercise = _input.GetSingleInput();
		if (exercise != null)
		{
			UI.ShowExercise(exercise);

			var updatedExercise = _input.CollectUpdatedExerciseInput();

			if (updatedExercise != null)
			{
				_services.Update(updatedExercise);
				AnsiConsole.MarkupLine("[green]Exercise updated successfully![/]");
			}
			else
			{
				AnsiConsole.MarkupLine("[red]Exercise update failed due to invalid input![/]");
			}
		}
		else
		{
			AnsiConsole.MarkupLine("[red]Exercise not found for update![/]");
		}
		UserInput.AwaitKeyPress();
	}

}


