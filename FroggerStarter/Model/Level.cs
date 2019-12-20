using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using FroggerStarter.Model.GameObjects;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Defines the properties and behavior of a Level.
    ///     Manages a collection of lanes.
    /// </summary>
    public class Level : IEnumerable<Obstacle>
    {
        #region Data members

        private const int ShoulderLanes = 2;

        private readonly IList<Lane> lanes;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the visible obstacles.
        /// </summary>
        /// <value>
        ///     The visible obstacles.
        /// </value>
        public IEnumerable<Obstacle> VisibleObstacles =>
            this.Where(obstacle => obstacle.Sprite.Visibility == Visibility.Visible);

        /// <summary>
        ///     Gets a value indicating whether this instance is water level.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is water level; otherwise, <c>false</c>.
        /// </value>
        public bool IsWaterLevel => GameSettings.WaterLevelIds.Contains(this.Id);

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public int Id { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Level" /> class.
        ///     Precondition: roadYArea &gt; 0
        ///     startingY &gt; 0
        ///     Postcondition: this.lanes == populated list of lanes
        ///     all obstacles Y locations aligned
        ///     all obstacles set to Visibility.Collapsed except for the first obstacle in each lane
        /// </summary>
        /// <param name="roadYArea">The road y area.</param>
        /// <param name="startingY">The starting y.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="lanes">The lanes.</param>
        public Level(double roadYArea, double startingY, int id, IEnumerable<Lane> lanes)
        {
            this.lanes = new List<Lane>();
            foreach (var lane in lanes ?? Enumerable.Empty<Lane>())
            {
                this.lanes.Add(lane);
            }

            this.Id = id;
            this.alignAllObstaclesY(roadYArea, startingY);
            this.ResetLanes();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns an enumerator that iterates through the lanes.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the lanes.
        /// </returns>
        public IEnumerator<Obstacle> GetEnumerator()
        {
            return this.lanes.SelectMany(lane => lane).GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        ///     Determines whether [is player on platform].
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     <c>true</c> if [is player on platform]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsPlayerOnPlatform()
        {
            foreach (var obstacle in this.Where(gameObject => gameObject is Platform))
            {
                var platform = (Platform) obstacle;
                if (platform.ContainsPlayer())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Resets the lanes so that all obstacles are set to Visibility.Collapsed except the
        ///     first obstacle in each lane, unless it is a water level.
        ///     Precondition: this.lanes != null
        ///     Postcondition: each obstacle in the road will have its visibility collapsed except
        ///     the first obstacle in every lane (unless it is a water level)
        /// </summary>
        /// <exception cref="ArgumentNullException">lanes</exception>
        public void ResetLanes()
        {
            if (this.lanes == null)
            {
                throw new ArgumentNullException(nameof(this.lanes));
            }

            if (!this.IsWaterLevel)
            {
                foreach (var lane in this.lanes)
                {
                    lane.Reset();
                }
            }
        }

        private void alignAllObstaclesY(double roadYArea, double startingY)
        {
            var laneHeight = roadYArea / (this.lanes.Count + ShoulderLanes);
            startingY -= laneHeight;

            foreach (var lane in this.lanes)
            {
                startingY -= laneHeight;
                lane.AlignObstaclesY(laneHeight, startingY);
            }
        }

        /// <summary>
        ///     Finds all invisible obstacles outside the lane boundaries and returns them.
        ///     Precondition: none
        ///     Postcondition: none
        /// </summary>
        /// <returns>
        ///     All obstacles that simultaneously have their Visibility set to Collapsed and are beyond the bounds of the
        ///     lane.
        /// </returns>
        public IEnumerable<Obstacle> FindAllInvisibleObstaclesOutsideLaneBoundaries()
        {
            var obstaclesToMakeVisible = new List<List<Obstacle>>();
            foreach (var lane in this.lanes)
            {
                obstaclesToMakeVisible.Add(lane.FindInvisibleObstaclesOutsideLaneBoundaries().ToList());
            }

            return obstaclesToMakeVisible.SelectMany(obstacle => obstacle);
        }

        /// <summary>
        ///     Moves all obstacles.
        ///     Precondition: none
        ///     Postcondition: obstacle.X for every obstacle in the road either
        ///     + obstacle.Speed if lane direction == right
        ///     or - obstacle.Speed if lane direction == left
        /// </summary>
        public void MoveAllObstacles()
        {
            foreach (var lane in this.lanes)
            {
                lane.MoveAll();
            }
        }

        #endregion
    }
}