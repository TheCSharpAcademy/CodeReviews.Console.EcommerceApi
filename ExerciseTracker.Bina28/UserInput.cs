using ExerciseTracker.Controller;
using ExerciseTracker.Controllers;
using ExerciseTracker.Models;
using Spectre.Console;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace ExerciseTracker;

internal class UserInput
{
		private readonly ExerciseController controller;

		// Constructor injection of controller
		public UserInput(ExerciseController controller)
		{
			this.controller = controller;
		}

	public UserInput()
	{
	}

	internal void UpdateInput(ExerciseModel exerciseModel)
		{
			exerciseModel.DateStart = AnsiConsole.Confirm("Update start time?")
				? AnsiConsole.Ask<DateTime>("Enter start time: ")
				: exerciseModel.DateStart;
			exerciseModel.DateEnd = AnsiConsole.Confirm("Update end time?")
				? AnsiConsole.Ask<DateTime>("Enter end time: ")
				: exerciseModel.DateEnd;
			exerciseModel.Comments = AnsiConsole.Confirm("Update comments?")
				? AnsiConsole.Ask<string>("Enter comments: ")
				: exerciseModel.Comments;

			controller.Update(exerciseModel);
			AnsiConsole.MarkupLine("[green]Exercise updated successfully![/]");
		}

		internal void AddInput()
		{
			var exercise = new ExerciseModel();
			exercise.DateStart = AnsiConsole.Ask<DateTime>("Enter start time: ");
			exercise.DateEnd = AnsiConsole.Ask<DateTime>("Enter end time: ");
			exercise.Comments = AnsiConsole.Ask<string>("Enter comments: ");

			controller.Add(exercise);
			AnsiConsole.MarkupLine("[green]Exercise added successfully![/]");
			AwaitKeyPress();
		}

		internal void DeleteInput()
		{
			var exercise = GetOptionInput();
			if (exercise == null)
			{
				Console.WriteLine("The exercise to be removed does not exist.");
				AwaitKeyPress();
				return;
			}

			controller.Delete(exercise);  // Use the controller to delete the exercise
			AnsiConsole.MarkupLine("[green]Exercise removed successfully![/]");
			AwaitKeyPress();
		}

		private ExerciseModel? GetOptionInput()
		{
			var exercises = controller.GetAll();
			if (!exercises.Any())
			{
				return null;
			}

			var exerciseList = exercises.Select(x => x.Id.ToString()).ToArray();
			var option = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
					.Title("Select an exercise: ")
					.AddChoices(exerciseList)
			);

			var id = int.Parse(option);
			var selectedExercise = exercises.FirstOrDefault(x => x.Id == id);
			return selectedExercise;
		}

		internal void GetAll()
		{
			Console.Clear();
			IEnumerable<ExerciseModel> data = controller.GetAll();
			UI.ShowTable(data);
		}
	internal  void GetSingleInput()
	{
		var data = GetOptionInput();
		if (data==null)
		{
			Console.WriteLine("The exercise does not exist.");
			AwaitKeyPress();
			return;
		}
		UI.ShowExercise(data);
	}
	static internal void AwaitKeyPress()
		{
			Console.WriteLine("\nPress any key to return to the menu...");
			Console.ReadKey();
		}
	}

