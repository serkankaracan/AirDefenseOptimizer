namespace AirDefenseOptimizer.Models
{
    /// <summary>
    /// 3 eksenli pozisyon sınıfı: Enlem (Latitude), boylam (Longitude) ve irtifa (Altitude).
    /// Hem uçaklar hem de hava savunma sistemleri için konum bilgilerini içerir.
    /// </summary>
    public class Position
    {
        public double Latitude { get; set; }   // Enlem
        public double Longitude { get; set; }  // Boylam
        public double Altitude { get; set; }   // İrtifa (deniz seviyesinden metre cinsinden)

        /// <summary>
        /// Pozisyon sınıfı için constructor
        /// </summary>
        public Position(double latitude, double longitude, double altitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }

        /// <summary>
        /// İki pozisyon arasındaki mesafeyi (km) hesaplayan metod.
        /// Hem enlem-boylam, hem de yükseklik (irtifa) farkını hesaba katar.
        /// </summary>
        public static double CalculateDistance(Position pos1, Position pos2)
        {
            const double EarthRadiusKm = 6371;  // Dünya'nın yarıçapı

            // Enlem ve boylam farklarını radyan cinsinden al
            var latDifference = ToRadians(pos2.Latitude - pos1.Latitude);
            var lngDifference = ToRadians(pos2.Longitude - pos1.Longitude);

            // Haversine formülü ile yatay mesafeyi hesapla (enlem-boylam farkları ile)
            var a = Math.Sin(latDifference / 2) * Math.Sin(latDifference / 2) +
                    Math.Cos(ToRadians(pos1.Latitude)) * Math.Cos(ToRadians(pos2.Latitude)) *
                    Math.Sin(lngDifference / 2) * Math.Sin(lngDifference / 2);
            var horizontalDistance = 2 * EarthRadiusKm * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Yükseklik farkını hesapla (irtifa)
            var verticalDistance = (pos2.Altitude - pos1.Altitude) / 1000;  // İrtifayı km'ye çevir

            // Pythagoras teoremi ile toplam mesafeyi hesapla (hipotenüs)
            return Math.Sqrt(Math.Pow(horizontalDistance, 2) + Math.Pow(verticalDistance, 2));
        }

        /// <summary>
        /// Dereceyi radyana çevirir.
        /// </summary>
        private static double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }
}
