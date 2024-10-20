namespace AirDefenseOptimizer.FuzzyEnums
{
    public class EnumMunition
    {
        /// <summary>
        /// Mühimmatın ağırlığı için kullanılacak bulanık kümeler
        /// </summary>
        public enum Weight
        {
            Light,  // Hafif mühimmat
            Medium, // Orta ağırlıkta mühimmat
            Heavy   // Ağır mühimmat
        }

        /// <summary>
        /// Mühimmatın hızı için kullanılacak bulanık kümeler
        /// </summary>
        public enum Speed
        {
            Slow,    // Yavaş mühimmat
            Medium,  // Orta hızlı mühimmat
            Fast     // Hızlı mühimmat
        }

        /// <summary>
        /// Mühimmatın menzili için kullanılacak bulanık kümeler
        /// </summary>
        public enum Range
        {
            Short,  // Kısa menzil
            Medium, // Orta menzil
            Long    // Uzun menzil
        }

        /// <summary>
        /// Mühimmatın manevra kabiliyeti için kullanılacak bulanık kümeler
        /// </summary>
        public enum Maneuverability
        {
            Low,    // Düşük manevra kabiliyeti
            Medium, // Orta manevra kabiliyeti
            High    // Yüksek manevra kabiliyeti
        }

        /// <summary>
        /// Mühimmatın patlayıcı gücü için kullanılacak bulanık kümeler
        /// </summary>
        public enum ExplosivePower
        {
            Low,    // Düşük patlayıcı güç
            Medium, // Orta patlayıcı güç
            High    // Yüksek patlayıcı güç
        }

        /// <summary>
        /// Mühimmatın maliyeti için kullanılacak bulanık kümeler
        /// </summary>
        public enum Cost
        {
            Cheap,      // Ucuz mühimmat
            Moderate,   // Orta maliyetli mühimmat
            Expensive   // Pahalı mühimmat
        }
    }
}
