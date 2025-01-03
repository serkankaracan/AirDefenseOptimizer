using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.FuzzyEnums;
using AirDefenseOptimizer.FuzzyLogic;
using AirDefenseOptimizer.Models;

namespace AirDefenseOptimizer.FuzzyRules
{
    /// <summary>
    /// Uçaklar (Aircraft) için fuzzy mantık kurallarını tanımlar.
    /// Uçağın hızı, menzili, irtifası gibi değişkenlerin yanı sıra taşıdığı mühimmatları da değerlendirir.
    /// </summary>
    public class AircraftRules
    {
        public List<FuzzyRule> Rules { get; set; }

        public AircraftRules()
        {
            Rules = new List<FuzzyRule>();
            GenerateAllRules();
        }

        /// <summary>
        /// Tüm olası kuralları oluşturur ve Rules listesine ekler.
        /// </summary>
        private void GenerateAllRules()
        {
            foreach (var speed in Enum.GetValues(typeof(EnumAircraft.Speed)).Cast<EnumAircraft.Speed>())
            {
                foreach (var range in Enum.GetValues(typeof(EnumAircraft.Range)).Cast<EnumAircraft.Range>())
                {
                    foreach (var altitude in Enum.GetValues(typeof(EnumAircraft.MaxAltitude)).Cast<EnumAircraft.MaxAltitude>())
                    {
                        foreach (var maneuverability in Enum.GetValues(typeof(EnumAircraft.Maneuverability)).Cast<EnumAircraft.Maneuverability>())
                        {
                            foreach (var ecmCapability in Enum.GetValues(typeof(EnumAircraft.EcmCapability)).Cast<EnumAircraft.EcmCapability>())
                            {
                                foreach (var payloadCapacity in Enum.GetValues(typeof(EnumAircraft.PayloadCapacity)).Cast<EnumAircraft.PayloadCapacity>())
                                {
                                    foreach (var rcs in Enum.GetValues(typeof(EnumAircraft.RadarCrossSection)).Cast<EnumAircraft.RadarCrossSection>())
                                    {
                                        foreach (var cost in Enum.GetValues(typeof(EnumAircraft.Cost)).Cast<EnumAircraft.Cost>())
                                        {
                                            // Yeni bir kural oluştur ve koşulları ekle
                                            var rule = new FuzzyRule();
                                            rule.AddCondition("Speed", speed.ToString());
                                            rule.AddCondition("Range", range.ToString());
                                            rule.AddCondition("MaxAltitude", altitude.ToString());
                                            rule.AddCondition("Maneuverability", maneuverability.ToString());
                                            rule.AddCondition("ECMCapability", ecmCapability.ToString());
                                            rule.AddCondition("PayloadCapacity", payloadCapacity.ToString());
                                            rule.AddCondition("RadarCrossSection", rcs.ToString());
                                            rule.AddCondition("Cost", cost.ToString());

                                            var (threatLevel, threatScore) = CalculateThreatScore(new Aircraft
                                            {
                                                Speed = (double)speed,
                                                Range = (double)range,
                                                MaxAltitude = (double)altitude,
                                                Maneuverability = (Maneuverability)maneuverability,
                                                ECMCapability = (ECMCapability)ecmCapability,
                                                PayloadCapacity = (double)payloadCapacity,
                                                RadarCrossSection = (double)rcs,
                                                Cost = (double)cost,
                                            });

                                            //var threatLevel = DetermineThreatLevel(speed, altitude, maneuverability, ecmCapability, rcs, cost);

                                            rule.AddConsequence("ThreatLevel", threatLevel); // Tehdit seviyesi (Critical, High, vb.)
                                            rule.AddConsequence("ThreatScore", threatScore.ToString()); // Tehdit skoru

                                            // Kuralı Rules listesine ekle
                                            Rules.Add(rule);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Hız, irtifa, manevra kabiliyeti gibi değerlere göre tehdit seviyesini belirler.
        /// </summary>
        private string DetermineThreatLevel(
            EnumAircraft.Speed speed,
            EnumAircraft.Range range,
            EnumAircraft.MaxAltitude altitude,
            EnumAircraft.Maneuverability maneuverability,
            EnumAircraft.EcmCapability ecmCapability,
            EnumAircraft.RadarCrossSection rcs,
            EnumAircraft.PayloadCapacity payloadCapacity,
            EnumAircraft.Cost cost)
        {
            int score = 0;

            // Hız
            score += speed == EnumAircraft.Speed.Fast ? 3 : speed == EnumAircraft.Speed.Medium ? 2 : 1;

            // İrtifa
            score += altitude == EnumAircraft.MaxAltitude.High ? 3 : altitude == EnumAircraft.MaxAltitude.Medium ? 2 : 1;

            // Manevra Kabiliyeti
            score += maneuverability == EnumAircraft.Maneuverability.High ? 3 : maneuverability == EnumAircraft.Maneuverability.Medium ? 2 : 1;

            // ECM Kabiliyeti
            score += ecmCapability == EnumAircraft.EcmCapability.High ? 3 : ecmCapability == EnumAircraft.EcmCapability.Medium ? 2 : 1;

            // Radar Kesit Alanı (RCS)
            score += rcs == EnumAircraft.RadarCrossSection.High ? 3 : rcs == EnumAircraft.RadarCrossSection.Medium ? 2 : 1;

            // Maliyet
            score += cost == EnumAircraft.Cost.Expensive ? 3 : cost == EnumAircraft.Cost.Moderate ? 2 : 1;

            // Toplam skora göre tehdit seviyesi belirleme
            if (score >= 15)
                return "High";
            else if (score >= 10)
                return "Medium";
            else
                return "Low";
        }


        /// <summary>
        /// Uçağın ve mühimmatlarının birleşik etkisiyle tehdit skorunu hesaplar.
        /// </summary>
        public (string, int) CalculateThreatScore(Aircraft aircraft)
        {
            // Uçağın parametrelerine dayalı maksimum skor
            int maxAircraftScore = CalculateMaxAircraftScore();

            // Uçağın angaje skorunu hesapla
            int aircraftScore = CalculateAircraftScore(aircraft);

            // Mühimmatların parametrelerine dayalı maksimum skor
            int maxMunitionScore = CalculateMaxMunitionScore(aircraft.Munitions);

            // Uçak ve mühimmat skorlarını birleştir
            int totalScore = aircraftScore + CalculateTotalMunitionScore(aircraft.Munitions);

            // Maksimum skor
            int maxTotalScore = maxAircraftScore + maxMunitionScore;

            // Yüzdesel tehdit skoru hesapla
            double scorePercentage = (double)totalScore / maxTotalScore * 100;

            // Yüzdelik tehdit skoru aralıklarına göre sınıflandırma
            if (scorePercentage >= 80)
                return ("Critical", totalScore);
            else if (scorePercentage >= 60)
                return ("High", totalScore);
            else if (scorePercentage >= 40)
                return ("Medium", totalScore);
            else if (scorePercentage >= 20)
                return ("Low", totalScore);
            else
                return ("Very Low", totalScore);
        }

        /// <summary>
        /// Uçağın kendi parametrelerine göre angaje skorunu hesaplar.
        /// </summary>
        private int CalculateAircraftScore(Aircraft aircraft)
        {
            int score = 0;

            // Speed değerlendirmesi
            switch ((EnumAircraft.Speed)aircraft.Speed)
            {
                case EnumAircraft.Speed.Fast:
                    score += 3;
                    break;
                case EnumAircraft.Speed.Medium:
                    score += 2;
                    break;
                case EnumAircraft.Speed.Slow:
                    score += 1;
                    break;
            }

            // Range değerlendirmesi
            switch ((EnumAircraft.Range)aircraft.Range)
            {
                case EnumAircraft.Range.Long:
                    score += 3;
                    break;
                case EnumAircraft.Range.Medium:
                    score += 2;
                    break;
                case EnumAircraft.Range.Short:
                    score += 1;
                    break;
            }

            // Max Altitude değerlendirmesi
            switch ((EnumAircraft.MaxAltitude)aircraft.MaxAltitude)
            {
                case EnumAircraft.MaxAltitude.High:
                    score += 3;
                    break;
                case EnumAircraft.MaxAltitude.Medium:
                    score += 2;
                    break;
                case EnumAircraft.MaxAltitude.Low:
                    score += 1;
                    break;
            }

            // Maneuverability değerlendirmesi
            switch ((EnumAircraft.Maneuverability)aircraft.Maneuverability)
            {
                case EnumAircraft.Maneuverability.High:
                    score += 3;
                    break;
                case EnumAircraft.Maneuverability.Medium:
                    score += 2;
                    break;
                case EnumAircraft.Maneuverability.Low:
                    score += 1;
                    break;
            }

            // Payload Capacity değerlendirmesi
            switch ((EnumAircraft.PayloadCapacity)aircraft.PayloadCapacity)
            {
                case EnumAircraft.PayloadCapacity.Large:
                    score += 3;
                    break;
                case EnumAircraft.PayloadCapacity.Medium:
                    score += 2;
                    break;
                case EnumAircraft.PayloadCapacity.Small:
                    score += 1;
                    break;
            }

            switch ((EnumAircraft.EcmCapability)aircraft.ECMCapability)
            {
                case EnumAircraft.EcmCapability.High:
                    score += 3;
                    break;
                case EnumAircraft.EcmCapability.Medium:
                    score += 2;
                    break;
                case EnumAircraft.EcmCapability.Low:
                    score += 1;
                    break;
            }

            // RadarCrossSection değerlendirmesi
            switch ((EnumAircraft.RadarCrossSection)aircraft.RadarCrossSection)
            {
                case EnumAircraft.RadarCrossSection.Low:
                    score += 3;
                    break;
                case EnumAircraft.RadarCrossSection.Medium:
                    score += 2;
                    break;
                case EnumAircraft.RadarCrossSection.High:
                    score += 1;
                    break;
            }

            // Cost değerlendirmesi
            switch ((EnumAircraft.Cost)aircraft.Cost)
            {
                case EnumAircraft.Cost.Expensive:
                    score += 3;
                    break;
                case EnumAircraft.Cost.Moderate:
                    score += 2;
                    break;
                case EnumAircraft.Cost.Cheap:
                    score += 1;
                    break;
            }

            return score;
        }

        /// <summary>
        /// Uçağın taşıdığı mühimmatların toplam tehdit skorunu hesaplar.
        /// </summary>
        private int CalculateTotalMunitionScore(List<AircraftMunition> munitions)
        {
            int totalMunitionScore = 0;
            foreach (var munition in munitions)
            {
                totalMunitionScore += CalculateMunitionScore(munition.Munition);
            }

            return totalMunitionScore;
        }

        /// <summary>
        /// Tek bir mühimmatın tehdit skorunu hesaplar.
        /// </summary>
        private int CalculateMunitionScore(Munition munition)
        {
            int score = 0;

            // Weight değerlendirmesi
            switch ((EnumMunition.Weight)munition.Weight)
            {
                case EnumMunition.Weight.Heavy:
                    score += 3;
                    break;
                case EnumMunition.Weight.Medium:
                    score += 2;
                    break;
                case EnumMunition.Weight.Light:
                    score += 1;
                    break;
            }

            // Speed değerlendirmesi
            switch ((EnumMunition.Speed)munition.Speed)
            {
                case EnumMunition.Speed.Fast:
                    score += 3;
                    break;
                case EnumMunition.Speed.Medium:
                    score += 2;
                    break;
                case EnumMunition.Speed.Slow:
                    score += 1;
                    break;
            }

            // Range değerlendirmesi
            switch ((EnumMunition.Range)munition.Range)
            {
                case EnumMunition.Range.Long:
                    score += 3;
                    break;
                case EnumMunition.Range.Medium:
                    score += 2;
                    break;
                case EnumMunition.Range.Short:
                    score += 1;
                    break;
            }

            // Explosive Power değerlendirmesi
            switch ((EnumMunition.ExplosivePower)munition.ExplosivePower)
            {
                case EnumMunition.ExplosivePower.High:
                    score += 3;
                    break;
                case EnumMunition.ExplosivePower.Medium:
                    score += 2;
                    break;
                case EnumMunition.ExplosivePower.Low:
                    score += 1;
                    break;
            }

            switch ((EnumMunition.Cost)munition.Cost)
            {
                case EnumMunition.Cost.Cheap:
                    score += 1;
                    break;
                case EnumMunition.Cost.Moderate:
                    score += 2;
                    break;
                case EnumMunition.Cost.Expensive:
                    score += 3;
                    break;
                default:
                    break;
            }

            return score;
        }

        /// <summary>
        /// Maksimum uçak skoru hesaplanır.
        /// </summary>
        private int CalculateMaxAircraftScore()
        {
            // Maksimum her parametre için 3 puan olduğu varsayılır.
            return 3 * 8; // Speed, Range, MaxAltitude, Maneuverability, EcmCapability, PayloadCapacity, RadarCrossSection, Cost için
        }

        /// <summary>
        /// Maksimum mühimmat skoru hesaplanır.
        /// </summary>
        private int CalculateMaxMunitionScore(List<AircraftMunition> munitions)
        {
            // Maksimum her parametre için 3 puan olduğu varsayılır. Burada her mühimmatın 5 parametresi var.
            return munitions.Count * 3 * 5; // Weight, Speed, Range, ExplosivePower, Cost
        }
    }
}
