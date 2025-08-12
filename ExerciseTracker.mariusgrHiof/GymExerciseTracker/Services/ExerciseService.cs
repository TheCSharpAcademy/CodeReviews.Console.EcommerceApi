using GymExerciseTracker.Dtos;
using GymExerciseTracker.Models;
using GymExerciseTracker.Repository;

namespace GymExerciseTracker.Services;
public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;

    public ExerciseService(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public GymSession AddGymSession(AddGymSessionDto newGymSession)
    {
        if (newGymSession == null) return null;
        GymSession newSession = new GymSession()
        {
            Name = newGymSession.Name,
            Sets = newGymSession.Sets,
            Reps = newGymSession.Reps,
            StartDate = newGymSession.StartDate,
            EndDate = newGymSession.EndDate,
            Comments = newGymSession.Comments,
        };

        _exerciseRepository.CreateSession(newSession);

        return newSession;
    }

    public GymSession GetGymSession(int sessionId)
    {
        var session = _exerciseRepository.GetSession(sessionId);
        if (session == null) return null;

        return session;
    }

    public List<GymSession> GetGymSessions()
    {
        return _exerciseRepository.GetAllSessions();
    }

    public GymSession UpdateGymSession(int id, UpdateGymSessionDto updatedGymSession)
    {
        if (updatedGymSession == null || id == null) return null;
        if (updatedGymSession.Id != id) return null;

        var session = _exerciseRepository.GetSession(id);
        if (session == null) return null;

        session.Name = updatedGymSession.Name;
        session.Sets = updatedGymSession.Sets;
        session.Reps = updatedGymSession.Reps;
        session.StartDate = updatedGymSession.StartDate;
        session.EndDate = updatedGymSession.EndDate;
        session.Comments = updatedGymSession.Comments;

        var updatedSession = _exerciseRepository.UpdateSession(id, session);
        if (updatedSession == null) return null;

        return updatedSession;


    }

    public GymSession DeleteGymSession(int id)
    {
        var session = _exerciseRepository.GetSession(id);
        if (session == null) return null;

        var deletedSession = _exerciseRepository.DeleteSession(id);
        if (deletedSession == null) return null;

        return deletedSession;


    }
}

