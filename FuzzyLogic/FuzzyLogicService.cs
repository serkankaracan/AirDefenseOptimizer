namespace AirDefenseOptimizer.FuzzyLogic
{
    public class FuzzyLogicService
    {
        public double CalculateThreatLevel(double speed, double distance)
        {
            // Mesafe ve hız için bulanıklaştırma
            double threatLow = FuzzySets.Near(distance) * FuzzySets.Slow(speed);
            double threatHigh = FuzzySets.Far(distance) * FuzzySets.Fast(speed);

            // Tehdit seviyelerini bulanıklaştır
            return Defuzzify(threatLow, 0, threatHigh);
        }

        public double Defuzzify(double threatLow, double threatMedium, double threatHigh)
        {
            double numerator = (threatLow * 25) + (threatMedium * 50) + (threatHigh * 75);
            double denominator = threatLow + threatMedium + threatHigh;

            return denominator != 0 ? numerator / denominator : 0;
        }
    }
}
