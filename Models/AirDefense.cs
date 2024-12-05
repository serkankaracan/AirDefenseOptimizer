using AirDefenseOptimizer.Enums;

namespace AirDefenseOptimizer.Models
{
    public class AirDefense
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double AerodynamicTargetRangeMin { get; set; }
        public double AerodynamicTargetRangeMax { get; set; }
        public double BallisticTargetRangeMin { get; set; }
        public double BallisticTargetRangeMax { get; set; }
        public int CurrentEngagements { get; set; }
        public int MaxEngagements { get; set; }
        public int MaxMissilesFired { get; set; }
        public ECMCapability ECMCapability { get; set; }
        public double Cost { get; set; }

        public List<AirDefenseRadar> Radars { get; set; } = new List<AirDefenseRadar>(); // List of radars
        public List<AirDefenseMunition> Munitions { get; set; } = new List<AirDefenseMunition>(); // List of munitions
    }

    public class AirDefenseRadar
    {
        public Radar Radar { get; set; } = null!;
        public int Quantity { get; set; }
    }

    public class AirDefenseMunition
    {
        public Munition Munition { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
