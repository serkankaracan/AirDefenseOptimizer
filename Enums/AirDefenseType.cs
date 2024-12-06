namespace AirDefenseOptimizer.Enums
{
    public enum AirDefenseType
    {
        PointDefense,
        ShortRange,               // Kısa Menzilli Hava Savunma Sistemleri
        MediumRange,              // Orta Menzilli Hava Savunma Sistemleri
        LongRange                 // Uzun Menzilli Hava Savunma Sistemleri
    }

    public static class AirDefenseTypeExtensions
    {
        public static string GetAirDefenseTypeName(this AirDefenseType airDefenseType)
        {
            return airDefenseType switch
            {
                AirDefenseType.PointDefense => "Point Defense",
                AirDefenseType.ShortRange => "Short Range",
                AirDefenseType.MediumRange => "Medium Range",
                AirDefenseType.LongRange => "Long Range",
                _ => "Unknown"
            };
        }
    }
}
