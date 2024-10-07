using AirDefenseOptimizer.Database;

namespace AirDefenseOptimizer.Services
{
    public class RadarService
    {
        private readonly ConnectionManager _connectionManager;
        private readonly DatabaseHelper _databaseHelper;

        public RadarService(ConnectionManager connectionManager, DatabaseHelper databaseHelper)
        {
            _connectionManager = connectionManager;
            _databaseHelper = databaseHelper;
        }

        public void AddRadar(string name, string radarType, double minDetectionRange, double maxDetectionRange, double maxAltitude, double minAltitude, double maxTargetSpeed, double maxTargetVelocity, int redeploymentTime)
        {
            string insertQuery = @"INSERT INTO Radar (Name, RadarType, MinDetectionRange, MaxDetectionRange, MaxAltitude, MinAltitude, MaxTargetSpeed, MaxTargetVelocity, RedeploymentTime) 
                                   VALUES (@name, @radarType, @minDetectionRange, @maxDetectionRange, @maxAltitude, @minAltitude, @maxTargetSpeed, @maxTargetVelocity, @redeploymentTime);";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object>
            {
                { "@name", name },
                { "@radarType", radarType },
                { "@minDetectionRange", minDetectionRange },
                { "@maxDetectionRange", maxDetectionRange },
                { "@maxAltitude", maxAltitude },
                { "@minAltitude", minAltitude },
                { "@maxTargetSpeed", maxTargetSpeed },
                { "@maxTargetVelocity", maxTargetVelocity },
                { "@redeploymentTime", redeploymentTime }
            };

            _databaseHelper.ExecuteNonQuery(insertQuery, connection, parameters);
        }

        public void UpdateRadar(int id, string name, string radarType, double minDetectionRange, double maxDetectionRange, double maxAltitude, double minAltitude, double maxTargetSpeed, double maxTargetVelocity, int redeploymentTime)
        {
            string updateQuery = @"UPDATE Radar SET Name = @name, RadarType = @radarType, MinDetectionRange = @minDetectionRange, MaxDetectionRange = @maxDetectionRange, 
                                   MaxAltitude = @maxAltitude, MinAltitude = @minAltitude, MaxTargetSpeed = @maxTargetSpeed, MaxTargetVelocity = @maxTargetVelocity, 
                                   RedeploymentTime = @redeploymentTime WHERE Id = @id;";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object>
            {
                { "@id", id },
                { "@name", name },
                { "@radarType", radarType },
                { "@minDetectionRange", minDetectionRange },
                { "@maxDetectionRange", maxDetectionRange },
                { "@maxAltitude", maxAltitude },
                { "@minAltitude", minAltitude },
                { "@maxTargetSpeed", maxTargetSpeed },
                { "@maxTargetVelocity", maxTargetVelocity },
                { "@redeploymentTime", redeploymentTime }
            };

            _databaseHelper.ExecuteNonQuery(updateQuery, connection, parameters);
        }

        public void DeleteRadar(int id)
        {
            string deleteQuery = @"DELETE FROM Radar WHERE Id = @id;";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object> { { "@id", id } };

            _databaseHelper.ExecuteNonQuery(deleteQuery, connection, parameters);
        }

        public Dictionary<string, object>? GetRadar(int id)
        {
            string selectQuery = @"SELECT * FROM Radar WHERE Id = @id;";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object> { { "@id", id } };

            var result = _databaseHelper.ExecuteReader(selectQuery, connection, parameters);

            // İlk sonucu döndür, ya da null
            return result.Count > 0 ? result[0] : null;
        }

        public List<Dictionary<string, object>> GetAllRadars()
        {
            string selectQuery = @"SELECT * FROM Radar;";

            using var connection = _connectionManager.GetConnection();

            var result = _databaseHelper.ExecuteReader(selectQuery, connection, null);

            return result;
        }
    }
}
