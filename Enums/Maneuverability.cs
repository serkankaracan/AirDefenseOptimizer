namespace AirDefenseOptimizer.Enums
{
    public enum Maneuverability
    {
        Limited,                // Sınırlı manevra kabiliyeti: Belirli kullanım durumları için minimal manevra kabiliyeti
        VeryLow,                // Çok düşük manevra kabiliyeti: Ağır, yavaş ve büyük uçaklar veya füzeler
        Low,                    // Düşük manevra kabiliyeti: Temel manevra yeteneği
        BelowAverage,           // Ortalama altı manevra kabiliyeti: Sınırlı manevra yeteneğine sahip bazı uçaklar veya füzeler
        Medium,                 // Orta düzey manevra kabiliyeti: Standart savaş uçakları veya füzeler için
        AboveAverage,           // Ortalama üstü manevra kabiliyeti: Standarttan daha iyi manevra kabiliyeti
        High,                   // Yüksek manevra kabiliyeti: Gelişmiş uçaklar veya füzeler için yüksek manevra yeteneği
        VeryHigh,               // Çok yüksek manevra kabiliyeti: Oldukça çevik sistemler, yüksek tepkisellik
        Agile,                  // Çevik: Ani yön değişiklikleri yapabilen yüksek manevra kabiliyetine sahip sistemler
        Extreme,                // Aşırı manevra kabiliyeti: En çevik platformlar için olağanüstü manevra yeteneği
        Supreme                 // Üstün manevra kabiliyeti: Performans açısından rakipsiz, en üst düzey manevra kabiliyeti
    }

    public static class ManeuverabilityExtensions
    {
        public static string GetManeuverabilityName(this Maneuverability maneuverability)
        {
            return maneuverability switch
            {
                Maneuverability.Limited => "Limited",
                Maneuverability.VeryLow => "Very Low",
                Maneuverability.Low => "Low",
                Maneuverability.BelowAverage => "Below Average",
                Maneuverability.Medium => "Medium",
                Maneuverability.AboveAverage => "Above Average",
                Maneuverability.High => "High",
                Maneuverability.VeryHigh => "Very High",
                Maneuverability.Agile => "Agile",
                Maneuverability.Extreme => "Extreme",
                Maneuverability.Supreme => "Supreme",
                _ => "Unknown"
            };
        }

        public static int GetManeuverabilityNumber(this Maneuverability maneuverability)
        {
            return maneuverability switch
            {
                Maneuverability.Limited => 1,
                Maneuverability.VeryLow => 2,
                Maneuverability.Low => 3,
                Maneuverability.BelowAverage => 4,
                Maneuverability.Medium => 5,
                Maneuverability.AboveAverage => 6,
                Maneuverability.High => 7,
                Maneuverability.VeryHigh => 8,
                Maneuverability.Agile => 9,
                Maneuverability.Extreme => 10,
                Maneuverability.Supreme => 11,
                _ => -1 // Belirsiz bir değer için varsayılan
            };
        }
    }
}
