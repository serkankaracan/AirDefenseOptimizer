using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Models;
using System.Windows;

namespace AirDefenseOptimizer.FuzzyCalculator
{
    public class AircraftThreatCalculator
    {
        private readonly FuzzyAircraftThreatCalculator _fuzzyCalculator = new FuzzyAircraftThreatCalculator();
        private readonly FuzzyMunitionThreatCalculator _munitionCalculator = new FuzzyMunitionThreatCalculator();

        public double CalculateThreatLevel(Aircraft aircraft, IFF iffStatus, double distanceToTarget, double speed, Maneuverability maneuverability, double altitude, double cost)
        {
            if (iffStatus == IFF.Friend || iffStatus == IFF.Neutral)
                return 0;

            double radarCrossSectionFuzzy = _fuzzyCalculator.FuzzifyRadarCrossSection(aircraft.RadarCrossSection);
            double ecmCapabilityFuzzy = _fuzzyCalculator.FuzzifyECM(aircraft.ECMCapability);
            double distanceFuzzy = _fuzzyCalculator.FuzzifyDistance(distanceToTarget);
            double speedFuzzy = _fuzzyCalculator.FuzzifySpeed(speed);
            double maneuverabilityFuzzy = _fuzzyCalculator.FuzzyfyManeuverability(maneuverability);
            double altitudeFuzzy = _fuzzyCalculator.FuzzyfyAltitude(altitude);
            double costFuzzy = _fuzzyCalculator.FuzzyfyCost(cost);

            double munitionThreatContribution = CalculateMunitionThreatContribution(aircraft.Munitions);

            //MessageBox.Show("munitionThreatContribution: " + munitionThreatContribution + "\n" +
            //"Munitions: " + string.Join(", ", aircraft.Munitions.Select(m => m.Munition.Name.ToString())));

            MessageBox.Show("radarCrossSectionFuzzy: " + radarCrossSectionFuzzy + "\n" +
                "ecmCapabilityFuzzy: " + ecmCapabilityFuzzy + "\n" +
                "distanceFuzzy: " + distanceFuzzy + "\n" +
                "speedFuzzy: " + speedFuzzy + "\n" +
                "maneuverabilityFuzzy: " + maneuverabilityFuzzy + "\n" +
                "altitudeFuzzy: " + altitudeFuzzy + "\n" +
                "costFuzzy: " + costFuzzy);

            double aircraftThreatLevel = _fuzzyCalculator.ApplyFuzzyRules(speedFuzzy, radarCrossSectionFuzzy, ecmCapabilityFuzzy, distanceFuzzy, maneuverabilityFuzzy, altitudeFuzzy, costFuzzy);

            // Normalize edilmiş toplam tehdit seviyesini hesapla
            double totalThreatLevel = (0.75 * aircraftThreatLevel) + (0.25 * munitionThreatContribution);

            return _fuzzyCalculator.Defuzzify(totalThreatLevel);
        }

        private double CalculateMunitionThreatContribution(List<AircraftMunition> munitions)
        {
            double totalMunitionThreat = 0;

            foreach (var aircraftMunition in munitions)
            {
                // Fuzzified değerler
                double explosivePowerFuzzy = _munitionCalculator.FuzzifyExplosivePower(aircraftMunition.Munition.ExplosivePower);
                double rangeFuzzy = _munitionCalculator.FuzzifyRange(aircraftMunition.Munition.Range);
                double speedFuzzy = _munitionCalculator.FuzzifySpeed(aircraftMunition.Munition.Speed);
                double maneuverabilityFuzzy = _munitionCalculator.FuzzifyManeuverability(aircraftMunition.Munition.Maneuverability);

                //MessageBox.Show("explosivePowerFuzzy: " + explosivePowerFuzzy + "\n" +
                //                "rangeFuzzy: " + rangeFuzzy + "\n" +
                //                "speedFuzzy: " + speedFuzzy + "\n" +
                //                "maneuverabilityFuzzy: " + maneuverabilityFuzzy);

                double munitionThreat = 0;
                string appliedRule = string.Empty;

                // Tehdit katkısını belirle
                if (explosivePowerFuzzy > 0.7 && rangeFuzzy > 0.7)
                {
                    munitionThreat = 0.8;
                    appliedRule = "High explosive power and long range";
                }
                else if (speedFuzzy > 0.5 && rangeFuzzy > 0.5)
                {
                    munitionThreat = Math.Max(munitionThreat, 0.6);
                    appliedRule = "High speed and medium range";
                }
                else
                {
                    // Fuzzified değerlerin ortalaması ile düşük tehdit
                    munitionThreat = (explosivePowerFuzzy + rangeFuzzy + speedFuzzy + maneuverabilityFuzzy) / 4 * 0.5;
                    appliedRule = "Low average fuzzy values - Threat Level: " + munitionThreat;
                }

                //MessageBox.Show("Applied rule for " + aircraftMunition.Munition.Name + ": " + appliedRule);

                // Her mühimmatın tehdit düzeyine göre toplam katkısını hesapla
                totalMunitionThreat += munitionThreat * aircraftMunition.Quantity;
            }

            return totalMunitionThreat;
        }
    }

}
