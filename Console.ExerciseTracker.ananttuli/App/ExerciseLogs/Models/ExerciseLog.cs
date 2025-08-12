using Microsoft.EntityFrameworkCore;

namespace App.ExerciseLogs.Models;

[PrimaryKey("Id")]
public class ExerciseLog
{
    public int Id { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string Comments { get; set; } = "";

    public TimeSpan Duration
    {
        get => EndDateTime.Subtract(StartDateTime);
    }
}