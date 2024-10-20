using AirDefenseOptimizer.FuzzyEnums;
using AirDefenseOptimizer.FuzzyLogic;

namespace AirDefenseOptimizer.FuzzyRules
{
    /// <summary>
    /// Mühimmat (Munition) için fuzzy mantık kurallarını otomatik oluşturan sınıf.
    /// Ağırlık, hız, menzil, patlayıcı güç, maliyet ve manevra kabiliyeti gibi faktörlerin tüm olası kombinasyonlarına dayalı kurallar oluşturulur.
    /// </summary>
    public class MunitionRules
    {
        public List<FuzzyRule> Rules { get; set; }

        public MunitionRules()
        {
            Rules = new List<FuzzyRule>();

            // Tüm olası kombinasyonları döngülerle oluştur
            foreach (var weight in Enum.GetValues(typeof(EnumMunition.Weight)).Cast<EnumMunition.Weight>())
            {
                foreach (var speed in Enum.GetValues(typeof(EnumMunition.Speed)).Cast<EnumMunition.Speed>())
                {
                    foreach (var range in Enum.GetValues(typeof(EnumMunition.Range)).Cast<EnumMunition.Range>())
                    {
                        foreach (var explosivePower in Enum.GetValues(typeof(EnumMunition.ExplosivePower)).Cast<EnumMunition.ExplosivePower>())
                        {
                            foreach (var cost in Enum.GetValues(typeof(EnumMunition.Cost)).Cast<EnumMunition.Cost>())
                            {
                                foreach (var maneuverability in Enum.GetValues(typeof(EnumMunition.Maneuverability)).Cast<EnumMunition.Maneuverability>())
                                {
                                    // Yeni bir kural oluştur
                                    var rule = new FuzzyRule();

                                    // Koşulları ekle (ağırlık, hız, menzil, patlayıcı güç, maliyet ve manevra kabiliyeti)
                                    rule.AddCondition("Weight", weight.ToString() ?? string.Empty);
                                    rule.AddCondition("Speed", speed.ToString() ?? string.Empty);
                                    rule.AddCondition("Range", range.ToString() ?? string.Empty);
                                    rule.AddCondition("ExplosivePower", explosivePower.ToString() ?? string.Empty);
                                    rule.AddCondition("Cost", cost.ToString() ?? string.Empty);
                                    rule.AddCondition("Maneuverability", maneuverability.ToString() ?? string.Empty);

                                    // Savunma skorunu hesapla
                                    var (impactLevel, totalScore) = CalculateEngagementScore(weight, speed, range, explosivePower, cost, maneuverability);

                                    // Sonuç olarak ImpactScore ve TotalScore belirle
                                    rule.AddConsequence("ImpactScore", impactLevel); // Seviye
                                    rule.AddConsequence("TotalScore", totalScore.ToString()); // Toplam skor

                                    // Kurala ekle
                                    Rules.Add(rule);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Koşullara göre mühimmat angaje skorunu hesaplar.
        /// </summary>
        private (string, int) CalculateEngagementScore(EnumMunition.Weight weight, EnumMunition.Speed speed, EnumMunition.Range range, EnumMunition.ExplosivePower explosivePower, EnumMunition.Cost cost, EnumMunition.Maneuverability maneuverability)
        {
            int score = 0;

            // Weight değerlendirmesi
            switch (weight)
            {
                case EnumMunition.Weight.Heavy:
                    score += 3; // Ağır mühimmat için yüksek puan
                    break;
                case EnumMunition.Weight.Medium:
                    score += 2; // Orta ağırlık mühimmat için orta puan
                    break;
                case EnumMunition.Weight.Light:
                    score += 1; // Hafif mühimmat için düşük puan
                    break;
            }

            // Speed değerlendirmesi
            switch (speed)
            {
                case EnumMunition.Speed.Fast:
                    score += 3; // Hızlı mühimmat için yüksek puan
                    break;
                case EnumMunition.Speed.Medium:
                    score += 2; // Orta hızlı mühimmat için orta puan
                    break;
                case EnumMunition.Speed.Slow:
                    score += 1; // Yavaş mühimmat için düşük puan
                    break;
            }

            // Range değerlendirmesi
            switch (range)
            {
                case EnumMunition.Range.Long:
                    score += 3; // Uzun menzil için yüksek puan
                    break;
                case EnumMunition.Range.Medium:
                    score += 2; // Orta menzil için orta puan
                    break;
                case EnumMunition.Range.Short:
                    score += 1; // Kısa menzil için düşük puan
                    break;
            }

            // ExplosivePower değerlendirmesi
            switch (explosivePower)
            {
                case EnumMunition.ExplosivePower.High:
                    score += 3; // Yüksek patlayıcı güç için yüksek puan
                    break;
                case EnumMunition.ExplosivePower.Medium:
                    score += 2; // Orta patlayıcı güç için orta puan
                    break;
                case EnumMunition.ExplosivePower.Low:
                    score += 1; // Düşük patlayıcı güç için düşük puan
                    break;
            }

            // Cost değerlendirmesi
            switch (cost)
            {
                case EnumMunition.Cost.Expensive:
                    score += 1; // Pahalı mühimmat düşük puan alır
                    break;
                case EnumMunition.Cost.Moderate:
                    score += 2;
                    break;
                case EnumMunition.Cost.Cheap:
                    score += 3; // Ucuz mühimmat yüksek puan alır
                    break;
            }

            // Maneuverability değerlendirmesi
            switch (maneuverability)
            {
                case EnumMunition.Maneuverability.High:
                    score += 3; // Yüksek manevra kabiliyeti için yüksek puan
                    break;
                case EnumMunition.Maneuverability.Medium:
                    score += 2; // Orta manevra kabiliyeti için orta puan
                    break;
                case EnumMunition.Maneuverability.Low:
                    score += 1; // Düşük manevra kabiliyeti için düşük puan
                    break;
            }

            // Toplam maksimum puanı belirle
            int maxScore = 3 * 6; // Her parametre için 3 puan varsayılır

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
