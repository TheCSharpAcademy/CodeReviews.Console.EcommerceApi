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
                Console.Clear();

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
            Console.Clear();

            AnsiConsole.MarkupLine("[green]=== Add a New Exercise ===[/]");
            Exercise newExercise = UserInput.GetExerciseInput();
            _controller.CreateExercise(newExercise);
            AnsiConsole.MarkupLine("[green]Exercise created successfully![/]");

            AnsiConsole.MarkupLine("\nPress any key to continue.");
            Console.ReadLine();
        }

        private void GetAllExercises()
        {

            Console.Clear ();

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

            AnsiConsole.MarkupLine("\nPress any key to continue.");
            Console.ReadLine();
        }

        private void GetExercise()
        {
            Console.Clear();

            var exercise = UserInput.GetExerciseOptionInput(_controller);
            if (exercise == null) return;

            var details = $"[bold]ID:[/] {exercise.Id}\n" +
                          $"[bold]Start Date:[/] {exercise.DateStart:yyyy-MM-dd HH:mm}\n" +
                          $"[bold]End Date:[/] {exercise.DateEnd:yyyy-MM-dd HH:mm}\n" +
                          $"[bold]Duration:[/] {exercise.Duration}\n" +
                          $"[bold]Sets:[/] {exercise.Sets}\n" +
                          $"[bold]Reps:[/] {exercise.Reps}\n" +
                          $"[bold]Weight:[/] {exercise.Weight}";

            var panel = new Panel(details).Header("Exercise Details", Justify.Center);
            AnsiConsole.Write(panel);

            AnsiConsole.MarkupLine("\nPress any key to continue.");
            Console.ReadLine();
        }

        private void UpdateExercise()
        {
            Console.Clear();

            var exercise = UserInput.GetExerciseOptionInput(_controller);
            if (exercise == null) return;

            AnsiConsole.MarkupLine("[yellow]Enter new details for the exercise:[/]");
            Exercise updatedExercise = UserInput.GetExerciseInput();
            updatedExercise.Id = exercise.Id; // Preserve ID
            _controller.UpdateExercise(updatedExercise);
            AnsiConsole.MarkupLine("[green]Exercise updated successfully![/]");

            AnsiConsole.MarkupLine("\nPress any key to continue.");
            Console.ReadLine();
        }

        private void DeleteExercise()
        {
            Console.Clear();

            var exercise = UserInput.GetExerciseOptionInput(_controller);
            if (exercise == null) return;

            _controller.DeleteExercise(exercise);
            AnsiConsole.MarkupLine("[green]Exercise deleted successfully![/]");

            AnsiConsole.MarkupLine("\nPress any key to continue.");
            Console.ReadLine();
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
