using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using FroggerStarter.Model.GameObjects.PowerUps;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Defines the properties and behavior of a PowerUpManager.
    ///     Manages a collection of PowerUps.
    /// </summary>
    public class PowerUpManager : IEnumerable<PowerUp>
    {
        #region Data members

        private readonly Random rng;
        private readonly IList<PowerUp> powerUps;

        #endregion

        #region Properties

        private int RandomNumber { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PowerUpManager" /> class.
        ///     Precondition: powerUps != null
        ///     Postcondition: this.rng == a Random
        ///     this.randomNumber == a new random number
        /// </summary>
        /// <param name="powerUps">The power ups.</param>
        public PowerUpManager(IList<PowerUp> powerUps)
        {
            this.powerUps = powerUps ?? throw new ArgumentNullException(nameof(powerUps));
            this.rng = new Random();
            var lastQuarterOfLife = GameSettings.LifeCountdownInSeconds / 4;
            this.generateRandomNumberBetween(lastQuarterOfLife, GameSettings.LifeCountdownInSeconds);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<PowerUp> GetEnumerator()
        {
            return this.powerUps.GetEnumerator();
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
        ///     Makes all power ups invisible.
        ///     Precondition: None
        ///     Postcondition: each powerUp in this.powerUps has visibility set to collapsed.
        /// </summary>
        public void ResetAllPowerUps()
        {
            this.powerUps.ToList().ForEach(powerUp =>
            {
                powerUp.Sprite.Visibility = Visibility.Collapsed;
                powerUp.HasBeenCollected = false;
            });
        }

        /// <summary>
        ///     Randomly makes a power up visible.
        ///     Precondition: None
        ///     Postcondition: If currentTick == this.randomNumber, then a random powerUp is made visible if
        ///     there are any that have not been shown or collected.
        /// </summary>
        /// <param name="currentTick">The current tick.</param>
        public void RandomlyAddPowerUps(int currentTick)
        {
            if (currentTick == this.RandomNumber)
            {
                this.makeRandomPowerUpVisible();
                this.generateRandomNumberBetween(GameSettings.LifeCountdownInSeconds / 4,
                    GameSettings.LifeCountdownInSeconds);
            }
        }

        /// <summary>
        ///     Makes a random invisible power up visible.
        ///     Precondition: None
        ///     Postcondition: Makes a random invisible power up visible.
        /// </summary>
        private void makeRandomPowerUpVisible()
        {
            var validPowerUps = this.powerUps.Where(powerUp =>
                                        powerUp.Sprite.Visibility == Visibility.Collapsed && !powerUp.HasBeenCollected)
                                    .ToList();
            if (validPowerUps.Count != 0)
            {
                this.generateRandomNumberBetween(0, validPowerUps.Count);
                validPowerUps[this.RandomNumber].Sprite.Visibility = Visibility.Visible;
            }
        }

        private void generateRandomNumberBetween(int minValue, int maxValueExclusive)
        {
            this.RandomNumber = this.rng.Next(minValue, maxValueExclusive);
        }

        #endregion
    }
}