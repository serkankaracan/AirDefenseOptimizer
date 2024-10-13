using AirDefenseOptimizer.FuzzyLogic;

namespace AirDefenseOptimizer.FuzzyRules
{
    /// <summary>
    /// Uçaklar (Aircraft) için fuzzy mantık kurallarını tanımlar.
    /// Hız, menzil, manevra kabiliyeti gibi faktörlere dayalı kurallar içerir.
    /// </summary>
    public class AircraftRules
    {
        public List<FuzzyRule> Rules { get; set; }

        public AircraftRules()
        {
            Rules = new List<FuzzyRule>();

            // Örnek Kural 1: Eğer hız "yüksek" ve menzil "uzun" ise, angaje skoru "yüksek" olmalı
            FuzzyRule rule1 = new FuzzyRule();
            rule1.AddCondition("Speed", "High");
            rule1.AddCondition("Range", "Long");
            rule1.AddConsequence("EngagementScore", "High");
            Rules.Add(rule1);

            // Örnek Kural 2: Eğer hız "düşük" ve manevra kabiliyeti "yüksek" ise, angaje skoru "orta" olmalı
            FuzzyRule rule2 = new FuzzyRule();
            rule2.AddCondition("Speed", "Low");
            rule2.AddCondition("Maneuverability", "High");
            rule2.AddConsequence("EngagementScore", "Medium");
            Rules.Add(rule2);

            // Örnek Kural 3: Eğer maliyet "yüksek" ve menzil "orta" ise, angaje skoru "düşük" olmalı
            FuzzyRule rule3 = new FuzzyRule();
            rule3.AddCondition("Cost", "Expensive");
            rule3.AddCondition("Range", "Medium");
            rule3.AddConsequence("EngagementScore", "Low");
            Rules.Add(rule3);

            // İhtiyaç duyduğun başka kurallar da eklenebilir
        }

        /// <summary>
        /// Kuralları değerlendirir ve sonuçları döndürür.
        /// </summary>
        /// <param name="inputValues">Girdi değerleri (örneğin hız, menzil)</param>
        /// <returns>Sonuç olarak angaje skoru</returns>
        public Dictionary<string, string> EvaluateAircraftRules(Dictionary<string, string> inputValues)
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
