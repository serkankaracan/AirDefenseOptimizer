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

            // Hız 1500 veya daha büyükse, sonucu 1'e ayarlayın
            if (speed >= 1500)
                result = 1;

            return result;
        }

        // Radar görünürlüğü için üçgen bulanık kümeler (Düşük, Orta, Yüksek RCS eklendi)
        public double FuzzifyRadarCrossSection(double rcs)
        {
            double veryLow = FuzzyLogicHelper.TriangularMembership(rcs, 0, 0.5, 1);
            double low = FuzzyLogicHelper.TriangularMembership(rcs, 0.5, 2, 3);
            double medium = FuzzyLogicHelper.TriangularMembership(rcs, 2, 5, 7);
            double high = FuzzyLogicHelper.TriangularMembership(rcs, 5, 8, 10);
            double veryHigh = FuzzyLogicHelper.TriangularMembership(rcs, 8, 10, 12);

            double numerator = veryLow * 0.1 + low * 0.3 + medium * 0.5 + high * 0.7 + veryHigh * 0.9;
            double denominator = veryLow + low + medium + high + veryHigh;

            double result = denominator != 0 ? numerator / denominator : 0;

            // Hız 1500 veya daha büyükse, sonucu 1'e ayarlayın
            if (rcs >= 12)
                result = 1;

            return result;
        }

        // ECM yeteneği için üçgen bulanık kümeler (Düşük, Orta, Yüksek ECM kategorileri)
        public double FuzzifyECM(ECMCapability ecmCapability)
        {
            double low = FuzzyLogicHelper.TriangularMembership(ecmCapability.GetECMCapabilityNumber(), 0, 1, 2);
            double medium = FuzzyLogicHelper.TriangularMembership(ecmCapability.GetECMCapabilityNumber(), 1, 2, 3);
            double high = FuzzyLogicHelper.TriangularMembership(ecmCapability.GetECMCapabilityNumber(), 2, 3, 4);

            double numerator = low * 0.1 + medium * 0.3 + high * 0.5;
            double denominator = low + medium + high;

            double result = denominator != 0 ? numerator / denominator : 0;
            if (ecmCapability.GetECMCapabilityNumber() >= 4)
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

            double numerator = veryClose * 0.1 + close * 0.3 + medium * 0.5 + far * 0.7 + veryFar * 0.9;
            double denominator = veryClose + close + medium + far + veryFar;
            double result = denominator != 0 ? numerator / denominator : 0;
            if (distance >= 400)
                result = 1;

            return result;
        }

        public double FuzzyfyManeuverability(Maneuverability maneuverability)
        {
            double low = FuzzyLogicHelper.TriangularMembership(maneuverability.GetManeuverabilityNumber(), 0, 3, 5);
            double medium = FuzzyLogicHelper.TriangularMembership(maneuverability.GetManeuverabilityNumber(), 4, 7, 9);
            double high = FuzzyLogicHelper.TriangularMembership(maneuverability.GetManeuverabilityNumber(), 6, 8, 11);
            
            double numerator = low * 0.1 + medium * 0.3 + high * 0.5;
            double denominator = low + medium + high;
            double result = denominator != 0 ? numerator / denominator : 0;
            if (maneuverability.GetManeuverabilityNumber() >= 11)
                result = 1;

            return result;
        }

        public double FuzzyfyAltitude(double altitude)
        {
            double low = FuzzyLogicHelper.TriangularMembership(altitude, 0, 5000, 7500);
            double medium = FuzzyLogicHelper.TriangularMembership(altitude, 6000, 10000, 15000);
            double high = FuzzyLogicHelper.TriangularMembership(altitude, 12000, 20000, 25000);
            
            double numerator = low * 0.1 + medium * 0.3 + high * 0.5;
            double denominator = low + medium + high;
            double result = denominator != 0 ? numerator / denominator : 0;
            if (altitude >= 25000)
                result = 1;

            return result;
        }

        public double FuzzyfyCost(double cost)
        {
            double veryCheap = FuzzyLogicHelper.TriangularMembership(cost, 500000, 1000000, 2000000);
            double cheap = FuzzyLogicHelper.TriangularMembership(cost, 1000000, 3000000, 5000000);
            double normal = FuzzyLogicHelper.TriangularMembership(cost, 4000000, 10000000, 15000000);
            double expensive = FuzzyLogicHelper.TriangularMembership(cost, 10000000, 20000000, 30000000);
            double veryExpensive = FuzzyLogicHelper.TriangularMembership(cost, 25000000, 40000000, 50000000);

            double numerator = veryCheap * 0.1 + cheap * 0.3 + normal * 0.5 + expensive * 0.7 + veryExpensive * 0.9;
            double denominator = veryCheap + cheap + normal + expensive + veryExpensive;
            double result = denominator != 0 ? numerator / denominator : 0;
            if (cost >= 50000000)
                result = 1;

            return result;
        }

        // Hava aracı için bulanık kurallar
        public double ApplyFuzzyRules(double speed, double radarCrossSection, double ecmCapability, double distance, double maneuverability, double altitude, double cost)
        {
            double threatLevel = 0;

            string speedLevel = GetFuzzyLevel(speed);
            string radarCrossSectionLevel = GetFuzzyLevel(radarCrossSection);
            string ecmCapabilityLevel = GetFuzzyLevel(ecmCapability);
            string distanceLevel = GetFuzzyLevel(distance);
            string maneuverabilityLevel = GetFuzzyLevel(maneuverability);
            string altitudeLevel = GetFuzzyLevel(altitude);
            string costLevel = GetFuzzyLevel(cost);

            // Çok Yüksek Tehdit Durumları
            if (speedLevel == "Çok Hızlı" && ecmCapabilityLevel == "Çok Yüksek" && radarCrossSectionLevel == "Çok Düşük" && distanceLevel == "Çok Yakın" && maneuverabilityLevel == "Çok Yüksek" && altitudeLevel == "Alçak" && costLevel == "Yüksek")
                threatLevel = Math.Max(threatLevel, 1.0);

            if (speedLevel == "Çok Hızlı" && ecmCapabilityLevel == "Yüksek" && radarCrossSectionLevel == "Düşük" && distanceLevel == "Yakın" && maneuverabilityLevel == "Yüksek" && altitudeLevel == "Alçak" && costLevel == "Yüksek")
                threatLevel = Math.Max(threatLevel, 0.95);

            // Yüksek Tehdit Durumları
            if (speedLevel == "Hızlı" && ecmCapabilityLevel == "Yüksek" && radarCrossSectionLevel == "Orta" && distanceLevel == "Orta" && maneuverabilityLevel == "Yüksek" && altitudeLevel == "Alçak")
                threatLevel = Math.Max(threatLevel, 0.9);

            if (speedLevel == "Hızlı" && ecmCapabilityLevel == "Yüksek" && radarCrossSectionLevel == "Düşük" && distanceLevel == "Yakın" && maneuverabilityLevel == "Orta" && altitudeLevel == "Alçak" && costLevel == "Orta")
                threatLevel = Math.Max(threatLevel, 0.85);

            // Orta-Yüksek Tehdit Durumları
            if (speedLevel == "Orta" && ecmCapabilityLevel == "Orta" && radarCrossSectionLevel == "Orta" && distanceLevel == "Orta" && maneuverabilityLevel == "Orta" && altitudeLevel == "Orta")
                threatLevel = Math.Max(threatLevel, 0.75);

            if ((speedLevel == "Hızlı" || speedLevel == "Orta") && ecmCapabilityLevel == "Yüksek" && radarCrossSectionLevel == "Düşük" && distanceLevel == "Orta" && maneuverabilityLevel == "Orta" && altitudeLevel == "Orta" && costLevel == "Yüksek")
                threatLevel = Math.Max(threatLevel, 0.7);

            // Orta Tehdit Durumları
            if (speedLevel == "Orta" && ecmCapabilityLevel == "Orta" && radarCrossSectionLevel == "Yüksek" && distanceLevel == "Orta" && maneuverabilityLevel == "Düşük" && altitudeLevel == "Yüksek")
                threatLevel = Math.Max(threatLevel, 0.6);

            if (speedLevel == "Yavaş" && ecmCapabilityLevel == "Düşük" && radarCrossSectionLevel == "Orta" && distanceLevel == "Orta" && maneuverabilityLevel == "Düşük" && altitudeLevel == "Orta" && costLevel == "Orta")
                threatLevel = Math.Max(threatLevel, 0.55);

            // Düşük-Orta Tehdit Durumları
            if (speedLevel == "Yavaş" && ecmCapabilityLevel == "Düşük" && radarCrossSectionLevel == "Yüksek" && distanceLevel == "Uzak" && maneuverabilityLevel == "Düşük" && altitudeLevel == "Yüksek")
                threatLevel = Math.Max(threatLevel, 0.4);

            if (speedLevel == "Çok Yavaş" && ecmCapabilityLevel == "Çok Düşük" && radarCrossSectionLevel == "Çok Yüksek" && distanceLevel == "Çok Uzak" && maneuverabilityLevel == "Düşük" && altitudeLevel == "Yüksek" && costLevel == "Düşük")
                threatLevel = Math.Max(threatLevel, 0.35);

            // Düşük Tehdit Durumları
            if (speedLevel == "Çok Yavaş" && ecmCapabilityLevel == "Düşük" && radarCrossSectionLevel == "Yüksek" && distanceLevel == "Uzak" && maneuverabilityLevel == "Çok Düşük" && altitudeLevel == "Yüksek")
                threatLevel = Math.Max(threatLevel, 0.2);

            if (speedLevel == "Çok Yavaş" && ecmCapabilityLevel == "Çok Düşük" && radarCrossSectionLevel == "Çok Yüksek" && distanceLevel == "Çok Uzak" && maneuverabilityLevel == "Çok Düşük" && altitudeLevel == "Yüksek" && costLevel == "Çok Düşük")
                threatLevel = Math.Max(threatLevel, 0.1);

            // Varsayılan düşük tehdit seviyesi
            threatLevel = Math.Max(threatLevel, 0.05);

            return threatLevel;
        }

        // Parametreyi 5 bulanık seviyeye ayıran yardımcı fonksiyon
        private string GetFuzzyLevel(double value)
        {
            if (value >= 0 && value < 0.2)
                return "Çok Düşük"; // 0 - 0.2 aralığı
            else if (value >= 0.2 && value < 0.4)
                return "Düşük"; // 0.2 - 0.4 aralığı
            else if (value >= 0.4 && value < 0.6)
                return "Orta"; // 0.4 - 0.6 aralığı
            else if (value >= 0.6 && value < 0.8)
                return "Yüksek"; // 0.6 - 0.8 aralığı
            else
                return "Çok Yüksek"; // 0.8 - 1 aralığı
        }

        // Kesinleştirme işlemi
        public double Defuzzify(double threatLevel)
        {
            return Math.Min(1, Math.Max(0, threatLevel));
        }
    }
}