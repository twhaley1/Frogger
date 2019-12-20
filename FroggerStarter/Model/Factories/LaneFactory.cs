using System;
using System.Collections.ObjectModel;
using FroggerStarter.Enums;
using FroggerStarter.Model.GameObjects;

namespace FroggerStarter.Model.Factories
{
    /// <summary>
    ///     Creates lanes.
    /// </summary>
    public static class LaneFactory
    {
        #region Methods

        /// <summary>
        ///     Creates a lane.
        ///     Precondition: speed &gt;= 0 AND population &gt;= 0
        ///     Postcondition: None
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="type">The type.</param>
        /// <param name="population">The population.</param>
        /// <returns>
        ///     A Lane with the specified speed, direction, and type with population number of obstacles.
        /// </returns>
        public static Lane CreateLane(double speed, DirectionType direction,
            GameObjectType type, int population)
        {
            if (speed < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(speed));
            }

            if (population < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(population));
            }

            var obstacles = new Collection<Obstacle>();
            for (var i = 0; i < population; i++)
            {
                obstacles.Add(ObstacleFactory.CreateObstacle(direction, type));
            }

            return new Lane(speed, obstacles);
        }

        #endregion
    }
}