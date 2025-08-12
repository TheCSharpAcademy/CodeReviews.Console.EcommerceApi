using ExerciseTracker.Cactus.Model;
using Spectre.Console;

namespace ExerciseTracker.Cactus.UI
{
    public class UserInterface
    {
        public static void ShowExercise(IEnumerable<Exercise> exercises)
        {
            if (exercises is null)
            {
                Console.WriteLine("No Exercise.");
                return;
            }

            var table = new Table();
            table.Title("Exercise Info");
            table.AddColumn("Id");
            table.AddColumn("StartTime");
            table.AddColumn("EndTime");
            table.AddColumn("Duration");
            table.AddColumn("Comments");

            foreach (Exercise exercise in exercises)
            {
                table.AddRow(exercise.Id.ToString(),
                    exercise.DateStart.ToString("yyyy/MM/dd"), exercise.DateEnd.ToString("yyyy/MM/dd"),
                    exercise.Duration.ToString(), exercise.Comments);
            }

            AnsiConsole.Write(table);
        }

        public static void ShowExercise(Exercise exercise)
        {
            if (exercise == null)
            {
                Console.WriteLine("No Exercise.");
                return;
            }

            var panel = new Panel($"{exercise.DateStart.ToString("yyyy/MM/dd")} - {exercise.DateEnd.ToString("yyyy/MM/dd")} {exercise.Duration} {exercise.Comments}")
                .Header("[bold]Exercise Info[/]")
                .BorderColor(Color.Blue);

            panel.Padding(2, 2, 2, 2);

            AnsiConsole.Write(panel);
        }

        public static void BackToMainMenuPrompt()
        {
            Console.WriteLine("Press any key to go back to Main Menu");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
