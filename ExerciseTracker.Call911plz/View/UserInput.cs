using Spectre.Console;

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