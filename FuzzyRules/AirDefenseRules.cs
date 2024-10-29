using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.FuzzyEnums;
using AirDefenseOptimizer.FuzzyLogic;
using AirDefenseOptimizer.Models;

namespace AirDefenseOptimizer.FuzzyRules
{
    /// <summary>
    /// Hava Savunma Sistemleri (Air Defense) için fuzzy mantık kurallarını tanımlar.
    /// ECM kabiliyeti, menzil, füze ateşleme kapasitesi gibi faktörlere dayalı kurallar içerir.
    /// Ayrıca radarlar ve mühimmatlar da değerlendirir.
    /// </summary>
    public class AirDefenseRules
    {
        public List<FuzzyRule> Rules { get; set; }

        public AirDefenseRules()
        {
            Rules = new List<FuzzyRule>();

            // Tüm olası kombinasyonları döngülerle oluştur
            foreach (var range in Enum.GetValues(typeof(EnumAirDefense.Range)).Cast<EnumAirDefense.Range>())
            {
                foreach (var maxEngagements in Enum.GetValues(typeof(EnumAirDefense.MaxEngagements)).Cast<EnumAirDefense.MaxEngagements>())
                {
                    foreach (var maxMissilesFired in Enum.GetValues(typeof(EnumAirDefense.MaxMissilesFired)).Cast<EnumAirDefense.MaxMissilesFired>())
                    {
                        foreach (var ecmCapability in Enum.GetValues(typeof(EnumAirDefense.ECMCapability)).Cast<EnumAirDefense.ECMCapability>())
                        {
                            foreach (var cost in Enum.GetValues(typeof(EnumAirDefense.Cost)).Cast<EnumAirDefense.Cost>())
                            {
                                var rule = new FuzzyRule();

                                // Hava savunma sisteminin koşullarını ekle
                                rule.AddCondition("Range", range.ToString());
                                rule.AddCondition("MaxEngagements", maxEngagements.ToString());
                                rule.AddCondition("MaxMissilesFired", maxMissilesFired.ToString());
                                rule.AddCondition("ECMCapability", ecmCapability.ToString());
                                rule.AddCondition("Cost", cost.ToString());

                                // Hava savunma sisteminin savunma seviyesini ve toplam skorunu hesapla
                                //var (defenseLevel, totalScore) = CalculateDefenseScore(range, ecmCapability, maxMissilesFired, maxEngagements, cost);//, radar, munition);

                                var (defenseLevel, totalScore) = CalculateTotalDefenseScore(new AirDefense
                                {
                                    AerodynamicTargetRangeMin = (double)range,
                                    AerodynamicTargetRangeMax = (double)range,
                                    BallisticTargetRangeMin = (double)range,
                                    BallisticTargetRangeMax = (double)range,
                                    MaxEngagements = (int)maxEngagements,
                                    MaxMissilesFired = (int)maxMissilesFired,
                                    ECMCapability = (ECMCapability)ecmCapability,
                                    Cost = (double)cost,
                                });

                                // Sonuç olarak DefenseScore ve TotalScore belirle
                                rule.AddConsequence("DefenseScore", defenseLevel); // Savunma seviyesi
                                rule.AddConsequence("TotalScore", totalScore.ToString()); // Toplam skor

                                // Kurala ekle
                                Rules.Add(rule);
                            }
                        }
                    }
                }
            }
        }

