using ExerciseTracker.Arashi256.Classes;
using ExerciseTracker.Arashi256.Interfaces;
using ExerciseTracker.Arashi256.Models;
using ExerciseTracker.Arashi256.Repositories;
using ExerciseTracker.Arashi256.Enums;

namespace ExerciseTracker.Arashi256.Services
{
    internal class ExerciseSessionService
    {
        private readonly IExerciseSessionRepository _exerciseRepository;
        private readonly IExerciseSessionRepository _exerciseRepositoryDapper;

        public ExerciseSessionService(ExerciseSessionRepository repEFCore, ExerciseSessionRepositoryDapper repDapper)
        {
            _exerciseRepository = repEFCore;
            _exerciseRepositoryDapper = repDapper;
        }

        // Method to get all exercise sessions and assign DisplayIds
        public ServiceResponse GetAllExerciseSessions()
        {
            ServiceResponse serviceResponse = _exerciseRepository.GetExerciseSessions();
            if (serviceResponse.Status.Equals(ResponseStatus.Success))
            {
                // Translate ExerciseSessions to ExerciseSessionsOutputDtos
                List<ExerciseSession>? sessions = serviceResponse.Data as List<ExerciseSession>;
                if (sessions != null && sessions.Count > 0)
                {
                    serviceResponse.Data = ConvertToOutputDtoList(sessions);
                }
                return serviceResponse;
            }
            else
                return serviceResponse;
        }

        private List<ExerciseSessionOutputDto> ConvertToOutputDtoList(List<ExerciseSession> sessions) 
        {
            int displayID = 0;
            List<ExerciseSessionOutputDto> outputSessions = new List<ExerciseSessionOutputDto>();
            foreach (var session in sessions)
            {
                outputSessions.Add(ConvertToOutputDto(++displayID, session));
            }
            return outputSessions;
        }

        private ExerciseSessionOutputDto ConvertToOutputDto(int displayID, ExerciseSession session)
        {
            var sessionDto = new ExerciseSessionOutputDto
            {
                Id = session.Id,
                DisplayId = displayID,
                Type = session.Type,
                DateTimeStart = session.DateTimeStart,
                DateTimeEnd = session.DateTimeEnd,
                Duration = session.Duration,
                Comments = session.Comments,
            };
            return sessionDto;
        }

        public ServiceResponse AddNewExerciseSession(ExerciseSessionInputDto newSession)
        {
            ServiceResponse response;
            bool hasOverlap = false;
            if (!Validation.ValidateDatesForDuration(newSession.DateTimeStart, newSession.DateTimeEnd))
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "End datetime cannot be before start datetime", null);
            else
            {
                if (newSession.Type.Equals(ExerciseType.HIKING))
                {
                    // If HIKING, check for overlapping exercise sessions before doing anything from EF Core.
                    response = _exerciseRepository.ExerciseSessionExistsInRange(newSession.DateTimeStart, newSession.DateTimeEnd);
                }
                else
                {
                    // If CARDIO, Check for overlapping exercise sessions before doing anything from Dapper.
                    response = _exerciseRepositoryDapper.ExerciseSessionExistsInRange(newSession.DateTimeStart, newSession.DateTimeEnd);
                }
                if (response.Status.Equals(ResponseStatus.Success))
                {
                    if (response.Data is bool isOverlapping)
                    {
                        hasOverlap = isOverlapping;
                        if (hasOverlap)
                        {
                            return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "The exercise session overlaps with an existing exercise session", null);
                        }
                    }
                    else
                    {
                        return response; // If not boolean, return response which will be error anyway.
                    }
                }
                TimeSpan duration = Validation.CalculateDuration(newSession.DateTimeStart, newSession.DateTimeEnd);
                var addSession = new ExerciseSession
                {
                    Type = newSession.Type,
                    DateTimeStart = newSession.DateTimeStart,
                    DateTimeEnd = newSession.DateTimeEnd,
                    Duration = duration,
                    Comments = newSession.Comments
                };
                if (addSession.Type.Equals(ExerciseType.HIKING))
                    return _exerciseRepository.AddExerciseSession(addSession);
                else
                    return _exerciseRepositoryDapper.AddExerciseSession(addSession);
            }
        }

        public ServiceResponse DeleteExistingExerciseSession(int id)
        {
            ServiceResponse serviceResponse = _exerciseRepository.GetExerciseSessionById(id);
            if (serviceResponse.Status.Equals(ResponseStatus.Success))
            {
                return _exerciseRepository.DeleteExerciseSession(id);
            }
            else
                return serviceResponse;
        }

        public ServiceResponse UpdateExistingExerciseSession(int id, ExerciseSessionInputDto updateSession)
        {
            ServiceResponse response;
            bool hasOverlap = false;
            if (!Validation.ValidateDatesForDuration(updateSession.DateTimeStart, updateSession.DateTimeEnd))
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "End datetime cannot be before start datetime", null);
            else
            {
                if (updateSession.Type.Equals(ExerciseType.HIKING))
                {
                    // If HIKING, check for overlapping exercise sessions before doing anything from EF Core.
                    response = _exerciseRepository.ExerciseSessionExistsInRange(updateSession.DateTimeStart, updateSession.DateTimeEnd, updateSession.Id);
                }
                else
                {
                    // If CARDIO, Check for overlapping exercise sessions before doing anything from Dapper.
                    response = _exerciseRepositoryDapper.ExerciseSessionExistsInRange(updateSession.DateTimeStart, updateSession.DateTimeEnd, updateSession.Id);
                }
                if (response.Status.Equals(ResponseStatus.Success))
                {
                    if (response.Data is bool isOverlapping)
                    {
                        hasOverlap = isOverlapping;
                        if (hasOverlap)
                        {
                            return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "The exercise session overlaps with an existing exercise session", null);
                        }
                    }
                    else
                    {
                        return response; // If not boolean, return response.
                    }
                }
                TimeSpan duration = Validation.CalculateDuration(updateSession.DateTimeStart, updateSession.DateTimeEnd);
                var newSession = new ExerciseSession
                {
                    Id = updateSession.Id,
                    Type = updateSession.Type,
                    DateTimeStart = updateSession.DateTimeStart,
                    DateTimeEnd = updateSession.DateTimeEnd,
                    Duration = duration,
                    Comments = updateSession.Comments
                };
                if (newSession.Type.Equals(ExerciseType.HIKING))
                {
                    // If HIKING, check for overlapping exercise sessions before doing anything from EF Core.
                    response = _exerciseRepository.UpdateExerciseSession(updateSession.Id, newSession);
                }
                else
                {
                    // If CARDIO, Check for overlapping exercise sessions before doing anything from Dapper.
                    response = _exerciseRepositoryDapper.UpdateExerciseSession(updateSession.Id, newSession);
                }
                if (response.Status.Equals(ResponseStatus.Success))
                {
                    var exerciseSessionUpdated = response.Data as ExerciseSession;
                    if (exerciseSessionUpdated != null)
                    {
                        return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", ConvertToOutputDto(1, exerciseSessionUpdated));
                    }
                    else
                        return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "Cannot format output session", null);
                }
                else
                    return response;
            }
        }

        public ServiceResponse GetExerciseSessionById(int id)
        {
            ServiceResponse response = _exerciseRepository.GetExerciseSessionById(id);
            if (response.Equals(ResponseStatus.Success))
            {
                ExerciseSession? session = response.Data as ExerciseSession;
                if (session != null)
                {
                    var outputSession = ConvertToOutputDto(1, session);
                    return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", outputSession);
                }
                else
                    return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "Could not retrieve ExerciseSession", null);
            }
            else
                return response;
        }
    }
}
