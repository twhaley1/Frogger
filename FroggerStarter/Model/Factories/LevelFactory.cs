using System;
using System.Collections.ObjectModel;

namespace FroggerStarter.Model.Factories
{
    /// <summary>
    ///     Creates levels.
    /// </summary>
    public static class LevelFactory
    {
        #region Methods

        /// <summary>
        ///     Creates the level.
        ///     Precondition: roadYArea &gt;= 0 AND id &gt;= 0
        ///     Postcondition: None
        /// </summary>
        /// <param name="roadYArea">The road y area.</param>
        /// <param name="startingY">The starting y.</param>
        /// <param name="id">The id.</param>
        /// <returns>A Level starting at startingY and spans roadYArea containing a collection of lanes.</returns>
        public static Level CreateLevel(double roadYArea, double startingY, int id)
        {
            if (roadYArea < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(roadYArea));
            }

            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var lanes = new Collection<Lane>();
            for (var i = 0; i < GameSettings.MaxNumberOfLanes; i++)
            {
                lanes.Add(LaneFactory.CreateLane(GameSettings.SpeedSetting[id][i], GameSettings.DirectionSetting[id][i],
                    GameSettings.TypeSetting[id][i], GameSettings.MaxPopulationSetting[id][i]));
            }

            return new Level(roadYArea, startingY, id, lanes);
        }

        #endregion
    }
}