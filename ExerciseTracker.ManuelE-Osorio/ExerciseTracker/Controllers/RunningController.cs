using ExerciseTracker.Models;
using ExerciseTracker.Services;
using ExerciseTracker.UserInterface;

namespace ExerciseTracker.Controllers;

public class RunningController(IExerciseService<Running> runningService)
{
    private readonly IExerciseService<Running> RunningServiceInstance = runningService;

    public bool TryConnection()
    {
        try
        {
            return RunningServiceInstance.TryConnection();
        }
        catch
        {
            throw;
        }
    }

    public List<Running>? GetAll()
    {
        MainUI.DisplayLoadingMessage();
        return RunningServiceInstance.GetAll()?.ToList();
    }

    public Running? GetById()
    {
        var exerciseList = GetAll();
        if(exerciseList is null || exerciseList.Count == 0)
            return null;

        var id = InputController.GetId(exerciseList);
        if(id is null)
            return null;

        MainUI.DisplayLoadingMessage();
        return RunningServiceInstance.GetById((int)id);     
    }

    public bool Insert()
    {
        var exercise = InputController.GetExercise<Running>(ConfirmationOptions.add);
        if (exercise is null)
            return false;
        MainUI.DisplayLoadingMessage();
        return RunningServiceInstance.Insert(exercise);
    }

    public bool Delete(Running runningToDelete)
    {
        MainUI.DisplayLoadingMessage();
        return RunningServiceInstance.Delete(runningToDelete);
    }

    public bool Update(UpdateOptions updateOption, Running runningToUpdate)
    {
        DateTime? dateToUpdate;
        string? commentsToUpdate;

        switch(updateOption)
        {
            case(UpdateOptions.startdate):
                dateToUpdate = InputController.GetDate(DateOptions.start, ConfirmationOptions.modify, 
                    runningToUpdate.EndDate);
                if(dateToUpdate == null)
                    return false;
                runningToUpdate.StartDate = (DateTime) dateToUpdate;
                runningToUpdate.Duration = runningToUpdate.EndDate - runningToUpdate.StartDate;
                break;

            case(UpdateOptions.enddate):
                dateToUpdate = InputController.GetDate(DateOptions.end, ConfirmationOptions.modify, 
                    runningToUpdate.StartDate);
                if(dateToUpdate == null)
                    return false;
                runningToUpdate.EndDate = (DateTime) dateToUpdate;
                runningToUpdate.Duration = runningToUpdate.EndDate - runningToUpdate.StartDate;
                break;

            case(UpdateOptions.comments):
                commentsToUpdate = InputController.GetComments(ConfirmationOptions.modify);
                if(commentsToUpdate == null)
                    return false;
                runningToUpdate.Comments = commentsToUpdate;
                break;

        }
        MainUI.DisplayLoadingMessage();
        return RunningServiceInstance.Update(runningToUpdate);
    }
}