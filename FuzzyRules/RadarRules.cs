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
            foreach (var detectionRange in Enum.GetValues(typeof(DetectionRange)).Cast<DetectionRange>())
            {
                foreach (var altitude in Enum.GetValues(typeof(Altitude)).Cast<Altitude>())
                {
                    foreach (var maxTargetSpeed in Enum.GetValues(typeof(MaxTargetSpeed)).Cast<MaxTargetSpeed>())
                    {
                        foreach (var maxTargetVelocity in Enum.GetValues(typeof(MaxTargetVelocity)).Cast<MaxTargetVelocity>())
                        {
                            foreach (var redeploymentTime in Enum.GetValues(typeof(RedeploymentTime)).Cast<RedeploymentTime>())
                            {
                                // Yeni bir kural oluştur
                                var rule = new FuzzyRule();

                                // Koşulları ekle (algılama menzili, irtifa, hedef hızları, redeployment süresi)
                                rule.AddCondition("DetectionRange", detectionRange.ToString() ?? string.Empty);
                                rule.AddCondition("Altitude", altitude.ToString() ?? string.Empty);
                                rule.AddCondition("MaxTargetSpeed", maxTargetSpeed.ToString() ?? string.Empty);
                                rule.AddCondition("MaxTargetVelocity", maxTargetVelocity.ToString() ?? string.Empty);
                                rule.AddCondition("RedeploymentTime", redeploymentTime.ToString() ?? string.Empty);

                                // Her kombinasyon için sonuç belirle (örneğin, Angaje Skoru)
                                rule.AddConsequence("EngagementScore", CalculateEngagementScore(detectionRange, altitude, maxTargetSpeed, maxTargetVelocity, redeploymentTime));

                                // Kurala ekle
                                Rules.Add(rule);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Koşullara göre angaje skorunu hesaplar. Bu yöntem isteğe göre özelleştirilebilir.
        /// </summary>
        /// <param name="detectionRange">Algılama menzili</param>
        /// <param name="altitude">İrtifa</param>
        /// <param name="maxTargetSpeed">Maksimum hedef hızı</param>
        /// <param name="maxTargetVelocity">Maksimum hedef hızı (velocity)</param>
        /// <param name="redeploymentTime">Redeployment süresi</param>
        /// <returns>Hesaplanan angaje skoru</returns>
        private string CalculateEngagementScore(DetectionRange detectionRange, Altitude altitude, MaxTargetSpeed maxTargetSpeed, MaxTargetVelocity maxTargetVelocity, RedeploymentTime redeploymentTime)
        {
            int score = 0;

            // Detection Range değerlendirmesi
            switch (detectionRange)
            {
                case DetectionRange.Long:
                    score += 3; // Uzun menzil için yüksek puan
                    break;
                case DetectionRange.Medium:
                    score += 2; // Orta menzil için orta puan
                    break;
                case DetectionRange.Short:
                    score += 1; // Kısa menzil için düşük puan
                    break;
            }

            // Altitude değerlendirmesi
            switch (altitude)
            {
                case Altitude.High:
                    score += 3; // Yüksek irtifa için yüksek puan
                    break;
                case Altitude.Medium:
                    score += 2; // Orta irtifa için orta puan
                    break;
                case Altitude.Low:
                    score += 1; // Düşük irtifa için düşük puan
                    break;
            }

            // Max Target Speed değerlendirmesi
            switch (maxTargetSpeed)
            {
                case MaxTargetSpeed.High:
                    score += 3; // Yüksek hız için yüksek puan
                    break;
                case MaxTargetSpeed.Medium:
                    score += 2; // Orta hız için orta puan
                    break;
                case MaxTargetSpeed.Low:
                    score += 1; // Düşük hız için düşük puan
                    break;
            }

            // Max Target Velocity değerlendirmesi
            switch (maxTargetVelocity)
            {
                case MaxTargetVelocity.High:
                    score += 3; // Yüksek velocity için yüksek puan
                    break;
                case MaxTargetVelocity.Medium:
                    score += 2; // Orta velocity için orta puan
                    break;
                case MaxTargetVelocity.Low:
                    score += 1; // Düşük velocity için düşük puan
                    break;
            }

            // Redeployment Time değerlendirmesi
            switch (redeploymentTime)
            {
                case RedeploymentTime.Short:
                    score += 3; // Kısa redeployment zamanı için yüksek puan
                    break;
                case RedeploymentTime.Medium:
                    score += 2; // Orta redeployment zamanı için orta puan
                    break;
                case RedeploymentTime.Long:
                    score += 1; // Uzun redeployment zamanı için düşük puan
                    break;
            }

            // Puanın sonucuna göre EngagementScore belirlenir
            if (score >= 13)
            {
                return "High";
            }
            else if (score >= 8)
            {
                return "Medium";
            }
            else
            {
                return "Low";
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
