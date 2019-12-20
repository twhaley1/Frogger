using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FroggerStarter.Controller;
using FroggerStarter.Model.GameObjects;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     The row of frog homes that the player lands in.
    /// </summary>
    /// <seealso>
    ///     <cref>System.Collections.Generic.IEnumerable{FroggerStarter.Model.Home}</cref>
    /// </seealso>
    public class HomeRow : IEnumerable<Home>
    {
        #region Data members

        private readonly IList<Home> homes;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the number filled homes.
        /// </summary>
        /// <value>
        ///     The number filled homes.
        /// </value>
        public int NumberFilledHomes => this.Count(home => home.IsFull);

        /// <summary>
        ///     Gets a value indicating whether [are all homes full].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [are all homes full]; otherwise, <c>false</c>.
        /// </value>
        public bool AreAllHomesFull => this.NumberFilledHomes == GameSettings.Homes;

        /// <summary>
        ///     Gets the population.
        /// </summary>
        /// <value>
        ///     The population.
        /// </value>
        public int Population { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HomeRow" /> class.
        ///     Precondition: tileWidth &gt; 0
        ///     population &gt; 0
        ///     Postcondition: this.homes = a list of homes that is population big
        ///     this.Population = population
        ///     all homes aligned in the row centered and spaced evenly
        /// </summary>
        /// <param name="rowStartY">The row start y.</param>
        /// <param name="tileWidth">Width of the tile.</param>
        /// <param name="population">The population.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     tileWidth
        ///     or
        ///     population
        /// </exception>
        public HomeRow(double rowStartY, double tileWidth, int population)
        {
            if (tileWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tileWidth));
            }

            if (population <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(population));
            }

            this.homes = new List<Home>();
            this.Population = population;
            this.populate();
            this.alignHomesX(rowStartY, tileWidth);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns an enumerator that iterates through the homes.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the homes.
        /// </returns>
        public IEnumerator<Home> GetEnumerator()
        {
            return this.homes.GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        ///     Clears all homes in the row.
        ///     Precondition: None
        ///     Postcondition: Every home in the row has its visible sprite as the empty home.
        /// </summary>
        public void ClearAll()
        {
            foreach (var home in this.homes)
            {
                home.ClearHome();
            }
        }

        private void populate()
        {
            for (var i = 0; i < this.Population; i++)
            {
                this.homes.Add(new Home());
            }
        }

        private void alignHomesX(double rowStartY, double tileWidth)
        {
            var tiles = GameManager.BackgroundWidth / tileWidth;
            var gaps = this.Population - 1;
            var gapTilesTotal = tiles - this.Population;
            var tilesInEachGap = gapTilesTotal / gaps;
            var gapArea = tileWidth * tilesInEachGap;

            var xLocationToPlace = 0.0;
            foreach (var home in this.homes)
            {
                home.X = xLocationToPlace;
                home.Y = rowStartY;
                home.AllSprites.ToList().ForEach(sprite => sprite.RenderAt(home.X, home.Y));

                xLocationToPlace += gapArea + home.Width;
            }
        }

        #endregion
    }
}