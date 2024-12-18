﻿using AirDefenseOptimizer.Database;
using AirDefenseOptimizer.Enums;
using Microsoft.Data.Sqlite;

namespace AirDefenseOptimizer.Services
{
    public class AirDefenseService
    {
        private readonly ConnectionManager _connectionManager;
        private readonly DatabaseHelper _databaseHelper;

        public AirDefenseService(ConnectionManager connectionManager, DatabaseHelper databaseHelper)
        {
            _connectionManager = connectionManager;
            _databaseHelper = databaseHelper;
        }

        // Hava savunma sistemi ekle
        public int AddAirDefense(string name, AirDefenseType airDefenseType, double aerodynamicTargetRangeMax, double aerodynamicTargetRangeMin,
                                  double ballisticTargetRangeMax, double ballisticTargetRangeMin, int maxEngagements,
                                  int maxMissilesFired, string ecmCapability, double cost)
        {
            string insertQuery = @"INSERT INTO AirDefense (Name, AirDefenseType, AerodynamicTargetRangeMax, AerodynamicTargetRangeMin, 
                                        BallisticTargetRangeMax, BallisticTargetRangeMin, MaxEngagements, MaxMissilesFired, 
                                        ECMCapability, Cost) 
                                    VALUES (@name, @airDefenseType, @aerodynamicTargetRangeMax, @aerodynamicTargetRangeMin, 
                                        @ballisticTargetRangeMax, @ballisticTargetRangeMin, @maxEngagements, @maxMissilesFired, 
                                        @ecmCapability, @cost); 
                                    SELECT last_insert_rowid();";

            using var connection = _connectionManager.GetConnection();
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            var parameters = new Dictionary<string, object>
            {
                { "@name", name },
                { "@airDefenseType", airDefenseType.ToString() },
                { "@aerodynamicTargetRangeMax", aerodynamicTargetRangeMax },
                { "@aerodynamicTargetRangeMin", aerodynamicTargetRangeMin },
                { "@ballisticTargetRangeMax", ballisticTargetRangeMax },
                { "@ballisticTargetRangeMin", ballisticTargetRangeMin },
                { "@maxEngagements", maxEngagements },
                { "@maxMissilesFired", maxMissilesFired },
                { "@ecmCapability", ecmCapability },
                { "@cost", cost }
            };

