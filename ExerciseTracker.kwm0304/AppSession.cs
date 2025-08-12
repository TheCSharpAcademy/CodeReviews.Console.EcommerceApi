using ExerciseTracker.kwm0304.Controllers;
using ExerciseTracker.kwm0304.Views;
using Spectre.Console;

namespace ExerciseTracker.kwm0304
{
  public class AppSession
  {
    private readonly ExerciseController _controller;
    public AppSession(ExerciseController controller)
    {
      _controller = controller;
    }
    public async Task OnStart()
    {
      bool running = true;
      while (running)
      {
        Console.Clear();
        AnsiConsole.WriteLine(@"
___________                           .__                ___________                     __                 
\_   _____/__  ___ ___________   ____ |__| ______ ____   \__    ___/___________    ____ |  | __ ___________ 
 |    __)_\  \/  // __ \_  __ \_/ ___\|  |/  ___// __ \    |    |  \_  __ \__  \ _/ ___\|  |/ // __ \_  __ \
 |        \>    <\  ___/|  | \/\  \___|  |\___ \\  ___/    |    |   |  | \// __ \\  \___|    <\  ___/|  | \/
/_______  /__/\_ \\___  >__|    \___  >__/____  >\___  >   |____|   |__|  (____  /\___  >__|_ \\___  >__|   
        \/      \/    \/            \/        \/     \/                        \/     \/     \/    \/       
");
        string choice = SelectionMenus.MainMenu();
        switch (choice)
        {
          case "Add entry":
            running = await _controller.HandleAddEntry();
            break;
          case "View entries":
            running = await _controller.HandleViewEntries();
            break;
          case "Exit":
            AnsiConsole.WriteLine("Goodbye!");
            Environment.Exit(0);
            break;
          default:
            break;
        }
      }
    }
  }
}