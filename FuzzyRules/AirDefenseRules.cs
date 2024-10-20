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
                foreach (var ecmCapability in Enum.GetValues(typeof(EnumAirDefense.ECMCapability)).Cast<EnumAirDefense.ECMCapability>())
                {
                    foreach (var maxMissilesFired in Enum.GetValues(typeof(EnumAirDefense.MaxMissilesFired)).Cast<EnumAirDefense.MaxMissilesFired>())
                    {
                        foreach (var maxEngagements in Enum.GetValues(typeof(EnumAirDefense.MaxEngagements)).Cast<EnumAirDefense.MaxEngagements>())
                        {
                            foreach (var cost in Enum.GetValues(typeof(EnumAirDefense.Cost)).Cast<EnumAirDefense.Cost>())
                            {
                                //foreach (var radar in GetRadarListFromEnum())
                                //{
                                //    foreach (var munition in GetMunitionListFromEnum())
                                //    {
                                        // Yeni bir kural oluştur
                                        var rule = new FuzzyRule();

                                        // Hava savunma sisteminin koşullarını ekle
                                        rule.AddCondition("Range", range.ToString());
                                        rule.AddCondition("ECMCapability", ecmCapability.ToString());
                                        rule.AddCondition("MaxMissilesFired", maxMissilesFired.ToString());
                                        rule.AddCondition("MaxEngagements", maxEngagements.ToString());
                                        rule.AddCondition("Cost", cost.ToString());

                                // Radar koşullarını eklerken cast işlemi uygulayın
                                //rule.AddCondition("RadarDetectionRange", ((double)radar.MaxDetectionRange).ToString());
                                //rule.AddCondition("RadarMaxAltitude", ((double)radar.MaxAltitude).ToString());
                                //rule.AddCondition("RadarMaxTargetSpeed", ((double)radar.MaxTargetSpeed).ToString());
                                //rule.AddCondition("RadarMaxTargetVelocity", ((double)radar.MaxTargetVelocity).ToString());
                                //rule.AddCondition("RadarRedeploymentTime", ((int)radar.RedeploymentTime).ToString());

                                //// Mühimmat koşullarını eklerken cast işlemi uygulayın
                                //rule.AddCondition("MunitionWeight", ((double)munition.Weight).ToString());
                                //rule.AddCondition("MunitionSpeed", ((double)munition.Speed).ToString());
                                //rule.AddCondition("MunitionRange", ((double)munition.Range).ToString());
                                //rule.AddCondition("MunitionExplosivePower", ((double)munition.ExplosivePower).ToString());
                                //rule.AddCondition("MunitionCost", ((double)munition.Cost).ToString());

                                // Hava savunma sisteminin savunma seviyesini ve toplam skorunu hesapla
                                var (defenseLevel, totalScore) = CalculateDefenseScore(range, ecmCapability, maxMissilesFired, maxEngagements, cost);//, radar, munition);

                                        // Radarlar ve Mühimmatlar skorlarını ayrı ayrı hesapla
                                        int radarScore = CalculateTotalRadarScore(new List<Radar>()); // Radar listesi
                                        int munitionScore = CalculateTotalMunitionScore(new List<Munition>()); // Mühimmat listesi

                                        // Toplam skorları ekle
                                        totalScore += radarScore + munitionScore;

                                        // Maksimum skor hesapla
                                        int maxTotalScore = CalculateMaxDefenseScore() + CalculateMaxRadarScore() + CalculateMaxMunitionScore(new List<Munition>());

                                        // Yüzdesel savunma skoru hesapla
                                        double scorePercentage = (double)totalScore / maxTotalScore * 100;

                                        // Yüzdelik savunma skoru aralıklarına göre sınıflandırma
                                        if (scorePercentage >= 80)
                                            defenseLevel = "Critical";
                                        else if (scorePercentage >= 60)
                                            defenseLevel = "High";
                                        else if (scorePercentage >= 40)
                                            defenseLevel = "Medium";
                                        else if (scorePercentage >= 20)
                                            defenseLevel = "Low";
                                        else
                                            defenseLevel = "Very Low";

                                        // Sonuç olarak DefenseScore ve TotalScore belirle
                                        rule.AddConsequence("DefenseScore", defenseLevel); // Savunma seviyesi
                                        rule.AddConsequence("TotalScore", totalScore.ToString()); // Toplam skor

                                        // Kurala ekle
                                        Rules.Add(rule);
                                //    }
                                //}
                            }
                        }
                    }
                }
            }
        }

        public List<Radar> GetRadarListFromEnum()
        {
            var radarList = new List<Radar>();

            foreach (EnumRadar.DetectionRange detectionRange in Enum.GetValues(typeof(EnumRadar.DetectionRange)))
            {
                foreach (EnumRadar.Altitude altitude in Enum.GetValues(typeof(EnumRadar.Altitude)))
                {
                    foreach (EnumRadar.MaxTargetSpeed maxTargetSpeed in Enum.GetValues(typeof(EnumRadar.MaxTargetSpeed)))
                    {
                        foreach (EnumRadar.MaxTargetVelocity maxTargetVelocity in Enum.GetValues(typeof(EnumRadar.MaxTargetVelocity)))
                        {
                            foreach (EnumRadar.RedeploymentTime redeploymentTime in Enum.GetValues(typeof(EnumRadar.RedeploymentTime)))
                            {
                                radarList.Add(new Radar
                                {
                                    MaxDetectionRange = (double)detectionRange,
                                    MaxAltitude = (double)altitude,
                                    MaxTargetSpeed = (double)maxTargetSpeed,
                                    MaxTargetVelocity = (double)maxTargetVelocity,
                                    RedeploymentTime = (int)redeploymentTime
                                });
                            }
                        }
                    }
                }
            }

            return radarList;
        }

        public List<Munition> GetMunitionListFromEnum()
        {
            var munitionList = new List<Munition>();

            foreach (EnumMunition.Weight weight in Enum.GetValues(typeof(EnumMunition.Weight)))
            {
                foreach (EnumMunition.Speed speed in Enum.GetValues(typeof(EnumMunition.Speed)))
                {
                    foreach (EnumMunition.Range range in Enum.GetValues(typeof(EnumMunition.Range)))
                    {
                        foreach (EnumMunition.ExplosivePower explosivePower in Enum.GetValues(typeof(EnumMunition.ExplosivePower)))
                        {
                            foreach (EnumMunition.Cost cost in Enum.GetValues(typeof(EnumMunition.Cost)))
                            {
                                munitionList.Add(new Munition
                                {
                                    Weight = (double)weight,
                                    Speed = (double)speed,
                                    Range = (double)range,
                                    ExplosivePower = (double)explosivePower,
                                    Cost = (double)cost
                                });
                            }
                        }
                    }
                }
            }

            return munitionList;
        }


        /// <summary>
        /// Hava savunma sisteminin savunma skorunu hesaplar.
        /// </summary>
        public (string, int) CalculateDefenseScore(
            EnumAirDefense.Range range,
            EnumAirDefense.ECMCapability ecmCapability,
            EnumAirDefense.MaxMissilesFired maxMissilesFired,
            EnumAirDefense.MaxEngagements maxEngagements,
            EnumAirDefense.Cost cost)
            //Radar radar,
            //Munition munition)
        {
            int score = 0;

            // Range değerlendirmesi
            switch (range)
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
            switch (ecmCapability)
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
            switch (maxMissilesFired)
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
            switch (maxEngagements)
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
            switch (cost)
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

            return ("", score);
        }

        /// <summary>
        /// Radarların toplam skorunu hesaplar.
        /// </summary>
        private int CalculateTotalRadarScore(List<Radar> radars)
        {
            int totalRadarScore = 0;

            foreach (var radar in radars)
            {
                totalRadarScore += CalculateRadarScore(radar);
            }

            return totalRadarScore;
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
        private int CalculateTotalMunitionScore(List<Munition> munitions)
        {
            int totalMunitionScore = 0;

            foreach (var munition in munitions)
            {
                totalMunitionScore += CalculateMunitionScore(munition);
            }

            return totalMunitionScore;
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

        /// <summary>
        /// Maksimum radar skoru hesaplanır.
        /// </summary>
        private int CalculateMaxRadarScore()
        {
            return 3 * 5; // 5 radar parametresi için 3 puan varsayılır
        }

        /// <summary>
        /// Maksimum mühimmat skoru hesaplanır.
        /// </summary>
        private int CalculateMaxMunitionScore(List<Munition> munitions)
        {
            return munitions.Count * 3 * 5; // Her mühimmat için 5 parametre ve 3 maksimum puan varsayılır
        }

        /// <summary>
        /// Maksimum savunma skoru hesaplanır.
        /// </summary>
        private int CalculateMaxDefenseScore()
        {
            return 3 * 5; // Range, ECMCapability, MaxMissilesFired, MaxEngagements, Cost için 3 puan varsayılır
        }
    }
}