            using var command = new SqliteCommand(insertQuery, connection);
            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value);
            }

            var result = command.ExecuteScalar();
            connection.Close();

            return Convert.ToInt32(result);
        }

        // Hava savunma sistemine radar ekle (Quantity ile)
        public void AddRadarToAirDefense(int airDefenseId, int radarId, int quantity)
        {
            string insertQuery = @"INSERT INTO AirDefenseRadar (AirDefenseId, RadarId, Quantity) 
                                   VALUES (@airDefenseId, @radarId, @quantity);";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object>
            {
                { "@airDefenseId", airDefenseId },
                { "@radarId", radarId },
                { "@quantity", quantity }
            };

            _databaseHelper.ExecuteNonQuery(insertQuery, connection, parameters);
        }

        // Hava savunma sistemine mühimmat ekle
        public void AddMunitionToAirDefense(int airDefenseId, int munitionId, int quantity)
        {
            string insertQuery = @"INSERT INTO AirDefenseMunition (AirDefenseId, MunitionId, Quantity) 
                                   VALUES (@airDefenseId, @munitionId, @quantity);";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object>
            {
                { "@airDefenseId", airDefenseId },
                { "@munitionId", munitionId },
                { "@quantity", quantity }
            };

            _databaseHelper.ExecuteNonQuery(insertQuery, connection, parameters);
        }

        // Hava savunma sistemindeki radarları güncelle (Quantity ile)
        public void UpdateAirDefenseRadar(int airDefenseId, int radarId, int quantity)
        {
            string updateQuery = @"UPDATE AirDefenseRadar SET Quantity = @quantity 
                                   WHERE AirDefenseId = @airDefenseId AND RadarId = @radarId;";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object>
            {
                { "@airDefenseId", airDefenseId },
                { "@radarId", radarId },
                { "@quantity", quantity }
            };

            _databaseHelper.ExecuteNonQuery(updateQuery, connection, parameters);
        }

        // Hava savunma sistemindeki mühimmatları güncelle
        public void UpdateAirDefenseMunition(int airDefenseId, int munitionId, int quantity)
        {
            string updateQuery = @"UPDATE AirDefenseMunition SET Quantity = @quantity 
                                   WHERE AirDefenseId = @airDefenseId AND MunitionId = @munitionId;";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object>
            {
                { "@airDefenseId", airDefenseId },
                { "@munitionId", munitionId },
                { "@quantity", quantity }
            };

            _databaseHelper.ExecuteNonQuery(updateQuery, connection, parameters);
        }

        // Hava savunma sistemindeki tüm radarları çek (Quantity ile)
        public List<Dictionary<string, object>> GetAirDefenseRadars(int airDefenseId)
        {
            using var connection = _connectionManager.GetConnection();
            string selectQuery = @"SELECT Radar.Id AS RadarId, 
                                        Radar.Name AS RadarName, 
                                        Radar.MaxDetectionTargets, 
                                        Radar.MaxTrackingTargets,
                                        Radar.MinDetectionRange, 
                                        Radar.MaxDetectionRange, 
                                        Radar.MinAltitude, 
                                        Radar.MaxAltitude, 
                                        Radar.MaxTargetSpeed, 
                                        Radar.RadarType, 
                                        Radar.MaxTargetVelocity, 
                                        Radar.RedeploymentTime, 
                                        AirDefenseRadar.Quantity
                                    FROM AirDefenseRadar
                                    JOIN Radar ON AirDefenseRadar.RadarId = Radar.Id
                                    WHERE AirDefenseRadar.AirDefenseId = @airDefenseId;";

            var parameters = new Dictionary<string, object>
            {
                { "@airDefenseId", airDefenseId }
            };

            return _databaseHelper.ExecuteReader(selectQuery, connection, parameters);
        }


        // Hava savunma sistemindeki tüm mühimmatları çek
        public List<Dictionary<string, object>> GetAirDefenseMunitions(int airDefenseId)
        {
            using var connection = _connectionManager.GetConnection();
            string selectQuery = @"SELECT 
                                        AirDefenseMunition.MunitionId AS MunitionId, 
                                        Munition.Name AS MunitionName, 
                                        AirDefenseMunition.Quantity, 
                                        Munition.Weight, 
                                        Munition.Speed, 
                                        Munition.Range, 
                                        Munition.Maneuverability, 
                                        Munition.ExplosivePower, 
                                        Munition.Cost, 
                                        Munition.MunitionType
                                  FROM AirDefenseMunition
                                  JOIN Munition ON AirDefenseMunition.MunitionId = Munition.Id
                                  WHERE AirDefenseMunition.AirDefenseId = @airDefenseId;";

            var parameters = new Dictionary<string, object>
            {
                { "@airDefenseId", airDefenseId }
            };

            return _databaseHelper.ExecuteReader(selectQuery, connection, parameters);
        }

        // Hava savunma sistemini güncelle
        public void UpdateAirDefense(int airDefenseId, string name, AirDefenseType airDefenseType, double aerodynamicTargetRangeMax, double aerodynamicTargetRangeMin,
                                     double ballisticTargetRangeMax, double ballisticTargetRangeMin, int maxEngagements,
                                     int maxMissilesFired, string ecmCapability, double cost)
        {
            string updateQuery = @"UPDATE AirDefense 
                                   SET Name = @name, AirDefenseType = @airDefenseType, AerodynamicTargetRangeMax = @aerodynamicTargetRangeMax, 
                                       AerodynamicTargetRangeMin = @aerodynamicTargetRangeMin, BallisticTargetRangeMax = @ballisticTargetRangeMax, 
                                       BallisticTargetRangeMin = @ballisticTargetRangeMin, MaxEngagements = @maxEngagements, 
                                       MaxMissilesFired = @maxMissilesFired, ECMCapability = @ecmCapability, Cost = @cost 
                                   WHERE Id = @airDefenseId;";

            using var connection = _connectionManager.GetConnection();

            var parameters = new Dictionary<string, object>
            {
                { "@airDefenseId", airDefenseId },
                { "@name", name },
                { "@airDefenseType", airDefenseType.ToString() },
                { "@aerodynamicTargetRangeMax", aerodynamicTargetRangeMax },
                { "@aerodynamicTargetRangeMin", aerodynamicTargetRangeMin },
                { "@ballisticTargetRangeMax", ballisticTargetRangeMax },
                { "@ballisticTargetRangeMin", ballisticTargetRangeMin },
                { "@maxEngagements", maxEngagements },
                { "@maxMissilesFired", maxMissilesFired },
                { "@ecmCapability", ecmCapability },
                { "@cost", cost }
            };

            _databaseHelper.ExecuteNonQuery(updateQuery, connection, parameters);
        }

        // Radarları listeleyen metot
        public List<Dictionary<string, object>> GetAllRadars()
        {
            using var connection = _connectionManager.GetConnection();
            string selectQuery = @"SELECT Id, Name, MaxDetectionTargets, MaxTrackingTargets, 
                                  MinDetectionRange, MaxDetectionRange, MinAltitude, 
                                  MaxAltitude, MaxTargetSpeed, RadarType, MaxTargetVelocity, 
                                  RedeploymentTime
                           FROM Radar;";

            return _databaseHelper.ExecuteReader(selectQuery, connection, null);
        }

        // Tüm hava savunma sistemlerini listeleyen metot
        public List<Dictionary<string, object>> GetAllAirDefenseSystems()
        {
            using var connection = _connectionManager.GetConnection();
            string selectQuery = @"SELECT * FROM AirDefense;";

            return _databaseHelper.ExecuteReader(selectQuery, connection, null);
        }

        // Hava savunma sistemini silme metodu
        public void DeleteAirDefense(int airDefenseId)
        {
            using var connection = _connectionManager.GetConnection();

            // Öncelikle ilgili radarları sil
            string deleteRadarsQuery = "DELETE FROM AirDefenseRadar WHERE AirDefenseId = @airDefenseId;";
            var radarParameters = new Dictionary<string, object> { { "@airDefenseId", airDefenseId } };
            _databaseHelper.ExecuteNonQuery(deleteRadarsQuery, connection, radarParameters);

            // Daha sonra ilgili mühimmatları sil
            string deleteMunitionsQuery = "DELETE FROM AirDefenseMunition WHERE AirDefenseId = @airDefenseId;";
            var munitionParameters = new Dictionary<string, object> { { "@airDefenseId", airDefenseId } };
            _databaseHelper.ExecuteNonQuery(deleteMunitionsQuery, connection, munitionParameters);

            // En son hava savunma sistemini sil
            string deleteAirDefenseQuery = "DELETE FROM AirDefense WHERE Id = @airDefenseId;";
            var airDefenseParameters = new Dictionary<string, object> { { "@airDefenseId", airDefenseId } };
            _databaseHelper.ExecuteNonQuery(deleteAirDefenseQuery, connection, airDefenseParameters);
        }

        // Hava savunma sisteminden radar silme
        public void DeleteAirDefenseRadar(int airDefenseId, int radarId)
        {
            string deleteQuery = @"DELETE FROM AirDefenseRadar WHERE AirDefenseId = @airDefenseId AND RadarId = @radarId;";

            using var connection = _connectionManager.GetConnection();
            var parameters = new Dictionary<string, object>
            {
                { "@airDefenseId", airDefenseId },
                { "@radarId", radarId }
            };

            _databaseHelper.ExecuteNonQuery(deleteQuery, connection, parameters);
        }

        // Hava savunma sisteminden mühimmat silme
        public void DeleteAirDefenseMunition(int airDefenseId, int munitionId)
        {
            string deleteQuery = @"DELETE FROM AirDefenseMunition WHERE AirDefenseId = @airDefenseId AND MunitionId = @munitionId;";

            using var connection = _connectionManager.GetConnection();
            var parameters = new Dictionary<string, object>
            {
                { "@airDefenseId", airDefenseId },
                { "@munitionId", munitionId }
            };

            _databaseHelper.ExecuteNonQuery(deleteQuery, connection, parameters);
        }

    }
}
