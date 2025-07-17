using System.Globalization;

namespace Exercise_Tracker.Utils;

public class Validator
{
    public static bool IsValidDate(string date, string format)
    {
        if (
            !DateTime.TryParseExact(
                date,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _
            )
        )
            return false;

        return true;
    }

    public static bool IsStartDateBeforeEndDate(string startDate, string endDate)
    {
        var start = DateTime.ParseExact(
            startDate,
            "yyyy-MM-dd HH:mm",
            CultureInfo.InvariantCulture
        );
        var end = DateTime.ParseExact(endDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

        return start <= end;
    }
}
