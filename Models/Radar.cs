using AirDefenseOptimizer.Enums;

namespace AirDefenseOptimizer.Models
{
    public class Radar
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public RadarType RadarType { get; set; }
        public double MinDetectionRange { get; set; }
        public double MaxDetectionRange { get; set; }
        public double MinAltitude { get; set; }
        public double MaxAltitude { get; set; }
        public double MaxTargetSpeed { get; set; }
        public double MaxTargetVelocity { get; set; }
        public int RedeploymentTime { get; set; }
    }
}
