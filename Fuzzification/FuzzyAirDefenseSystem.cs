using AirDefenseOptimizer.FuzzyLogic;

namespace AirDefenseOptimizer.Fuzzification
{
    /// <summary>
    /// Hava Savunma Sistemi (Air Defense) nesnesi için Fuzzification işlemini gerçekleştiren sınıf.
    /// Aerodinamik ve balistik hedef menzilleri, maksimum angaje olabilme, fırlatılabilecek füze sayısı, ECM kabiliyeti ve maliyet gibi değişkenleri bulanıklaştırır.
    /// </summary>
    public class FuzzyAirDefense
    {
        public FuzzyVariable AerodynamicTargetRange { get; set; }   // Aerodinamik Hedef Menzili
        public FuzzyVariable BallisticTargetRange { get; set; }     // Balistik Hedef Menzili
        public FuzzyVariable MaxEngagements { get; set; }           // Maksimum Angaje Olabilme
        public FuzzyVariable MaxMissilesFired { get; set; }         // Maksimum Ateşlenebilecek Füze Sayısı
        public FuzzyVariable ECMCapability { get; set; }            // ECM Kabiliyeti
        public FuzzyVariable Cost { get; set; }                     // Maliyet

        public FuzzyAirDefense()
        {
            // Aerodinamik Hedef Menzili (Min ve Max birleştirildi)
            AerodynamicTargetRange = new FuzzyVariable("AerodynamicTargetRange");
            AerodynamicTargetRange.AddMembershipFunction("Short", new TriangleMembershipFunction("Short", 0, 50, 100));
            AerodynamicTargetRange.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 80, 150, 200));
            AerodynamicTargetRange.AddMembershipFunction("Long", new TriangleMembershipFunction("Long", 150, 200, 300));

