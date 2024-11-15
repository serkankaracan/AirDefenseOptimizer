namespace AirDefenseOptimizer.FuzzyEnums
{
    public class EnumAircraft
    {
        /// <summary>
        /// Uçağın hızı için kullanılacak bulanık kümeler
        /// </summary>
        public enum Speed
        {
            VerySlow,   // Çok yavaş uçak
            Slow,       // Yavaş uçak
            Medium,     // Orta hızlı uçak
            Fast,       // Hızlı uçak
            VeryFast    // Çok hızlı uçak
        }

        /// <summary>
        /// Uçağın menzili için kullanılacak bulanık kümeler
        /// </summary>
        public enum Range
        {
            VeryShort,  // Çok kısa menzil
            Short,      // Kısa menzil
            Medium,     // Orta menzil
            Long,       // Uzun menzil
            VeryLong    // Çok uzun menzil
        }

        /// <summary>
        /// Uçağın maksimum irtifası için kullanılacak bulanık kümeler
        /// </summary>
        public enum MaxAltitude
        {
            VeryLow,    // Çok düşük irtifa
            Low,        // Düşük irtifa
            Medium,     // Orta irtifa
            High,       // Yüksek irtifa
            VeryHigh    // Çok yüksek irtifa
        }

        /// <summary>
        /// Uçağın manevra kabiliyeti için kullanılacak bulanık kümeler
        /// </summary>
        public enum Maneuverability
        {
            VeryLow,    // Çok düşük manevra kabiliyeti
            Low,        // Düşük manevra kabiliyeti
            Medium,     // Orta manevra kabiliyeti
            High,       // Yüksek manevra kabiliyeti
            VeryHigh    // Çok yüksek manevra kabiliyeti
        }

        /// <summary>
        /// Uçağın ECM yeteneği için kullanılacak bulanık kümeler
        /// </summary>
        public enum EcmCapability
        {
            VeryLow,    // Çok düşük ECM yeteneği
            Low,        // Düşük ECM yeteneği
            Medium,     // Orta ECM yeteneği
            High,       // Yüksek ECM yeteneği
            VeryHigh    // Çok yüksek ECM yeteneği
        }

        /// <summary>
        /// Uçağın yük kapasitesi için kullanılacak bulanık kümeler
        /// </summary>
        public enum PayloadCapacity
        {
            VerySmall,  // Çok küçük yük kapasitesi
            Small,      // Küçük yük kapasitesi
            Medium,     // Orta yük kapasitesi
            Large,      // Büyük yük kapasitesi
            VeryLarge   // Çok büyük yük kapasitesi
        }

        /// <summary>
        /// Uçağın radar kesit alanı için kullanılacak bulanık kümeler
        /// </summary>
        public enum RadarCrossSection
        {
            VeryLow,    // Çok düşük RKA
            Low,        // Düşük RKA
            Medium,     // Orta RKA
            High,       // Yüksek RKA
            VeryHigh    // Çok yüksek RKA
        }

        /// <summary>
        /// Uçağın kimliği için kullanılacak bulanık kümeler
        /// </summary>
        public enum IFFMode
        {
            Hostile,    // Düşman
            Suspect,    // Şüpheli
            Unknown,    // Bilinmeyen
            Neutral,    // Tarafsız
            Friendly    // Dost
        }

        /// <summary>
        /// Uçağın maliyeti için kullanılacak bulanık kümeler
        /// </summary>
        public enum Cost
        {
            VeryCheap,      // Çok ucuz uçak
            Cheap,          // Ucuz uçak
            Moderate,       // Orta maliyetli uçak
            Expensive,      // Pahalı uçak
            VeryExpensive   // Çok pahalı uçak
        }
    }
}
