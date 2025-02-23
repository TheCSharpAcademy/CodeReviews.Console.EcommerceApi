using ExerciseTracker.Controllers;

namespace ExerciseTracker;

public class App {
    private readonly MainMenuController Controller;
    public App(MainMenuController controller)
    {
        Controller = controller;
    }

    public async Task Run() {
        while(true)
        {
            UI.Clear();
            var option = UI.MenuSelection("[green]Exercise Tracker[/] Main Menu", [
                "Exit",
                ..MainMenuController.Options
            ]);

            if (option == "Exit")
            {
                break;
            }

            await Controller.HandleChoice(option);
        }
    }
}