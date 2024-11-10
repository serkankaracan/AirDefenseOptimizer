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
            double veryHigh = FuzzyLogicHelper.TriangularMembership(speed, 1000, 1300, 1500);

            return Math.Max(Math.Max(Math.Max(Math.Max(veryLow, low), medium), high), veryHigh);
        }

        // Radar görünürlüğü için üçgen bulanık kümeler (Düşük, Orta, Yüksek RCS eklendi)
        public double FuzzifyRadarCrossSection(double rcs)
        {
            double veryLow = FuzzyLogicHelper.TriangularMembership(rcs, 0, 0.5, 1);
            double low = FuzzyLogicHelper.TriangularMembership(rcs, 0.5, 2, 3);
            double medium = FuzzyLogicHelper.TriangularMembership(rcs, 2, 5, 7);
            double high = FuzzyLogicHelper.TriangularMembership(rcs, 5, 8, 10);
            double veryHigh = FuzzyLogicHelper.TriangularMembership(rcs, 8, 10, 12);

            return Math.Max(Math.Max(Math.Max(Math.Max(veryLow, low), medium), high), veryHigh);
        }

        // ECM yeteneği için üçgen bulanık kümeler (Düşük, Orta, Yüksek ECM kategorileri)
        public double FuzzifyECM(ECMCapability ecmCapability)
        {
            double low = FuzzyLogicHelper.TriangularMembership(ecmCapability.GetECMCapabilityNumber(), 0, 1, 2);
            double medium = FuzzyLogicHelper.TriangularMembership(ecmCapability.GetECMCapabilityNumber(), 1, 2, 3);
            double high = FuzzyLogicHelper.TriangularMembership(ecmCapability.GetECMCapabilityNumber(), 2, 3, 4);

            return Math.Max(Math.Max(low, medium), high);
        }

        // Mesafe için üçgen bulanık kümeler (Çok Yakın, Yakın, Orta, Uzak, Çok Uzak mesafe kategorileri)
        public double FuzzifyDistance(double distance)
        {
            double veryClose = FuzzyLogicHelper.TriangularMembership(distance, 0, 25, 50);
            double close = FuzzyLogicHelper.TriangularMembership(distance, 30, 100, 200);
            double medium = FuzzyLogicHelper.TriangularMembership(distance, 150, 300, 500);
            double far = FuzzyLogicHelper.TriangularMembership(distance, 400, 600, 800);
            double veryFar = FuzzyLogicHelper.TriangularMembership(distance, 700, 1000, 1500);

            return Math.Max(Math.Max(Math.Max(Math.Max(veryClose, close), medium), far), veryFar);
        }

        // Hava aracı için bulanık kurallar
        public double ApplyFuzzyRules(double speed, double radarCrossSection, double ecmCapability, double distance)
        {
            double threatLevel = 0;

            // Çok Yüksek Tehdit Durumları
            if (speed > 0.8 && ecmCapability > 0.8 && (radarCrossSection < 0.3 || distance < 0.2))
                threatLevel = Math.Max(threatLevel, 0.95); // Çok yüksek tehdit

            if (speed > 0.7 && ecmCapability > 0.7 && radarCrossSection < 0.3 && distance < 0.5)
                threatLevel = Math.Max(threatLevel, 0.85); // Yüksek tehdit

            if ((speed > 0.7 || ecmCapability > 0.7) && radarCrossSection < 0.5 && distance < 0.3)
                threatLevel = Math.Max(threatLevel, 0.8); // Orta-yüksek tehdit

            // Orta Tehdit Durumları
            if (speed > 0.5 && ecmCapability > 0.5 && radarCrossSection > 0.3 && distance >= 0.3 && distance < 0.7)
                threatLevel = Math.Max(threatLevel, 0.6); // Orta tehdit

            if (speed > 0.3 && radarCrossSection > 0.3 && ecmCapability > 0.3 && distance > 0.5)
                threatLevel = Math.Max(threatLevel, 0.5); // Düşük-orta tehdit

            // Düşük Tehdit Durumları
            if (speed < 0.3 && radarCrossSection > 0.7 && ecmCapability <= 0.3 && distance > 0.7)
                threatLevel = Math.Max(threatLevel, 0.2); // Düşük tehdit

            // Varsayılan düşük tehdit seviyesi
            threatLevel = Math.Max(threatLevel, 0.1);

            return threatLevel;
        }

        // Kesinleştirme işlemi
        public double Defuzzify(double threatLevel)
        {
            return Math.Min(1, Math.Max(0, threatLevel));
        }
    }
}
