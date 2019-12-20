using System;
using FroggerStarter.Enums;
using FroggerStarter.Model.GameObjects;

namespace FroggerStarter.Model.Factories
{
    /// <summary>
    ///     Creates obstacles.
    /// </summary>
    public static class ObstacleFactory
    {
        #region Methods

        /// <summary>
        ///     Creates an obstacle.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="type">The type.</param>
        /// <returns>An obstacle with the specified direction and type.</returns>
        /// <exception cref="ArgumentException">type</exception>
        public static Obstacle CreateObstacle(DirectionType direction, GameObjectType type)
        {
            switch (type)
            {
                case GameObjectType.Limo:
                    return new Limo(direction);
                case GameObjectType.PodRacer:
                    return new PodRacer(direction);
                case GameObjectType.PodRacerX:
                    return new PodRacerX(direction);
                case GameObjectType.LilyPad:
                    return new LilyPad(direction);
                case GameObjectType.Log:
                    return new Log(direction);
                default:
                    throw new ArgumentException(nameof(type));
            }
        }

        #endregion
    }
}