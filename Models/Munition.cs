using AirDefenseOptimizer.Enums;

namespace AirDefenseOptimizer.Models
{
    public class Munition
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public MunitionType MunitionType { get; set; }
        public double Weight { get; set; }
        public double Speed { get; set; }
        public double Range { get; set; }
        public Maneuverability Maneuverability { get; set; }
        public double ExplosivePower { get; set; } // Explosive power in TNT equivalent
        public double Cost { get; set; }
    }
}
