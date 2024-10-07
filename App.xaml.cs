using AirDefenseOptimizer.Database;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace AirDefenseOptimizer
{
    public partial class App : Application
    {
        public static IConfiguration? Configuration { get; private set; }
        public static ConnectionManager? ConnectionManager { get; private set; }
        public static DatabaseHelper? DatabaseHelper { get; private set; }

        // Kök dizini doğru şekilde almak için
        private static readonly string ProjectRootDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName ?? string.Empty;
        private static readonly string ConfigFileName = Path.Combine(ProjectRootDirectory, "appsettings.json");

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Uygulama başlarken appsettings.json dosyasını yükle ya da oluştur
            Configuration = LoadOrCreateConfiguration();

            // ConnectionManager ve DatabaseHelper nesnelerini başlatıyoruz
            string dbPath = Configuration?["DatabaseSettings:DatabasePath"] ?? "AirDefenseOptimizer.db";
            ConnectionManager = new ConnectionManager();
            DatabaseHelper = new DatabaseHelper(ConnectionManager);
        }

        private static IConfiguration LoadOrCreateConfiguration()
        {
            var builder = new ConfigurationBuilder();

            // Eğer appsettings.json dosyası mevcutsa, dosyayı yükle
            if (File.Exists(ConfigFileName))
            {
                builder.AddJsonFile(ConfigFileName, optional: false, reloadOnChange: true);
            }
            else
            {
                // Eğer dosya mevcut değilse varsayılan ayarlarla oluştur ve yükle
                CreateDefaultConfigFile();
                builder.AddJsonFile(ConfigFileName, optional: false, reloadOnChange: true);
            }

            return builder.Build();
        }

        // Varsayılan ayarlarla appsettings.json dosyasını oluşturma
        private static void CreateDefaultConfigFile()
        {
            var defaultSettings = new
            {
                DatabaseSettings = new
                {
                    DatabaseName = "AirDefenseOptimizer.db",
                    DatabasePath = Path.Combine(ProjectRootDirectory, "AirDefenseOptimizer.db")
                },
                ApplicationSettings = new
                {
                    MaxConnectionRetry = 3,
                    LogFilePath = Path.Combine(ProjectRootDirectory, "Logs", "AirDefenseLogs.txt")
                }
            };

            // JSON formatına dönüştür ve dosyayı yaz
            string json = JsonSerializer.Serialize(defaultSettings, new JsonSerializerOptions { WriteIndented = true });

            // Eğer Logs dizini yoksa oluştur
            Directory.CreateDirectory(Path.Combine(ProjectRootDirectory, "Logs"));

            // appsettings.json dosyasını yaz
            File.WriteAllText(ConfigFileName, json);
        }
    }
}
