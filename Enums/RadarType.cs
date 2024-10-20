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

        // Radar tipi için işlevler
        public static (bool CanDetect, bool CanClassify, bool CanTrack, bool CanEngage) GetRadarCapabilities(this RadarType radarType)
        {
            return radarType switch
            {
                RadarType.LongRangeDetection => (true, false, false, false),
                RadarType.MissileGuidance => (false, true, true, true),
                RadarType.LowAltitudeDetection => (true, false, false, false),
                RadarType.EarlyWarning => (true, false, false, false),
                RadarType.TargetTracking => (true, true, true, true),
                RadarType.MultiFunction => (true, true, true, true),
                RadarType.FireControl => (true, true, true, true),
                RadarType.SamRadar => (true, true, true, true),
                RadarType.Airborne => (true, true, true, true),
                _ => (false, false, false, false)
            };
        }
    }
}
