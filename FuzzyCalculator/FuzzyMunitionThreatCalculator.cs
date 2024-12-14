using AirDefenseOptimizer.Enums;

namespace AirDefenseOptimizer.FuzzyCalculator
{
    public class FuzzyMunitionThreatCalculator
    {
        public double FuzzifyExplosivePower(double explosivePower)
        {
            double low = FuzzyLogicHelper.TriangularMembership(explosivePower, 25, 85, 145);
            double medium = FuzzyLogicHelper.TriangularMembership(explosivePower, 85, 145, 205);
            double high = FuzzyLogicHelper.TrapezoidalMembership(explosivePower, 150, 275, 400, double.MaxValue);

            double numerator = low * 0.2 + medium * 0.5 + high * 0.8;
            double denominator = low + medium + high;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (result <= 25)
                result = 0;
            if (result >= 400)
                result = 1;

            return result;
        }

        public double FuzzifyRange(double range)
        {
            double shortRange = FuzzyLogicHelper.TriangularMembership(range, 0, 25, 50);
            double mediumRange = FuzzyLogicHelper.TriangularMembership(range, 25, 50, 75);
            double longRange = FuzzyLogicHelper.TrapezoidalMembership(range, 50, 100, 150, double.MaxValue);

            double numerator = shortRange * 0.2 + mediumRange * 0.5 + longRange * 0.8;
            double denominator = shortRange + mediumRange + longRange;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (result <= 5)
                result = 0;
            if (result >= 150)
                result = 1;

            return result;
        }

        public double FuzzifySpeed(double speed)
        {
            double low = FuzzyLogicHelper.TriangularMembership(speed, 0, 1000, 2000);
            double medium = FuzzyLogicHelper.TriangularMembership(speed, 1000, 2000, 3000);
            double high = FuzzyLogicHelper.TrapezoidalMembership(speed, 2000, 3000, 4000, double.MaxValue);

            double numerator = low * 0.2 + medium * 0.5 + high * 0.8;
            double denominator = low + medium + high;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (result <= 500)
                result = 0;
            if (result >= 4000)
                result = 1;

            return result;
        }

        public double FuzzifyManeuverability(Maneuverability maneuverability)
        {
            double low = FuzzyLogicHelper.TriangularMembership(maneuverability.GetManeuverabilityNumber(), 0, 2, 4);
            double medium = FuzzyLogicHelper.TriangularMembership(maneuverability.GetManeuverabilityNumber(), 2, 5, 8);
            double high = FuzzyLogicHelper.TriangularMembership(maneuverability.GetManeuverabilityNumber(), 4, 7, 10);

            double numerator = low * 0.2 + medium * 0.5 + high * 0.8;
            double denominator = low + medium + high;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (result <= 0)
                result = 0;
            if (result >= 9)
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
