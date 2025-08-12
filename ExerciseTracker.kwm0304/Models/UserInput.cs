namespace ExerciseTracker.kwm0304.Models;

public class UserInput
{
  public int Id { get; set; }
  public DateTime StartDate { get; set; }
  public DateTime EndDate { get; set; }
  public TimeSpan Duration { get; set; }
  public string? Title { get; set; }
  public string? Comments { get; set; }

  public UserInput() { }
  public UserInput(DateTime startDate, DateTime endDate, TimeSpan duration, string title, string comments)
  {
    StartDate = startDate;
    EndDate = endDate;
    Duration = duration;
    Title = title;
    Comments = comments;
  }
  public override string ToString()
  {
    return $"Title: {Title} | Start: {StartDate:yyyy-MM-dd HH:mm:ss} | End: {EndDate:yyyy-MM-dd HH:mm:ss} | Duration: {Duration:hh\\:mm\\:ss} | Comments: {Comments}";
  }
}