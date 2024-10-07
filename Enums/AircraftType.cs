namespace AirDefenseOptimizer.Enums
{
    public enum AircraftType
    {
        FighterJet,
        Bomber,
        Helicopter,
        Transport,
        Reconnaissance,
        Trainer,
        Drone,
        UAV,
        AirborneWarning,
        Tanker,
        MultiRole
    }

    public static class AircraftTypeExtensions
    {
        public static string GetAircraftTypeName(this AircraftType aircraftType)
        {
            return aircraftType switch
            {
                AircraftType.FighterJet => "Fighter Jet",
                AircraftType.Bomber => "Bomber",
                AircraftType.Helicopter => "Helicopter",
                AircraftType.Transport => "Transport",
                AircraftType.Reconnaissance => "Reconnaissance",
                AircraftType.Trainer => "Trainer",
                AircraftType.Drone => "Drone",
                AircraftType.UAV => "Unmanned Aerial Vehicle (UAV)",
                AircraftType.AirborneWarning => "Airborne Warning and Control System (AWACS)",
                AircraftType.Tanker => "Tanker",
                AircraftType.MultiRole => "Multi-Role Aircraft",
                _ => "Unknown"
            };
        }
    }
}
