using AirDefenseOptimizer.FuzzyLogic;

namespace AirDefenseOptimizer.Fuzzification
{
    /// <summary>
    /// Uçak (Aircraft) nesnesi için Fuzzification işlemini gerçekleştiren sınıf.
    /// Hız, menzil, maksimum irtifa, manevra kabiliyeti, yük kapasitesi, radar kesit alanı (RCS) ve maliyet gibi değişkenleri bulanıklaştırır.
    /// </summary>
    public class FuzzyAircraft
    {
        public FuzzyVariable Speed { get; set; }            // Hız
        public FuzzyVariable Range { get; set; }            // Menzil
        public FuzzyVariable MaxAltitude { get; set; }      // Maksimum İrtifa
        public FuzzyVariable Maneuverability { get; set; }  // Manevra Kabiliyeti
        public FuzzyVariable PayloadCapacity { get; set; }  // Yük Kapasitesi
        public FuzzyVariable RadarCrossSection { get; set; } // Radar Kesit Alanı (RCS)
        public FuzzyVariable Cost { get; set; }             // Maliyet

        public FuzzyAircraft()
        {
            // Hız için bulanık kümeler
            Speed = new FuzzyVariable("Speed");
            Speed.AddMembershipFunction("Slow", new TriangleMembershipFunction("Slow", 0, 500, 1000));
            Speed.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 800, 1000, 1200));
            Speed.AddMembershipFunction("Fast", new TriangleMembershipFunction("Fast", 1000, 1500, 2000));

            // Menzil için bulanık kümeler
            Range = new FuzzyVariable("Range");
            Range.AddMembershipFunction("Short", new TriangleMembershipFunction("Short", 0, 500, 1000));
            Range.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 800, 1500, 2000));
            Range.AddMembershipFunction("Long", new TriangleMembershipFunction("Long", 1800, 2500, 3000));

            // Maksimum İrtifa için bulanık kümeler
            MaxAltitude = new FuzzyVariable("MaxAltitude");
            MaxAltitude.AddMembershipFunction("Low", new TriangleMembershipFunction("Low", 0, 5000, 10000));
            MaxAltitude.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 8000, 15000, 20000));
            MaxAltitude.AddMembershipFunction("High", new TriangleMembershipFunction("High", 18000, 25000, 30000));

            // Manevra Kabiliyeti için bulanık kümeler
            Maneuverability = new FuzzyVariable("Maneuverability");
            Maneuverability.AddMembershipFunction("Low", new TriangleMembershipFunction("Low", 0, 3, 6));
            Maneuverability.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 5, 7, 9));
            Maneuverability.AddMembershipFunction("High", new TriangleMembershipFunction("High", 8, 10, 12));

            // Yük Kapasitesi için bulanık kümeler
            PayloadCapacity = new FuzzyVariable("PayloadCapacity");
            PayloadCapacity.AddMembershipFunction("Small", new TriangleMembershipFunction("Small", 0, 500, 1000));
            PayloadCapacity.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 800, 1500, 2000));
            PayloadCapacity.AddMembershipFunction("Large", new TriangleMembershipFunction("Large", 1800, 2500, 3000));

            // Radar Kesit Alanı (RCS) için bulanık kümeler
            RadarCrossSection = new FuzzyVariable("RadarCrossSection");
            RadarCrossSection.AddMembershipFunction("Low", new TriangleMembershipFunction("Low", 0, 1, 2));
            RadarCrossSection.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 1.5, 3, 4.5));
            RadarCrossSection.AddMembershipFunction("High", new TriangleMembershipFunction("High", 4, 6, 8));

            // Maliyet için bulanık kümeler
            Cost = new FuzzyVariable("Cost");
            Cost.AddMembershipFunction("Cheap", new TriangleMembershipFunction("Cheap", 0, 50000, 100000));
            Cost.AddMembershipFunction("Moderate", new TriangleMembershipFunction("Moderate", 75000, 150000, 200000));
            Cost.AddMembershipFunction("Expensive", new TriangleMembershipFunction("Expensive", 150000, 250000, 350000));
        }

        /// <summary>
        /// Uçağın hız, menzil, maksimum irtifa, manevra kabiliyeti, yük kapasitesi, radar kesit alanı ve maliyet gibi değişkenlerini bulanıklaştırır.
        /// </summary>
        /// <param name="speed">Hız</param>
        /// <param name="range">Menzil</param>
        /// <param name="maxAltitude">Maksimum İrtifa</param>
        /// <param name="maneuverability">Manevra Kabiliyeti</param>
        /// <param name="payloadCapacity">Yük Kapasitesi</param>
        /// <param name="radarCrossSection">Radar Kesit Alanı</param>
        /// <param name="cost">Maliyet</param>
        /// <returns>Her bir değişkenin bulanıklaştırılmış sonuçları</returns>
        public Dictionary<string, double> FuzzifyAircraft(double speed, double range, double maxAltitude, double maneuverability, double payloadCapacity, double radarCrossSection, double cost)
        {
            var results = new Dictionary<string, double>();

            // Hız Fuzzification
            results["Speed_Slow"] = Speed.Fuzzify(speed)["Slow"];
            results["Speed_Medium"] = Speed.Fuzzify(speed)["Medium"];
            results["Speed_Fast"] = Speed.Fuzzify(speed)["Fast"];

            // Menzil Fuzzification
            results["Range_Short"] = Range.Fuzzify(range)["Short"];
            results["Range_Medium"] = Range.Fuzzify(range)["Medium"];
            results["Range_Long"] = Range.Fuzzify(range)["Long"];

            // Maksimum İrtifa Fuzzification
            results["MaxAltitude_Low"] = MaxAltitude.Fuzzify(maxAltitude)["Low"];
            results["MaxAltitude_Medium"] = MaxAltitude.Fuzzify(maxAltitude)["Medium"];
            results["MaxAltitude_High"] = MaxAltitude.Fuzzify(maxAltitude)["High"];

            // Manevra Kabiliyeti Fuzzification
            results["Maneuverability_Low"] = Maneuverability.Fuzzify(maneuverability)["Low"];
            results["Maneuverability_Medium"] = Maneuverability.Fuzzify(maneuverability)["Medium"];
            results["Maneuverability_High"] = Maneuverability.Fuzzify(maneuverability)["High"];

            // Yük Kapasitesi Fuzzification
            results["PayloadCapacity_Small"] = PayloadCapacity.Fuzzify(payloadCapacity)["Small"];
            results["PayloadCapacity_Medium"] = PayloadCapacity.Fuzzify(payloadCapacity)["Medium"];
            results["PayloadCapacity_Large"] = PayloadCapacity.Fuzzify(payloadCapacity)["Large"];

            // Radar Kesit Alanı (RCS) Fuzzification
            results["RadarCrossSection_Low"] = RadarCrossSection.Fuzzify(radarCrossSection)["Low"];
            results["RadarCrossSection_Medium"] = RadarCrossSection.Fuzzify(radarCrossSection)["Medium"];
            results["RadarCrossSection_High"] = RadarCrossSection.Fuzzify(radarCrossSection)["High"];

            // Maliyet Fuzzification
            results["Cost_Cheap"] = Cost.Fuzzify(cost)["Cheap"];
            results["Cost_Moderate"] = Cost.Fuzzify(cost)["Moderate"];
            results["Cost_Expensive"] = Cost.Fuzzify(cost)["Expensive"];

            return results;
        }
    }
}