            // Balistik Hedef Menzili (Min ve Max birleştirildi)
            BallisticTargetRange = new FuzzyVariable("BallisticTargetRange");
            BallisticTargetRange.AddMembershipFunction("Short", new TriangleMembershipFunction("Short", 0, 50, 100));
            BallisticTargetRange.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 80, 150, 200));
            BallisticTargetRange.AddMembershipFunction("Long", new TriangleMembershipFunction("Long", 150, 200, 300));

            // Maksimum Angaje Olabilme
            MaxEngagements = new FuzzyVariable("MaxEngagements");
            MaxEngagements.AddMembershipFunction("Few", new TriangleMembershipFunction("Few", 0, 5, 10));
            MaxEngagements.AddMembershipFunction("Moderate", new TriangleMembershipFunction("Moderate", 8, 15, 20));
            MaxEngagements.AddMembershipFunction("Many", new TriangleMembershipFunction("Many", 18, 25, 30));

            // Maksimum Ateşlenebilecek Füze Sayısı
            MaxMissilesFired = new FuzzyVariable("MaxMissilesFired");
            MaxMissilesFired.AddMembershipFunction("Low", new TriangleMembershipFunction("Low", 0, 10, 20));
            MaxMissilesFired.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 15, 25, 35));
            MaxMissilesFired.AddMembershipFunction("High", new TriangleMembershipFunction("High", 30, 40, 50));

            // ECM Kabiliyeti için bulanık kümeler
            ECMCapability = new FuzzyVariable("ECMCapability");
            ECMCapability.AddMembershipFunction("Weak", new TriangleMembershipFunction("Weak", 0, 1, 2));
            ECMCapability.AddMembershipFunction("Moderate", new TriangleMembershipFunction("Moderate", 1, 2, 3));
            ECMCapability.AddMembershipFunction("Strong", new TriangleMembershipFunction("Strong", 2, 3, 4));

            // Maliyet için bulanık kümeler
            Cost = new FuzzyVariable("Cost");
            Cost.AddMembershipFunction("Cheap", new TriangleMembershipFunction("Cheap", 0, 50000, 100000));
            Cost.AddMembershipFunction("Moderate", new TriangleMembershipFunction("Moderate", 75000, 150000, 200000));
            Cost.AddMembershipFunction("Expensive", new TriangleMembershipFunction("Expensive", 150000, 250000, 350000));
        }

        /// <summary>
        /// Hava savunma sisteminin aerodinamik hedef menzili, balistik hedef menzili, maksimum angaje olabilme kapasitesi, füze ateşleme kapasitesi, ECM kabiliyeti ve maliyet gibi değişkenlerini bulanıklaştırır.
        /// </summary>
        /// <param name="aerodynamicTargetRange">Aerodinamik hedef menzili</param>
        /// <param name="ballisticTargetRange">Balistik hedef menzili</param>
        /// <param name="maxEngagements">Maksimum angaje olabilme</param>
        /// <param name="maxMissilesFired">Maksimum füze ateşleme kapasitesi</param>
        /// <param name="ecmCapability">ECM kabiliyeti</param>
        /// <param name="cost">Maliyet</param>
        /// <returns>Her bir değişkenin bulanıklaştırılmış sonuçları</returns>
        public Dictionary<string, double> FuzzifyAirDefense(double aerodynamicTargetRange, double ballisticTargetRange, double maxEngagements, double maxMissilesFired, double ecmCapability, double cost)
        {
            var results = new Dictionary<string, double>();

            // Aerodinamik Hedef Menzili Fuzzification
            results["AerodynamicTargetRange_Short"] = AerodynamicTargetRange.Fuzzify(aerodynamicTargetRange)["Short"];
            results["AerodynamicTargetRange_Medium"] = AerodynamicTargetRange.Fuzzify(aerodynamicTargetRange)["Medium"];
            results["AerodynamicTargetRange_Long"] = AerodynamicTargetRange.Fuzzify(aerodynamicTargetRange)["Long"];

            // Balistik Hedef Menzili Fuzzification
            results["BallisticTargetRange_Short"] = BallisticTargetRange.Fuzzify(ballisticTargetRange)["Short"];
            results["BallisticTargetRange_Medium"] = BallisticTargetRange.Fuzzify(ballisticTargetRange)["Medium"];
            results["BallisticTargetRange_Long"] = BallisticTargetRange.Fuzzify(ballisticTargetRange)["Long"];

            // Maksimum Angaje Olabilme Fuzzification
            results["MaxEngagements_Few"] = MaxEngagements.Fuzzify(maxEngagements)["Few"];
            results["MaxEngagements_Moderate"] = MaxEngagements.Fuzzify(maxEngagements)["Moderate"];
            results["MaxEngagements_Many"] = MaxEngagements.Fuzzify(maxEngagements)["Many"];

            // Maksimum Füze Ateşleme Kapasitesi Fuzzification
            results["MaxMissilesFired_Low"] = MaxMissilesFired.Fuzzify(maxMissilesFired)["Low"];
            results["MaxMissilesFired_Medium"] = MaxMissilesFired.Fuzzify(maxMissilesFired)["Medium"];
            results["MaxMissilesFired_High"] = MaxMissilesFired.Fuzzify(maxMissilesFired)["High"];

            // ECM Kabiliyeti Fuzzification
            results["ECMCapability_Weak"] = ECMCapability.Fuzzify(ecmCapability)["Weak"];
            results["ECMCapability_Moderate"] = ECMCapability.Fuzzify(ecmCapability)["Moderate"];
            results["ECMCapability_Strong"] = ECMCapability.Fuzzify(ecmCapability)["Strong"];

            // Maliyet Fuzzification
            results["Cost_Cheap"] = Cost.Fuzzify(cost)["Cheap"];
            results["Cost_Moderate"] = Cost.Fuzzify(cost)["Moderate"];
            results["Cost_Expensive"] = Cost.Fuzzify(cost)["Expensive"];

            return results;
        }
    }
}
