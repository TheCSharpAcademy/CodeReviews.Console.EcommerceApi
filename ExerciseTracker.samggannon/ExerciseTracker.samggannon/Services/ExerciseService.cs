using ExerciseTracker.samggannon.Data.Models;
using ExerciseTracker.samggannon.Data.Repositories;
using ExerciseTracker.samggannon.UserInterface;
using ExerciseTracker.samggannon.Validation;

namespace ExerciseTracker.samggannon.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _cardioRepository;
    private readonly IExerciseRepository _resistanceRepository;
    private IExerciseRepository? _currentRepository;

    public ExerciseService()
    {
        _cardioRepository = new ExerciseRepository();
        _resistanceRepository = new ExerciseRepository();
    }

    public ExerciseService(ExerciseRepository exerciseRepository, ResistanceRespository resistanceRepository)
    {
        _cardioRepository = exerciseRepository;
        _resistanceRepository = resistanceRepository;
    }

    public void SetRepository(bool isResistanceTraining)
    {
        _currentRepository = isResistanceTraining ? (IExerciseRepository)_resistanceRepository : _cardioRepository;
    }

    public void DeleteSessionById()
    {
        GetAllSessions();

        int sessionIdToDelete = UserInput.GetIdInput();
        var exerciseSession = _currentRepository.GetSessionById(sessionIdToDelete);

        if (exerciseSession == null)
        {
            Console.WriteLine($"No session found with the id: {sessionIdToDelete}. Press [enter] to continue.");
            Console.ReadLine();
            return;
        }
    }

    public void EditSession()
    {
        GetAllSessions();

        int sessionId = UserInput.GetIdInput();
        var exerciseSession = _currentRepository.GetSessionById(sessionId);

        if (exerciseSession == null)
        {
            Console.WriteLine($"No session found with the id: {sessionId}. Press [enter] to continue.");
            Console.ReadLine();
            return;
        }

        var updatedSession = MainMenu.UpdateMenu(exerciseSession);

        bool editTimeWasValid = InputValidation.ValidateTime(updatedSession.DateStart, updatedSession.DateEnd);

        if(editTimeWasValid)
        {
            updatedSession.Id = sessionId;
            _currentRepository.Update(updatedSession);
        }
        else
        {
            Console.WriteLine("Start time must be before end time. No changes were made. Press [enter] to continue.");
            Console.ReadLine();
        }
    }

    public void GetAllSessions()
    {
       // defaulted EF Core repo
        List<Exercise> exercises = _cardioRepository.GetAllSessions();
        Visualization.ShowTable(exercises);
    
        Console.WriteLine("Press [enter] to continue");
        Console.ReadLine();
    }

    public void InsertSession()
    {
        Exercise exerciseSession = UserInput.GetSessionDetails();

       _currentRepository.Add(exerciseSession);
    }
}
