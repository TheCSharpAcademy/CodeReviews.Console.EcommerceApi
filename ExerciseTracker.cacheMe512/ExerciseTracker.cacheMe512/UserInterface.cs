using Spectre.Console;

namespace ExerciseTracker.cacheMe512
{
    internal class UserInterface
    {
        private readonly ExerciseController _controller;

        public UserInterface(ExerciseController controller)
        {
            _controller = controller;
        }

        public void Run()
        {
            bool isAppRunning = true;
            while (isAppRunning)
            {
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<MainMenuOptions>()
                        .Title("[bold yellow]What would you like to do?[/]")
                        .AddChoices(Enum.GetValues(typeof(MainMenuOptions)).Cast<MainMenuOptions>()));

                switch (option)
                {
                    case MainMenuOptions.AddExercise:
                        AddExercise();
                        break;
                    case MainMenuOptions.GetAllExercises:
                        GetAllExercises();
                        break;
                    case MainMenuOptions.GetExercise:
                        GetExercise();
                        break;
                    case MainMenuOptions.UpdateExercise:
                        UpdateExercise();
                        break;
                    case MainMenuOptions.DeleteExercise:
                        DeleteExercise();
                        break;
                    case MainMenuOptions.Quit:
                        AnsiConsole.MarkupLine("[cyan]Goodbye![/]");
                        isAppRunning = false;
                        break;
                }
            }
        }

        private void AddExercise()
        {
            AnsiConsole.MarkupLine("[green]=== Add a New Exercise ===[/]");
            Exercise newExercise = UserInput.GetExerciseInput();
            _controller.CreateExercise(newExercise);
            AnsiConsole.MarkupLine("[green]Exercise created successfully![/]");
        }

        private void GetAllExercises()
        {
            AnsiConsole.MarkupLine("[blue]=== List of All Exercises ===[/]");
            var exercises = _controller.GetExercises().ToList();

            if (!exercises.Any())
            {
                AnsiConsole.MarkupLine("[red]No exercises found.[/]");
                return;
            }

            var table = new Table();
            table.AddColumn("ID");
            table.AddColumn("Start Date");
            table.AddColumn("End Date");
            table.AddColumn("Duration");
            table.AddColumn("Sets");
            table.AddColumn("Reps");
            table.AddColumn("Weight");

            foreach (var exercise in exercises)
            {
                table.AddRow(
                    exercise.Id.ToString(),
                    exercise.DateStart.ToString("yyyy-MM-dd HH:mm"),
                    exercise.DateEnd.ToString("yyyy-MM-dd HH:mm"),
                    exercise.Duration.ToString(),
                    exercise.Sets.ToString(),
                    exercise.Reps.ToString(),
                    exercise.Weight.ToString());
            }

            AnsiConsole.Write(table);
        }

        private void GetExercise()
        {
            int id = AnsiConsole.Ask<int>("Enter the [green]ID[/] of the exercise to view:");
            var exercise = _controller.GetExercise(id);
            if (exercise == null)
            {
                AnsiConsole.MarkupLine("[red]Exercise not found.[/]");
                return;
            }

            var details = $"[bold]ID:[/] {exercise.Id}\n" +
                          $"[bold]Start Date:[/] {exercise.DateStart:yyyy-MM-dd HH:mm}\n" +
                          $"[bold]End Date:[/] {exercise.DateEnd:yyyy-MM-dd HH:mm}\n" +
                          $"[bold]Duration:[/] {exercise.Duration}\n" +
                          $"[bold]Sets:[/] {exercise.Sets}\n" +
                          $"[bold]Reps:[/] {exercise.Reps}\n" +
                          $"[bold]Weight:[/] {exercise.Weight}";
            var panel = new Panel(details).Header("Exercise Details", Justify.Center);
            AnsiConsole.Write(panel);
        }

        private void UpdateExercise()
        {
            int id = AnsiConsole.Ask<int>("Enter the [green]ID[/] of the exercise to update:");
            var exercise = _controller.GetExercise(id);
            if (exercise == null)
            {
                AnsiConsole.MarkupLine("[red]Exercise not found.[/]");
                return;
            }

            AnsiConsole.MarkupLine("[yellow]Enter new details for the exercise:[/]");
            Exercise updatedExercise = UserInput.GetExerciseInput();
            updatedExercise.Id = id;
            _controller.UpdateExercise(updatedExercise);
            AnsiConsole.MarkupLine("[green]Exercise updated successfully![/]");
        }

        private void DeleteExercise()
        {
            int id = AnsiConsole.Ask<int>("Enter the [green]ID[/] of the exercise to delete:");
            var exercise = _controller.GetExercise(id);
            if (exercise == null)
            {
                AnsiConsole.MarkupLine("[red]Exercise not found.[/]");
                return;
            }
            _controller.DeleteExercise(exercise);
            AnsiConsole.MarkupLine("[green]Exercise deleted successfully![/]");
        }
    }

    internal enum MainMenuOptions
    {
        AddExercise,
        GetAllExercises,
        GetExercise,
        UpdateExercise,
        DeleteExercise,
        Quit
    }
}
