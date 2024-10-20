using AirDefenseOptimizer.FuzzyEnums;
using AirDefenseOptimizer.FuzzyLogic;

namespace AirDefenseOptimizer.FuzzyRules
{
    /// <summary>
    /// Radar (Radar) için fuzzy mantık kurallarını otomatik oluşturan sınıf.
    /// Algılama menzili, irtifa, hedef hızı gibi faktörlerin tüm olası kombinasyonlarına dayalı kurallar oluşturulur.
    /// </summary>
    public class RadarRules
    {
        public List<FuzzyRule> Rules { get; set; }

        public RadarRules()
        {
            Rules = new List<FuzzyRule>();

            // Tüm olası kombinasyonları döngülerle oluştur
            foreach (var detectionRange in Enum.GetValues(typeof(EnumRadar.DetectionRange)).Cast<EnumRadar.DetectionRange>())
            {
                foreach (var altitude in Enum.GetValues(typeof(EnumRadar.Altitude)).Cast<EnumRadar.Altitude>())
                {
                    foreach (var maxTargetSpeed in Enum.GetValues(typeof(EnumRadar.MaxTargetSpeed)).Cast<EnumRadar.MaxTargetSpeed>())
                    {
                        foreach (var maxTargetVelocity in Enum.GetValues(typeof(EnumRadar.MaxTargetVelocity)).Cast<EnumRadar.MaxTargetVelocity>())
                        {
                            foreach (var redeploymentTime in Enum.GetValues(typeof(EnumRadar.RedeploymentTime)).Cast<EnumRadar.RedeploymentTime>())
                            {
                                // Yeni bir kural oluştur
                                var rule = new FuzzyRule();

                                // Koşulları ekle (algılama menzili, irtifa, hedef hızları, redeployment süresi)
                                rule.AddCondition("DetectionRange", detectionRange.ToString() ?? string.Empty);
                                rule.AddCondition("Altitude", altitude.ToString() ?? string.Empty);
                                rule.AddCondition("MaxTargetSpeed", maxTargetSpeed.ToString() ?? string.Empty);
                                rule.AddCondition("MaxTargetVelocity", maxTargetVelocity.ToString() ?? string.Empty);
                                rule.AddCondition("RedeploymentTime", redeploymentTime.ToString() ?? string.Empty);

                                // Savunma skorunu hesapla
                                var (surveillanceLevel, totalScore) = CalculateEngagementScore(detectionRange, altitude, maxTargetSpeed, maxTargetVelocity, redeploymentTime);

                                // Sonuçları ekle
                                rule.AddConsequence("SurveillanceScore", surveillanceLevel); // Seviye
                                rule.AddConsequence("TotalScore", totalScore.ToString());    // Toplam puan

                                // Kurala ekle
                                Rules.Add(rule);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Koşullara göre radar savunma skorunu hesaplar.
        /// </summary>
        private (string, int) CalculateEngagementScore(EnumRadar.DetectionRange detectionRange, EnumRadar.Altitude altitude, EnumRadar.MaxTargetSpeed maxTargetSpeed, EnumRadar.MaxTargetVelocity maxTargetVelocity, EnumRadar.RedeploymentTime redeploymentTime)
        {
            int score = 0;

            // Detection Range değerlendirmesi
            switch (detectionRange)
            {
                case EnumRadar.DetectionRange.Long:
                    score += 3; // Uzun menzil için yüksek puan
                    break;
                case EnumRadar.DetectionRange.Medium:
                    score += 2; // Orta menzil için orta puan
                    break;
                case EnumRadar.DetectionRange.Short:
                    score += 1; // Kısa menzil için düşük puan
                    break;
            }

            // Altitude değerlendirmesi
            switch (altitude)
            {
                case EnumRadar.Altitude.High:
                    score += 3; // Yüksek irtifa için yüksek puan
                    break;
                case EnumRadar.Altitude.Medium:
                    score += 2; // Orta irtifa için orta puan
                    break;
                case EnumRadar.Altitude.Low:
                    score += 1; // Düşük irtifa için düşük puan
                    break;
            }

            // Max Target Speed değerlendirmesi
            switch (maxTargetSpeed)
            {
                case EnumRadar.MaxTargetSpeed.High:
                    score += 3; // Yüksek hız için yüksek puan
                    break;
                case EnumRadar.MaxTargetSpeed.Medium:
                    score += 2; // Orta hız için orta puan
                    break;
                case EnumRadar.MaxTargetSpeed.Low:
                    score += 1; // Düşük hız için düşük puan
                    break;
            }

            // Max Target Velocity değerlendirmesi
            switch (maxTargetVelocity)
            {
                case EnumRadar.MaxTargetVelocity.High:
                    score += 3; // Yüksek velocity için yüksek puan
                    break;
                case EnumRadar.MaxTargetVelocity.Medium:
                    score += 2; // Orta velocity için orta puan
                    break;
                case EnumRadar.MaxTargetVelocity.Low:
                    score += 1; // Düşük velocity için düşük puan
                    break;
            }

            // Redeployment Time değerlendirmesi
            switch (redeploymentTime)
            {
                case EnumRadar.RedeploymentTime.Short:
                    score += 3; // Kısa redeployment zamanı için yüksek puan
                    break;
                case EnumRadar.RedeploymentTime.Medium:
                    score += 2; // Orta redeployment zamanı için orta puan
                    break;
                case EnumRadar.RedeploymentTime.Long:
                    score += 1; // Uzun redeployment zamanı için düşük puan
                    break;
            }

            // Toplam maksimum puanı belirle
            int maxScore = 3 * 5; // Her parametre için 3 puan varsayılır

            // Yüzdesel tehdit skoru hesapla
            double scorePercentage = (double)score / maxScore * 100;

            // Yüzdelik tehdit skoru aralıklarına göre sınıflandırma
            if (scorePercentage >= 80)
            {
                return ("Critical", score);
            }
            else if (scorePercentage >= 60)
            {
                return ("High", score);
            }
            else if (scorePercentage >= 40)
            {
                return ("Medium", score);
            }
            else if (scorePercentage >= 20)
            {
                return ("Low", score);
            }
            else
            {
                return ("Very Low", score);
            }
        }

        /// <summary>
        /// Radar kurallarını değerlendirir ve sonuçları döndürür.
        /// </summary>
        /// <param name="inputValues">Girdi değerleri (örneğin algılama menzili, hedef hızı)</param>
        /// <returns>Sonuç olarak angaje skoru</returns>
        public Dictionary<string, string> EvaluateRadarRules(Dictionary<string, string> inputValues)
        {
            foreach (var rule in Rules)
            {
                bool match = true;

                // Tüm koşulları kontrol et
                foreach (var condition in rule.Conditions)
                {
                    if (!inputValues.ContainsKey(condition.Key) || inputValues[condition.Key] != condition.Value)
                    {
                        match = false;
                        break;
                    }
                }

                // Eğer tüm koşullar uyuyorsa, sonucu döndür
                if (match)
                {
                    return rule.Consequences;
                }
            }

            // Eğer hiçbir kural uymuyorsa, boş sonuç döndür
            return new Dictionary<string, string>();
        }
    }
}
