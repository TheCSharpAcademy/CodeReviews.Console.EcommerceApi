using ExerciseTracker.KroksasC.Models;

namespace ExerciseTracker.KroksasC.Repositaries
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Exercise> Exercises { get; }

        Task<int> CompleteAsync();
    }
}
