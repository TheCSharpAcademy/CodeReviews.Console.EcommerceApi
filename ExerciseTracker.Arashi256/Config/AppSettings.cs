namespace ExerciseTracker.Arashi256.Config
{
    public class AppSettings
    {
        private string? _databaseConnectionString;
        private string? _preferredDateTime;

        public AppSettings()
        {
            AppManager appManager = new AppManager();
            _databaseConnectionString = appManager.DatabaseConnectionString;
            _preferredDateTime = appManager.DateTimeFormatString;
        }

        public string? DatabaseConnectionString { get { return _databaseConnectionString; } }
        public string? DateTimeFormat { get { return _preferredDateTime; } }
    }
}