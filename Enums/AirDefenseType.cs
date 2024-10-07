namespace AirDefenseOptimizer.Enums
{
    public enum AirDefenseType
    {
        SHORAD,                   // Kısa Menzilli Hava Savunma Sistemleri
        MEADS,                    // Orta Menzilli Hava Savunma Sistemleri (MRAD yerine daha yaygın olan bu terim kullanılabilir)
        LRSAM,                    // Uzun Menzilli Hava Savunma Sistemleri (LRAD yerine)
        BMD,                      // Balistik Füze Savunma Sistemleri
        NavalAirDefense,          // Deniz Tabanlı Hava Savunma Sistemleri
        CUAS,                     // İHA ve Dronlara Karşı Savunma Sistemleri
        RadarBasedDefense         // Radar Tabanlı Savunma Sistemleri
    }

    public static class AirDefenseTypeExtensions
    {
        public static string GetAirDefenseTypeName(this AirDefenseType airDefenseType)
        {
            return airDefenseType switch
            {
                AirDefenseType.SHORAD => "Short Range Air Defense",
                AirDefenseType.MEADS => "Medium Extended Air Defense System",
                AirDefenseType.LRSAM => "Long Range Surface to Air Missile",
                AirDefenseType.BMD => "Ballistic Missile Defense",
                AirDefenseType.NavalAirDefense => "Naval Air Defense",
                AirDefenseType.CUAS => "Counter-Unmanned Aircraft Systems",
                AirDefenseType.RadarBasedDefense => "Radar Based Defense",
                _ => "Unknown"
            };
        }
    }

}
