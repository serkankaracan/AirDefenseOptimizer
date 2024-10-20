using AirDefenseOptimizer.Database;

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

        public int AddAircraft(string name, string type, double speed, double range, double maxAltitude, string maneuverability, double payloadCapacity, double cost, double? radarCrossSection = null, int? radarId = null)
        {
            string insertQuery = @"INSERT INTO Aircraft (Name, AircraftType, Speed, Range, MaxAltitude, Maneuverability, PayloadCapacity, RadarCrossSection, RadarId, Cost) 
                           VALUES (@name, @type, @speed, @range, @maxAltitude, @maneuverability, @payloadCapacity, @radarCrossSection, @radarId, @cost);";

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
                { "@radarCrossSection", radarCrossSection ?? (object)DBNull.Value },
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
        public void UpdateAircraft(int aircraftId, string name, string type, double speed, double range, double maxAltitude, string maneuverability, double payloadCapacity, double cost, double? radarCrossSection = null, int? radarId = null)
        {
            string updateQuery = @"UPDATE Aircraft SET Name = @name, AircraftType = @type, Speed = @speed, Range = @range, MaxAltitude = @maxAltitude, 
                           Maneuverability = @maneuverability, PayloadCapacity = @payloadCapacity, RadarCrossSection = @radarCrossSection, RadarId = @radarId, Cost = @cost 
                           WHERE Id = @aircraftId;";

            using var connection = _connectionManager.GetConnection();

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
                { "@radarCrossSection", radarCrossSection ?? (object)DBNull.Value },
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
        // Örneğin, GetAircraftMunitions'da hata kontrolü ekleyebilirsiniz:
        public List<Dictionary<string, object>> GetAircraftMunitions(int aircraftId)
        {
            string selectQuery = @"SELECT AircraftMunition.MunitionId, 
                                           Munition.Name AS MunitionName, 
                                           Munition.MunitionType,
                                           Munition.Weight,
                                           Munition.Speed,
                                           Munition.Range,
                                           Munition.Maneuverability,
                                           Munition.ExplosivePower,
                                           Munition.Cost,
                                           AircraftMunition.Quantity 
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

        public Dictionary<string, object>? GetAircraftRadar(int aircraftId)
        {
            string selectQuery = @"SELECT Radar.Id, 
                                  Radar.Name AS RadarName, 
                                  Radar.MinDetectionRange, 
                                  Radar.MaxDetectionRange, 
                                  Radar.MinAltitude, 
                                  Radar.MaxAltitude, 
                                  Radar.MaxTargetSpeed, 
                                  Radar.MaxTargetVelocity, 
                                  Radar.RedeploymentTime,
                                  Radar.RadarType
                           FROM Aircraft
                           JOIN Radar ON Aircraft.RadarId = Radar.Id
                           WHERE Aircraft.Id = @aircraftId;";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object>
            {
                { "@aircraftId", aircraftId }
            };

            var result = _databaseHelper.ExecuteReader(selectQuery, connection, parameters);
            return result.FirstOrDefault(); // Tek bir radar kaydı dönecek
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
