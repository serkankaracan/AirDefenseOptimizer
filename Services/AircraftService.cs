using AirDefenseOptimizer.Database;
using System.Windows;

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

        public int AddAircraft(
                                string name,
                                string type,
                                double speed,
                                double range,
                                double maxAltitude,
                                string maneuverability,
                                double payloadCapacity,
                                double radarCrossSection,
                                string ecmCapability, // Bu satırı ekliyoruz
                                double cost,
                                int? radarId = null)
        {
            string insertQuery = @"INSERT INTO Aircraft (Name, AircraftType, Speed, Range, MaxAltitude, Maneuverability, PayloadCapacity, RadarCrossSection, ECMCapability, RadarId, Cost) 
                           VALUES (@name, @type, @speed, @range, @maxAltitude, @maneuverability, @payloadCapacity, @radarCrossSection, @ecmCapability, @radarId, @cost);";

            try
            {
                using var connection = _connectionManager.GetConnection();

                if (connection == null) return -1;

                var parameters = new Dictionary<string, object>
                {
                    { "@name", name ?? string.Empty },
                    { "@type", type ?? string.Empty },
                    { "@speed", speed },
                    { "@range", range },
                    { "@maxAltitude", maxAltitude },
                    { "@maneuverability", maneuverability ?? string.Empty },
                    { "@payloadCapacity", payloadCapacity },
                    { "@radarCrossSection", radarCrossSection },
                    { "@ecmCapability", ecmCapability ?? string.Empty },
                    { "@cost", cost },
                    { "@radarId", radarId ?? (object)DBNull.Value }
                };

                _databaseHelper.ExecuteNonQuery(insertQuery, connection, parameters);

                return (int)connection.LastInsertRowId;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: Uçak eklenemedi. Detay: {ex.Message}");
                if (ex.InnerException != null)
                    MessageBox.Show($"İç Hata: {ex.InnerException.Message}");
                return -1;
            }
        }

        public void AddMunitionToAircraft(int aircraftId, int munitionId, int quantity)
        {
            string insertQuery = @"INSERT INTO AircraftMunition (AircraftId, MunitionId, Quantity) 
                                   VALUES (@aircraftId, @munitionId, @quantity);";

            try
            {
                using var connection = _connectionManager.GetConnection();
                if (connection == null) return;

                var parameters = new Dictionary<string, object>
                {
                    { "@aircraftId", aircraftId },
                    { "@munitionId", munitionId },
                    { "@quantity", quantity }
                };

                _databaseHelper.ExecuteNonQuery(insertQuery, connection, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: Mühimmat uçağa eklenemedi. Detay: {ex.Message}");
            }
        }

        public void UpdateAircraftMunition(int aircraftId, int munitionId, int quantity)
        {
            string updateQuery = @"UPDATE AircraftMunition SET Quantity = @quantity 
                                   WHERE AircraftId = @aircraftId AND MunitionId = @munitionId;";

            try
            {
                using var connection = _connectionManager.GetConnection();

                if (connection == null) return;

                var parameters = new Dictionary<string, object>
                {
                    { "@aircraftId", aircraftId },
                    { "@munitionId", munitionId },
                    { "@quantity", quantity }
                };

                _databaseHelper.ExecuteNonQuery(updateQuery, connection, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: Mühimmat güncellenemedi. Detay: {ex.Message}");
            }
        }

        public void UpdateAircraft(int aircraftId, string name, string type, double speed, double range, double maxAltitude, string maneuverability, double payloadCapacity, double radarCrossSection, string ecmCapability, double cost, int? radarId = null)
        {
            string updateQuery = @"UPDATE Aircraft SET Name = @name, AircraftType = @type, Speed = @speed, Range = @range, MaxAltitude = @maxAltitude, 
                                   Maneuverability = @maneuverability, PayloadCapacity = @payloadCapacity, RadarCrossSection = @radarCrossSection, ECMCapability = @ecmCapability, RadarId = @radarId, Cost = @cost 
                                   WHERE Id = @aircraftId;";

            try
            {
                using var connection = _connectionManager.GetConnection();

                if (connection == null) return;

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
                    { "@radarCrossSection", radarCrossSection },
                    { "@ecmCapability", ecmCapability},
                    { "@radarId", radarId ?? (object)DBNull.Value },
                    { "@cost", cost }
                };

                _databaseHelper.ExecuteNonQuery(updateQuery, connection, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: Uçak güncellenemedi. Detay: {ex.Message}");
            }
        }

        public void DeleteAircraft(int aircraftId)
        {
            try
            {
                using var connection = _connectionManager.GetConnection();

                if (connection == null) return;

                // Öncelikle ilgili mühimmatları sil
                string deleteMunitionsQuery = "DELETE FROM AircraftMunition WHERE AircraftId = @aircraftId;";
                var munitionParameters = new Dictionary<string, object> { { "@aircraftId", aircraftId } };
                _databaseHelper.ExecuteNonQuery(deleteMunitionsQuery, connection, munitionParameters);

                // Daha sonra uçağı sil
                string deleteAircraftQuery = "DELETE FROM Aircraft WHERE Id = @aircraftId;";
                var aircraftParameters = new Dictionary<string, object> { { "@aircraftId", aircraftId } };
                _databaseHelper.ExecuteNonQuery(deleteAircraftQuery, connection, aircraftParameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: Uçak silinemedi. Detay: {ex.Message}");
            }
        }

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

            try
            {
                using var connection = _connectionManager.GetConnection();

                if (connection == null) return new List<Dictionary<string, object>>();

                var parameters = new Dictionary<string, object>
                {
                    { "@aircraftId", aircraftId }
                };

                return _databaseHelper.ExecuteReader(selectQuery, connection, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: Mühimmat bilgileri alınamadı. Detay: {ex.Message}");
                return new List<Dictionary<string, object>>();
            }
        }

        public Dictionary<string, object>? GetAircraftRadar(int aircraftId)
        {
            string selectQuery = @"SELECT Radar.Id, 
                                           Radar.Name AS RadarName, 
                                           Radar.MaxDetectionTargets, 
                                           Radar.MaxTrackingTargets,
                                           Radar.MinDetectionRange, 
                                           Radar.MaxDetectionRange, 
                                           Radar.MinAltitude, 
                                           Radar.MaxAltitude, 
                                           Radar.MaxTargetSpeed, 
                                           Radar.MaxTargetVelocity, 
                                           Radar.RedeploymentTime,
                                           Radar.RadarType AS RadarType
                                   FROM Aircraft
                                   JOIN Radar ON Aircraft.RadarId = Radar.Id
                                   WHERE Aircraft.Id = @aircraftId;";

            try
            {
                using var connection = _connectionManager.GetConnection();

                if (connection == null) return null;

                var parameters = new Dictionary<string, object>
                {
                    { "@aircraftId", aircraftId }
                };

                var result = _databaseHelper.ExecuteReader(selectQuery, connection, parameters);

                if (result == null || result.Count == 0)
                {
                    MessageBox.Show("Radar bilgisi bulunamadı.");
                    return null;
                }
                else
                {
                    return result.FirstOrDefault(); // Tek bir radar kaydı dönecek
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: Radar bilgisi alınamadı. Detay: {ex.Message}");
                return null;
            }
        }

        public List<Dictionary<string, object>> GetAllAircrafts()
        {
            string selectQuery = @"SELECT * FROM Aircraft;";

            try
            {
                using var connection = _connectionManager.GetConnection();

                if (connection == null) return new List<Dictionary<string, object>>();

                return _databaseHelper.ExecuteReader(selectQuery, connection, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: Uçak bilgileri alınamadı. Detay: {ex.Message}");
                return new List<Dictionary<string, object>>();
            }
        }

        public void DeleteAircraftMunition(int aircraftId, int munitionId)
        {
            string deleteQuery = @"DELETE FROM AircraftMunition 
                                   WHERE AircraftId = @aircraftId AND MunitionId = @munitionId;";

            try
            {
                using var connection = _connectionManager.GetConnection();

                if (connection == null) return;

                var parameters = new Dictionary<string, object>
                {
                    { "@aircraftId", aircraftId },
                    { "@munitionId", munitionId }
                };

                _databaseHelper.ExecuteNonQuery(deleteQuery, connection, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: Mühimmat silinemedi. Detay: {ex.Message}");
            }
        }
    }
}
