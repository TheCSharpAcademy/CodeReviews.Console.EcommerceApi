using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Exercisetacker.Controllers;
using System;
using System.Threading;
using System.Threading.Tasks;
using Exercisetacker.UI;

public class ConsoleAppService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _appLifetime;

    public ConsoleAppService(IServiceProvider serviceProvider, IHostApplicationLifetime appLifetime)
    {
        _serviceProvider = serviceProvider;
        _appLifetime = appLifetime;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var joggingController = scope.ServiceProvider.GetRequiredService<JoggingController>();

            var menu = new Menu();
            var runApplication = true;
            while (runApplication)
            {
                Console.Clear();
                var choice = menu.GetMainMenu();
                switch (choice)
                {
                    case "View All Sessions":
                        await joggingController.DisplayAllJoggingSessions();
                        break;
                    case "Add Jogging Session":
                        await joggingController.AddJogging();
                        break;
                    case "Update Jogging Session":
                        await joggingController.UpdateJogging();
                        break;
                    case "Delete Jogging Session":
                        await joggingController.DeleteJogging();
                        break;
                    case "Exit":
                        runApplication = false;
                        _appLifetime.StopApplication();
                        break;
                    default:
                        break;
                }
            }
        }
        await Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

}
