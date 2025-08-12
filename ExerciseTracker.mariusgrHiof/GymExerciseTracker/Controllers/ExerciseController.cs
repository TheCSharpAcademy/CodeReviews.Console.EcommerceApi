using GymExerciseTracker.Dtos;
using GymExerciseTracker.Models;
using GymExerciseTracker.Services;

namespace GymExerciseTracker.Controllers;
public class ExerciseController
{
    private readonly IExerciseService _exerciseService;

    public ExerciseController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    public List<GymSession> GetAllGymSessions()
    {
        return _exerciseService.GetGymSessions();
    }

    public GymSession GetGymSession(int id)
    {
        var session = _exerciseService.GetGymSession(id);
        if (session == null) return null;

        return session;
    }

    public GymSession AddGymSession(AddGymSessionDto newSession)
    {
        if (newSession == null) return null;

        var sessionToAdd = _exerciseService.AddGymSession(newSession);
        if (sessionToAdd == null) return null;

        return sessionToAdd;
    }

    public GymSession UpdateGymSession(int id, UpdateGymSessionDto updatedGymSessionDto)
    {
        var sessionToUpdate = _exerciseService.UpdateGymSession(id, updatedGymSessionDto);
        if (sessionToUpdate == null) return null;

        return sessionToUpdate;
    }

    public GymSession DeleteGymSession(int id)
    {
        var sessionToDelete = _exerciseService.DeleteGymSession(id);
        if (sessionToDelete == null) return null;

        return sessionToDelete;
    }
}

