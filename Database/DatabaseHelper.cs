using System.Data.SqlClient;
using System.Windows;
using Microsoft.Data.Sqlite;

namespace AirDefenseOptimizer.Database
{
    public class DatabaseHelper
    {
        private readonly ConnectionManager _connectionManager;

        public DatabaseHelper(ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
            InitializeDatabase();
        }

        // Veritabanını başlat ve tabloları oluştur
        private void InitializeDatabase()
        {
            try
            {
                using var connection = _connectionManager.GetConnection();

                if (connection == null) return;

                ExecuteNonQuery(DatabaseQueries.CreateRadarTable, connection);
                ExecuteNonQuery(DatabaseQueries.CreateMunitionTable, connection);
                ExecuteNonQuery(DatabaseQueries.CreateAircraftTable, connection);
                ExecuteNonQuery(DatabaseQueries.CreateAircraftMunitionTable, connection);
                ExecuteNonQuery(DatabaseQueries.CreateAirDefenseTable, connection);
                ExecuteNonQuery(DatabaseQueries.CreateAirDefenseRadarTable, connection);
                ExecuteNonQuery(DatabaseQueries.CreateAirDefenseMunitionTable, connection);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veritabanı başlatılamadı: {ex.Message}");
            }
        }

        // SQL sorgularını çalıştıran yardımcı fonksiyon (INSERT, UPDATE, DELETE için)
        public void ExecuteNonQuery(string query, SqliteConnection connection, Dictionary<string, object>? parameters = null)
        {
            try
            {
                using var command = new SqliteCommand(query, connection);

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                MessageBox.Show($"SQLite Hatası: {ex.Message}");
                // Hata yönetimi ve loglama burada yapılabilir
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Genel Hata: {ex.Message}");
                // Hata yönetimi ve loglama burada yapılabilir
            }
        }

        // SQL sorgularını çalıştıran ve veri okuyan fonksiyon (SELECT için)
        public List<Dictionary<string, object>> ExecuteReader(string query, SqliteConnection connection, Dictionary<string, object>? parameters = null)
        {
            var resultList = new List<Dictionary<string, object>>();

            try
            {
                using var command = new SqliteCommand(query, connection);

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.GetValue(i);
                    }
                    resultList.Add(row);
                }
            }
            catch (SqliteException ex)
            {
                MessageBox.Show($"SQLite Hatası: {ex.Message}");
                // Hata yönetimi ve loglama burada yapılabilir
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Genel Hata: {ex.Message}");
                // Hata yönetimi ve loglama burada yapılabilir
            }

            return resultList;
        }

        public object? ExecuteScalar(string query, SqlConnection connection, Dictionary<string, object> parameters)
        {
            try
            {
                using var command = new SqlCommand(query, connection);

                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }

                return command.ExecuteScalar(); // Sorgudan ilk değeri döndürür
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"SQL Hatası: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Genel Hata: {ex.Message}");
                return null;
            }
        }

        // Bağlantıyı kapatma işlemi
        public void CloseConnection(SqliteConnection connection)
        {
            try
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı kapatılamadı: {ex.Message}");
            }
        }
    }
}
