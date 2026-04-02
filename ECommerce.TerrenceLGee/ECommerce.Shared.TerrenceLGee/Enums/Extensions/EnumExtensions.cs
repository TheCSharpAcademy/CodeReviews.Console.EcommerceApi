using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.TerrenceLGee.Enums.Extensions;

public static class EnumExtensions
{
    extension(Enum enumValue)
    {
        public string GetDisplayName()
        {
            var fieldInfo = enumValue
                .GetType()
                .GetField(enumValue.ToString());

            return (fieldInfo?.GetCustomAttributes(typeof(DisplayAttribute), false)
                is DisplayAttribute[] { Length: > 0 } descriptionAttributes
                ? descriptionAttributes[0].Name
                : enumValue.ToString()) ?? string.Empty;
        }
    }
}
