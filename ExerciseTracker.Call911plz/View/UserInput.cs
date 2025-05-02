using Spectre.Console;

public static class GetData
{
    public static Exercise GetExercise()
    {
        Exercise exercise = new()
        {
            Id = default,
            Start = GetDateTime("Enter start time: "),
            End = GetDateTime("Enter end time: "),
            Comments = GetComments(),
        };
        exercise.Duration = exercise.End - exercise.Start;

        return exercise;
    }

    public static Exercise UpdateExercise(Exercise existingExercise)
    {
        Exercise exercise = new()
        {
            Id = existingExercise.Id,
            Start = GetDateTime("Enter updated start time: ", existingExercise.Start),
            End = GetDateTime("Enter updated end time: ", existingExercise.End),
            Comments = GetComments(existingExercise.Comments),
        };
        exercise.Duration = exercise.End - exercise.Start;

        return exercise;
    }

    private static DateTime GetDateTime(string stringPrompt, DateTime existingDateTime = default)
    {
        TextPrompt<string> prompt = new($"[bold grey]{stringPrompt}[/]");

        if (existingDateTime != default)
            prompt.DefaultValue(existingDateTime.ToString());
        else
            prompt.DefaultValue(DateTime.Now.ToString());
        
        prompt.Validate( (input) => {
            if (DateTime.TryParse(input, out var _))
                return ValidationResult.Success();
            return ValidationResult.Error("Invalid date time format");
        });

        return DateTime.Parse(AnsiConsole.Prompt(prompt));
    }

    private static string GetComments(string? existingComments = default)
    {
        TextPrompt<string> prompt = new("[bold grey](Optional) Additional comments[/]");

        prompt.AllowEmpty();

        if (existingComments != default)
            prompt.DefaultValue(existingComments);

        return AnsiConsole.Prompt(prompt);
    }
}

public class GetMenu
{
    public MenuEnums.Main MainMenu()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<MenuEnums.Main>()
                .AddChoices(Enum.GetValues<MenuEnums.Main>())
                .UseConverter((input) => input switch
                {
                    MenuEnums.Main.CREATE => "Create new exercise log",
                    MenuEnums.Main.READ => "Find all exercise logs",
                    MenuEnums.Main.READALL => "Find specific exercise log",
                    MenuEnums.Main.UPDATE => "Update exercise log",
                    MenuEnums.Main.DELETE => "Delete exercise log",
                    MenuEnums.Main.EXIT => "Exit program",
                    _ => throw new Exception("Selection somehow went wrong")
                })
        );
    }
}