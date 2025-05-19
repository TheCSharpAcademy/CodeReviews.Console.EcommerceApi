using System.ComponentModel.DataAnnotations;

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

    [StringLength(200)]
    public string? Comment { get; set; }
}
