namespace AirDefenseOptimizer.FuzzyLogic
{
    public static class FuzzyRules
    {
        public static double GetThreatLevel(double speed, double altitude)
        {
            // Hız ve irtifaya dayalı bir tehdit seviyesi hesaplayalım
            if (speed > 800 && altitude > 30000)
                return 0.9; // Yüksek tehdit seviyesi
            if (speed > 600)
                return 0.7; // Orta tehdit seviyesi
            return 0.5; // Düşük tehdit seviyesi
        }
    }
}
