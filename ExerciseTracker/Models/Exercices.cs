namespace ExerciseTracker.Models;

public interface IExercices
{
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public TimeSpan? Duration => End - Start;
    public string? Comment { get; set; }
}

public class Exercice : IExercices
{
    public Exercice(int id, DateTime start, DateTime end, string comment)
    {
        Id = id;
        Start = start;
        End = end;
        Comment = comment;
    }

    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public TimeSpan? Duration => End - Start;
    public string? Comment { get; set; }
}

public class FieldTours : Exercice
{
    public FieldTours(int id, DateTime start, DateTime end, string comment) : base(id,start,end,comment)
    {   
        Id = id;
        Start = start;
        End = end;
        Comment = comment;
    }

    public new int Id { get; set; }
    public new DateTime Start { get; set; }
    public new DateTime End { get; set; }
    public new TimeSpan? Duration => End - Start;
    public new string? Comment { get; set; }

}

public class FreeKicks : Exercice
{
    public FreeKicks(int id, DateTime start, DateTime end, string comment) : base(id, start, end, comment)
    {
        Id = id;
        Start = start;
        End = end;
        Comment = comment;
    }

    public new int Id { get; set; }
    public new DateTime Start { get; set; }
    public new DateTime End { get; set; }
    public new TimeSpan? Duration => End - Start;
    public new string? Comment { get; set; }

}