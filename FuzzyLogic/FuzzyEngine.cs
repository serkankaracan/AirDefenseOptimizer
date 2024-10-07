namespace AirDefenseOptimizer.FuzzyLogic
{
    public class FuzzyEngine
    {
        // Burada bulanık mantık hesaplamaları yapılacak
        public double CalculateEngagementProbability(double distance, double speed, double threatLevel)
        {
            // Örnek basit bulanık mantık hesaplaması
            double engagementProbability = 0.0;

            // Örneğin hız ve tehdit seviyesine göre karar verebilirsiniz.
            if (speed < 500 && threatLevel > 0.7)
            {
                engagementProbability = 0.9; // Yüksek angajman olasılığı
            }
            else if (speed < 1000 && threatLevel > 0.5)
            {
                engagementProbability = 0.7;
            }
            else
            {
                engagementProbability = 0.4; // Düşük angajman olasılığı
            }

            return engagementProbability;
        }
    }
}
