using AirDefenseOptimizer.Enums;

namespace AirDefenseOptimizer.Models
{
    public class Aircraft
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public AircraftType AircraftType { get; set; }
        public double Speed { get; set; }
        public double Range { get; set; }
        public double MaxAltitude { get; set; }
        public Maneuverability Maneuverability { get; set; }
        public double PayloadCapacity { get; set; }
        public Radar? Radar { get; set; }
        public double Cost { get; set; }

        public List<AircraftMunition> Munitions { get; set; } = new List<AircraftMunition>();

        public List<MunitionDetail> GetMunitionDetails()
        {
            List<MunitionDetail> munitionDetails = new List<MunitionDetail>();

            foreach (var aircraftMunition in Munitions)
            {
                MunitionDetail detail = new MunitionDetail
                {
                    Id = aircraftMunition.Munition.Id,
                    Name = aircraftMunition.Munition.Name,
                    Quantity = aircraftMunition.Quantity
                };
                munitionDetails.Add(detail);
            }

            return munitionDetails;
        }
    }

    public class AircraftMunition
    {
        public Munition Munition { get; set; } = null!;
        public int Quantity { get; set; }
    }

    public class MunitionDetail
    {
        public int Id { get; set; } // Mühimmat Id'si
        public string Name { get; set; } = string.Empty; // Mühimmat adı
        public int Quantity { get; set; } // Mühimmat miktarı
    }
}
