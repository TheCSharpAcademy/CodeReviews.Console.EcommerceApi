using Exercisetacker.Services.Interfaces;
using Exercisetacker.UI;

namespace Exercisetacker.Controllers;

public class JoggingController
{
    private readonly IJoggingService _joggingService;

    public JoggingController(IJoggingService joggingService)
    {
        _joggingService = joggingService;
    }

    public async Task DisplayAllJoggingSessions()
    {
        var joggings = await _joggingService.GetAllJoggings();
        VisualizationEngine.DisplayAllJoggings(joggings, "Showing All Jogging Sessions");
        VisualizationEngine.DisplayContinueMessage();
    }

    internal async Task AddJogging()
    {
        var jogging = UserInput.GetNewJogging();

        if (jogging == null)
        {
            VisualizationEngine.DisplayCancelOperation();
            return;
        }
        
        var addedjogging = await _joggingService.AddJogging(jogging);
        if (addedjogging == null)
        {
            VisualizationEngine.DisplayFailureMessage("Jogging Session is not added to database.");
        }
        else
        {
            VisualizationEngine.DisplaySuccessMessage("Jogging Session is added to database successfully.");
        }
    }

    internal async Task UpdateJogging()
    {
        var joggings = await _joggingService.GetAllJoggings();
        VisualizationEngine.DisplayAllJoggings(joggings, "All Joggings Table");
        int joggingId = UserInput.GetIntInput();
        if(joggingId == 0)
        {
            VisualizationEngine.DisplayCancelOperation();
            return;
        }

        var jogging = await _joggingService.GetJoggingById(joggingId);
        
        if(jogging == null)
        {
            VisualizationEngine.DisplayFailureMessage($"Jogging with id: [green]{joggingId}[/] not found.");
            return;
        }

        if(!UserInput.GetUpdateJogging(jogging))
        {
            VisualizationEngine.DisplayCancelOperation();
            return;
        }
        if(await _joggingService.UpdateJogging(jogging) == 0)
        {
            VisualizationEngine.DisplayFailureMessage("Jogging is not updated.");
        }
        else
        {
            VisualizationEngine.DisplaySuccessMessage("Jogging is updated.");
        }
    }

    internal async Task DeleteJogging()
    {
        var joggings = await _joggingService.GetAllJoggings();
        VisualizationEngine.DisplayAllJoggings(joggings, "All Joggings Table");
        int joggingId = UserInput.GetIntInput();
        if(joggingId == 0)
        {
            VisualizationEngine.DisplayCancelOperation();
            return;
        }

        var jogging = await _joggingService.GetJoggingById(joggingId);
        
        if(jogging == null)
        {
            VisualizationEngine.DisplayFailureMessage($"Jogging with id: [green]{joggingId}[/] not found.");
            return;
        }

        if(await _joggingService.DeleteJogging(jogging) == 0)
        {
            VisualizationEngine.DisplayFailureMessage("Jogging is not deleted.");
        }
        else
        {
            VisualizationEngine.DisplaySuccessMessage("Jogging is deleted.");
        }
    }

}