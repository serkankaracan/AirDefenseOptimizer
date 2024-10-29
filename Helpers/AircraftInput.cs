using AirDefenseOptimizer.Models;

namespace AirDefenseOptimizer.Helpers
{
    internal class AircraftInput
    {
        public Aircraft Aircraft { get; set; }
        public string IFFMode { get; set; }
        public string Location { get; set; } // Latitude, Longitude, Altitude gibi bilgileri birleştirir

        public AircraftInput(Aircraft aircraft, string iffMode, string location)
        {
            Aircraft = aircraft;
            IFFMode = iffMode;
            Location = location;
        }
    }
}
