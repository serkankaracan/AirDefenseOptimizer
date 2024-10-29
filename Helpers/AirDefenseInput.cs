using AirDefenseOptimizer.Models;

namespace AirDefenseOptimizer.Helpers
{
    internal class AirDefenseInput
    {
        public AirDefense AirDefense { get; set; }
        public string Location { get; set; } // Latitude, Longitude, Altitude gibi bilgileri birleştirir

        public AirDefenseInput(AirDefense airDefense, string location)
        {
            AirDefense = airDefense;
            Location = location;
        }
    }
}
