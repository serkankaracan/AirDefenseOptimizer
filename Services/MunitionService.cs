using AirDefenseOptimizer.Database;  // ConnectionManager ve DatabaseHelper sınıflarını kullanmak için
using System.Windows;

namespace AirDefenseOptimizer.Services
{
    public class MunitionService
    {
        private readonly ConnectionManager _connectionManager;
        private readonly DatabaseHelper _databaseHelper;

        public MunitionService(ConnectionManager connectionManager, DatabaseHelper databaseHelper)
        {
            _connectionManager = connectionManager;
            _databaseHelper = databaseHelper;
        }

        // Mühimmat ekleme işlemi
        public void AddMunition(string name, string type, double weight, double speed, double range, string maneuverability, double explosivePower, double cost)
        {
            try
            {
                string insertQuery = @"INSERT INTO Munition (Name, MunitionType, Weight, Speed, Range, Maneuverability, ExplosivePower, Cost) 
                                       VALUES (@name, @type, @weight, @speed, @range, @maneuverability, @explosivePower, @cost);";

                using var connection = _connectionManager.GetConnection();
                var parameters = new Dictionary<string, object>
                {
                    { "@name", name },
                    { "@type", type },
                    { "@weight", weight },
                    { "@speed", speed },
                    { "@range", range },
                    { "@maneuverability", maneuverability },
                    { "@explosivePower", explosivePower },
                    { "@cost", cost }
                };

                _databaseHelper.ExecuteNonQuery(insertQuery, connection, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding munition: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Mühimmat güncelleme işlemi
        public void UpdateMunition(int id, string name, string type, double weight, double speed, double range, string maneuverability, double explosivePower, double cost)
        {
            try
            {
                string updateQuery = @"UPDATE Munition SET Name = @name, MunitionType = @type, Weight = @weight, Speed = @speed, Range = @range, 
                                       Maneuverability = @maneuverability, ExplosivePower = @explosivePower, Cost = @cost WHERE Id = @id;";

                using var connection = _connectionManager.GetConnection();
                var parameters = new Dictionary<string, object>
                {
                    { "@id", id },
                    { "@name", name },
                    { "@type", type },
                    { "@weight", weight },
                    { "@speed", speed },
                    { "@range", range },
                    { "@maneuverability", maneuverability },
                    { "@explosivePower", explosivePower },
                    { "@cost", cost }
                };

                _databaseHelper.ExecuteNonQuery(updateQuery, connection, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating munition: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Mühimmat silme işlemi
        public void DeleteMunition(int id)
        {
            try
            {
                string deleteQuery = @"DELETE FROM Munition WHERE Id = @id;";
                using var connection = _connectionManager.GetConnection();
                var parameters = new Dictionary<string, object> { { "@id", id } };

                _databaseHelper.ExecuteNonQuery(deleteQuery, connection, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting munition: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Tek bir mühimmatı çekme
        public Dictionary<string, object>? GetMunition(int id)
        {
            try
            {
                string selectQuery = @"SELECT * FROM Munition WHERE Id = @id;";
                using var connection = _connectionManager.GetConnection();
                var parameters = new Dictionary<string, object> { { "@id", id } };

                var result = _databaseHelper.ExecuteReader(selectQuery, connection, parameters);
                return result.Count > 0 ? result[0] : null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving munition: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        // Tüm mühimmatları listeleme
        public List<Dictionary<string, object>> GetAllMunitions()
        {
            try
            {
                string selectQuery = @"SELECT * FROM Munition;";
                using var connection = _connectionManager.GetConnection();

                return _databaseHelper.ExecuteReader(selectQuery, connection, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving all munitions: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Dictionary<string, object>>();
            }
        }
    }
}
