using AirDefenseOptimizer.Enums;

namespace AirDefenseOptimizer.FuzzyCalculator
{
    public class FuzzyAircraftThreatCalculator
    {
        // Hız için üçgen bulanık kümeler (Düşük, Orta, Yüksek, Çok Yüksek hız kategorileri eklendi)
        public double FuzzifySpeed(double speed)
        {
            double veryLow = FuzzyLogicHelper.TriangularMembership(speed, 0, 100, 200);
            double low = FuzzyLogicHelper.TriangularMembership(speed, 150, 300, 450);
            double medium = FuzzyLogicHelper.TriangularMembership(speed, 400, 600, 800);
            double high = FuzzyLogicHelper.TriangularMembership(speed, 700, 1000, 1200);
            double veryHigh = FuzzyLogicHelper.TrapezoidalMembership(speed, 1000, 1300, 1500, double.MaxValue);

            // Tehdit seviyesini üyelik derecelerinin ağırlıklı toplamı olarak hesaplayın
            double numerator = veryLow * 0.1 + low * 0.3 + medium * 0.5 + high * 0.7 + veryHigh * 0.9;
            double denominator = veryLow + low + medium + high + veryHigh;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (speed >= 1500)
                result = 1;

            return result;
        }

        // Radar görünürlüğü için üçgen bulanık kümeler (Düşük, Orta, Yüksek RCS eklendi)
        public double FuzzifyRadarCrossSection(double rcs)
        {
            double veryLow = FuzzyLogicHelper.TriangularMembership(rcs, 0, 0.3, 1);
            double low = FuzzyLogicHelper.TriangularMembership(rcs, 0.5, 2, 3);
            double medium = FuzzyLogicHelper.TriangularMembership(rcs, 2, 5, 8);
            double high = FuzzyLogicHelper.TriangularMembership(rcs, 6, 8, 10);
            double veryHigh = FuzzyLogicHelper.TrapezoidalMembership(rcs, 9, 12, 15, 20);

            double numerator = veryLow * 0.9 + low * 0.7 + medium * 0.5 + high * 0.3 + veryHigh * 0.1;
            double denominator = veryLow + low + medium + high + veryHigh;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (result >= 15)
                result = 0;

            return result;
        }

        // ECM yeteneği için üçgen bulanık kümeler (Düşük, Orta, Yüksek ECM kategorileri)
        public double FuzzifyECM(ECMCapability ecmCapability)
        {
            double veryLow = FuzzyLogicHelper.TriangularMembership(ecmCapability.GetECMCapabilityNumber(), 0, 1, 2);
            double low = FuzzyLogicHelper.TriangularMembership(ecmCapability.GetECMCapabilityNumber(), 1, 2, 3);
            double medium = FuzzyLogicHelper.TriangularMembership(ecmCapability.GetECMCapabilityNumber(), 2, 3, 4);
            double high = FuzzyLogicHelper.TriangularMembership(ecmCapability.GetECMCapabilityNumber(), 3, 4, 5);
            double veryHigh = FuzzyLogicHelper.TrapezoidalMembership(ecmCapability.GetECMCapabilityNumber(), 4, 5, 6, double.MaxValue);

            double numerator = veryLow * 0.1 + low * 0.3 + medium * 0.5 + high * 0.7 + veryHigh * 0.9;
            double denominator = veryLow + low + medium + high + veryHigh;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (ecmCapability.GetECMCapabilityNumber() >= 6)
                result = 1;

            return result;
        }

        // Mesafe için üçgen bulanık kümeler (Çok Yakın, Yakın, Orta, Uzak, Çok Uzak mesafe kategorileri)
        public double FuzzifyDistance(double distance)
        {
            double veryClose = FuzzyLogicHelper.TriangularMembership(distance, 0, 5, 10);
            double close = FuzzyLogicHelper.TriangularMembership(distance, 8, 20, 40);
            double medium = FuzzyLogicHelper.TriangularMembership(distance, 30, 60, 100);
            double far = FuzzyLogicHelper.TriangularMembership(distance, 80, 150, 250);
            double veryFar = FuzzyLogicHelper.TriangularMembership(distance, 200, 300, 400);

            double numerator = veryClose * 0.9 + close * 0.7 + medium * 0.5 + far * 0.3 + veryFar * 0.1;
            double denominator = veryClose + close + medium + far + veryFar;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (distance >= 400)
                result = 0;
            if (distance <= 5)
                result = 1;

            return result;
        }

