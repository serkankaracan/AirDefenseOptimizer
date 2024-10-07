using AirDefenseOptimizer.Enums;

namespace AirDefenseOptimizer.Models
{
    public class AirDefense
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double AerodynamicTargetRangeMin { get; set; } // Minimum range for aerodynamic targets in m
        public double AerodynamicTargetRangeMax { get; set; } // Maximum range for aerodynamic targets in km
        public double BallisticTargetRangeMin { get; set; } // Minimum range for ballistic targets in m
        public double BallisticTargetRangeMax { get; set; } // Maximum range for ballistic targets in km
        public int MaxEngagements { get; set; } // Maximum number of simultaneous engagements
        public int MaxMissilesFired { get; set; } // Maximum number of missiles fired at once
        public ECMCapability ECMCapability { get; set; } // Enum for ECM capability
        public double Cost { get; set; } // Cost in USD

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
