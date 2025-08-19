

namespace Shiftlogger.UI.Constants;

public static class Messages
{
    public static readonly string StartDateUpdate = "Do you want to Update Start Date and Time";
    public static readonly string StartTime = "Enter Start date and time in [bold green](yyyy-MM-dd HH:mm:ss)[/] format.";
    public static readonly string EndDateUpdate = "Do you want to Update End Date and Time";
    public static readonly string EndTime = "Enter End date and time in [bold green](yyyy-MM-dd HH:mm:ss)[/] format.";
    public static readonly string CommentsUpdate = "Do you want to Update Comments";
    public static readonly string Comments = "Enter Comments for the Jogging Session";
    public static readonly string TimeFormat = "yyyy-MM-dd HH:mm:ss";

    internal static string GetCommentsUpdate(string comments) => $"{CommentsUpdate} [maroon]{comments}[/]?";

    internal static object GetEndDateUpdate(DateTime dateEnd) => $"{EndDateUpdate} [maroon]{dateEnd}[/]?";

    internal static object GetStartDateUpdate(DateTime dateStart) => $"{StartDateUpdate} [maroon]{dateStart}[/]?";
}