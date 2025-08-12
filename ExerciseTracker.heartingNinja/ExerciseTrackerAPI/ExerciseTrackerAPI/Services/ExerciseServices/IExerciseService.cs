namespace ExerciseTrackerAPI.Services.ExerciseServices
{
    public interface IExerciseService
    {
        Task<List<Exercise>> GetAll();
        Task<List<Exercise>> AddExercise(Exercise exercise);
        Task<List<Exercise>> DeleteExercisesByCustomerId(int customerId);
        Task<Exercise> Get(int id);
        Task<List<Exercise>> GetExercisesByCustomerId(int customerId);
        Task<List<Exercise>> UpdateExercise(int id, Exercise response);
        Task<List<Exercise>> Delete(int id);
    }
}
