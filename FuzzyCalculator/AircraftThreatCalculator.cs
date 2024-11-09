using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Models;

namespace AirDefenseOptimizer.FuzzyCalculator
{
    public class AircraftThreatCalculator
    {
        private readonly FuzzyAircraftThreatCalculator _fuzzyCalculator = new FuzzyAircraftThreatCalculator();
        private readonly FuzzyMunitionThreatCalculator _munitionCalculator = new FuzzyMunitionThreatCalculator();

        public double CalculateThreatLevel(Aircraft aircraft, IFF iffStatus, double distanceToTarget, double speed)
        {
            if (iffStatus == IFF.Friend || iffStatus == IFF.Neutral)
                return 0;

            double radarCrossSectionFuzzy = _fuzzyCalculator.FuzzifyRadarCrossSection(aircraft.RadarCrossSection);
            double ecmCapabilityFuzzy = _fuzzyCalculator.FuzzifyECM(aircraft.ECMCapability);
            double distanceFuzzy = _fuzzyCalculator.FuzzifyDistance(distanceToTarget);
            double speedFuzzy = _fuzzyCalculator.FuzzifySpeed(speed);

            double munitionThreatContribution = CalculateMunitionThreatContribution(aircraft.Munitions);

            double aircraftThreatLevel = _fuzzyCalculator.ApplyFuzzyRules(speedFuzzy, radarCrossSectionFuzzy, ecmCapabilityFuzzy, distanceFuzzy);

            double totalThreatLevel = aircraftThreatLevel + munitionThreatContribution;

            return _fuzzyCalculator.Defuzzify(totalThreatLevel);
            //return Math.Min(1, Math.Max(0, totalThreatLevel / 100));
            //return Math.Min(1, Math.Max(0, _fuzzyCalculator.Defuzzify(totalThreatLevel) / 100));
        }

        private double CalculateMunitionThreatContribution(List<AircraftMunition> munitions)
        {
            double totalMunitionThreat = 0;

            foreach (var aircraftMunition in munitions)
            {
                double explosivePowerFuzzy = _munitionCalculator.FuzzifyExplosivePower(aircraftMunition.Munition.ExplosivePower);
                double rangeFuzzy = _munitionCalculator.FuzzifyRange(aircraftMunition.Munition.Range);
                double speedFuzzy = _munitionCalculator.FuzzifySpeed(aircraftMunition.Munition.Speed);
                double mFuzzy = _munitionCalculator.FuzzifySpeed(aircraftMunition.Munition.Speed);

                double munitionThreat = 0;
                if (explosivePowerFuzzy > 0.7 && rangeFuzzy > 0.7)
                    munitionThreat = 0.8;

                if (speedFuzzy > 0.5 && rangeFuzzy > 0.5)
                    munitionThreat = Math.Max(munitionThreat, 0.6);

                totalMunitionThreat += munitionThreat * aircraftMunition.Quantity;
            }

            return totalMunitionThreat;
        }
    }

}
