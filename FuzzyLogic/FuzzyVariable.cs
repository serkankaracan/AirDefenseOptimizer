namespace AirDefenseOptimizer.FuzzyLogic
{
    /// <summary>
    /// Bulanık değişkenleri temsil eden sınıf.
    /// Bir değişkenin adı, crisp değeri ve üyelik fonksiyonlarını içerir.
    /// </summary>
    public class FuzzyVariable
    {
        public string Name { get; set; } // Değişkenin adı
        public double CrispValue { get; set; } // Kesin (crisp) değer
        public Dictionary<string, MembershipFunction> MembershipFunctions { get; set; } // Üyelik fonksiyonları

        /// <summary>
        /// Fuzzy değişkeni oluşturur.
        /// </summary>
        /// <param name="name">Değişkenin adı</param>
        public FuzzyVariable(string name)
        {
            Name = name;
            MembershipFunctions = new Dictionary<string, MembershipFunction>();
        }

        /// <summary>
        /// Üyelik fonksiyonlarına bir fonksiyon ekler.
        /// </summary>
        /// <param name="name">Üyelik fonksiyonunun adı</param>
        /// <param name="function">Üyelik fonksiyonu</param>
        public void AddMembershipFunction(string name, MembershipFunction function)
        {
            MembershipFunctions[name] = function;
        }

        /// <summary>
        /// Crisp değeri üyelik fonksiyonlarına göre bulanıklaştırır (fuzzify).
        /// </summary>
        /// <param name="value">Kesin (crisp) giriş değeri</param>
        /// <returns>Her bir üyelik fonksiyonu için üyelik derecelerini döndürür</returns>
        public Dictionary<string, double> Fuzzify(double value)
        {
            CrispValue = value;
            var fuzzifiedValues = new Dictionary<string, double>();

            foreach (var function in MembershipFunctions)
            {
                fuzzifiedValues[function.Key] = function.Value.CalculateMembership(value);
            }

            return fuzzifiedValues;
        }

        /// <summary>
        /// Bulanık değerleri kullanarak durulaştırma (defuzzification) işlemi yapar.
        /// </summary>
        /// <param name="fuzzyValues">Her bir üyelik fonksiyonu için bulanık değerler</param>
        /// <returns>Durulaştırılmış kesin (crisp) değer</returns>
        public double Defuzzify(Dictionary<string, double> fuzzyValues)
        {
            double sumProduct = 0;
            double sumMembership = 0;

            foreach (var fuzzyValue in fuzzyValues)
            {
                // Her bir bulanık küme için üyelik derecesi ve zirve değeri ile ağırlıklandırılmış ortalama hesaplayabiliriz
                sumProduct += fuzzyValue.Value * MembershipFunctions[fuzzyValue.Key].GetRepresentativeValue();
                sumMembership += fuzzyValue.Value;
            }

            return sumProduct / sumMembership;
        }
    }
}