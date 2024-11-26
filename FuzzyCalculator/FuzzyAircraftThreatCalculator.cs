using System.Windows;
using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.FuzzyEnums;

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
            double veryLow = FuzzyLogicHelper.TriangularMembership(rcs, 0, 0.5, 1);
            double low = FuzzyLogicHelper.TriangularMembership(rcs, 0.5, 2, 3);
            double medium = FuzzyLogicHelper.TriangularMembership(rcs, 2, 5, 7);
            double high = FuzzyLogicHelper.TriangularMembership(rcs, 5, 8, 10);
            double veryHigh = FuzzyLogicHelper.TriangularMembership(rcs, 8, 10, 12);

            double numerator = veryLow * 0.9 + low * 0.7 + medium * 0.5 + high * 0.3 + veryHigh * 0.1;
            double denominator = veryLow + low + medium + high + veryHigh;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (rcs >= 12)
                result = 0;
            if (rcs <= 0.1f)
                result = 1;

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
            double veryCheap = FuzzyLogicHelper.TriangularMembership(cost, 500000, 1000000, 2000000);
            double cheap = FuzzyLogicHelper.TriangularMembership(cost, 1000000, 3000000, 5000000);
            double normal = FuzzyLogicHelper.TriangularMembership(cost, 4000000, 10000000, 15000000);
            double expensive = FuzzyLogicHelper.TriangularMembership(cost, 10000000, 20000000, 30000000);
            double veryExpensive = FuzzyLogicHelper.TrapezoidalMembership(cost, 25000000, 40000000, 50000000, double.MaxValue);

            double numerator = veryCheap * 0.1 + cheap * 0.3 + normal * 0.5 + expensive * 0.7 + veryExpensive * 0.9;
            double denominator = veryCheap + cheap + normal + expensive + veryExpensive;
            double result = denominator != 0 ? numerator / denominator : 0;

            if (cost >= 50000000)
                result = 1;

            return result;
        }

        // Tüm olası kombinasyonları ve tehdit seviyelerini üreten fonksiyon
        //public List<(EnumAircraft.Speed, EnumAircraft.RadarCrossSection, EnumAircraft.EcmCapability, EnumAircraft.Range, EnumAircraft.Maneuverability, EnumAircraft.MaxAltitude, EnumAircraft.Cost, double)> GenerateThreatCombinations()
        //{
        //    var combinations = new List<(EnumAircraft.Speed, EnumAircraft.RadarCrossSection, EnumAircraft.EcmCapability, EnumAircraft.Range, EnumAircraft.Maneuverability, EnumAircraft.MaxAltitude, EnumAircraft.Cost, double)>();

        //    foreach (EnumAircraft.Speed speed in Enum.GetValues(typeof(EnumAircraft.Speed)))
        //    {
        //        foreach (EnumAircraft.RadarCrossSection rcs in Enum.GetValues(typeof(EnumAircraft.RadarCrossSection)))
        //        {
        //            foreach (EnumAircraft.EcmCapability ecm in Enum.GetValues(typeof(EnumAircraft.EcmCapability)))
        //            {
        //                foreach (EnumAircraft.Range distance in Enum.GetValues(typeof(EnumAircraft.Range)))
        //                {
        //                    foreach (EnumAircraft.Maneuverability maneuverability in Enum.GetValues(typeof(EnumAircraft.Maneuverability)))
        //                    {
        //                        foreach (EnumAircraft.MaxAltitude altitude in Enum.GetValues(typeof(EnumAircraft.MaxAltitude)))
        //                        {
        //                            foreach (EnumAircraft.Cost cost in Enum.GetValues(typeof(EnumAircraft.Cost)))
        //                            {
        //                                // Tehdit seviyesini hesaplıyoruz
        //                                //double threatLevel =
        //                                //    speedWeights[speed] * 0.2 +
        //                                //    ecmWeights[ecm] * 0.2 +
        //                                //    rcsWeights[rcs] * 0.15 +
        //                                //    distanceWeights[distance] * 0.15 +
        //                                //    maneuverabilityWeights[maneuverability] * 0.1 +
        //                                //    altitudeWeights[altitude] * 0.1 +
        //                                //    costWeights[cost] * 0.1;

        //                                //threatLevel = Math.Max(0.0, Math.Min(1.0, threatLevel));

        //                                // Kombinasyonu listeye ekliyoruz
        //                                combinations.Add((speed, rcs, ecm, distance, maneuverability, altitude, cost, 0));
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return combinations;
        //}

        // Hava aracı için bulanık kurallar

        public double ApplyFuzzyRules(double speed, double radarCrossSection, double ecmCapability, double distance, double maneuverability, double altitude, double cost)
        {
            double threatLevel = 0;

            EnumAircraft.Speed speedLevel = GetFuzzyLevel<EnumAircraft.Speed>(speed);
            EnumAircraft.RadarCrossSection radarCrossSectionLevel = GetFuzzyLevel<EnumAircraft.RadarCrossSection>(radarCrossSection);
            EnumAircraft.EcmCapability ecmCapabilityLevel = GetFuzzyLevel<EnumAircraft.EcmCapability>(ecmCapability);
            EnumAircraft.Range distanceLevel = GetFuzzyLevel<EnumAircraft.Range>(distance);
            EnumAircraft.Maneuverability maneuverabilityLevel = GetFuzzyLevel<EnumAircraft.Maneuverability>(maneuverability);
            EnumAircraft.MaxAltitude altitudeLevel = GetFuzzyLevel<EnumAircraft.MaxAltitude>(altitude);
            EnumAircraft.Cost costLevel = GetFuzzyLevel<EnumAircraft.Cost>(cost);

            double fatihkombinasyonu_aircraft = (speed + radarCrossSection + ecmCapability + distance + maneuverability + altitude + cost) / 7;
            //double fatihkombinasyonu_mauniiton= 

            MessageBox.Show("fatihkombinasyonu_aircraft: " + fatihkombinasyonu_aircraft);

            MessageBox.Show(
                $"Speed Level: {speedLevel} = {speed}\n" +
                $"radarCrossSection: {radarCrossSectionLevel} = {radarCrossSection}\n" +
                $"ecmCapability: {ecmCapabilityLevel} = {ecmCapability}\n" +
                $"distance: {distanceLevel} = {distance}\n" +
                $"maneuverability: {maneuverabilityLevel} = {maneuverability}\n" +
                $"altitude: {altitudeLevel} = {altitude}\n" +
                $"cost: {costLevel} = {cost}\n" +
                $"Threat Level: {threatLevel} = {threatLevel}");


            /*
            // Çok Yüksek Tehdit Durumları
            if (speedLevel == EnumAircraft.Speed.VeryFast &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.VeryHigh &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.VeryLow &&
                distanceLevel == EnumAircraft.Range.VeryShort &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.VeryHigh &&
                altitudeLevel == EnumAircraft.MaxAltitude.VeryLow &&
                costLevel == EnumAircraft.Cost.Expensive)
            {
                threatLevel = Math.Max(threatLevel, 1.0);
            }

            if (speedLevel == EnumAircraft.Speed.VeryFast &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.High &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.Low &&
                distanceLevel == EnumAircraft.Range.Short &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.High &&
                altitudeLevel == EnumAircraft.MaxAltitude.VeryLow &&
                costLevel == EnumAircraft.Cost.Expensive)
            {
                threatLevel = Math.Max(threatLevel, 0.95);
            }

            if (speedLevel == EnumAircraft.Speed.VeryFast &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.VeryHigh &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.Low &&
                distanceLevel == EnumAircraft.Range.VeryShort &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.Medium &&
                altitudeLevel == EnumAircraft.MaxAltitude.Low &&
                costLevel == EnumAircraft.Cost.Expensive)
            {
                threatLevel = Math.Max(threatLevel, 0.92);
            }

            if (speedLevel == EnumAircraft.Speed.VeryFast &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.VeryHigh &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.VeryLow &&
                distanceLevel == EnumAircraft.Range.Short &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.High &&
                altitudeLevel == EnumAircraft.MaxAltitude.Low &&
                costLevel == EnumAircraft.Cost.Moderate)
            {
                threatLevel = Math.Max(threatLevel, 0.90);
            }

            // Yüksek Tehdit Durumları
            if (speedLevel == EnumAircraft.Speed.Fast &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.High &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.Medium &&
                distanceLevel == EnumAircraft.Range.Medium &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.High &&
                altitudeLevel == EnumAircraft.MaxAltitude.Low)
            {
                threatLevel = Math.Max(threatLevel, 0.9);
            }

            if (speedLevel == EnumAircraft.Speed.Fast &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.High &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.Low &&
                distanceLevel == EnumAircraft.Range.Short &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.Medium &&
                altitudeLevel == EnumAircraft.MaxAltitude.Low &&
                costLevel == EnumAircraft.Cost.Moderate)
            {
                threatLevel = Math.Max(threatLevel, 0.85);
            }

            if (speedLevel == EnumAircraft.Speed.Fast &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.Medium &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.Low &&
                distanceLevel == EnumAircraft.Range.Short &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.High &&
                altitudeLevel == EnumAircraft.MaxAltitude.Medium)
            {
                threatLevel = Math.Max(threatLevel, 0.82);
            }

            if (speedLevel == EnumAircraft.Speed.Fast &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.High &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.Medium &&
                distanceLevel == EnumAircraft.Range.Medium &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.Medium &&
                altitudeLevel == EnumAircraft.MaxAltitude.Medium)
            {
                threatLevel = Math.Max(threatLevel, 0.8);
            }

            // Orta-Yüksek Tehdit Durumları
            if (speedLevel == EnumAircraft.Speed.Medium &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.Medium &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.Medium &&
                distanceLevel == EnumAircraft.Range.Medium &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.Medium &&
                altitudeLevel == EnumAircraft.MaxAltitude.Medium)
            {
                threatLevel = Math.Max(threatLevel, 0.75);
            }

            if ((speedLevel == EnumAircraft.Speed.Fast || speedLevel == EnumAircraft.Speed.Medium) &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.High &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.Low &&
                distanceLevel == EnumAircraft.Range.Medium &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.Medium &&
                altitudeLevel == EnumAircraft.MaxAltitude.Medium &&
                costLevel == EnumAircraft.Cost.Expensive)
            {
                threatLevel = Math.Max(threatLevel, 0.7);
            }

            if (speedLevel == EnumAircraft.Speed.Medium &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.Medium &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.High &&
                distanceLevel == EnumAircraft.Range.Short &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.High &&
                altitudeLevel == EnumAircraft.MaxAltitude.Medium)
            {
                threatLevel = Math.Max(threatLevel, 0.68);
            }

            if (speedLevel == EnumAircraft.Speed.Medium &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.Medium &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.Medium &&
                distanceLevel == EnumAircraft.Range.Medium &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.Low &&
                altitudeLevel == EnumAircraft.MaxAltitude.High &&
                costLevel == EnumAircraft.Cost.Moderate)
            {
                threatLevel = Math.Max(threatLevel, 0.65);
            }

            // Orta Tehdit Durumları
            if (speedLevel == EnumAircraft.Speed.Medium &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.Medium &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.High &&
                distanceLevel == EnumAircraft.Range.Medium &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.Low &&
                altitudeLevel == EnumAircraft.MaxAltitude.High)
            {
                threatLevel = Math.Max(threatLevel, 0.6);
            }

            if (speedLevel == EnumAircraft.Speed.Slow &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.Low &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.Medium &&
                distanceLevel == EnumAircraft.Range.Medium &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.Low &&
                altitudeLevel == EnumAircraft.MaxAltitude.Medium &&
                costLevel == EnumAircraft.Cost.Moderate)
            {
                threatLevel = Math.Max(threatLevel, 0.55);
            }

            if (speedLevel == EnumAircraft.Speed.Medium &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.Low &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.High &&
                distanceLevel == EnumAircraft.Range.Long &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.Low &&
                altitudeLevel == EnumAircraft.MaxAltitude.High)
            {
                threatLevel = Math.Max(threatLevel, 0.52);
            }

            if (speedLevel == EnumAircraft.Speed.Slow &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.Low &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.High &&
                distanceLevel == EnumAircraft.Range.Short &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.Low &&
                altitudeLevel == EnumAircraft.MaxAltitude.Medium)
            {
                threatLevel = Math.Max(threatLevel, 0.5);
            }

            // Düşük-Orta Tehdit Durumları
            if (speedLevel == EnumAircraft.Speed.Slow &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.Low &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.High &&
                distanceLevel == EnumAircraft.Range.Long &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.Low &&
                altitudeLevel == EnumAircraft.MaxAltitude.High)
            {
                threatLevel = Math.Max(threatLevel, 0.4);
            }

            if (speedLevel == EnumAircraft.Speed.VerySlow &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.VeryLow &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.VeryHigh &&
                distanceLevel == EnumAircraft.Range.VeryLong &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.Low &&
                altitudeLevel == EnumAircraft.MaxAltitude.High &&
                costLevel == EnumAircraft.Cost.Cheap)
            {
                threatLevel = Math.Max(threatLevel, 0.35);
            }

            if (speedLevel == EnumAircraft.Speed.Slow &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.Low &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.Medium &&
                distanceLevel == EnumAircraft.Range.Medium &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.Low &&
                altitudeLevel == EnumAircraft.MaxAltitude.Low &&
                costLevel == EnumAircraft.Cost.Moderate)
            {
                threatLevel = Math.Max(threatLevel, 0.38);
            }

            if (speedLevel == EnumAircraft.Speed.VerySlow &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.VeryLow &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.VeryHigh &&
                distanceLevel == EnumAircraft.Range.VeryLong &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.Medium &&
                altitudeLevel == EnumAircraft.MaxAltitude.Medium &&
                costLevel == EnumAircraft.Cost.Cheap)
            {
                threatLevel = Math.Max(threatLevel, 0.36);
            }

            // Düşük Tehdit Durumları
            if (speedLevel == EnumAircraft.Speed.VerySlow &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.Low &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.High &&
                distanceLevel == EnumAircraft.Range.Long &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.VeryLow &&
                altitudeLevel == EnumAircraft.MaxAltitude.High)
            {
                threatLevel = Math.Max(threatLevel, 0.2);
            }

            if (speedLevel == EnumAircraft.Speed.VerySlow &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.VeryLow &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.VeryHigh &&
                distanceLevel == EnumAircraft.Range.VeryLong &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.VeryLow &&
                altitudeLevel == EnumAircraft.MaxAltitude.High &&
                costLevel == EnumAircraft.Cost.VeryCheap)
            {
                threatLevel = Math.Max(threatLevel, 0.1);
            }

            if (speedLevel == EnumAircraft.Speed.VerySlow &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.Low &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.High &&
                distanceLevel == EnumAircraft.Range.Medium &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.VeryLow &&
                altitudeLevel == EnumAircraft.MaxAltitude.Medium)
            {
                threatLevel = Math.Max(threatLevel, 0.15);
            }

            if (speedLevel == EnumAircraft.Speed.Slow &&
                ecmCapabilityLevel == EnumAircraft.EcmCapability.VeryLow &&
                radarCrossSectionLevel == EnumAircraft.RadarCrossSection.Medium &&
                distanceLevel == EnumAircraft.Range.Long &&
                maneuverabilityLevel == EnumAircraft.Maneuverability.Low &&
                altitudeLevel == EnumAircraft.MaxAltitude.Low)
            {
                threatLevel = Math.Max(threatLevel, 0.18);
            }
            */

            // Varsayılan düşük tehdit seviyesi
            threatLevel = Math.Max(fatihkombinasyonu_aircraft, 0.05);
            //threatLevel = Math.Max(threatLevel, 0.05);

            return threatLevel;
        }

        //    public (EnumAircraft.Speed, EnumAircraft.RadarCrossSection, EnumAircraft.EcmCapability, EnumAircraft.Range, EnumAircraft.Maneuverability, EnumAircraft.MaxAltitude, EnumAircraft.Cost, double) FindMatchingCombination(
        //double speed, double radarCrossSection, double ecmCapability, double distance, double maneuverability, double altitude, double cost)
        //    {
        //        EnumAircraft.Speed speedLevel = GetFuzzyLevel<EnumAircraft.Speed>(speed);
        //        EnumAircraft.RadarCrossSection rcsLevel = GetFuzzyLevel<EnumAircraft.RadarCrossSection>(radarCrossSection);
        //        EnumAircraft.EcmCapability ecmLevel = GetFuzzyLevel<EnumAircraft.EcmCapability>(ecmCapability);
        //        EnumAircraft.Range distanceLevel = GetFuzzyLevel<EnumAircraft.Range>(distance);
        //        EnumAircraft.Maneuverability maneuverabilityLevel = GetFuzzyLevel<EnumAircraft.Maneuverability>(maneuverability);
        //        EnumAircraft.MaxAltitude altitudeLevel = GetFuzzyLevel<EnumAircraft.MaxAltitude>(altitude);
        //        EnumAircraft.Cost costLevel = GetFuzzyLevel<EnumAircraft.Cost>(cost);

        //        // Kombinasyonlar listesini alıyoruz
        //        var combinations = GenerateThreatCombinations();

        //        // Eşleşen kombinasyonu arıyoruz
        //        foreach (var combo in combinations)
        //        {
        //            if (combo.Item1 == speedLevel &&
        //                combo.Item2 == rcsLevel &&
        //                combo.Item3 == ecmLevel &&
        //                combo.Item4 == distanceLevel &&
        //                combo.Item5 == maneuverabilityLevel &&
        //                combo.Item6 == altitudeLevel &&
        //                combo.Item7 == costLevel)
        //            {
        //                // Eşleşen kombinasyonu bulduk
        //                return combo;
        //            }
        //        }

        //        // Eğer eşleşme bulunamazsa, null döndürüyoruz veya uygun bir değer döndürüyoruz
        //        return (speedLevel, rcsLevel, ecmLevel, distanceLevel, maneuverabilityLevel, altitudeLevel, costLevel, 0.0);
        //    }

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
            return Math.Min(1, Math.Max(0, threatLevel));
        }
    }
}