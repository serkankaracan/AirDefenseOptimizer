namespace AirDefenseOptimizer.FuzzyLogic
{
    /// <summary>
    /// Bulanık mantık çıkarım motoru (inference engine), 
    /// kurallara göre giriş değerlerini değerlendirir ve çıkışları hesaplar.
    /// </summary>
    public class FuzzyInferenceEngine
    {
        public List<FuzzyRule> Rules { get; set; } // Tüm bulanık kurallar

        public FuzzyInferenceEngine()
        {
            Rules = new List<FuzzyRule>();
        }

        /// <summary>
        /// Sisteme bir kural ekler.
        /// </summary>
        /// <param name="rule">Eklenen kural</param>
        public void AddRule(FuzzyRule rule)
        {
            Rules.Add(rule);
        }

        /// <summary>
        /// Verilen giriş değişkenlerine göre sonuçları hesaplar.
        /// </summary>
        /// <param name="inputVariables">Giriş değişkenleri (örneğin hız, menzil)</param>
        /// <returns>Çıkış değişkenleri ve ait oldukları bulanık kümeler</returns>
        public Dictionary<string, double> Evaluate(Dictionary<string, FuzzyVariable> inputVariables)
        {
            var outputValues = new Dictionary<string, double>();

            // Tüm kuralları kontrol et
            foreach (var rule in Rules)
            {
                double ruleStrength = 1.0; // Kuralın gücünü belirler

                // Koşulları değerlendir
                foreach (var condition in rule.Conditions)
                {
                    var variable = inputVariables[condition.Key];
                    var fuzzySet = condition.Value;

                    // İlgili koşulun üyelik derecesini al
                    var fuzzifiedValue = variable.Fuzzify(variable.CrispValue)[fuzzySet];
                    ruleStrength = Math.Min(ruleStrength, fuzzifiedValue); // En düşük üyelik derecesini seç
                }

                // Sonuçları güncelle
                foreach (var consequence in rule.Consequences)
                {
                    if (!outputValues.ContainsKey(consequence.Key))
                    {
                        outputValues[consequence.Key] = 0;
                    }

                    // Sonuç olarak çıkan üyelik derecesini kuralın gücü ile çarp
                    outputValues[consequence.Key] = Math.Max(outputValues[consequence.Key], ruleStrength);
                }
            }

            return outputValues;
        }
    }
}
