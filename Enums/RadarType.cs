namespace AirDefenseOptimizer.Enums
{
    public enum RadarType
    {
        LongRangeDetection,
        MissileGuidance,
        LowAltitudeDetection,
        EarlyWarning,
        TargetTracking,
        MultiFunction,
        FireControl,
        SamRadar,
        Airborne
    }

    public static class RadarTypeExtensions
    {
        public static string GetRadarTypeName(this RadarType radarType)
        {
            return radarType switch
            {
                RadarType.LongRangeDetection => "Long Range Detection",
                RadarType.MissileGuidance => "Missile Guidance",
                RadarType.LowAltitudeDetection => "Low Altitude Detection",
                RadarType.EarlyWarning => "Early Warning",
                RadarType.TargetTracking => "Target Tracking",
                RadarType.MultiFunction => "Multi-Function",
                RadarType.FireControl => "Fire Control",
                RadarType.SamRadar => "Surface-to-Air Missile (SAM) Radar",
                RadarType.Airborne => "Airborne Radar",
                _ => "Unknown"
            };
        }
    }
}
