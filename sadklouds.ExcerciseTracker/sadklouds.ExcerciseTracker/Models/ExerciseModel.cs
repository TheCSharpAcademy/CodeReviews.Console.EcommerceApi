using System.ComponentModel.DataAnnotations;

namespace sadklouds.ExcerciseTracker.Models;
public class ExerciseModel
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan Duration { get; set; }
    [MaxLength(255)]
    public string? Comments { get; set; }
}
