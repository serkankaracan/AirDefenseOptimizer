using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Models;

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

            //MessageBox.Show("radarCrossSectionFuzzy: " + radarCrossSectionFuzzy + "\n" +
            //    "ecmCapabilityFuzzy: " + ecmCapabilityFuzzy + "\n" +
            //    "distanceFuzzy: " + distanceFuzzy + "\n" +
            //    "speedFuzzy: " + speedFuzzy + "\n" +
            //    "maneuverabilityFuzzy: " + maneuverabilityFuzzy + "\n" +
            //    "altitudeFuzzy: " + altitudeFuzzy + "\n" +
            //    "costFuzzy: " + costFuzzy);

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
                //double ecmFuzzy=_munitionCalculator.FuzzifyECM(aircraftMunition.Munition.ECMCapability)
                double quantityFuzzy = _munitionCalculator.FuzzifyQuantity(aircraftMunition.Quantity);
                double munitionThreat = 0;
                string appliedRule = string.Empty;

                double fatihkombinasyonu_munition = (explosivePowerFuzzy + rangeFuzzy + speedFuzzy + maneuverabilityFuzzy) / 4;

                /*
                // Yüksek tehdit durumları
                if (explosivePowerFuzzy > 0.8 && rangeFuzzy > 0.8 && speedFuzzy > 0.7)
                {
                    munitionThreat = Math.Max(munitionThreat, 0.9);
                    appliedRule = "Very high explosive power, long range, and high speed";
                }
                else if (explosivePowerFuzzy > 0.7 && rangeFuzzy > 0.7 && maneuverabilityFuzzy > 0.6)
                {
                    munitionThreat = Math.Max(munitionThreat, 0.85);
                    appliedRule = "High explosive power, long range, and good maneuverability";
                }

                // Orta-yüksek tehdit durumları
                else if ((explosivePowerFuzzy > 0.6 && rangeFuzzy > 0.6) || (speedFuzzy > 0.5 && maneuverabilityFuzzy > 0.5))
                {
                    munitionThreat = Math.Max(munitionThreat, 0.7);
                    appliedRule = "Moderate explosive power and range, or moderate speed and maneuverability";
                }
                else if (explosivePowerFuzzy > 0.5 && rangeFuzzy > 0.5 && speedFuzzy > 0.4)
                {
                    munitionThreat = Math.Max(munitionThreat, 0.65);
                    appliedRule = "Moderate explosive power, range, and speed";
                }

                // Orta tehdit durumları
                else if ((explosivePowerFuzzy > 0.4 && rangeFuzzy > 0.4) || (speedFuzzy > 0.4 && maneuverabilityFuzzy > 0.4))
                {
                    munitionThreat = Math.Max(munitionThreat, 0.5);
                    appliedRule = "Moderate threat due to balanced explosive power and maneuverability";
                }

                // Düşük tehdit durumları
                else if (explosivePowerFuzzy < 0.3 && rangeFuzzy < 0.3 && speedFuzzy < 0.3)
                {
                    munitionThreat = Math.Max(munitionThreat, 0.3);
                    appliedRule = "Low explosive power, range, and speed";
                }
                else
                {
                    // Ortalama değerler için düşük tehdit
                    munitionThreat = (explosivePowerFuzzy + rangeFuzzy + speedFuzzy + maneuverabilityFuzzy) / 4 * 0.5;
                    appliedRule = "Low-average fuzzy values - Threat Level: " + munitionThreat;
                }
                */

                // Her mühimmatın tehdit düzeyine göre toplam katkısını hesapla
                totalMunitionThreat += munitionThreat;// * aircraftMunition.Quantity;

                // Mesaj gösterme seçeneği: Her mühimmat için uygulanan kural
                //MessageBox.Show("Applied rule for " + aircraftMunition.Munition.Name + ": \n" + appliedRule + "\n" + "munitionThreat: " + munitionThreat);
            }
            //MessageBox.Show("totalMunitionThreat: " + totalMunitionThreat);
            return totalMunitionThreat;
        }
    }

}
