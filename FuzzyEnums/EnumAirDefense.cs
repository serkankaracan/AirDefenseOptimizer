namespace AirDefenseOptimizer.FuzzyEnums
{
    public class EnumAirDefense
    {
        /// <summary>
        /// Hava savunma sisteminin menzili için kullanılacak bulanık kümeler
        /// </summary>
        public enum Range
        {
            Short,   // Kısa menzil
            Medium,  // Orta menzil
            Long     // Uzun menzil
        }

        /// <summary>
        /// Hava savunma sisteminin maksimum angaje kapasitesi için kullanılacak bulanık kümeler
        /// </summary>
        public enum MaxEngagements
        {
            Few,      // Az angaje kapasitesi
            Moderate, // Orta angaje kapasitesi
            Many      // Yüksek angaje kapasitesi
        }

        /// <summary>
        /// Hava savunma sisteminin maksimum füze ateşleme kapasitesi için kullanılacak bulanık kümeler
        /// </summary>
        public enum MaxMissilesFired
        {
            Low,     // Düşük füze ateşleme kapasitesi
            Medium,  // Orta füze ateşleme kapasitesi
            High     // Yüksek füze ateşleme kapasitesi
        }

        /// <summary>
        /// Hava savunma sisteminin ECM kabiliyeti için kullanılacak bulanık kümeler
        /// </summary>
        public enum ECMCapability
        {
            Weak,     // Zayıf ECM kabiliyeti
            Moderate, // Orta ECM kabiliyeti
            Strong    // Güçlü ECM kabiliyeti
        }

        /// <summary>
        /// Hava savunma sisteminin maliyeti için kullanılacak bulanık kümeler
        /// </summary>
        public enum Cost
        {
            Cheap,      // Ucuz sistem
            Moderate,   // Orta maliyetli sistem
            Expensive   // Pahalı sistem
        }
    }
}
