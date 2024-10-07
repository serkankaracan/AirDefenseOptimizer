namespace AirDefenseOptimizer.Enums
{
    public enum MunitionType
    {
        Missile,
        Bomb,
        Rocket,
        Bullet,
        Torpedo,
        ClusterBomb,
        Napalm,
        NuclearBomb,
        GuidedMissile,
        AntiTankMissile,
        AirToAirMissile,
        AirToSurfaceMissile,
        SurfaceToAirMissile
    }

    public static class MunitionTypeExtensions
    {
        public static string GetMunitionTypeName(this MunitionType munitionType)
        {
            return munitionType switch
            {
                MunitionType.Missile => "Missile",
                MunitionType.Bomb => "Bomb",
                MunitionType.Rocket => "Rocket",
                MunitionType.Bullet => "Bullet",
                MunitionType.Torpedo => "Torpedo",
                MunitionType.ClusterBomb => "Cluster Bomb",
                MunitionType.Napalm => "Napalm",
                MunitionType.NuclearBomb => "Nuclear Bomb",
                MunitionType.GuidedMissile => "Guided Missile",
                MunitionType.AntiTankMissile => "Anti-Tank Missile",
                MunitionType.AirToAirMissile => "Air-to-Air Missile",
                MunitionType.AirToSurfaceMissile => "Air-to-Surface Missile",
                MunitionType.SurfaceToAirMissile => "Surface-to-Air Missile",
                _ => "Unknown"
            };
        }
    }

}
