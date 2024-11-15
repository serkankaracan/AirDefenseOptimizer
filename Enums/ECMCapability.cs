namespace AirDefenseOptimizer.Enums
{
    /// <summary>
    /// ECM (Electronic Countermeasures) kabiliyeti seviyeleri
    /// </summary>
    public enum ECMCapability
    {
        None,          // ECM kabiliyeti yok
        Basic,         // Temel ECM kabiliyeti
        Intermediate,  // Orta seviye ECM kabiliyeti
        Jammer,        // Jammer (karıştırma) yeteneği
        Decoy,         // Decoy (yanıltma) yeteneği
        Advanced,      // Gelişmiş ECM kabiliyeti
        MultiMode,     // Çok modlu ECM yeteneği
        Stealth        // Stealth (görünmezlik) ECM yeteneği
    }

    public static class ECMCapabilityExtensions
    {
        /// <summary>
        /// ECM kabiliyeti için açıklayıcı isim döndürür
        /// </summary>
        public static string GetECMCapabilityName(this ECMCapability ecmCapability)
        {
            return ecmCapability switch
            {
                ECMCapability.None => "None (No ECM Capability)",
                ECMCapability.Basic => "Basic ECM Capability",
                ECMCapability.Intermediate => "Intermediate ECM Capability",
                ECMCapability.Jammer => "Jammer Capability",
                ECMCapability.Decoy => "Decoy Capability",
                ECMCapability.Advanced => "Advanced ECM Capability",
                ECMCapability.MultiMode => "Multi-Mode ECM Capability",
                ECMCapability.Stealth => "Stealth ECM Capability",
                _ => "Unknown"
            };
        }

        /// <summary>
        /// ECM kabiliyeti seviyesine karşılık gelen sayısal değeri döndürür
        /// </summary>
        public static int GetECMCapabilityNumber(this ECMCapability ecmCapability)
        {
            return ecmCapability switch
            {
                ECMCapability.None => 0,           // Kabiliyet yok
                ECMCapability.Basic => 1,          // Temel seviye
                ECMCapability.Intermediate => 2,   // Orta seviye
                ECMCapability.Jammer => 3,         // Karıştırıcı
                ECMCapability.Decoy => 3,          // Yanıltıcı
                ECMCapability.Advanced => 4,       // Gelişmiş seviye
                ECMCapability.MultiMode => 5,      // Çok modlu yüksek seviye
                ECMCapability.Stealth => 6,        // En üst seviye (stealth)
                _ => -1 // Belirsiz durumlar için
            };
        }
    }
}
