using AirDefenseOptimizer.FuzzyEnums;
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
                                // Yeni bir kural oluştur
                                var rule = new FuzzyRule();

                                // Koşulları ekle
                                rule.AddCondition("Range", range.ToString());
                                rule.AddCondition("ECMCapability", ecmCapability.ToString());
                                rule.AddCondition("MaxMissilesFired", maxMissilesFired.ToString());
                                rule.AddCondition("MaxEngagements", maxEngagements.ToString());
                                rule.AddCondition("Cost", cost.ToString());

                                // Angaje skoru hesapla ve sonuçları ekle
                                rule.AddConsequence("EngagementScore", CalculateEngagementScore(range, ecmCapability, maxMissilesFired, maxEngagements, cost));

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
        private string CalculateEngagementScore(EnumAirDefense.Range range, EnumAirDefense.ECMCapability ecmCapability, EnumAirDefense.MaxMissilesFired maxMissilesFired, EnumAirDefense.MaxEngagements maxEngagements, EnumAirDefense.Cost cost)
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
                    score += 1; // Pahalıysa düşük angaje skoru
                    break;
                case EnumAirDefense.Cost.Moderate:
                    score += 2;
                    break;
                case EnumAirDefense.Cost.Cheap:
                    score += 3; // Ucuzsa yüksek angaje skoru
                    break;
            }

            // Toplam puana göre angaje skoru
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
