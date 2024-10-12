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
    }
}
