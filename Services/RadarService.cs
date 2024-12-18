﻿using AirDefenseOptimizer.Database;
using System.Windows;

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

        public void AddRadar(string name, string radarType, int maxDetectionTargets, int maxTrackingTargets, double minDetectionRange, double maxDetectionRange, double maxAltitude, double minAltitude, double maxTargetSpeed, double maxTargetVelocity, int redeploymentTime)
        {
            try
            {
                string insertQuery = @"INSERT INTO Radar (Name, RadarType, MaxDetectionTargets, MaxTrackingTargets, MinDetectionRange, MaxDetectionRange, MaxAltitude, MinAltitude, MaxTargetSpeed, MaxTargetVelocity, RedeploymentTime) 
                                       VALUES (@name, @radarType, @maxDetectionTargets, @maxTrackingTargets, @minDetectionRange, @maxDetectionRange, @maxAltitude, @minAltitude, @maxTargetSpeed, @maxTargetVelocity, @redeploymentTime);";

                using var connection = _connectionManager.GetConnection();
                var parameters = new Dictionary<string, object>
                {
                    { "@name", name },
                    { "@radarType", radarType },
                    { "@maxDetectionTargets", maxDetectionTargets },
                    { "@maxTrackingTargets", maxTrackingTargets },
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding radar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateRadar(int id, string name, string radarType, int maxDetectionTargets, int maxTrackingTargets, double minDetectionRange, double maxDetectionRange, double maxAltitude, double minAltitude, double maxTargetSpeed, double maxTargetVelocity, int redeploymentTime)
        {
            try
            {
                string updateQuery = @"UPDATE Radar SET Name = @name, RadarType = @radarType, MaxDetectionTargets = @maxDetectionTargets, MaxTrackingTargets = @maxTrackingTargets, MinDetectionRange = @minDetectionRange, MaxDetectionRange = @maxDetectionRange, 
                                       MaxAltitude = @maxAltitude, MinAltitude = @minAltitude, MaxTargetSpeed = @maxTargetSpeed, MaxTargetVelocity = @maxTargetVelocity, 
                                       RedeploymentTime = @redeploymentTime WHERE Id = @id;";

                using var connection = _connectionManager.GetConnection();
                var parameters = new Dictionary<string, object>
                {
                    { "@id", id },
                    { "@name", name },
                    { "@radarType", radarType },
                    { "@maxDetectionTargets", maxDetectionTargets },
                    { "@maxTrackingTargets", maxTrackingTargets },
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating radar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DeleteRadar(int id)
        {
            try
            {
                string deleteQuery = @"DELETE FROM Radar WHERE Id = @id;";
                using var connection = _connectionManager.GetConnection();
                var parameters = new Dictionary<string, object> { { "@id", id } };

                _databaseHelper.ExecuteNonQuery(deleteQuery, connection, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting radar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Dictionary<string, object>? GetRadar(int id)
        {
            try
            {
                string selectQuery = @"SELECT * FROM Radar WHERE Id = @id;";
                using var connection = _connectionManager.GetConnection();
                var parameters = new Dictionary<string, object> { { "@id", id } };

                var result = _databaseHelper.ExecuteReader(selectQuery, connection, parameters);
                return result.Count > 0 ? result[0] : null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving radar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public List<Dictionary<string, object>> GetAllRadars()
        {
            try
            {
                string selectQuery = @"SELECT * FROM Radar;";
                using var connection = _connectionManager.GetConnection();

                return _databaseHelper.ExecuteReader(selectQuery, connection, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving all radars: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Dictionary<string, object>>();
            }
        }
    }
}
