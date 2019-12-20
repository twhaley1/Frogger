using Windows.Foundation;
using FroggerStarter.Enums;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Keeps track of various statistics related to the player.
    /// </summary>
    public class GameSettings
    {
        #region Data members

        /// <summary>
        ///     The maximum lives for a player.
        /// </summary>
        public const int BaseMaximumLives = 4;

        /// <summary>
        ///     The maximum lives for a player in hardcore.
        /// </summary>
        public const int HardcoreMaximumLives = 1;

        /// <summary>
        ///     The life countdown in seconds until the frog loses a life.
        /// </summary>
        public const int LifeCountdownInSeconds = 20;

        /// <summary>
        ///     The base score multiplier.
        /// </summary>
        public const int BaseScoreMultiplier = 10;

        /// <summary>
        ///     The hardcore score multiplier.
        /// </summary>
        public const int HardcoreScoreMultiplier = 20;

        /// <summary>
        ///     The amount of seconds added to the life countdown timer when the bonusTimePowerUp is collected.
        /// </summary>
        public const int BonusTimePowerUpSecondsToAdd = 10;

        /// <summary>
        ///     The invuln time in seconds until the frog becomes vulnerable again.
        /// </summary>
        public const int InvulnTimeInSeconds = 5;

        /// <summary>
        ///     The obstacle add frequency in game ticks.
        /// </summary>
        public const int ObstacleAddFrequency = 50;

        /// <summary>
        ///     The amount of homes in the home row.
        /// </summary>
        public const int Homes = 5;

        /// <summary>
        ///     The maximum number of lanes.
        /// </summary>
        public const int MaxNumberOfLanes = 5;

        /// <summary>
        ///     The maximum level
        /// </summary>
        public const int MaxLevels = 3;

        private static readonly double[] LevelOneLaneSpeed = {0.75, 1.0, 1.25, 1.5, 1.75};
        private static readonly double[] LevelTwoLaneSpeed = {1.0, 1.25, 1.5, 1.75, 2.0};
        private static readonly double[] LevelThreeLaneSpeed = {2.0, 3.0, 3.25, 2.25, 4.0};

        /// <summary>
        ///     The level speed settings
        /// </summary>
        public static readonly double[][] SpeedSetting = {
            LevelOneLaneSpeed,
            LevelTwoLaneSpeed,
            LevelThreeLaneSpeed
        };

        private static readonly DirectionType[] LevelOneLaneDirection = {
            DirectionType.Left,
            DirectionType.Right,
            DirectionType.Left,
            DirectionType.Left,
            DirectionType.Right
        };

        private static readonly DirectionType[] LevelTwoLaneDirection = {
            DirectionType.Left,
            DirectionType.Right,
            DirectionType.Right,
            DirectionType.Left,
            DirectionType.Right
        };

        private static readonly DirectionType[] LevelThreeLaneDirection = {
            DirectionType.Right,
            DirectionType.Left,
            DirectionType.Right,
            DirectionType.Left,
            DirectionType.Right
        };

        /// <summary>
        ///     The level direction settings
        /// </summary>
        public static readonly DirectionType[][] DirectionSetting = {
            LevelOneLaneDirection,
            LevelTwoLaneDirection,
            LevelThreeLaneDirection
        };

        private static readonly GameObjectType[] LevelOneLaneType = {
            GameObjectType.PodRacer,
            GameObjectType.Limo,
            GameObjectType.PodRacer,
            GameObjectType.Limo,
            GameObjectType.PodRacer
        };

        private static readonly GameObjectType[] LevelTwoLaneType = {
            GameObjectType.Limo,
            GameObjectType.PodRacerX,
            GameObjectType.Limo,
            GameObjectType.PodRacerX,
            GameObjectType.Limo
        };

        private static readonly GameObjectType[] LevelThreeLaneType = {
            GameObjectType.LilyPad,
            GameObjectType.Log,
            GameObjectType.Log,
            GameObjectType.LilyPad,
            GameObjectType.Log
        };

        /// <summary>
        ///     The level type settings
        /// </summary>
        public static readonly GameObjectType[][] TypeSetting = {
            LevelOneLaneType,
            LevelTwoLaneType,
            LevelThreeLaneType
        };

        /// <summary>
        ///     The water level ids.
        ///     (Not setting this dynamically allows for some fun shenanigans like dodging hurtling logs on roads
        ///     or jumping across flooded limos to get across a river.)
        /// </summary>
        public static readonly int[] WaterLevelIds = {2};

        private static readonly int[] LevelOneLaneMaxPopulation = {2, 2, 3, 3, 3};
        private static readonly int[] LevelTwoLaneMaxPopulation = {3, 3, 3, 4, 4};
        private static readonly int[] LevelThreeLaneMaxPopulation = {4, 3, 3, 3, 2};

        /// <summary>
        ///     The level maximum population settings
        /// </summary>
        public static readonly int[][] MaxPopulationSetting = {
            LevelOneLaneMaxPopulation,
            LevelTwoLaneMaxPopulation,
            LevelThreeLaneMaxPopulation
        };

        /// <summary>
        ///     The bonus time power up location.
        /// </summary>
        public static readonly Point BonusTimePowerUpLocation = new Point(400, 305);

        /// <summary>
        ///     The invuln power up location.
        /// </summary>
        public static readonly Point InvulnPowerUpLocation = new Point(100, 205);

        #endregion
    }
}