namespace AirDefenseOptimizer.Enums
{
    public enum RadarType
    {
        // Range Detection Radars
        LongRangeDetection,
        MediumRangeDetection,
        ShortRangeDetection,

        // Altitude Detection Radars
        HighAltitudeDetection,
        MediumAltitudeDetection,
        LowAltitudeDetection,

        // Special Function Radars
        EarlyWarning,
        TargetAcquisition,           // Hedef Arama/Tespit Radarı
        MissileGuidance,             // Füzelerin güdüm işlemleri
        TargetTracking,              // Hedef takip işlemleri
        Airborne,                    // Havadaki hedeflerin izlenmesi için

        // Multi-function and Control Radars
        MultiFunction,
        FireControl,
        SamRadar
    }

    public static class RadarTypeExtensions
    {
        public static string GetRadarTypeName(this RadarType radarType)
        {
            return radarType switch
            {
                // Range Detection Radars
                RadarType.LongRangeDetection => "Long Range Detection",
                RadarType.MediumRangeDetection => "Medium Range Detection",
                RadarType.ShortRangeDetection => "Short Range Detection",

                // Altitude Detection Radars
                RadarType.HighAltitudeDetection => "High Altitude Detection",
                RadarType.MediumAltitudeDetection => "Medium Altitude Detection",
                RadarType.LowAltitudeDetection => "Low Altitude Detection",

                // Special Function Radars
                RadarType.EarlyWarning => "Early Warning",
                RadarType.TargetAcquisition => "Target Acquisition Radar",
                RadarType.MissileGuidance => "Missile Guidance",
                RadarType.TargetTracking => "Target Tracking",
                RadarType.Airborne => "Airborne Radar",

                // Multi-function and Control Radars
                RadarType.MultiFunction => "Multi-Function",
                RadarType.FireControl => "Fire Control",
                RadarType.SamRadar => "Surface-to-Air Missile (SAM) Radar",
                _ => "Unknown"
            };
        }

        // Radar tipi için işlevler
        public static (bool CanDetect, bool CanClassify, bool CanTrack, bool CanEngage) GetRadarCapabilities(this RadarType radarType)
        {
            return radarType switch
            {
                // Range Detection Radars
                RadarType.LongRangeDetection => (true, false, false, false),
                RadarType.MediumRangeDetection => (true, false, false, false),
                RadarType.ShortRangeDetection => (true, false, false, false),

                // Altitude Detection Radars
                RadarType.HighAltitudeDetection => (true, false, false, false),
                RadarType.MediumAltitudeDetection => (true, false, false, false),
                RadarType.LowAltitudeDetection => (true, false, false, false),

                // Special Function Radars
                RadarType.EarlyWarning => (true, false, false, false),
                RadarType.TargetAcquisition => (true, true, true, false),
                RadarType.MissileGuidance => (false, true, true, true),
                RadarType.TargetTracking => (true, true, true, true),
                RadarType.Airborne => (true, true, true, true),

                // Multi-function and Control Radars
                RadarType.MultiFunction => (true, true, true, true),
                RadarType.FireControl => (true, true, true, true),
                RadarType.SamRadar => (true, true, true, true),
                _ => (false, false, false, false)
            };
        }
    }
}
