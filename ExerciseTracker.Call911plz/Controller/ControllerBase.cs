using Spectre.Console;

public class ControllerBase
{
    internal virtual void OnStartOfLoop(){ Console.Clear(); }
    internal virtual Task<bool> HandleUserInputAsync() { return Task.FromResult(false); } 
    
    public async Task StartAsync()
    {
        bool exit = false;

        while (exit == false)
        {
            try { OnStartOfLoop(); }
            catch (Exception e) { AnsiConsole.MarkupLine($"[bold red]Error on Start Of Loop[/]\n{e}"); }

            try { exit = await HandleUserInputAsync(); }
            catch (Exception e) { AnsiConsole.MarkupLine($"[bold red]Error on Handling User Input[/]\n{e}"); }

            if (exit == false)
            {
                AnsiConsole.MarkupLine("[bold yellow]Press Enter to continue[/]");
                Console.Read();
            }
        }
    }
}