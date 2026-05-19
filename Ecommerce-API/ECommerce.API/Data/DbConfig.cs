namespace ECommerce.API.Data;

public class DbConfig
{
    public static string GetConnectionString()
    {
        return $"Data Source={Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ECommerce",
            "api.db")}";
    }
}