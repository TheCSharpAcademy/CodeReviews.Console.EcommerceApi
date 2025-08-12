using GymExerciseTracker.Dtos;
using GymExerciseTracker.Models;

namespace GymExerciseTracker.Services
{
    public interface IExerciseService
    {
        List<GymSession> GetGymSessions();
        GymSession GetGymSession(int sessionId);
        GymSession AddGymSession(AddGymSessionDto newGymSession);
        GymSession UpdateGymSession(int id, UpdateGymSessionDto updatedGymSession);
        GymSession DeleteGymSession(int id);
    }
}