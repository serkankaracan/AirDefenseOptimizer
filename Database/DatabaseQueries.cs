namespace AirDefenseOptimizer.Database
{
    public static class DatabaseQueries
    {
        public const string CreateRadarTable = @"CREATE TABLE IF NOT EXISTS Radar (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            RadarType TEXT NOT NULL,
            MaxDetectionTargets INTEGER NOT NULL,
            MaxTrackingTargets INTEGER NOT NULL,
            MinDetectionRange REAL NOT NULL,
            MaxDetectionRange REAL NOT NULL,
            MaxAltitude REAL NOT NULL,
            MinAltitude REAL NOT NULL,
            MaxTargetSpeed REAL NOT NULL,
            MaxTargetVelocity REAL NOT NULL,
            RedeploymentTime INTEGER NOT NULL
        );";

        public const string CreateMunitionTable = @"CREATE TABLE IF NOT EXISTS Munition (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            MunitionType TEXT NOT NULL,
            Weight REAL NOT NULL,
            Speed REAL NOT NULL,
            Range REAL NOT NULL,
            Maneuverability TEXT NOT NULL,
            ExplosivePower REAL NOT NULL,
            Cost REAL NOT NULL
        );";

        public const string CreateAircraftTable = @"CREATE TABLE IF NOT EXISTS Aircraft (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            AircraftType TEXT NOT NULL,
            Speed REAL NOT NULL,
            Range REAL NOT NULL,
            MaxAltitude REAL NOT NULL,
            Maneuverability TEXT NOT NULL,
            ECMCapability TEXT NOT NULL, 
            PayloadCapacity REAL NOT NULL,
            RadarCrossSection REAL NOT NULL,  
            RadarId INTEGER,
            Cost REAL NOT NULL,
            FOREIGN KEY (RadarId) REFERENCES Radar (Id) ON DELETE SET NULL
        );";

        public const string CreateAircraftMunitionTable = @"CREATE TABLE IF NOT EXISTS AircraftMunition (
            AircraftId INTEGER NOT NULL,
            MunitionId INTEGER NOT NULL,
            Quantity INTEGER NOT NULL,
            PRIMARY KEY (AircraftId, MunitionId),
            FOREIGN KEY (AircraftId) REFERENCES Aircraft (Id) ON DELETE CASCADE,
            FOREIGN KEY (MunitionId) REFERENCES Munition (Id) ON DELETE CASCADE
        );";

        public const string CreateAirDefenseTable = @"CREATE TABLE IF NOT EXISTS AirDefense (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            AerodynamicTargetRangeMax REAL NOT NULL,
            AerodynamicTargetRangeMin REAL NOT NULL,
            BallisticTargetRangeMax REAL NOT NULL,
            BallisticTargetRangeMin REAL NOT NULL,
            MaxEngagements INTEGER NOT NULL,
            MaxMissilesFired INTEGER NOT NULL,
            ECMCapability TEXT NOT NULL,
            Cost REAL NOT NULL
        );";

        public const string CreateAirDefenseRadarTable = @"CREATE TABLE IF NOT EXISTS AirDefenseRadar (
            AirDefenseId INTEGER NOT NULL,
            RadarId INTEGER NOT NULL,
            Quantity INTEGER NOT NULL,
            PRIMARY KEY (AirDefenseId, RadarId),
            FOREIGN KEY (AirDefenseId) REFERENCES AirDefense (Id) ON DELETE CASCADE,
            FOREIGN KEY (RadarId) REFERENCES Radar (Id) ON DELETE CASCADE
        );";

        public const string CreateAirDefenseMunitionTable = @"CREATE TABLE IF NOT EXISTS AirDefenseMunition (
            AirDefenseId INTEGER NOT NULL,
            MunitionId INTEGER NOT NULL,
            Quantity INTEGER NOT NULL,
            PRIMARY KEY (AirDefenseId, MunitionId),
            FOREIGN KEY (AirDefenseId) REFERENCES AirDefense (Id) ON DELETE CASCADE,
            FOREIGN KEY (MunitionId) REFERENCES Munition (Id) ON DELETE CASCADE
        );";
    }
}
