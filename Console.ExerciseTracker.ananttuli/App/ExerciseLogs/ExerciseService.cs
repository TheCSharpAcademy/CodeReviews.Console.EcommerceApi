using App.Database;
using App.ExerciseLogs.Models;
using App.Util;

namespace App.ExerciseLogs;

public class ExerciseService
{
    private readonly IRepositoryBase<ExerciseLog> ExerciseLogsRepo;

    public ExerciseService(IRepositoryBase<ExerciseLog> exerciseLogsRepo)
    {
        ExerciseLogsRepo = exerciseLogsRepo;
    }

    public async Task<ExerciseLog> CreateExerciseLog(ExerciseLogCreateDto payload)
    {
        AssertLogTimesValid(payload.StartDateTime, payload.EndDateTime);

        return await ExerciseLogsRepo.CreateOne(new ExerciseLog
        {
            StartDateTime = payload.StartDateTime,
            EndDateTime = payload.EndDateTime,
            Comments = payload.Comments
        });
    }

    public async Task<ExerciseLog> GetExerciseLog(int id) => await ExerciseLogsRepo.GetById(id);

    public async Task<List<ExerciseLog>> ListAllLogs() => await ExerciseLogsRepo.ListAll();

    public async Task<ExerciseLog> UpdateExerciseLog(int id, ExerciseLog payload)
    {
        if (id != payload.Id)
        {
            throw new Exception("Update ID must match entity ID");
        }

        AssertLogTimesValid(payload.StartDateTime, payload.EndDateTime);

        var updatedLog = await ExerciseLogsRepo.UpdateOne(payload);

        return updatedLog;
    }

    public async Task<bool?> DeleteExerciseLog(int id)
    {
        return await ExerciseLogsRepo.DeleteOne(id);
    }

    private void AssertLogTimesValid(DateTime? startDateTime, DateTime? endDateTime)
    {
        if (startDateTime == null || endDateTime == null)
        {
            throw new UserFacingException("Dates must be supplied");
        }

        if (startDateTime > endDateTime)
        {
            throw new UserFacingException("Start date cannot be later than end date");
        }
    }
}