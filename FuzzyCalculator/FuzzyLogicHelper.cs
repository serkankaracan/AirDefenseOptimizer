﻿namespace AirDefenseOptimizer.FuzzyCalculator
{
    public class FuzzyLogicHelper
    {
        // Üçgen üyelik fonksiyonu: x değeri ile a, b ve c noktalarına göre üyelik hesaplar
        public static double TriangularMembership(double x, double a, double b, double c)
        {
            if (x <= a || x >= c)
                return 0;
            else if (x == b)
                return 1;
            else if (x > a && x < b)
                return (x - a) / (b - a);
            else // x > b && x < c
                return (c - x) / (c - b);
        }
    }

}