namespace AirDefenseOptimizer.FuzzyLogic
{
    /// <summary>
    /// Bulanık mantıkta kullanılan kuralları temsil eder.
    /// "Eğer... ise..." yapısında kurallar tanımlanır.
    /// </summary>
    public class FuzzyRule
    {
        /// <summary>
        /// Kuralın koşul kısmı (Eğer... kısmı).
        /// Değişken adı ve onun ait olduğu bulanık kümenin adı.
        /// Örneğin: Eğer hız "yüksek" ise...
        /// </summary>
        public Dictionary<string, string> Conditions { get; set; }

        /// <summary>
        /// Kuralın sonucunu (ise kısmını) temsil eder.
        /// Hangi değişkenin hangi bulanık kümeye ait olacağını gösterir.
        /// Örneğin: Angaje skoru "yüksek" olmalı.
        /// </summary>
        public Dictionary<string, string> Consequences { get; set; }

        /// <summary>
        /// Yeni bir kural oluşturur.
        /// </summary>
        public FuzzyRule()
        {
            Conditions = new Dictionary<string, string>();
            Consequences = new Dictionary<string, string>();
        }

        /// <summary>
        /// Kurala koşul ekler.
        /// </summary>
        /// <param name="variable">Değişkenin adı (örneğin hız)</param>
        /// <param name="fuzzySet">Değişkenin ait olduğu bulanık küme (örneğin yüksek)</param>
        public void AddCondition(string variable, string fuzzySet)
        {
            Conditions[variable] = fuzzySet;
        }

        /// <summary>
        /// Kurala sonuç ekler.
        /// </summary>
        /// <param name="variable">Sonuç değişkeninin adı (örneğin angaje skoru)</param>
        /// <param name="fuzzySet">Sonuç olarak ait olacağı bulanık küme (örneğin yüksek)</param>
        public void AddConsequence(string variable, string fuzzySet)
        {
            Consequences[variable] = fuzzySet;
        }
    }
}
