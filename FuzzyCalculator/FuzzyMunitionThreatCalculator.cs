using AirDefenseOptimizer.Enums;

namespace AirDefenseOptimizer.FuzzyCalculator
{
    public class FuzzyMunitionThreatCalculator
    {
        public double FuzzifyExplosivePower(double explosivePower)
        {
            double low = FuzzyLogicHelper.TriangularMembership(explosivePower, 0, 50, 100);
            double medium = FuzzyLogicHelper.TriangularMembership(explosivePower, 75, 150, 225);
            double high = FuzzyLogicHelper.TriangularMembership(explosivePower, 200, 400, 600);
            return Math.Max(Math.Max(low, medium), high);
        }

        public double FuzzifyRange(double range)
        {
            double shortRange = FuzzyLogicHelper.TriangularMembership(range, 0, 200, 400);
            double mediumRange = FuzzyLogicHelper.TriangularMembership(range, 300, 600, 900);
            double longRange = FuzzyLogicHelper.TriangularMembership(range, 800, 1000, 1200);
            return Math.Max(Math.Max(shortRange, mediumRange), longRange);
        }

        public double FuzzifySpeed(double speed)
        {
            double low = FuzzyLogicHelper.TriangularMembership(speed, 0, 200, 400);
            double medium = FuzzyLogicHelper.TriangularMembership(speed, 300, 600, 900);
            double high = FuzzyLogicHelper.TriangularMembership(speed, 800, 1000, 1200);
            return Math.Max(Math.Max(low, medium), high);
        }

        public double FuzzifyManeuverability(Maneuverability maneuverability)
        {
            double low = FuzzyLogicHelper.TriangularMembership(maneuverability.GetManeuverabilityNumber(), 0, 1, 2);
            double medium = FuzzyLogicHelper.TriangularMembership(maneuverability.GetManeuverabilityNumber(), 1, 2, 3);
            double high = FuzzyLogicHelper.TriangularMembership(maneuverability.GetManeuverabilityNumber(), 2, 3, 4);
            return Math.Max(Math.Max(low, medium), high);
        }

        public double FuzzifyECM(ECMCapability ecmCapability)
        {
            double low = FuzzyLogicHelper.TriangularMembership(ecmCapability.GetECMCapabilityNumber(), 0, 1, 2);
            double medium = FuzzyLogicHelper.TriangularMembership(ecmCapability.GetECMCapabilityNumber(), 1, 2, 3);
            double high = FuzzyLogicHelper.TriangularMembership(ecmCapability.GetECMCapabilityNumber(), 2, 3, 4);
            return Math.Max(Math.Max(low, medium), high);
        }
    }

}
