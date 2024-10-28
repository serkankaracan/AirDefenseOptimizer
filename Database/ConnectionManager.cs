using AirDefenseOptimizer.Helpers;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace AirDefenseOptimizer.Database
{
    public class ConnectionManager
    {
        private readonly string? _connectionString;

        public ConnectionManager()
        {
            try
            {
                string? databasePath = ConfigHelper.GetDatabasePath();
                if (!File.Exists(databasePath))
                {
                    SQLiteConnection.CreateFile(databasePath);
                }
                _connectionString = $"Data Source={databasePath};Version=3;";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veritabanı dosyası oluşturulurken hata oluştu: {ex.Message}");
                _connectionString = null; // Hata durumunda bağlantı dizesi null olarak ayarlanır
            }
        }

        public SQLiteConnection? GetConnection()
        {
            if (_connectionString == null)
            {
                MessageBox.Show("Bağlantı dizesi geçerli değil.");
                return null;
            }

            try
            {
                var connection = new SQLiteConnection(_connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı oluşturulurken hata oluştu: {ex.Message}");
                return null;
            }
        }

        public void CloseConnection(SQLiteConnection? connection)
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bağlantı kapatılırken hata oluştu: {ex.Message}");
                }
            }
        }
    }
}
