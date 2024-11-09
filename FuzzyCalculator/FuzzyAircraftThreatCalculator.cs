using AirDefenseOptimizer.Enums;

namespace AirDefenseOptimizer.FuzzyCalculator
{
    public class FuzzyAircraftThreatCalculator
    {
        // Hız için üçgen bulanık kümeler
        public double FuzzifySpeed(double speed)
        {
            double low = FuzzyLogicHelper.TriangularMembership(speed, 0, 200, 400);
            double medium = FuzzyLogicHelper.TriangularMembership(speed, 300, 600, 900);
            double high = FuzzyLogicHelper.TriangularMembership(speed, 800, 1000, 1200);
            return Math.Max(Math.Max(low, medium), high);
        }

        // Radar görünürlüğü için üçgen bulanık kümeler
        public double FuzzifyRadarCrossSection(double rcs)
        {
            double low = FuzzyLogicHelper.TriangularMembership(rcs, 0, 1, 3);
            double medium = FuzzyLogicHelper.TriangularMembership(rcs, 2, 4, 6);
            double high = FuzzyLogicHelper.TriangularMembership(rcs, 5, 8, 10);
            return Math.Max(Math.Max(low, medium), high);
        }

        // ECM yeteneği için üçgen bulanık kümeler
        public double FuzzifyECM(ECMCapability ecmCapability)
        {
            double low = FuzzyLogicHelper.TriangularMembership(ecmCapability.GetECMCapabilityNumber(), 0, 1, 2);
            double medium = FuzzyLogicHelper.TriangularMembership(ecmCapability.GetECMCapabilityNumber(), 1, 2, 3);
            double high = FuzzyLogicHelper.TriangularMembership(ecmCapability.GetECMCapabilityNumber(), 2, 3, 4);
            return Math.Max(Math.Max(low, medium), high);
        }

        // Mesafe için üçgen bulanık kümeler
        public double FuzzifyDistance(double distance)
        {
            double close = FuzzyLogicHelper.TriangularMembership(distance, 0, 50, 100);
            double medium = FuzzyLogicHelper.TriangularMembership(distance, 75, 150, 225);
            double far = FuzzyLogicHelper.TriangularMembership(distance, 200, 300, 400);
            return Math.Max(Math.Max(close, medium), far);
        }

        // Hava aracı için bulanık kurallar
        public double ApplyFuzzyRules(double speed, double radarCrossSection, double ecmCapability, double distance)
        {
            double threatLevel = 0;

            if (speed > 0.7 && radarCrossSection < 0.3 && ecmCapability > 0.7)
                threatLevel = Math.Max(threatLevel, 0.9);

            if (speed > 0.3 && speed <= 0.7 && distance < 0.3)
                threatLevel = Math.Max(threatLevel, 0.6);

            if (radarCrossSection > 0.7 && distance < 0.3)
                threatLevel = Math.Max(threatLevel, 0.8);

            if (speed < 0.3 && distance > 0.7)
                threatLevel = Math.Max(threatLevel, 0.2);

            return threatLevel;
        }

        // Kesinleştirme işlemi
        public double Defuzzify(double threatLevel)
        {
            //return threatLevel * 100;

            // Tehdit seviyesini 0 ile 1 arasında normalize et
            return Math.Min(1, Math.Max(0, threatLevel / 100));
        }
    }

}
