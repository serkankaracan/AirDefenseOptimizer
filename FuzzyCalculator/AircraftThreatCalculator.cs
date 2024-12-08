using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Models;
using AirDefenseOptimizer.Services;
using System.Windows;

namespace AirDefenseOptimizer.FuzzyCalculator
{
    public class AircraftThreatCalculator
    {
        private readonly FuzzyAircraftThreatCalculator _fuzzyCalculator = new FuzzyAircraftThreatCalculator();
        private readonly FuzzyMunitionThreatCalculator _munitionCalculator = new FuzzyMunitionThreatCalculator();

        private readonly MunitionService _munitionService;

        public AircraftThreatCalculator(MunitionService munitionService)
        {
            _munitionService = munitionService;
        }

        public double CalculateThreatLevel(Aircraft aircraft, IFF iffStatus, double distanceToTarget, double speed, Maneuverability maneuverability, double altitude, double cost)
        {
            if (iffStatus == IFF.Friend || iffStatus == IFF.Neutral)
                return 0;

            double radarCrossSectionFuzzy = _fuzzyCalculator.FuzzifyRadarCrossSection(aircraft.RadarCrossSection);
            double ecmCapabilityFuzzy = _fuzzyCalculator.FuzzifyECM(aircraft.ECMCapability);

            // Eğer mesafe bulanıklaştırma yakınlık için uygunsa, olduğu gibi kullan
            double distanceFuzzy = _fuzzyCalculator.FuzzifyDistance(distanceToTarget); // "yakınlık" derecesi olmalı

            double speedFuzzy = _fuzzyCalculator.FuzzifySpeed(speed);
            double maneuverabilityFuzzy = _fuzzyCalculator.FuzzyfyManeuverability(maneuverability);
            double altitudeFuzzy = _fuzzyCalculator.FuzzyfyAltitude(altitude);
            double costFuzzy = _fuzzyCalculator.FuzzyfyCost(cost);

            //string message = $"Radar Cross Section Fuzzy: {radarCrossSectionFuzzy}\n" +
            //     $"ECM Capability Fuzzy: {ecmCapabilityFuzzy}\n" +
            //     $"Distance Fuzzy: {distanceFuzzy}\n" +
            //     $"Speed Fuzzy: {speedFuzzy}\n" +
            //     $"Maneuverability Fuzzy: {maneuverabilityFuzzy}\n" +
            //     $"Altitude Fuzzy: {altitudeFuzzy}\n" +
            //     $"Cost Fuzzy: {costFuzzy}";

            //MessageBox.Show(message, "Fuzzy Values", MessageBoxButton.OK, MessageBoxImage.Information);


            double munitionThreatContribution = CalculateMunitionThreatContribution(aircraft.Munitions);

            //MessageBox.Show($"munitionThreatContribution: {munitionThreatContribution}");

            double aircraftThreatLevel = _fuzzyCalculator.ApplyFuzzyRules(speedFuzzy, radarCrossSectionFuzzy, ecmCapabilityFuzzy, distanceFuzzy, maneuverabilityFuzzy, altitudeFuzzy, costFuzzy);

            // Normalize edilmiş toplam tehdit seviyesini hesapla
            double totalThreatLevel = (0.8 * aircraftThreatLevel) + (0.2 * munitionThreatContribution);

            MessageBox.Show($"aircraftThreatLevel: {aircraftThreatLevel}\ntotalThreatLevel: {totalThreatLevel}");

            //return _fuzzyCalculator.Defuzzify(totalThreatLevel);
            return totalThreatLevel;
        }


        double CalculateMunitionThreatContribution(List<AircraftMunition> munitions)
        {
            double totalMunitionThreat = 0;

            // MunitionService'ten tüm mühimmat verilerini alın
            var allMunitions = _munitionService.GetAllMunitions();

            // Minimum ve maksimum değerleri dinamik olarak hesaplayın
            double minExplosivePower = allMunitions.Min(m => Convert.ToDouble(m["ExplosivePower"]));
            double maxExplosivePower = allMunitions.Max(m => Convert.ToDouble(m["ExplosivePower"]));

            double minRange = allMunitions.Min(m => Convert.ToDouble(m["Range"]));
            double maxRange = allMunitions.Max(m => Convert.ToDouble(m["Range"]));

            double minSpeed = allMunitions.Min(m => Convert.ToDouble(m["Speed"]));
            double maxSpeed = allMunitions.Max(m => Convert.ToDouble(m["Speed"]));

            double minManeuverability = allMunitions.Min(m => (int)Enum.Parse(typeof(Maneuverability), m["Maneuverability"].ToString()));
            double maxManeuverability = allMunitions.Max(m => (int)Enum.Parse(typeof(Maneuverability), m["Maneuverability"].ToString()));

            // Toplam tehdit için normalize edilecek birikim
            double maxPossibleThreat = 0;

            foreach (var munition in munitions)
            {
                //MessageBox.Show($"munitionNAme: {munition.Munition.Name}");
                // Her özellik için normalize değerleri hesapla
                double explosivePower = Normalize(munition.Munition.ExplosivePower, minExplosivePower, maxExplosivePower);
                double range = Normalize(munition.Munition.Range, minRange, maxRange);
                double speed = Normalize(munition.Munition.Speed, minSpeed, maxSpeed);
                double maneuverability = Normalize(munition.Munition.Maneuverability.GetManeuverabilityNumber(), minManeuverability, maxManeuverability);

                // Ağırlıklar
                double weightExplosivePower = 0.4;
                double weightRange = 0.2;
                double weightSpeed = 0.2;
                double weightManeuverability = 0.2;

                // Normalize edilmiş tehdit katkısı
                double munitionThreat = (weightExplosivePower * explosivePower) +
                                        (weightRange * range) +
                                        (weightSpeed * speed) +
                                        (weightManeuverability * maneuverability);

                // Mühimmat katkısını toplam tehdite ekle
                totalMunitionThreat += munitionThreat * munition.Quantity / munitions.Count;

                // Maksimum olası tehdit skorunu hesapla (tüm özellikler maks. değer)
                double maxThreat = (weightExplosivePower * 1) +
                                   (weightRange * 1) +
                                   (weightSpeed * 1) +
                                   (weightManeuverability * 1);
                maxPossibleThreat += maxThreat * munition.Quantity / munitions.Count;
            }

            // Toplam tehdit skorunu normalize et
            return Normalize(totalMunitionThreat, 0, maxPossibleThreat);
        }

        // Normalize fonksiyonu
        private double Normalize(double value, double min, double max)
        {
            if (max - min == 0)
                return 0; // Bölme hatalarını önlemek için
            return (value - min) / (max - min);
        }

    }

}
