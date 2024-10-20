namespace AirDefenseOptimizer.FuzzyEnums
{
    public class EnumRadar
    {
        /// <summary>
        /// Radar algılama menzili için kullanılacak bulanık kümeler
        /// </summary>
        public enum DetectionRange
        {
            Short,  // Kısa menzil
            Medium, // Orta menzil
            Long    // Uzun menzil
        }

        /// <summary>
        /// Radarın irtifa değerleri için kullanılacak bulanık kümeler
        /// </summary>
        public enum Altitude
        {
            Low,    // Düşük irtifa
            Medium, // Orta irtifa
            High    // Yüksek irtifa
        }

        /// <summary>
        /// Radarın algılayabileceği maksimum hedef hızları için kullanılacak bulanık kümeler
        /// </summary>
        public enum MaxTargetSpeed
        {
            Low,    // Düşük hız
            Medium, // Orta hız
            High    // Yüksek hız
        }

        /// <summary>
        /// Radarın algılayabileceği maksimum hedef hız (velocity) değerleri için kullanılacak bulanık kümeler
        /// </summary>
        public enum MaxTargetVelocity
        {
            Low,    // Düşük hız (velocity)
            Medium, // Orta hız (velocity)
            High    // Yüksek hız (velocity)
        }

        /// <summary>
        /// Radarın yeniden konuşlanma süresi için kullanılacak bulanık kümeler
        /// </summary>
        public enum RedeploymentTime
        {
            Short,  // Kısa süre
            Medium, // Orta süre
            Long    // Uzun süre
        }
    }
}