        public double FuzzyfyManeuverability(Maneuverability maneuverability)
        {
            double veryLow = FuzzyLogicHelper.TriangularMembership(maneuverability.GetManeuverabilityNumber(), 0, 2, 4);
            double low = FuzzyLogicHelper.TriangularMembership(maneuverability.GetManeuverabilityNumber(), 1, 3, 5);
            double medium = FuzzyLogicHelper.TriangularMembership(maneuverability.GetManeuverabilityNumber(), 2, 4, 6);
            double high = FuzzyLogicHelper.TriangularMembership(maneuverability.GetManeuverabilityNumber(), 3, 5, 7);
            double veryHigh = FuzzyLogicHelper.TrapezoidalMembership(maneuverability.GetManeuverabilityNumber(), 4, 6, 8, double.MaxValue);

            double numerator = veryLow * 0.1 + low * 0.3 + medium * 0.5 + high * 0.7 + veryHigh * 0.9;
            double denominator = veryLow + low + medium + high + veryHigh;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (maneuverability.GetManeuverabilityNumber() >= 8)
                result = 1;

            return result;
        }

        public double FuzzyfyAltitude(double altitude)
        {
            double veryLow = FuzzyLogicHelper.TriangularMembership(altitude, 0, 500, 1000);
            double low = FuzzyLogicHelper.TriangularMembership(altitude, 500, 2000, 4000);
            double medium = FuzzyLogicHelper.TriangularMembership(altitude, 3000, 7000, 10000);
            double high = FuzzyLogicHelper.TriangularMembership(altitude, 8000, 15000, 20000);
            double veryHigh = FuzzyLogicHelper.TrapezoidalMembership(altitude, 18000, 25000, 30000, double.MaxValue);

            double numerator = veryLow * 0.9 + low * 0.7 + medium * 0.5 + high * 0.3 + veryHigh * 0.1;
            double denominator = veryLow + low + medium + high + veryHigh;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (altitude >= 30000)
                result = 0;
            if (altitude <= 500)
                result = 1;

            return result;
        }

        public double FuzzyfyCost(double cost)
        {
            double veryCheap = FuzzyLogicHelper.TriangularMembership(cost, 0, 20000000, 40000000);
            double cheap = FuzzyLogicHelper.TriangularMembership(cost, 20000000, 40000000, 60000000);
            double normal = FuzzyLogicHelper.TriangularMembership(cost, 40000000, 60000000, 80000000);
            double expensive = FuzzyLogicHelper.TriangularMembership(cost, 60000000, 80000000, 100000000);
            double veryExpensive = FuzzyLogicHelper.TrapezoidalMembership(cost, 80000000, 100000000, 120000000, double.MaxValue);

            double numerator = veryCheap * 0.1 + cheap * 0.3 + normal * 0.5 + expensive * 0.7 + veryExpensive * 0.9;
            double denominator = veryCheap + cheap + normal + expensive + veryExpensive;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (cost >= 120000000)
                result = 1;

            return result;
        }

        public double ApplyFuzzyRules(double speed, double radarCrossSection, double ecmCapability, double distance, double maneuverability, double altitude, double cost)
        {
            double weightSpeed = 0.15;
            double weightRadarCrossSection = 0.15;
            double weightECM = 0.15;
            double weightDistance = 0.3;
            double weightManeuverability = 0.1;
            double weightAltitude = 0.1;
            double weightCost = 0.05;

            // Mesafe yakın oldukça tehdit artmalı, uzak oldukça tehdit azalmalı
            //double invertedDistance = 1 - distance; // Mesafeyi ters çevir (yakın = 1, uzak = 0)

            double totalThreat = (weightSpeed * speed) +
                                 (weightRadarCrossSection * radarCrossSection) +
                                 (weightECM * ecmCapability) +
                                 (weightDistance * distance) + // İnverse distance kullan
                                 (weightManeuverability * maneuverability) +
                                 (weightAltitude * altitude) +
                                 (weightCost * cost);

            return totalThreat;
        }

        private T GetFuzzyLevel<T>(double value) where T : Enum
        {
            int numLevels = Enum.GetValues(typeof(T)).Length;
            double interval = 1.0 / numLevels;
            int index = (int)(value / interval);

            // Index sınırlarını kontrol ediyoruz
            if (index >= numLevels)
                index = numLevels - 1;

            return (T)Enum.GetValues(typeof(T)).GetValue(index);
        }

        // Kesinleştirme işlemi
        public double Defuzzify(double threatLevel)
        {
            // Normalize tehdit seviyesi
            threatLevel = Math.Min(1.0, Math.Max(0.0, threatLevel));

            // 5 seviyeli tehdit kategorileri
            if (threatLevel > 0.8)
                return 1.0; // Çok yüksek tehdit
            else if (threatLevel > 0.6)
                return 0.8; // Yüksek tehdit
            else if (threatLevel > 0.4)
                return 0.6; // Orta tehdit
            else if (threatLevel > 0.2)
                return 0.4; // Düşük tehdit
            else
                return 0.2; // Çok düşük tehdit
        }

    }
}