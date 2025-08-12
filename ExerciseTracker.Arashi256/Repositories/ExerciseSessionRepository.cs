using ExerciseTracker.Arashi256.Interfaces;
using ExerciseTracker.Arashi256.Models;
using ExerciseTracker.Arashi256.Classes;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Arashi256.Repositories
{
    public class ExerciseSessionRepository : IExerciseSessionRepository
    {
        private readonly ExerciseDbContext _context;

        public ExerciseSessionRepository(ExerciseDbContext context)
        {
            _context = context;
        }

        public ServiceResponse AddExerciseSession(ExerciseSession exercise)
        {
            try
            {
                _context.ExerciseSessions.Add(exercise);
                _context.SaveChanges();
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", exercise);
            }
            catch (DbUpdateException ex)
            {
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, ex.Message, null);
            }
        }

        public ServiceResponse DeleteExerciseSession(int id)
        {
            try
            {
                var session = _context.ExerciseSessions.Find(id);
                if (session == null)
                    return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "Exercise session not found", null);
                else
                {
                    _context.ExerciseSessions.Remove(session);
                    _context.SaveChanges();
                    return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", null);
                }
            }
            catch (DbUpdateException ex)
            {
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, ex.Message, null);
            }
        }

        public ServiceResponse GetExerciseSessions()
        {
            // AsNoTracking() used here to cope with operations outside EF Core like when mixing DB operations with Dapper for the extra challenge.
            List<ExerciseSession> sessions = _context.ExerciseSessions.AsNoTracking().ToList();
            if (sessions != null && sessions.Count > 0)
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", sessions);
            else
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "No exercise sessions found", sessions);
        }

        public ServiceResponse GetExerciseSessionById(int id)
        {
            var session = _context.ExerciseSessions.Find(id);
            if (session != null)
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", session);
            else
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "Exercise session not found", null);
        }

        public ServiceResponse UpdateExerciseSession(int id, ExerciseSession exercise)
        {
            if (id != exercise.Id)
            {
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "Update exercise session and id mismatch", null);
            }
            var existingSession = _context.ExerciseSessions.Find(id);
            if (existingSession == null)
            {
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "Exercise session not found", null);
            }
            existingSession.Type = exercise.Type;
            existingSession.DateTimeStart = exercise.DateTimeStart;
            existingSession.DateTimeEnd = exercise.DateTimeEnd;
            existingSession.Duration = exercise.Duration;
            existingSession.Comments = exercise.Comments;
            try
            {
                _context.SaveChanges();
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", existingSession);
            }
            catch (DbUpdateException ex)
            {
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, ex.Message, null);
            }
        }

        public ServiceResponse ExerciseSessionExistsInRange(DateTime startDate, DateTime endDate, int? sessionIdToExclude = null)
        {
            try
            {
                bool isOverlapping = _context.ExerciseSessions.AsNoTracking().Any(session => (!sessionIdToExclude.HasValue || session.Id != sessionIdToExclude.Value) && startDate < session.DateTimeEnd && endDate > session.DateTimeStart);
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", isOverlapping);
            }
            catch (Exception ex)
            {
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, ex.Message, null);
            }
        }
    }
}