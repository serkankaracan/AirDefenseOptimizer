using System.Data.SqlClient;
using System.Data.SQLite;

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
            using var connection = _connectionManager.GetConnection();

            ExecuteNonQuery(DatabaseQueries.CreateRadarTable, connection);
            ExecuteNonQuery(DatabaseQueries.CreateMunitionTable, connection);
            ExecuteNonQuery(DatabaseQueries.CreateAircraftTable, connection);
            ExecuteNonQuery(DatabaseQueries.CreateAircraftMunitionTable, connection);
            ExecuteNonQuery(DatabaseQueries.CreateAirDefenseTable, connection);
            ExecuteNonQuery(DatabaseQueries.CreateAirDefenseRadarTable, connection);
            ExecuteNonQuery(DatabaseQueries.CreateAirDefenseMunitionTable, connection);
        }

        // SQL sorgularını çalıştıran yardımcı fonksiyon (INSERT, UPDATE, DELETE için)
        public void ExecuteNonQuery(string query, SQLiteConnection connection, Dictionary<string, object>? parameters = null)
        {
            try
            {
                using var command = new SQLiteCommand(query, connection);

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                command.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"SQLite Exception: {ex.Message}");
                // Hata yönetimi ve loglama burada yapılabilir
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Exception: {ex.Message}");
                // Hata yönetimi ve loglama burada yapılabilir
            }
        }

        // SQL sorgularını çalıştıran ve veri okuyan fonksiyon (SELECT için)
        public List<Dictionary<string, object>> ExecuteReader(string query, SQLiteConnection connection, Dictionary<string, object>? parameters = null)
        {
            var resultList = new List<Dictionary<string, object>>();

            try
            {
                using var command = new SQLiteCommand(query, connection);

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
            catch (SQLiteException ex)
            {
                Console.WriteLine($"SQLite Exception: {ex.Message}");
                // Hata yönetimi ve loglama burada yapılabilir
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Exception: {ex.Message}");
                // Hata yönetimi ve loglama burada yapılabilir
            }

            return resultList;
        }

        public object ExecuteScalar(string query, SqlConnection connection, Dictionary<string, object> parameters)
        {
            using var command = new SqlCommand(query, connection);

            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value);
            }

            return command.ExecuteScalar(); // Sorgudan ilk değeri döndürür
        }


        // Bağlantıyı kapatma işlemi
        public void CloseConnection(SQLiteConnection connection)
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
