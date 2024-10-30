using AirDefenseOptimizer.FuzzyLogic;
using AirDefenseOptimizer.FuzzyRules;
using AirDefenseOptimizer.Models;

namespace AirDefenseOptimizer.Fuzzification
{
    public class FuzzyAircraft
    {
        public FuzzyVariable Speed { get; set; }
        public FuzzyVariable Range { get; set; }
        public FuzzyVariable MaxAltitude { get; set; }
        public FuzzyVariable Maneuverability { get; set; }
        public FuzzyVariable ECMCapability { get; set; }
        public FuzzyVariable PayloadCapacity { get; set; }
        public FuzzyVariable RadarCrossSection { get; set; }
        public FuzzyVariable Cost { get; set; }

        private FuzzyInferenceEngine inferenceEngine;
        private FuzzyVariable ThreatLevel;

        /*
            public AircraftType AircraftType { get; set; }
            public Radar? Radar { get; set; }
            public List<AircraftMunition> Munitions { get; set; } = new List<AircraftMunition>();
        */

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

            // ECM Kabiliyeti için bulanık kümeler
            ECMCapability = new FuzzyVariable("ECMCapability");
            ECMCapability.AddMembershipFunction("Low", new TriangleMembershipFunction("Low", 0, 1, 2));
            ECMCapability.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 1.5, 3, 4.5));
            ECMCapability.AddMembershipFunction("High", new TriangleMembershipFunction("High", 4, 6, 8));

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

            // Tehdit Seviyesi için çıkış bulanık kümeleri
            ThreatLevel = new FuzzyVariable("ThreatLevel");
            ThreatLevel.AddMembershipFunction("Very Low", new TriangleMembershipFunction("Very Low", 0, 1, 2));
            ThreatLevel.AddMembershipFunction("Low", new TriangleMembershipFunction("Low", 2, 3, 4));
            ThreatLevel.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 4, 5, 6));
            ThreatLevel.AddMembershipFunction("High", new TriangleMembershipFunction("High", 6, 7, 8));
            ThreatLevel.AddMembershipFunction("Critical", new TriangleMembershipFunction("Critical", 8, 9, 10));

            // Kurallar ve çıkarım motoru
            inferenceEngine = new FuzzyInferenceEngine();
            //DefineRules();

            var aircraftRules = new AircraftRules();
            foreach (var rule in aircraftRules.Rules)
            {
                inferenceEngine.AddRule(rule);
            }
        }

        private void DefineRules()
        {
            // Yüksek tehdit seviyesi
            var rule1 = new FuzzyRule();
            rule1.AddCondition("Speed", "Fast");
            rule1.AddCondition("MaxAltitude", "High");
            rule1.AddCondition("Maneuverability", "High");
            rule1.AddCondition("ECMCapability", "High");
            rule1.AddCondition("RadarCrossSection", "High");
            rule1.AddConsequence("ThreatLevel", "High");
            inferenceEngine.AddRule(rule1);

            // Orta tehdit seviyesi
            var rule2 = new FuzzyRule();
            rule2.AddCondition("Speed", "Medium");
            rule2.AddCondition("MaxAltitude", "Medium");
            rule2.AddCondition("Maneuverability", "Medium");
            rule2.AddCondition("ECMCapability", "Medium");
            rule2.AddCondition("RadarCrossSection", "Medium");
            rule2.AddConsequence("ThreatLevel", "Medium");
            inferenceEngine.AddRule(rule2);

            // Düşük tehdit seviyesi
            var rule3 = new FuzzyRule();
            rule3.AddCondition("Speed", "Slow");
            rule3.AddCondition("MaxAltitude", "Low");
            rule3.AddCondition("Maneuverability", "Low");
            rule3.AddCondition("ECMCapability", "Low");
            rule3.AddCondition("RadarCrossSection", "Low");
            rule3.AddConsequence("ThreatLevel", "Low");
            inferenceEngine.AddRule(rule3);

            // Ek kurallar - maliyet ve yük kapasitesine göre
            var rule4 = new FuzzyRule();
            rule4.AddCondition("Cost", "Expensive");
            rule4.AddCondition("PayloadCapacity", "Large");
            rule4.AddCondition("Speed", "Fast");
            rule4.AddConsequence("ThreatLevel", "High");
            inferenceEngine.AddRule(rule4);
        }

        /// <summary>
        /// Uçak özelliklerini bulanıklaştırır ve tehdit seviyesini hesaplar.
        /// </summary>
        public double CalculateThreatLevel(Aircraft aircraft)
        {
            // Giriş değişkenlerine crisp değerleri atayın
            Speed.CrispValue = aircraft.Speed;
            Range.CrispValue = aircraft.Range;
            MaxAltitude.CrispValue = aircraft.MaxAltitude;
            Maneuverability.CrispValue = (double)aircraft.Maneuverability;
            ECMCapability.CrispValue = (double)aircraft.ECMCapability;
            PayloadCapacity.CrispValue = aircraft.PayloadCapacity;
            RadarCrossSection.CrispValue = aircraft.RadarCrossSection;
            Cost.CrispValue = aircraft.Cost;

            var inputVariables = new Dictionary<string, FuzzyVariable>
            {
                { "Speed", Speed },
                { "Range", Range },
                { "MaxAltitude", MaxAltitude },
                { "Maneuverability", Maneuverability },
                { "ECMCapability", ECMCapability },
                { "PayloadCapacity", PayloadCapacity },
                { "RadarCrossSection", RadarCrossSection },
                { "Cost", Cost }
            };

            // Kuralları değerlendir ve tehdit seviyesini hesapla
            var outputValues = inferenceEngine.Evaluate(inputVariables);
            double threatLevel = inferenceEngine.Defuzzify(outputValues, ThreatLevel);
            return threatLevel;
        }
    }
}
