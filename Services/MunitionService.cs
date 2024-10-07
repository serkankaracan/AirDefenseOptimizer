using AirDefenseOptimizer.Database;  // ConnectionManager ve DatabaseHelper sınıflarını kullanmak için

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
        public void AddMunition(string name, string type, double weight, double speed, double range, double explosivePower, double cost)
        {
            string insertQuery = @"INSERT INTO Munition (Name, MunitionType, Weight, Speed, Range, ExplosivePower, Cost) 
                                   VALUES (@name, @type, @weight, @speed, @range, @explosivePower, @cost);";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object>
            {
                { "@name", name },
                { "@type", type },
                { "@weight", weight },
                { "@speed", speed },
                { "@range", range },
                { "@explosivePower", explosivePower },
                { "@cost", cost }
            };

            _databaseHelper.ExecuteNonQuery(insertQuery, connection, parameters);
        }

        // Mühimmat güncelleme işlemi
        public void UpdateMunition(int id, string name, string type, double weight, double speed, double range, double explosivePower, double cost)
        {
            string updateQuery = @"UPDATE Munition SET Name = @name, MunitionType = @type, Weight = @weight, Speed = @speed, Range = @range, 
                                   ExplosivePower = @explosivePower, Cost = @cost WHERE Id = @id;";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object>
            {
                { "@id", id },
                { "@name", name },
                { "@type", type },
                { "@weight", weight },
                { "@speed", speed },
                { "@range", range },
                { "@explosivePower", explosivePower },
                { "@cost", cost }
            };

            _databaseHelper.ExecuteNonQuery(updateQuery, connection, parameters);
        }

        // Mühimmat silme işlemi
        public void DeleteMunition(int id)
        {
            string deleteQuery = @"DELETE FROM Munition WHERE Id = @id;";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object> { { "@id", id } };

            _databaseHelper.ExecuteNonQuery(deleteQuery, connection, parameters);
        }

        // Tek bir mühimmatı çekme
        public Dictionary<string, object>? GetMunition(int id)
        {
            string selectQuery = @"SELECT * FROM Munition WHERE Id = @id;";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object> { { "@id", id } };

            var result = _databaseHelper.ExecuteReader(selectQuery, connection, parameters);

            return result.Count > 0 ? result[0] : null;
        }

        // Tüm mühimmatları listeleme
        public List<Dictionary<string, object>> GetAllMunitions()
        {
            string selectQuery = @"SELECT * FROM Munition;";

            using var connection = _connectionManager.GetConnection();

            return _databaseHelper.ExecuteReader(selectQuery, connection, null);
        }
    }
}
