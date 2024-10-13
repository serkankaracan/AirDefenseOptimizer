using AirDefenseOptimizer.FuzzyEnums;
using AirDefenseOptimizer.FuzzyLogic;

namespace AirDefenseOptimizer.FuzzyRules
{
    /// <summary>
    /// Mühimmat (Munition) için fuzzy mantık kurallarını otomatik oluşturan sınıf.
    /// Ağırlık, hız, menzil, patlayıcı güç ve maliyet gibi faktörlerin tüm olası kombinasyonlarına dayalı kurallar oluşturulur.
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
                                // Yeni bir kural oluştur
                                var rule = new FuzzyRule();

                                // Koşulları ekle (ağırlık, hız, menzil, patlayıcı güç, maliyet)
                                rule.AddCondition("Weight", weight.ToString() ?? string.Empty);
                                rule.AddCondition("Speed", speed.ToString() ?? string.Empty);
                                rule.AddCondition("Range", range.ToString() ?? string.Empty);
                                rule.AddCondition("ExplosivePower", explosivePower.ToString() ?? string.Empty);
                                rule.AddCondition("Cost", cost.ToString() ?? string.Empty);

                                // Her kombinasyon için sonuç belirle (örneğin, Angaje Skoru)
                                rule.AddConsequence("EngagementScore", CalculateEngagementScore(weight, speed, range, explosivePower, cost));

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
        /// <param name="weight">Mühimmatın ağırlığı</param>
        /// <param name="speed">Mühimmatın hızı</param>
        /// <param name="range">Mühimmatın menzili</param>
        /// <param name="explosivePower">Mühimmatın patlayıcı gücü</param>
        /// <param name="cost">Mühimmatın maliyeti</param>
        /// <returns>Hesaplanan angaje skoru</returns>
        private string CalculateEngagementScore(EnumMunition.Weight weight, EnumMunition.Speed speed, EnumMunition.Range range, EnumMunition.ExplosivePower explosivePower, EnumMunition.Cost cost)
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
                    score += 3; // Pahalı mühimmat için yüksek puan
                    break;
                case EnumMunition.Cost.Moderate:
                    score += 2; // Orta maliyetli mühimmat için orta puan
                    break;
                case EnumMunition.Cost.Cheap:
                    score += 1; // Ucuz mühimmat için düşük puan
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
