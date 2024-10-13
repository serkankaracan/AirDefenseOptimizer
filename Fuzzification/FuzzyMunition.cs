using AirDefenseOptimizer.FuzzyLogic;

namespace AirDefenseOptimizer.Fuzzification
{
    /// <summary>
    /// Mühimmat (Munition) nesnesi için Fuzzification işlemini gerçekleştiren sınıf.
    /// Ağırlık, hız, menzil, patlayıcı güç, maliyet ve manevra kabiliyeti gibi değişkenleri bulanıklaştırır.
    /// </summary>
    public class FuzzyMunition
    {
        public FuzzyVariable Weight { get; set; }
        public FuzzyVariable Speed { get; set; }
        public FuzzyVariable Range { get; set; }
        public FuzzyVariable ExplosivePower { get; set; }
        public FuzzyVariable Cost { get; set; }
        public FuzzyVariable Maneuverability { get; set; } // Manevra Kabiliyeti

        public FuzzyMunition()
        {
            // Ağırlık değişkeni için bulanık kümeler
            Weight = new FuzzyVariable("Weight");
            Weight.AddMembershipFunction("Light", new TriangleMembershipFunction("Light", 0, 50, 100));
            Weight.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 75, 125, 175));
            Weight.AddMembershipFunction("Heavy", new TriangleMembershipFunction("Heavy", 150, 200, 250));

            // Hız değişkeni için bulanık kümeler
            Speed = new FuzzyVariable("Speed");
            Speed.AddMembershipFunction("Slow", new TriangleMembershipFunction("Slow", 0, 500, 1000));
            Speed.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 800, 1000, 1200));
            Speed.AddMembershipFunction("Fast", new TriangleMembershipFunction("Fast", 1000, 1500, 2000));

            // Menzil değişkeni için bulanık kümeler
            Range = new FuzzyVariable("Range");
            Range.AddMembershipFunction("Short", new TriangleMembershipFunction("Short", 0, 20, 50));
            Range.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 30, 50, 70));
            Range.AddMembershipFunction("Long", new TriangleMembershipFunction("Long", 60, 100, 150));

            // Patlayıcı güç değişkeni için bulanık kümeler
            ExplosivePower = new FuzzyVariable("ExplosivePower");
            ExplosivePower.AddMembershipFunction("Low", new TriangleMembershipFunction("Low", 0, 100, 200));
            ExplosivePower.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 150, 250, 350));
            ExplosivePower.AddMembershipFunction("High", new TriangleMembershipFunction("High", 300, 400, 500));

            // Maliyet değişkeni için bulanık kümeler
            Cost = new FuzzyVariable("Cost");
            Cost.AddMembershipFunction("Cheap", new TriangleMembershipFunction("Cheap", 0, 50000, 100000));
            Cost.AddMembershipFunction("Moderate", new TriangleMembershipFunction("Moderate", 75000, 150000, 200000));
            Cost.AddMembershipFunction("Expensive", new TriangleMembershipFunction("Expensive", 150000, 250000, 350000));

            // Manevra Kabiliyeti değişkeni için bulanık kümeler
            Maneuverability = new FuzzyVariable("Maneuverability");
            Maneuverability.AddMembershipFunction("Low", new TriangleMembershipFunction("Low", 0, 3, 6));
            Maneuverability.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 5, 7, 9));
            Maneuverability.AddMembershipFunction("High", new TriangleMembershipFunction("High", 8, 10, 12));
        }

        /// <summary>
        /// Mühimmatın ağırlık, hız, menzil, patlayıcı güç, maliyet ve manevra kabiliyeti gibi değişkenlerini bulanıklaştırır.
        /// </summary>
        /// <param name="weight">Ağırlık</param>
        /// <param name="speed">Hız</param>
        /// <param name="range">Menzil</param>
        /// <param name="explosivePower">Patlayıcı Güç</param>
        /// <param name="cost">Maliyet</param>
        /// <param name="maneuverability">Manevra Kabiliyeti</param>
        /// <returns>Her bir değişkenin bulanıklaştırılmış sonuçları</returns>
        public Dictionary<string, double> FuzzifyMunition(double weight, double speed, double range, double explosivePower, double cost, double maneuverability)
        {
            var results = new Dictionary<string, double>();

            // Ağırlık Fuzzification
            results["Weight_Light"] = Weight.Fuzzify(weight)["Light"];
            results["Weight_Medium"] = Weight.Fuzzify(weight)["Medium"];
            results["Weight_Heavy"] = Weight.Fuzzify(weight)["Heavy"];

            // Hız Fuzzification
            results["Speed_Slow"] = Speed.Fuzzify(speed)["Slow"];
            results["Speed_Medium"] = Speed.Fuzzify(speed)["Medium"];
            results["Speed_Fast"] = Speed.Fuzzify(speed)["Fast"];

            // Menzil Fuzzification
            results["Range_Short"] = Range.Fuzzify(range)["Short"];
            results["Range_Medium"] = Range.Fuzzify(range)["Medium"];
            results["Range_Long"] = Range.Fuzzify(range)["Long"];

            // Patlayıcı Güç Fuzzification
            results["ExplosivePower_Low"] = ExplosivePower.Fuzzify(explosivePower)["Low"];
            results["ExplosivePower_Medium"] = ExplosivePower.Fuzzify(explosivePower)["Medium"];
            results["ExplosivePower_High"] = ExplosivePower.Fuzzify(explosivePower)["High"];

            // Maliyet Fuzzification
            results["Cost_Cheap"] = Cost.Fuzzify(cost)["Cheap"];
            results["Cost_Moderate"] = Cost.Fuzzify(cost)["Moderate"];
            results["Cost_Expensive"] = Cost.Fuzzify(cost)["Expensive"];

            // Manevra Kabiliyeti Fuzzification
            results["Maneuverability_Low"] = Maneuverability.Fuzzify(maneuverability)["Low"];
            results["Maneuverability_Medium"] = Maneuverability.Fuzzify(maneuverability)["Medium"];
            results["Maneuverability_High"] = Maneuverability.Fuzzify(maneuverability)["High"];

            return results;
        }
    }
}
