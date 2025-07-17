using System.ComponentModel.DataAnnotations;

namespace Exercise_Tracker.Models;

public class Exercise
{
    [Key]
    public int Id { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public TimeSpan Duration => EndTime - StartTime;

    public string Comments { get; set; } = string.Empty;
}
