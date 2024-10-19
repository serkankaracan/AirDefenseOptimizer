using AirDefenseOptimizer.Database;  // ConnectionManager ve DatabaseHelper sınıflarını kullanmak için

namespace AirDefenseOptimizer.Services
{
    public class AircraftService
    {
        private readonly ConnectionManager _connectionManager;
        private readonly DatabaseHelper _databaseHelper;

        public AircraftService(ConnectionManager connectionManager, DatabaseHelper databaseHelper)
        {
            _connectionManager = connectionManager;
            _databaseHelper = databaseHelper;
        }

        public int AddAircraft(string name, string type, double speed, double range, double maxAltitude, string maneuverability, double payloadCapacity, double cost, int? radarId = null)
        {
            string insertQuery = @"INSERT INTO Aircraft (Name, AircraftType, Speed, Range, MaxAltitude, Maneuverability, PayloadCapacity, RadarId, Cost) 
                                   VALUES (@name, @type, @speed, @range, @maxAltitude, @maneuverability, @payloadCapacity, @radarId, @cost);";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object>
            {
                { "@name", name },
                { "@type", type },
                { "@speed", speed },
                { "@range", range },
                { "@maxAltitude", maxAltitude },
                { "@maneuverability", maneuverability },
                { "@payloadCapacity", payloadCapacity },
                { "@radarId", radarId ?? (object)DBNull.Value },
                { "@cost", cost }
            };

            _databaseHelper.ExecuteNonQuery(insertQuery, connection, parameters);

            // Yeni eklenen uçağın ID'sini döndür
            return (int)connection.LastInsertRowId;
        }


        // Uçağa mühimmat ekle
        public void AddMunitionToAircraft(int aircraftId, int munitionId, int quantity)
        {
            string insertQuery = @"INSERT INTO AircraftMunition (AircraftId, MunitionId, Quantity) 
                                   VALUES (@aircraftId, @munitionId, @quantity);";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object>
            {
                { "@aircraftId", aircraftId },
                { "@munitionId", munitionId },
                { "@quantity", quantity }
            };

            _databaseHelper.ExecuteNonQuery(insertQuery, connection, parameters);
        }

        // Uçaktaki mühimmatları güncelle
        public void UpdateAircraftMunition(int aircraftId, int munitionId, int quantity)
        {
            string updateQuery = @"UPDATE AircraftMunition SET Quantity = @quantity 
                                   WHERE AircraftId = @aircraftId AND MunitionId = @munitionId;";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object>
            {
                { "@aircraftId", aircraftId },
                { "@munitionId", munitionId },
                { "@quantity", quantity }
            };

            _databaseHelper.ExecuteNonQuery(updateQuery, connection, parameters);
        }

        // Uçağı güncelle
        public void UpdateAircraft(int aircraftId, string name, string type, double speed, double range, double maxAltitude, string maneuverability, double payloadCapacity, double cost, int? radarId = null)
        {
            string updateQuery = @"UPDATE Aircraft SET Name = @name, AircraftType = @type, Speed = @speed, Range = @range, MaxAltitude = @maxAltitude, 
                                   Maneuverability = @maneuverability, PayloadCapacity = @payloadCapacity, RadarId = @radarId, Cost = @cost 
                                   WHERE Id = @aircraftId;";

            using var connection = _connectionManager.GetConnection();

            // Parametreler sözlüğü
            var parameters = new Dictionary<string, object>
            {
                { "@aircraftId", aircraftId },
                { "@name", name },
                { "@type", type },
                { "@speed", speed },
                { "@range", range },
                { "@maxAltitude", maxAltitude },
                { "@maneuverability", maneuverability },
                { "@payloadCapacity", payloadCapacity },
                { "@radarId", radarId ?? (object)DBNull.Value },
                { "@cost", cost }
            };

            _databaseHelper.ExecuteNonQuery(updateQuery, connection, parameters);
        }

        // Uçak silme metodu
        public void DeleteAircraft(int aircraftId)
        {
            using var connection = _connectionManager.GetConnection();

            // Öncelikle ilgili mühimmatları sil
            string deleteMunitionsQuery = "DELETE FROM AircraftMunition WHERE AircraftId = @aircraftId;";
            var munitionParameters = new Dictionary<string, object> { { "@aircraftId", aircraftId } };
            _databaseHelper.ExecuteNonQuery(deleteMunitionsQuery, connection, munitionParameters);

            // Daha sonra uçağı sil
            string deleteAircraftQuery = "DELETE FROM Aircraft WHERE Id = @aircraftId;";
            var aircraftParameters = new Dictionary<string, object> { { "@aircraftId", aircraftId } };
            _databaseHelper.ExecuteNonQuery(deleteAircraftQuery, connection, aircraftParameters);
        }

        // Bir uçaktaki tüm mühimmatları çek
        public List<Dictionary<string, object>> GetAircraftMunitions(int aircraftId)
        {
            string selectQuery = @"SELECT AircraftMunition.MunitionId, Munition.Name AS MunitionName, AircraftMunition.Quantity 
                           FROM AircraftMunition
                           JOIN Munition ON AircraftMunition.MunitionId = Munition.Id
                           WHERE AircraftMunition.AircraftId = @aircraftId;";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object>
            {
                { "@aircraftId", aircraftId }
            };

            return _databaseHelper.ExecuteReader(selectQuery, connection, parameters);
        }

        public List<Dictionary<string, object>> GetAircraftRadars(int aircraftId)
        {
            string selectQuery = @"SELECT AircraftRadar.RadarId, Radar.Name AS RadarName, Radar.DetectionRange, Radar.MaxTargetsTracked, Radar.RadarType
                           FROM AircraftRadar
                           JOIN Radar ON AircraftRadar.RadarId = Radar.Id
                           WHERE AircraftRadar.AircraftId = @aircraftId;";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object>
            {
                { "@aircraftId", aircraftId }
            };

            return _databaseHelper.ExecuteReader(selectQuery, connection, parameters);
        }



        // Tüm uçakları listeleyen metot
        public List<Dictionary<string, object>> GetAllAircrafts()
        {
            string selectQuery = @"SELECT * FROM Aircraft;";

            using var connection = _connectionManager.GetConnection();

            return _databaseHelper.ExecuteReader(selectQuery, connection, null);
        }

        public void DeleteAircraftMunition(int aircraftId, int munitionId)
        {
            string deleteQuery = @"DELETE FROM AircraftMunition 
                           WHERE AircraftId = @aircraftId AND MunitionId = @munitionId;";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object>
            {
                { "@aircraftId", aircraftId },
                { "@munitionId", munitionId }
            };

            _databaseHelper.ExecuteNonQuery(deleteQuery, connection, parameters);
        }

    }
}
