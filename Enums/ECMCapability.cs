namespace AirDefenseOptimizer.Enums
{
    public enum ECMCapability
    {
        None,                 // ECM kabiliyeti yok
        Basic,                // Temel ECM kabiliyeti
        Advanced,             // Gelişmiş ECM kabiliyeti
        Jammer,               // Jammer yeteneği
        Decoy,                // Decoy (yanıltma) yeteneği
        MultiMode,            // Çok modlu ECM yeteneği
        Stealth               // Stealth ECM yeteneği
    }

    public static class ECMCapabilityExtensions
    {
        public static string GetECMCapabilityName(this ECMCapability ecmCapability)
        {
            return ecmCapability switch
            {
                ECMCapability.None => "None",
                ECMCapability.Basic => "Basic",
                ECMCapability.Advanced => "Advanced",
                ECMCapability.Jammer => "Jammer",
                ECMCapability.Decoy => "Decoy",
                ECMCapability.MultiMode => "MultiMode",
                ECMCapability.Stealth => "Stealth",
                _ => "Unknown"
            };
        }

        public static int GetECMCapabilityNumber(this ECMCapability ecmCapability)
        {
            return ecmCapability switch
            {
                ECMCapability.None => 0,
                ECMCapability.Basic => 1,
                ECMCapability.Advanced => 3,
                ECMCapability.Jammer => 2,
                ECMCapability.Decoy => 2,
                ECMCapability.MultiMode => 3,
                ECMCapability.Stealth => 4,
                _ => -1 // Belirsiz bir değer için varsayılan
            };
        }
    }
}
