using AirDefenseOptimizer.FuzzyLogic;

namespace AirDefenseOptimizer.FuzzyRules
{
    /// <summary>
    /// Mühimmat (Munition) için fuzzy mantık kurallarını tanımlar.
    /// Ağırlık, hız, menzil, patlayıcı güç ve maliyet gibi faktörlere dayalı kurallar içerir.
    /// </summary>
    public class MunitionRules
    {
        public List<FuzzyRule> Rules { get; set; }

        public MunitionRules()
        {
            Rules = new List<FuzzyRule>();

            // Örnek Kural 1: Eğer patlayıcı güç "yüksek" ve menzil "uzun" ise, angaje skoru "yüksek" olmalı
            FuzzyRule rule1 = new FuzzyRule();
            rule1.AddCondition("ExplosivePower", "High");
            rule1.AddCondition("Range", "Long");
            rule1.AddConsequence("EngagementScore", "High");
            Rules.Add(rule1);

            // Örnek Kural 2: Eğer hız "yavaş" ve maliyet "düşük" ise, angaje skoru "düşük" olmalı
            FuzzyRule rule2 = new FuzzyRule();
            rule2.AddCondition("Speed", "Slow");
            rule2.AddCondition("Cost", "Cheap");
            rule2.AddConsequence("EngagementScore", "Low");
            Rules.Add(rule2);

            // Örnek Kural 3: Eğer ağırlık "hafif" ve menzil "orta" ise, angaje skoru "orta" olmalı
            FuzzyRule rule3 = new FuzzyRule();
            rule3.AddCondition("Weight", "Light");
            rule3.AddCondition("Range", "Medium");
            rule3.AddConsequence("EngagementScore", "Medium");
            Rules.Add(rule3);
        }

        /// <summary>
        /// Mühimmat kurallarını değerlendirir ve sonuçları döndürür.
        /// </summary>
        /// <param name="inputValues">Girdi değerleri (örneğin patlayıcı güç, hız)</param>
        /// <returns>Sonuç olarak angaje skoru</returns>
        public Dictionary<string, string> EvaluateMunitionRules(Dictionary<string, string> inputValues)
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
