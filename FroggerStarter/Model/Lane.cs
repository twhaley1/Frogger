using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using FroggerStarter.Controller;
using FroggerStarter.Model.GameObjects;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Defines the properties and behaviors of a Lane.
    ///     Manages a collection of obstacles.
    /// </summary>
    public class Lane : IEnumerable<Obstacle>
    {
        #region Data members

        private readonly IList<Obstacle> obstacles;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the speed.
        /// </summary>
        /// <value>
        ///     The speed.
        /// </value>
        public double Speed { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Lane" /> class.
        ///     Precondition: speed &lt;= 0
        ///     Postcondition: this.obstacles == populated list of obstacles
        ///     this.Speed == speed
        ///     each obstacle evenly spaced horizontally across BackgroundWidth
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <param name="obstacles">The obstacles to add to the lane.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     speed
        /// </exception>
        public Lane(double speed, IEnumerable<Obstacle> obstacles)
        {
            if (speed < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(speed));
            }

            this.Speed = speed;

            this.obstacles = new List<Obstacle>();
            foreach (var obstacle in obstacles ?? Enumerable.Empty<Obstacle>())
            {
                this.add(obstacle);
            }

            this.alignObstaclesX(GameManager.BackgroundWidth);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns an enumerator that iterates through the obstacles.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the obstacles.
        /// </returns>
        public IEnumerator<Obstacle> GetEnumerator()
        {
            return this.obstacles.GetEnumerator();
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

        private void alignObstaclesX(double backgroundWidth)
        {
            var obstacleGap = this.obstacles.Count != 0 ? (int) backgroundWidth / this.obstacles.Count : 0;
            var startingX = 0;

            foreach (var obstacle in this.obstacles)
            {
                obstacle.X = (obstacleGap - obstacle.Sprite.Width) / 2 + startingX;
                startingX += obstacleGap;
            }
        }

        /// <summary>
        ///     Aligns the obstacles vertically in each lane, starting at a specified Y location.
        ///     Precondition: laneHeight &gt; 0
        ///     startingY &gt;= 0
        ///     Postcondition: each obstacle in this.obstacles centered vertically in their respective lanes
        /// </summary>
        /// <param name="laneHeight">Height of the lane.</param>
        /// <param name="startingY">The starting y.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     laneHeight
        ///     or
        ///     startingY
        /// </exception>
        public void AlignObstaclesY(double laneHeight, double startingY)
        {
            if (laneHeight <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(laneHeight));
            }

            if (startingY < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startingY));
            }

            foreach (var obstacle in this.obstacles)
            {
                obstacle.Y = (laneHeight - obstacle.Sprite.Height) / 2 + startingY;
            }
        }

        private void add(Obstacle obstacle)
        {
            this.obstacles.Add(obstacle);
            obstacle.SetSpeedX(this.Speed);
        }

        /// <summary>
        ///     Resets the lane so that all obstacles except the first have their Visibility set to Collapsed.
        ///     Precondition: this.obstacles != null
        ///     Postcondition: all obstacles in lane have visibility collapsed except the first index
        /// </summary>
        /// <exception cref="ArgumentNullException">obstacles</exception>
        public void Reset()
        {
            if (this.obstacles == null)
            {
                throw new ArgumentNullException(nameof(this.obstacles));
            }

            foreach (var obstacle in this.obstacles.Skip(1))
            {
                obstacle.Sprite.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        ///     Finds the invisible obstacles outside lane boundaries.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     All obstacles in the road that simultaneously have their Visibility set to Collapsed and are outside the
        ///     bounds of the lane.
        /// </returns>
        public IList<Obstacle> FindInvisibleObstaclesOutsideLaneBoundaries()
        {
            return this.obstacles.ToList().FindAll(obstacle => obstacle.Sprite.Visibility == Visibility.Collapsed &&
                                                               (obstacle.X <= -obstacle.Width ||
                                                                obstacle.X >= GameManager.BackgroundWidth)).ToList();
        }

        /// <summary>
        ///     Moves all obstacles in the lane forward, according to their direction.
        ///     Precondition: none
        ///     Postcondition: X location of each obstacle in this.obstacles changes
        ///     based on speed and direction accounting for
        ///     boundaries of backgroundWidth
        /// </summary>
        public void MoveAll()
        {
            this.obstacles.ToList().ForEach(obstacle => obstacle.MoveForward());
        }

        #endregion
    }
}