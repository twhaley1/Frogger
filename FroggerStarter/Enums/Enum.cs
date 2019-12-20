namespace FroggerStarter.Enums
{
    /// <summary>
    ///     The only possible types for a vehicle.
    /// </summary>
    public enum GameObjectType
    {
        /// <summary>
        ///     Type that gives an obstacle a PodRacer Sprite.
        /// </summary>
        PodRacer,

        /// <summary>
        ///     Type that gives an obstacle a Limo Sprite.
        /// </summary>
        Limo,

        /// <summary>
        ///     Type that gives an obstacle a PodRacerX Sprite.
        /// </summary>
        PodRacerX,

        /// <summary>
        ///     Type that gives an obstacle a LilyPad Sprite.
        /// </summary>
        LilyPad,

        /// <summary>
        ///     Type that gives an obstacle a Log Sprite.
        /// </summary>
        Log
    }

    /// <summary>
    ///     The only possible directions for a game object.
    /// </summary>
    public enum DirectionType
    {
        /// <summary>
        ///     Direction that makes a game object move from right to left.
        /// </summary>
        Left,

        /// <summary>
        ///     Direction that makes a game object move from left to right.
        /// </summary>
        Right,

        /// <summary>
        ///     Direction that makes a game object move up.
        /// </summary>
        Up,

        /// <summary>
        ///     Direction that makes a game object move down.
        /// </summary>
        Down
    }

    /// <summary>
    ///     The types of sound effects in the game.
    /// </summary>
    public enum Sound
    {
        /// <summary>
        ///     The obstacle collision sound effect.
        /// </summary>
        ObstacleCollision,

        /// <summary>
        ///     The wall collision sound effect.
        /// </summary>
        WallCollision,

        /// <summary>
        ///     The fall in water sound effect.
        /// </summary>
        FallInWater,

        /// <summary>
        ///     The timer complete sound effect.
        /// </summary>
        TimerComplete,

        /// <summary>
        ///     The player at home sound effect.
        /// </summary>
        PlayerAtHome,

        /// <summary>
        ///     The level complete sound effect.
        /// </summary>
        CompleteLevel,

        /// <summary>
        ///     The game over sound effect.
        /// </summary>
        GameOver,

        /// <summary>
        ///     The power up sound effect.
        /// </summary>
        PowerUp
    }

    /// <summary>
    ///     All possible types of animations.
    /// </summary>
    public enum AnimationType
    {
        /// <summary>
        ///     The movement animation type.
        /// </summary>
        Movement,

        /// <summary>
        ///     The death animation type.
        /// </summary>
        Death
    }
}