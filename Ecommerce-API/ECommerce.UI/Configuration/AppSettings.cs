namespace ECommerce.UI.Configuration;

internal class AppSettings
{
    internal static readonly string AppDataPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ECommerce");
}