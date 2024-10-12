using AirDefenseOptimizer.Enums;

namespace AirDefenseOptimizer.Models
{
    public class Munition
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public MunitionType MunitionType { get; set; } // Enum for Munition Type
        public double Weight { get; set; } // Weight in kg
        public double Speed { get; set; } // Speed in Mach
        public double Range { get; set; } // Range in km
        public string Maneuverability { get; set; } = string.Empty;
        public double ExplosivePower { get; set; } // Explosive power in TNT equivalent
        public double Cost { get; set; } // Cost in USD
    }
}
