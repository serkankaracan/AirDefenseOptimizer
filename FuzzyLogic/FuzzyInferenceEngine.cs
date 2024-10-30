using System.Windows;

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

        /// <summary>
        /// Durulaştırma işlemi (Defuzzification) için merkezi ortalama yöntemini uygular.
        /// </summary>
        /// <param name="outputValues">Çıkış bulanık kümeleri ve üyelik dereceleri</param>
        /// <param name="outputVariable">Çıkış değişkeni (örneğin tehdit seviyesi)</param>
        /// <returns>Durulaştırılmış tehdit seviyesi</returns>
        public double Defuzzify(Dictionary<string, double> outputValues, FuzzyVariable outputVariable)
        {
            double sumProduct = 0;
            double sumMembership = 0;

            foreach (var output in outputValues)
            {
                // Anahtarın mevcut olup olmadığını kontrol et
                if (outputVariable.MembershipFunctions.ContainsKey(output.Key))
                {
                    double representativeValue = outputVariable.MembershipFunctions[output.Key].GetRepresentativeValue();
                    double membershipDegree = output.Value;

                    sumProduct += membershipDegree * representativeValue;
                    sumMembership += membershipDegree;
                }
                else
                {
                    MessageBox.Show($"Warning: The key '{output.Key}' is not present in ThreatLevel's membership functions.");
                }
            }

            return sumMembership > 0 ? sumProduct / sumMembership : 0;
        }
    }
}
