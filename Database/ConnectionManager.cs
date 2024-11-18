using System.IO;
using System.Windows;
using AirDefenseOptimizer.Helpers;
using Microsoft.Data.Sqlite;

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

                // Veritabanı dosyasını manuel olarak oluşturma
                if (!File.Exists(databasePath))
                {
                    // Veritabanı dosyası yoksa, oluşturulacak bir bağlantı açalım
                    using (var connection = new SqliteConnection($"Data Source={databasePath};"))
                    {
                        connection.Open();
                    }
                }

                _connectionString = $"Data Source={databasePath};";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veritabanı dosyası oluşturulurken hata oluştu: {ex.Message}");
                _connectionString = null; // Hata durumunda bağlantı dizesi null olarak ayarlanır
            }
        }

        public SqliteConnection? GetConnection()
        {
            if (_connectionString == null)
            {
                MessageBox.Show("Bağlantı dizesi geçerli değil.");
                return null;
            }

            try
            {
                var connection = new SqliteConnection(_connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı oluşturulurken hata oluştu: {ex.Message}");
                return null;
            }
        }

        public void CloseConnection(SqliteConnection? connection)
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

        // LastInsertRowId'yi almak için yeni bir yöntem
        public long GetLastInsertRowId(SqliteConnection connection)
        {
            try
            {
                using (var cmd = new SqliteCommand("SELECT last_insert_rowid();", connection))
                {
                    return (long)cmd.ExecuteScalar(); // Son eklenen satırın ID'sini döndürür
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"LastInsertRowId alınırken hata oluştu: {ex.Message}");
                return -1; // Hata durumunda -1 döndürülür
            }
        }
    }
}
