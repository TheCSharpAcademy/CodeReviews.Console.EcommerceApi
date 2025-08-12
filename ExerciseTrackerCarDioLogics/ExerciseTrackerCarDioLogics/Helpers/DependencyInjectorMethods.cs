using ExerciseTrackerCarDioLogics.Data;
using ExerciseTrackerCarDioLogics.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTrackerCarDioLogics.Helpers;

public class DependencyInjectorMethods
{
    public SessionController EFImplementationService()
    {
        //test
        Console.WriteLine("Entity framework is being used!");
        Console.ReadLine();

        SessionContext SessionContext = new SessionContext();
        ISessionRepository eFSessionRepository = new EFSessionRepository(SessionContext);
        SessionService SessionServiceEF = new SessionService(eFSessionRepository);
        SessionController SessionControllerEF = new SessionController(SessionServiceEF);

        return SessionControllerEF;
    }

    public SessionController RawSQLImplementationService()
    {
        //test
        Console.WriteLine("Raw SQL is being used!");
        Console.ReadLine();

        string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=ExerciseTrackerDB;Trusted_Connection=True;";
        ISessionRepository rawSQLSessionRepository = new RawSQLSessionRepository(connectionString);
        SessionService SessionServiceRawSQL = new SessionService(rawSQLSessionRepository);
        SessionController SessionControllerRawSQL = new SessionController(SessionServiceRawSQL);

        return SessionControllerRawSQL;
    }
}
