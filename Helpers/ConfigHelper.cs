namespace AirDefenseOptimizer.Helpers
{
    public static class ConfigHelper
    {
        public static string? GetDatabaseName()
        {
            return App.Configuration?["DatabaseSettings:DatabaseName"];
        }

        public static string? GetDatabasePath()
        {
            return App.Configuration?["DatabaseSettings:DatabasePath"];
        }

        public static int GetMaxConnectionRetry()
        {
            return int.TryParse(App.Configuration?["ApplicationSettings:MaxConnectionRetry"], out int retry) ? retry : 3;
        }

        public static string? GetLogFilePath()
        {
            return App.Configuration?["ApplicationSettings:LogFilePath"];
        }
    }
}
