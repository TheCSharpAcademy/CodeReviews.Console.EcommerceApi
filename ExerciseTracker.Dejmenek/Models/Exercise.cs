using System.ComponentModel.DataAnnotations.Schema;

namespace ExerciseTracker.Dejmenek.Models;
public class Exercise
{
    public int Id { get; set; }
    [Column(TypeName = "DateTime")]
    public DateTime StartTime { get; set; }
    [Column(TypeName = "DateTime")]
    public DateTime EndTime { get; set; }
    public string Duration { get; set; } = null!;
    public string? Comments { get; set; }
}

public class ExerciseUpdateDTO
{
    public DateTime EndTime { get; set; }
    public string Duration { get; set; } = null!;
    public string? Comments { get; set; }
}

public class ExerciseReadDTO
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Duration { get; set; } = null!;
    public string? Comments { get; set; }
}
