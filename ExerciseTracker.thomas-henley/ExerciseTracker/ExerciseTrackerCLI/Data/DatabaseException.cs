namespace ExerciseTrackerCLI.Data;

public class DatabaseException : Exception
{
    public DatabaseException(string? s) : base(s) { }

    public DatabaseException(string? s, Exception? innerException) : base(s, innerException) { }
}