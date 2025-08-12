using System.ComponentModel;

namespace ExerciseTracker.frockett.UI;

public static class EnumExtensions
{
    public static string GetEnumDescription(this Enum enumValue)
    {
        var info = enumValue.GetType().GetField(enumValue.ToString());
        var attributes = (DescriptionAttribute[])info.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes != null && attributes.Length > 0)
        {
            return attributes[0].Description;
        }
        else
        {
            return enumValue.ToString();
        }
    }
}
