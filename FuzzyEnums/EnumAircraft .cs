namespace AirDefenseOptimizer.FuzzyEnums
{
    public class EnumAircraft
    {
        /// <summary>
        /// Uçağın hızı için kullanılacak bulanık kümeler
        /// </summary>
        public enum Speed
        {
            Slow,    // Yavaş uçak
            Medium,  // Orta hızlı uçak
            Fast     // Hızlı uçak
        }

        /// <summary>
        /// Uçağın menzili için kullanılacak bulanık kümeler
        /// </summary>
        public enum Range
        {
            Short,  // Kısa menzil
            Medium, // Orta menzil
            Long    // Uzun menzil
        }

        /// <summary>
        /// Uçağın maksimum irtifası için kullanılacak bulanık kümeler
        /// </summary>
        public enum MaxAltitude
        {
            Low,    // Düşük irtifa
            Medium, // Orta irtifa
            High    // Yüksek irtifa
        }

        /// <summary>
        /// Uçağın manevra kabiliyeti için kullanılacak bulanık kümeler
        /// </summary>
        public enum Maneuverability
        {
            Low,    // Düşük manevra kabiliyeti
            Medium, // Orta manevra kabiliyeti
            High    // Yüksek manevra kabiliyeti
        }

        public enum EcmCapability
        {
            Low,    // Düşük manevra kabiliyeti
            Medium, // Orta manevra kabiliyeti
            High    // Yüksek manevra kabiliyeti
        }

        /// <summary>
        /// Uçağın yük kapasitesi için kullanılacak bulanık kümeler
        /// </summary>
        public enum PayloadCapacity
        {
            Small,  // Küçük yük kapasitesi
            Medium, // Orta yük kapasitesi
            Large   // Büyük yük kapasitesi
        }

        public enum RadarCrossSection
        {
            Low,
            Medium,
            High
        }

        /// <summary>
        /// Uçağın kimliği için kullanılacak bulanık kümeler
        /// </summary>
        public enum IFFMode
        {
            Foe,
            Unknown,
            Friend
        }

        /// <summary>
        /// Uçağın maliyeti için kullanılacak bulanık kümeler
        /// </summary>
        public enum Cost
        {
            Cheap,      // Ucuz uçak
            Moderate,   // Orta maliyetli uçak
            Expensive   // Pahalı uçak
        }
    }
}
