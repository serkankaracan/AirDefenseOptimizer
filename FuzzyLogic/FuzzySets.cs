namespace AirDefenseOptimizer.FuzzyLogic
{
    public static class FuzzySets
    {
        // Bulanık set: Mesafe (Distance)
        public static double Near(double distance)
        {
            // Üyelik fonksiyonu (örneğin, triangular)
            return TriangularMembershipFunction(distance, 0, 25, 50);
        }

        public static double Far(double distance)
        {
            return TriangularMembershipFunction(distance, 30, 60, 90);
        }

        // Bulanık set: Hız (Speed)
        public static double Slow(double speed)
        {
            return TriangularMembershipFunction(speed, 0, 200, 400);
        }

        public static double Fast(double speed)
        {
            return TriangularMembershipFunction(speed, 300, 600, 900);
        }

        // Triangular üyelik fonksiyonu
        public static double TriangularMembershipFunction(double x, double a, double b, double c)
        {
            if (x <= a || x >= c)
                return 0;
            else if (x == b)
                return 1;
            else if (x > a && x < b)
                return (x - a) / (b - a);
            else
                return (c - x) / (c - b);
        }
    }
}
