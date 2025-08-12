using ExerciseTracker.Controllers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ExerciseTracker;

public class ExerciseTracker
{
    public static void Main()
    {
        IHost? app;
        try
        {
            app = StartUp.AppInit();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            Thread.Sleep(4000);
            return;
        }

        var menu = new MenuController(app.Services.CreateScope()
            .ServiceProvider.GetRequiredService<RunningController>());
        menu.Start();
    }
}
