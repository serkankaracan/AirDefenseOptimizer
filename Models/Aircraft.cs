using AirDefenseOptimizer.Enums;

namespace AirDefenseOptimizer.Models
{
    public class Aircraft
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public AircraftType AircraftType { get; set; }
        public double Speed { get; set; }
        public double Range { get; set; }
        public double MaxAltitude { get; set; }
        public Maneuverability Maneuverability { get; set; }
        public ECMCapability ECMCapability { get; set; }
        public double PayloadCapacity { get; set; }
        public double RadarCrossSection { get; set; }
        public Radar? Radar { get; set; }
        public double Cost { get; set; }

        public List<AircraftMunition> Munitions { get; set; } = new List<AircraftMunition>();
    }

    public class AircraftMunition
    {
        public Munition Munition { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
