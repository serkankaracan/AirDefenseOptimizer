using AirDefenseOptimizer.FuzzyLogic;

namespace AirDefenseOptimizer.FuzzyRules
{
    /// <summary>
    /// Radar (Radar) için fuzzy mantık kurallarını tanımlar.
    /// Algılama menzili, irtifa, hedef hızı gibi faktörlere dayalı kurallar içerir.
    /// </summary>
    public class RadarRules
    {
        public List<FuzzyRule> Rules { get; set; }

        public RadarRules()
        {
            Rules = new List<FuzzyRule>();

            // Örnek Kural 1: Eğer algılama menzili "uzun" ve irtifa "yüksek" ise, angaje skoru "yüksek" olmalı
            FuzzyRule rule1 = new FuzzyRule();
            rule1.AddCondition("DetectionRange", "Long");
            rule1.AddCondition("Altitude", "High");
            rule1.AddConsequence("EngagementScore", "High");
            Rules.Add(rule1);

            // Örnek Kural 2: Eğer hedef hızı "yüksek" ve redeployment süresi "kısa" ise, angaje skoru "orta" olmalı
            FuzzyRule rule2 = new FuzzyRule();
            rule2.AddCondition("MaxTargetSpeed", "High");
            rule2.AddCondition("RedeploymentTime", "Short");
            rule2.AddConsequence("EngagementScore", "Medium");
            Rules.Add(rule2);

            // Örnek Kural 3: Eğer algılama menzili "kısa" ve hedef hızı "düşük" ise, angaje skoru "düşük" olmalı
            FuzzyRule rule3 = new FuzzyRule();
            rule3.AddCondition("DetectionRange", "Short");
            rule3.AddCondition("MaxTargetSpeed", "Low");
            rule3.AddConsequence("EngagementScore", "Low");
            Rules.Add(rule3);
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
