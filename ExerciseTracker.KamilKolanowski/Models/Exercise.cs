using System.ComponentModel.DataAnnotations;
using ExerciseTracker.KamilKolanowski.Enums;

namespace ExerciseTracker.KamilKolanowski.Models;

public class Exercise
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string? Name { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public TimeSpan Duration
    {
        get => DateEnd - DateStart;
    }

    [StringLength(10)]
    public string ExerciseType { get; set; } = String.Empty;

    [StringLength(200)]
    public string? Comment { get; set; }
}
