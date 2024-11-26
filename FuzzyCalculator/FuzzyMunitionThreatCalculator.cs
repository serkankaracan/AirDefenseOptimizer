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

            double numerator = low * 0.1 + medium * 0.5 + high * 0.9;
            double denominator = low + medium + high;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (result >= 600)
                result = 1;

            return result;
        }

        public double FuzzifyRange(double range)
        {
            double shortRange = FuzzyLogicHelper.TriangularMembership(range, 0, 25, 50);
            double mediumRange = FuzzyLogicHelper.TriangularMembership(range, 30, 115, 200);
            double longRange = FuzzyLogicHelper.TriangularMembership(range, 150, 625, 1000);

            double numerator = shortRange * 0.1 + mediumRange * 0.5 + longRange * 0.9;
            double denominator = shortRange + mediumRange + longRange;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (result >= 1000)
                result = 1;

            return result;
        }

        public double FuzzifySpeed(double speed)
        {
            double low = FuzzyLogicHelper.TriangularMembership(speed, 0, 200, 400);
            double medium = FuzzyLogicHelper.TriangularMembership(speed, 300, 600, 900);
            double high = FuzzyLogicHelper.TriangularMembership(speed, 800, 1000, 1200);

            double numerator = low * 0.1 + medium * 0.5 + high * 0.9;
            double denominator = low + medium + high;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (result >= 1200)
                result = 1;

            return result;
        }

        public double FuzzifyManeuverability(Maneuverability maneuverability)
        {
            double low = FuzzyLogicHelper.TriangularMembership(maneuverability.GetManeuverabilityNumber(), 0, 1, 2);
            double medium = FuzzyLogicHelper.TriangularMembership(maneuverability.GetManeuverabilityNumber(), 1, 2, 3);
            double high = FuzzyLogicHelper.TriangularMembership(maneuverability.GetManeuverabilityNumber(), 2, 3, 4);

            double numerator = low * 0.1 + medium * 0.5 + high * 0.9;
            double denominator = low + medium + high;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (result >= 4)
                result = 1;

            return result;
        }

        public double FuzzifyQuantity(double quantity)
        {
            double low = FuzzyLogicHelper.TriangularMembership(quantity, 1, 3, 5);
            double medium = FuzzyLogicHelper.TriangularMembership(quantity, 4, 6, 8);
            double high = FuzzyLogicHelper.TriangularMembership(quantity, 5, 7, 9);

            double numerator = low * 0.1 + medium * 0.5 + high * 0.9;
            double denominator = low + medium + high;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (result >= 9)
                result = 1;
            if (result == 0)
                result = 0;

            return result;
        }
    }

}
