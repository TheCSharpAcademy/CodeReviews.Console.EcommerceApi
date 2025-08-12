using ExerciseTracker.Models;
using Spectre.Console;

namespace ExerciseTracker
{
	public class UserInput
	{
		public static ExerciseModel GetExerciseInfo()
		{
			var exercise = new ExerciseModel();
			string type = AnsiConsole.Ask<string>("What exercise did you do ? ").Trim();

			DateTime start;
			do
			{
				start = AnsiConsole.Ask<DateTime>("When did the exercise start ? format(yyyy-mm-dd hh:mm): ");
			} while (!Validation.IsDateNotInFuture(start));

			DateTime end;
			do
			{
				end = AnsiConsole.Ask<DateTime>("When did the exercise end ? format (yyyy-mm-dd hh:mm): ");
			} while (!Validation.IsEndDateGreaterThanStartDate(start,end));

			string comment = AnsiConsole.Prompt(new TextPrompt<string>("Add a comment about the exercise (optional): ")
				.AllowEmpty());

			exercise.ExerciseType = type;
			exercise.DateStart = start;
			exercise.DateEnd = end;
			exercise.Comments = comment;

			return exercise;
		}

		internal static int GetIdOfExercise(List<ExerciseModel> exercises)
		{
			var exerciseArray = exercises.Select(x => $"{x.ExerciseId} - {x.ExerciseType}").ToArray();
			var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
				.Title("Select the desired exercise:")
				.AddChoices(exerciseArray));
			var exerciseId = option.Split(" - ")[0];
			int id = exercises.Single(x => x.ExerciseId.ToString() == exerciseId).ExerciseId;
			return id;
		}

		internal static ExerciseModel GetUpdatedInfo(ExerciseModel exercise)
		{
			exercise.ExerciseType = AnsiConsole.Confirm($"Do you want to update the exercise name ({exercise.ExerciseType}) ?") ?
				AnsiConsole.Ask<string>("Enter a new name: ")
				: exercise.ExerciseType;
			do {
				do
				{
					exercise.DateStart = AnsiConsole.Confirm($"Do you want to edit the start time ({exercise.DateStart}) ?") ?
					AnsiConsole.Ask<DateTime>("Enter the new start time: ")
					: exercise.DateStart;
				} while (!Validation.IsDateNotInFuture(exercise.DateStart));

				do
				{
					exercise.DateEnd = AnsiConsole.Confirm($"Do you want to edit the end time ({exercise.DateEnd})?") ?
					AnsiConsole.Ask<DateTime>("Enter the new end time: ")
					: exercise.DateEnd; 
				} while (!Validation.IsDateNotInFuture(exercise.DateEnd));
			} while  (!Validation.IsEndDateGreaterThanStartDate(exercise.DateStart, exercise.DateEnd));
			exercise.Comments = AnsiConsole.Confirm($"Do you want to add or change the comment ({exercise.Comments}) ?") ?
				AnsiConsole.Prompt(new TextPrompt<string>("Enter comment: ").AllowEmpty())
				: exercise.Comments;
			return exercise;
		}
	}
}
