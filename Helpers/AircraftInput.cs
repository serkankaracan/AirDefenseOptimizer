﻿using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Models;

namespace AirDefenseOptimizer.Helpers
{
    internal class AircraftInput
    {
        public Aircraft Aircraft { get; set; }
        public IFF IFFMode { get; set; }
        public double Speed { get; set; }
        public string Location { get; set; } // Latitude, Longitude, Altitude gibi bilgileri birleştirir
        public double Distance { get; set; }
        public double ThreatLevel { get; set; }
        public double ThreatScore { get; set; }

        public AircraftInput(Aircraft aircraft, IFF iffMode, double speed, string location, double distance, double threatLevel, double threatScore)
        {
            Aircraft = aircraft;
            IFFMode = iffMode;
            Speed = speed;
            Location = location;
            Distance = distance;
            ThreatLevel = threatLevel;
            ThreatScore = threatScore;
        }
    }
}
