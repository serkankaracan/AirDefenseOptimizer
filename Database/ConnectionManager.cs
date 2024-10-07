using AirDefenseOptimizer.Helpers;
using System.Data.SQLite;
using System.IO;

namespace AirDefenseOptimizer.Database
{
    public class ConnectionManager
    {
        private readonly string _connectionString;

        public ConnectionManager()
        {
            if (!File.Exists(ConfigHelper.GetDatabasePath()))
            {
                SQLiteConnection.CreateFile(ConfigHelper.GetDatabasePath());
            }
            _connectionString = $"Data Source={ConfigHelper.GetDatabasePath()};Version=3;";
        }

        public SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection(_connectionString);
            connection.Open();  // Bağlantı her işlemde açılır
            return connection;  // Bağlantıyı geri döndür
        }

        public void CloseConnection(SQLiteConnection connection)
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();  // Bağlantı kapatılır
            }
        }
    }
}
