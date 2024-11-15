namespace AirDefenseOptimizer.Enums
{
    /// <summary>
    /// Hava savunma sistemlerinde kullanılan genel radar işlevlerini tanımlar.
    /// </summary>
    public enum RadarType
    {
        Detection,      // Tespit
        Search,         // Arama
        Identification, // Teşhis
        Tracking,       // İzleme
        Engagement,     // Angajman
        FireControl,    // Atış Kontrol
        Airborne        // Uçak radarı
    }

    /// <summary>
    /// Radar türü için açıklamalar ve işlevsel yetenekler.
    /// </summary>
    public static class RadarTypeExtensions
    {
        /// <summary>
        /// Radar türünün açıklamasını alır.
        /// </summary>
        public static string GetRadarTypeName(this RadarType radarType)
        {
            return radarType switch
            {
                RadarType.Detection => "Detection",
                RadarType.Search => "Search",
                RadarType.Identification => "Identification",
                RadarType.Tracking => "Tracking",
                RadarType.Engagement => "Engagement",
                RadarType.FireControl => "Fire Control",
                RadarType.Airborne => "Airborne",
                _ => "Unknown Radar Type"
            };
        }

        /// <summary>
        /// Radar türünün yeteneklerini döndürür.
        /// </summary>
        public static (bool CanDetect, bool CanClassify, bool CanTrack, bool CanEngage) GetRadarCapabilities(this RadarType radarType)
        {
            return radarType switch
            {
                RadarType.Detection => (true, false, false, false),          // Yalnızca tespit
                RadarType.Search => (true, false, false, false),             // Yalnızca arama
                RadarType.Identification => (true, true, false, false),      // Tespit ve teşhis
                RadarType.Tracking => (true, true, true, false),             // Tespit, teşhis ve izleme
                RadarType.Engagement => (false, true, true, true),           // Teşhis, izleme ve angajman
                RadarType.FireControl => (false, true, true, true),          // Teşhis, izleme ve angajman
                _ => (false, false, false, false)
            };
        }
    }
}
