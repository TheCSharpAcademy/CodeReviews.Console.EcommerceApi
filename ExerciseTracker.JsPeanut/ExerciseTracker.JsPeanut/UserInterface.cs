using Spectre.Console;

namespace ExerciseTracker.JsPeanut
{
    internal class UserInterface
    {
        static internal void ShowExercisesTable(List<Exercise> exercises)
        {
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("StartTime");
            table.AddColumn("EndTime");
            table.AddColumn("Duration");
            table.AddColumn("Comments");

            foreach (var exercise in exercises)
            {
                table.AddRow(exercise.Id.ToString(),
                    exercise.StartTime.ToString(),
                    exercise.EndTime.ToString(),
                    exercise.Duration.ToString(),
                    exercise.Comments.ToString());
            }

            AnsiConsole.Write(table);
        }

        static internal void ShowExercise(Exercise exercise)
        {
            var panel = new Panel($@"Id: {exercise.Id}
StartTime: {exercise.StartTime}
EndTime: {exercise.EndTime}
Duration: {exercise.Duration}
Comments: {exercise.Comments}");
            panel.Header = new PanelHeader("Exercise Session Info");
            panel.Padding = new Padding(2, 2, 2, 2);

            AnsiConsole.Write(panel);
        }
    }
}