        private (string defenseLevel, int totalScore) CalculateTotalDefenseScore(AirDefense airDefense)
        {
            int maxDefenseScore = CalculateMaxDefenseScore();

            int defenseScore = CalculateDefenseScore(airDefense);

            int maxMunitionScore = CalculateMaxMunitionScore(airDefense.Munitions);

            int maxRadarScore = CalculateMaxRadarScore(airDefense.Radars);

            int totalScore = defenseScore + CalculateTotalMunitionScore(airDefense.Munitions) + CalculateTotalRadarScore(airDefense.Radars);

            int maxTotalScore = maxDefenseScore + maxMunitionScore + maxRadarScore;

            double scorePercentage = (double)totalScore / maxTotalScore * 100;

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
        /// Maksimum savunma skoru hesaplanır.
        /// </summary>
        private int CalculateMaxDefenseScore()
        {
            return 3 * 5; // Range, ECMCapability, MaxMissilesFired, MaxEngagements, Cost için 3 puan varsayılır
        }

        /// <summary>
        /// Hava savunma sisteminin savunma skorunu hesaplar.
        /// </summary>
        public int CalculateDefenseScore(AirDefense airDefense)
        {
            int score = 0;

            // Range değerlendirmesi
            switch ((EnumAirDefense.Range)airDefense.AerodynamicTargetRangeMax)
            {
                case EnumAirDefense.Range.Long:
                    score += 3;
                    break;
                case EnumAirDefense.Range.Medium:
                    score += 2;
                    break;
                case EnumAirDefense.Range.Short:
                    score += 1;
                    break;
            }

            // ECM Capability değerlendirmesi
            switch ((EnumAirDefense.ECMCapability)airDefense.ECMCapability)
            {
                case EnumAirDefense.ECMCapability.Strong:
                    score += 3;
                    break;
                case EnumAirDefense.ECMCapability.Moderate:
                    score += 2;
                    break;
                case EnumAirDefense.ECMCapability.Weak:
                    score += 1;
                    break;
            }

            // MaxMissilesFired değerlendirmesi
            switch ((EnumAirDefense.MaxMissilesFired)airDefense.MaxMissilesFired)
            {
                case EnumAirDefense.MaxMissilesFired.High:
                    score += 3;
                    break;
                case EnumAirDefense.MaxMissilesFired.Medium:
                    score += 2;
                    break;
                case EnumAirDefense.MaxMissilesFired.Low:
                    score += 1;
                    break;
            }

            // MaxEngagements değerlendirmesi
            switch ((EnumAirDefense.MaxEngagements)airDefense.MaxEngagements)
            {
                case EnumAirDefense.MaxEngagements.Many:
                    score += 3;
                    break;
                case EnumAirDefense.MaxEngagements.Moderate:
                    score += 2;
                    break;
                case EnumAirDefense.MaxEngagements.Few:
                    score += 1;
                    break;
            }

            // Cost değerlendirmesi
            switch ((EnumAirDefense.Cost)airDefense.Cost)
            {
                case EnumAirDefense.Cost.Expensive:
                    score += 1;
                    break;
                case EnumAirDefense.Cost.Moderate:
                    score += 2;
                    break;
                case EnumAirDefense.Cost.Cheap:
                    score += 3;
                    break;
            }

            return score;
        }

        /// <summary>
        /// Radarların toplam skorunu hesaplar.
        /// </summary>
        private int CalculateTotalRadarScore(List<AirDefenseRadar> radars)
        {
            int totalRadarScore = 0;

            foreach (var radar in radars)
            {
                totalRadarScore += CalculateRadarScore(radar.Radar);
            }

            return totalRadarScore;
        }

        /// <summary>
        /// Maksimum radar skoru hesaplanır.
        /// </summary>
        private int CalculateMaxRadarScore(List<AirDefenseRadar> radars)
        {
            return radars.Count * 3 * 5; // 5 radar parametresi için 3 puan varsayılır
        }

        /// <summary>
        /// Tek bir radarın skorunu hesaplar.
        /// </summary>
        private int CalculateRadarScore(Radar radar)
        {
            int score = 0;

            // Detection Range değerlendirmesi
            switch ((EnumRadar.DetectionRange)radar.MaxDetectionRange)
            {
                case EnumRadar.DetectionRange.Long:
                    score += 3;
                    break;
                case EnumRadar.DetectionRange.Medium:
                    score += 2;
                    break;
                case EnumRadar.DetectionRange.Short:
                    score += 1;
                    break;
            }

            // Max Altitude değerlendirmesi
            switch ((EnumRadar.Altitude)radar.MaxAltitude)
            {
                case EnumRadar.Altitude.High:
                    score += 3;
                    break;
                case EnumRadar.Altitude.Medium:
                    score += 2;
                    break;
                case EnumRadar.Altitude.Low:
                    score += 1;
                    break;
            }

            // Max Target Speed değerlendirmesi
            switch ((EnumRadar.MaxTargetSpeed)radar.MaxTargetSpeed)
            {
                case EnumRadar.MaxTargetSpeed.High:
                    score += 3;
                    break;
                case EnumRadar.MaxTargetSpeed.Medium:
                    score += 2;
                    break;
                case EnumRadar.MaxTargetSpeed.Low:
                    score += 1;
                    break;
            }

            // Max Target Velocity değerlendirmesi
            switch ((EnumRadar.MaxTargetVelocity)radar.MaxTargetVelocity)
            {
                case EnumRadar.MaxTargetVelocity.High:
                    score += 3;
                    break;
                case EnumRadar.MaxTargetVelocity.Medium:
                    score += 2;
                    break;
                case EnumRadar.MaxTargetVelocity.Low:
                    score += 1;
                    break;
            }

            // Redeployment Time değerlendirmesi
            switch ((EnumRadar.RedeploymentTime)radar.RedeploymentTime)
            {
                case EnumRadar.RedeploymentTime.Short:
                    score += 3;
                    break;
                case EnumRadar.RedeploymentTime.Medium:
                    score += 2;
                    break;
                case EnumRadar.RedeploymentTime.Long:
                    score += 1;
                    break;
            }

            return score;
        }

        /// <summary>
        /// Mühimmatların toplam skorunu hesaplar.
        /// </summary>
        private int CalculateTotalMunitionScore(List<AirDefenseMunition> munitions)
        {
            int totalMunitionScore = 0;

            foreach (var munition in munitions)
            {
                totalMunitionScore += CalculateMunitionScore(munition.Munition);
            }

            return totalMunitionScore;
        }

        /// <summary>
        /// Maksimum mühimmat skoru hesaplanır.
        /// </summary>
        private int CalculateMaxMunitionScore(List<AirDefenseMunition> munitions)
        {
            return munitions.Count * 3 * 5; // Her mühimmat için 5 parametre ve 3 maksimum puan varsayılır
        }

        /// <summary>
        /// Tek bir mühimmatın skorunu hesaplar.
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

            // Cost değerlendirmesi
            switch ((EnumMunition.Cost)munition.Cost)
            {
                case EnumMunition.Cost.Expensive:
                    score += 1;
                    break;
                case EnumMunition.Cost.Moderate:
                    score += 2;
                    break;
                case EnumMunition.Cost.Cheap:
                    score += 3;
                    break;
            }

            return score;
        }

    }
}
