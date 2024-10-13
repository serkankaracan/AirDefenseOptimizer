using AirDefenseOptimizer.FuzzyLogic;

namespace AirDefenseOptimizer.Fuzzification
{
    /// <summary>
    /// Radar nesnesi için Fuzzification işlemini gerçekleştiren sınıf.
    /// Radarın algılama menzili, irtifa, hedef hızı ve redeployment süresi gibi değişkenleri bulanıklaştırır.
    /// </summary>
    public class FuzzyRadar
    {
        public FuzzyVariable DetectionRange { get; set; } // Algılama Menzili
        public FuzzyVariable Altitude { get; set; }       // İrtifa
        public FuzzyVariable MaxTargetSpeed { get; set; }
        public FuzzyVariable MaxTargetVelocity { get; set; }
        public FuzzyVariable RedeploymentTime { get; set; }

        public FuzzyRadar()
        {
            // Algılama Menzili (Min ve Max birleştirildi)
            DetectionRange = new FuzzyVariable("DetectionRange");
            DetectionRange.AddMembershipFunction("Short", new TriangleMembershipFunction("Short", 0, 50, 100));
            DetectionRange.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 80, 150, 200));
            DetectionRange.AddMembershipFunction("Long", new TriangleMembershipFunction("Long", 150, 200, 300));

            // İrtifa (Min ve Max birleştirildi)
            Altitude = new FuzzyVariable("Altitude");
            Altitude.AddMembershipFunction("Low", new TriangleMembershipFunction("Low", 0, 5000, 10000));
            Altitude.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 8000, 15000, 20000));
            Altitude.AddMembershipFunction("High", new TriangleMembershipFunction("High", 18000, 25000, 30000));

            // Maksimum Hedef Hızı için bulanık kümeler
            MaxTargetSpeed = new FuzzyVariable("MaxTargetSpeed");
            MaxTargetSpeed.AddMembershipFunction("Low", new TriangleMembershipFunction("Low", 0, 300, 600));
            MaxTargetSpeed.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 500, 800, 1000));
            MaxTargetSpeed.AddMembershipFunction("High", new TriangleMembershipFunction("High", 900, 1200, 1500));

            // Maksimum Hedef Hızı (Velocity) için bulanık kümeler
            MaxTargetVelocity = new FuzzyVariable("MaxTargetVelocity");
            MaxTargetVelocity.AddMembershipFunction("Low", new TriangleMembershipFunction("Low", 0, 300, 600));
            MaxTargetVelocity.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 500, 800, 1000));
            MaxTargetVelocity.AddMembershipFunction("High", new TriangleMembershipFunction("High", 900, 1200, 1500));

            // Redeployment Zamanı için bulanık kümeler
            RedeploymentTime = new FuzzyVariable("RedeploymentTime");
            RedeploymentTime.AddMembershipFunction("Short", new TriangleMembershipFunction("Short", 0, 10, 20));
            RedeploymentTime.AddMembershipFunction("Medium", new TriangleMembershipFunction("Medium", 15, 25, 35));
            RedeploymentTime.AddMembershipFunction("Long", new TriangleMembershipFunction("Long", 30, 40, 50));
        }

        /// <summary>
        /// Radarın algılama menzili, irtifa, hedef hızı ve redeployment zamanı gibi değişkenleri bulanıklaştırır.
        /// </summary>
        /// <param name="detectionRange">Algılama menzili</param>
        /// <param name="altitude">İrtifa</param>
        /// <param name="maxTargetSpeed">Maksimum hedef hızı</param>
        /// <param name="maxTargetVelocity">Maksimum hedef hızı (velocity)</param>
        /// <param name="redeploymentTime">Redeployment zamanı</param>
        /// <returns>Her bir değişkenin bulanıklaştırılmış sonuçları</returns>
        public Dictionary<string, double> FuzzifyRadar(double detectionRange, double altitude, double maxTargetSpeed, double maxTargetVelocity, double redeploymentTime)
        {
            var results = new Dictionary<string, double>();

            // Algılama Menzili Fuzzification
            results["DetectionRange_Short"] = DetectionRange.Fuzzify(detectionRange)["Short"];
            results["DetectionRange_Medium"] = DetectionRange.Fuzzify(detectionRange)["Medium"];
            results["DetectionRange_Long"] = DetectionRange.Fuzzify(detectionRange)["Long"];

            // İrtifa Fuzzification
            results["Altitude_Low"] = Altitude.Fuzzify(altitude)["Low"];
            results["Altitude_Medium"] = Altitude.Fuzzify(altitude)["Medium"];
            results["Altitude_High"] = Altitude.Fuzzify(altitude)["High"];

            // Maksimum Hedef Hızı Fuzzification
            results["MaxTargetSpeed_Low"] = MaxTargetSpeed.Fuzzify(maxTargetSpeed)["Low"];
            results["MaxTargetSpeed_Medium"] = MaxTargetSpeed.Fuzzify(maxTargetSpeed)["Medium"];
            results["MaxTargetSpeed_High"] = MaxTargetSpeed.Fuzzify(maxTargetSpeed)["High"];

            // Maksimum Hedef Hızı (Velocity) Fuzzification
            results["MaxTargetVelocity_Low"] = MaxTargetVelocity.Fuzzify(maxTargetVelocity)["Low"];
            results["MaxTargetVelocity_Medium"] = MaxTargetVelocity.Fuzzify(maxTargetVelocity)["Medium"];
            results["MaxTargetVelocity_High"] = MaxTargetVelocity.Fuzzify(maxTargetVelocity)["High"];

            // Redeployment Zamanı Fuzzification
            results["RedeploymentTime_Short"] = RedeploymentTime.Fuzzify(redeploymentTime)["Short"];
            results["RedeploymentTime_Medium"] = RedeploymentTime.Fuzzify(redeploymentTime)["Medium"];
            results["RedeploymentTime_Long"] = RedeploymentTime.Fuzzify(redeploymentTime)["Long"];

            return results;
        }
    }
}
