namespace AirDefenseOptimizer.FuzzyLogic
{
    /// <summary>
    /// FuzzyRuleSet tüm kuralların toplandığı yerdir. Uçak ve mühimmat gibi değişkenler bir arada değerlendirilir.
    /// Ayrıca tehditin hava savunma sistemine olan uzaklığı da değerlendirilir.
    /// </summary>
    public class FuzzyRuleSet
    {
        public List<FuzzyRule> Rules { get; set; }

        public FuzzyRuleSet()
        {
            Rules = new List<FuzzyRule>();

            // Örnek Kural 1: Uçak hızı "yüksek", mühimmat patlayıcı gücü "yüksek", tehdit hava savunma sistemine "yakın" ise tehdit seviyesi "çok yüksek"
            FuzzyRule rule1 = new FuzzyRule();
            rule1.AddCondition("AircraftSpeed", "High");
            rule1.AddCondition("MunitionExplosivePower", "High");
            rule1.AddCondition("ThreatDistance", "Close");
            rule1.AddConsequence("ThreatLevel", "VeryHigh");
            Rules.Add(rule1);

            // Örnek Kural 2: Uçak manevra kabiliyeti "düşük", mühimmat menzili "uzun", tehdit hava savunma sistemine "orta" uzaklıkta ise tehdit seviyesi "orta"
            FuzzyRule rule2 = new FuzzyRule();
            rule2.AddCondition("AircraftManeuverability", "Low");
            rule2.AddCondition("MunitionRange", "Long");
            rule2.AddCondition("ThreatDistance", "Medium");
            rule2.AddConsequence("ThreatLevel", "Medium");
            Rules.Add(rule2);

            // Örnek Kural 3: Uçak tipi "bombardıman", mühimmat maliyeti "yüksek", tehdit hava savunma sistemine "uzak" ise tehdit seviyesi "orta"
            FuzzyRule rule3 = new FuzzyRule();
            rule3.AddCondition("AircraftType", "Bomber");
            rule3.AddCondition("MunitionCost", "High");
            rule3.AddCondition("ThreatDistance", "Far");
            rule3.AddConsequence("ThreatLevel", "Medium");
            Rules.Add(rule3);

            // Örnek Kural 4: Uçak irtifası "yüksek", mühimmat hızı "orta", manevra kabiliyeti "yüksek", tehdit hava savunma sistemine "çok uzak" ise tehdit seviyesi "düşük"
            FuzzyRule rule4 = new FuzzyRule();
            rule4.AddCondition("AircraftAltitude", "High");
            rule4.AddCondition("MunitionSpeed", "Medium");
            rule4.AddCondition("MunitionManeuverability", "High");
            rule4.AddCondition("ThreatDistance", "VeryFar");
            rule4.AddConsequence("ThreatLevel", "Low");
            Rules.Add(rule4);

            // Örnek Kural 5: Uçak hızı "orta", mühimmat tipi "füze", patlayıcı gücü "yüksek", tehdit hava savunma sistemine "orta" uzaklıkta ise tehdit seviyesi "yüksek"
            FuzzyRule rule5 = new FuzzyRule();
            rule5.AddCondition("AircraftSpeed", "Medium");
            rule5.AddCondition("MunitionType", "Missile");
            rule5.AddCondition("MunitionExplosivePower", "High");
            rule5.AddCondition("ThreatDistance", "Medium");
            rule5.AddConsequence("ThreatLevel", "High");
            Rules.Add(rule5);

            // Örnek Kural 6: Uçak tipi "helikopter", mühimmat maliyeti "düşük", menzili "kısa", tehdit hava savunma sistemine "yakın" ise tehdit seviyesi "düşük"
            FuzzyRule rule6 = new FuzzyRule();
            rule6.AddCondition("AircraftType", "Helicopter");
            rule6.AddCondition("MunitionCost", "Low");
            rule6.AddCondition("MunitionRange", "Short");
            rule6.AddCondition("ThreatDistance", "Close");
            rule6.AddConsequence("ThreatLevel", "Low");
            Rules.Add(rule6);

            // Örnek Kural 7: Uçak manevra kabiliyeti "yüksek", mühimmat menzili "uzun", maliyeti "orta", tehdit hava savunma sistemine "uzak" ise tehdit seviyesi "orta"
            FuzzyRule rule7 = new FuzzyRule();
            rule7.AddCondition("AircraftManeuverability", "High");
            rule7.AddCondition("MunitionRange", "Long");
            rule7.AddCondition("MunitionCost", "Medium");
            rule7.AddCondition("ThreatDistance", "Far");
            rule7.AddConsequence("ThreatLevel", "Medium");
            Rules.Add(rule7);

            // Örnek Kural 8: Uçak hızı "yavaş", mühimmat patlayıcı gücü "düşük", maliyeti "düşük", tehdit hava savunma sistemine "çok uzak" ise tehdit seviyesi "çok düşük"
            FuzzyRule rule8 = new FuzzyRule();
            rule8.AddCondition("AircraftSpeed", "Slow");
            rule8.AddCondition("MunitionExplosivePower", "Low");
            rule8.AddCondition("MunitionCost", "Low");
            rule8.AddCondition("ThreatDistance", "VeryFar");
            rule8.AddConsequence("ThreatLevel", "VeryLow");
            Rules.Add(rule8);

            // Örnek Kural 9: Uçak irtifası "orta", mühimmat manevra kabiliyeti "orta", hızı "yüksek", tehdit hava savunma sistemine "yakın" ise tehdit seviyesi "yüksek"
            FuzzyRule rule9 = new FuzzyRule();
            rule9.AddCondition("AircraftAltitude", "Medium");
            rule9.AddCondition("MunitionManeuverability", "Medium");
            rule9.AddCondition("MunitionSpeed", "High");
            rule9.AddCondition("ThreatDistance", "Close");
            rule9.AddConsequence("ThreatLevel", "High");
            Rules.Add(rule9);

            // Örnek Kural 10: Uçak hızı "yüksek", mühimmat tipi "bomba", patlayıcı gücü "orta", tehdit hava savunma sistemine "orta" uzaklıkta ise tehdit seviyesi "orta"
            FuzzyRule rule10 = new FuzzyRule();
            rule10.AddCondition("AircraftSpeed", "High");
            rule10.AddCondition("MunitionType", "Bomb");
            rule10.AddCondition("MunitionExplosivePower", "Medium");
            rule10.AddCondition("ThreatDistance", "Medium");
            rule10.AddConsequence("ThreatLevel", "Medium");
            Rules.Add(rule10);
        }

        /// <summary>
        /// Kuralları değerlendirir ve sonuçları döndürür.
        /// </summary>
        /// <param name="inputValues">Girdi değerleri (örneğin uçak hızı, mühimmat patlayıcı gücü, tehdit mesafesi)</param>
        /// <returns>Sonuç olarak tehdit seviyesi</returns>
        public Dictionary<string, string> EvaluateRules(Dictionary<string, string> inputValues)
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

                // Eğer tüm koşullar sağlanıyorsa, sonucu döndür
                if (match)
                {
                    return rule.Consequences;
                }
            }

            // Eğer hiçbir kural sağlanmadıysa, boş sonuç döndür
            return new Dictionary<string, string>();
        }
    }
}
