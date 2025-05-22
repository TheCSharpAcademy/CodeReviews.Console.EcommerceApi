using ExerciseTracker.KamilKolanowski.Enums;
using ExerciseTracker.KamilKolanowski.Interfaces;


namespace ExerciseTracker.KamilKolanowski.Repositories;

public class ExerciseRepositoryFactory : IExerciseRepositoryFactory
{
    private readonly ExerciseRepository _repository;
    private readonly DapperExerciseRepository _dapperRepository;

    public ExerciseRepositoryFactory(ExerciseRepository repository, DapperExerciseRepository dapperRepository)
    {
        _repository = repository;
        _dapperRepository = dapperRepository;
    }
    
    public IExerciseRepository GetExerciseRepository(ExerciseType exerciseType) =>
        exerciseType == ExerciseType.Weight ? _repository : _dapperRepository;
}