using AirDefenseOptimizer.FuzzyLogic;

namespace AirDefenseOptimizer.FuzzyRules
{
    /// <summary>
    /// Hava Savunma Sistemleri (Air Defense) için fuzzy mantık kurallarını tanımlar.
    /// ECM kabiliyeti, menzil, füze ateşleme kapasitesi gibi faktörlere dayalı kurallar içerir.
    /// </summary>
    public class AirDefenseRules
    {
        public List<FuzzyRule> Rules { get; set; }

        public AirDefenseRules()
        {
            Rules = new List<FuzzyRule>();

            // Örnek Kural 1: Eğer menzil "uzun" ve ECM kabiliyeti "güçlü" ise, angaje skoru "yüksek" olmalı
            FuzzyRule rule1 = new FuzzyRule();
            rule1.AddCondition("Range", "Long");
            rule1.AddCondition("ECMCapability", "Strong");
            rule1.AddConsequence("EngagementScore", "High");
            Rules.Add(rule1);

            // Örnek Kural 2: Eğer füze ateşleme kapasitesi "düşük" ve maliyet "yüksek" ise, angaje skoru "orta" olmalı
            FuzzyRule rule2 = new FuzzyRule();
            rule2.AddCondition("MaxMissilesFired", "Low");
            rule2.AddCondition("Cost", "Expensive");
            rule2.AddConsequence("EngagementScore", "Medium");
            Rules.Add(rule2);

            // Örnek Kural 3: Eğer maksimum angaje olabilme "yüksek" ve menzil "orta" ise, angaje skoru "düşük" olmalı
            FuzzyRule rule3 = new FuzzyRule();
            rule3.AddCondition("MaxEngagements", "High");
            rule3.AddCondition("Range", "Medium");
            rule3.AddConsequence("EngagementScore", "Low");
            Rules.Add(rule3);
        }

        /// <summary>
        /// Hava savunma kurallarını değerlendirir ve sonuçları döndürür.
        /// </summary>
        /// <param name="inputValues">Girdi değerleri (örneğin menzil, ECM kabiliyeti)</param>
        /// <returns>Sonuç olarak angaje skoru</returns>
        public Dictionary<string, string> EvaluateAirDefenseRules(Dictionary<string, string> inputValues)
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
