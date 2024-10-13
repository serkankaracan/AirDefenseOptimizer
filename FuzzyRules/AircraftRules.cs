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

            // Tüm olası kombinasyonları döngülerle oluştur
            foreach (var speed in Enum.GetValues(typeof(EnumAircraft.Speed)).Cast<EnumAircraft.Speed>())
            {
                foreach (var range in Enum.GetValues(typeof(EnumAircraft.Range)).Cast<EnumAircraft.Range>())
                {
                    foreach (var maxAltitude in Enum.GetValues(typeof(EnumAircraft.MaxAltitude)).Cast<EnumAircraft.MaxAltitude>())
                    {
                        foreach (var maneuverability in Enum.GetValues(typeof(EnumAircraft.Maneuverability)).Cast<EnumAircraft.Maneuverability>())
                        {
                            foreach (var payloadCapacity in Enum.GetValues(typeof(EnumAircraft.PayloadCapacity)).Cast<EnumAircraft.PayloadCapacity>())
                            {
                                foreach (var cost in Enum.GetValues(typeof(EnumAircraft.Cost)).Cast<EnumAircraft.Cost>())
                                {
                                    // Yeni bir kural oluştur
                                    var rule = new FuzzyRule();

                                    // Koşulları ekle
                                    rule.AddCondition("Speed", speed.ToString());
                                    rule.AddCondition("Range", range.ToString());
                                    rule.AddCondition("MaxAltitude", maxAltitude.ToString());
                                    rule.AddCondition("Maneuverability", maneuverability.ToString());
                                    rule.AddCondition("PayloadCapacity", payloadCapacity.ToString());
                                    rule.AddCondition("Cost", cost.ToString());

                                    // Sonuç olarak EngagementScore belirle
                                    rule.AddConsequence("EngagementScore", CalculateEngagementScoreForAircraft(new Aircraft
                                    {
                                        Speed = (double)speed,
                                        Range = (double)range,
                                        MaxAltitude = (double)maxAltitude,
                                        Maneuverability = (Maneuverability)maneuverability,
                                        PayloadCapacity = (double)payloadCapacity,
                                        Cost = (double)cost,
                                    }));

                                    // Kurala ekle
                                    Rules.Add(rule);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Uçağın ve mühimmatlarının birleşik etkisiyle angaje skorunu hesaplar.
        /// </summary>
        private string CalculateEngagementScoreForAircraft(Aircraft aircraft)
        {
            // Uçağın angaje skorunu hesapla
            int aircraftScore = CalculateAircraftScore(aircraft);

            // Uçak ve mühimmat skorlarını birleştir (mühimmat listesini burada almanız gerekiyor)
            int munitionScore = CalculateTotalMunitionScore(aircraft.Munitions);

            // Uçak ve mühimmat skorlarını birleştir
            int totalScore = aircraftScore + munitionScore;

            // Toplam skora göre EngagementScore belirlenir
            if (totalScore >= 20)
            {
                return "High";
            }
            else if (totalScore >= 12)
            {
                return "Medium";
            }
            else
            {
                return "Low";
            }
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
            switch (aircraft.Maneuverability)
            {
                case Maneuverability.High:
                    score += 3;
                    break;
                case Maneuverability.Medium:
                    score += 2;
                    break;
                case Maneuverability.Low:
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
        /// Uçağın taşıdığı mühimmatların toplam angaje skorunu hesaplar.
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
        /// Tek bir mühimmatın angaje skorunu hesaplar.
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

            return score;
        }
    }
}
