using Microsoft.Extensions.Configuration;

namespace ExerciseTracker.hasona23;

public static class AppSettings
{
    public static string DefaultConnectionString { get; set; } = string.Empty;

    static AppSettings()
    {
        IConfigurationRoot config ;
        try
        {
             config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Loading ConnectionString :{ex.Message}");
            return;
        }

        DefaultConnectionString = config.GetConnectionString("DefaultConnection");
       
        
    }
}