namespace ExerciseTrackerCLI.Services;

public class Notifications
{
    private readonly Queue<string> _queue = new();

    public void AddError(string msg)
    {
        _queue.Enqueue($"[red]{msg}[/]");
    }

    public void AddSuccess(string msg)
    {
        _queue.Enqueue($"[green]{msg}[/]");
    }

    public bool HasNext()
    {
        return _queue.Count > 0;
    }

    public string GetNext()
    {
        return _queue.Dequeue();
    }
}