using FroggerStarter.View.Sprites.Powerups;

namespace FroggerStarter.Model.GameObjects.PowerUps
{
    /// <summary>
    ///     Defines the properties and behavior of a BonusTimePowerUp.
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObjects.GameObject" />
    public class BonusTimePowerUp : PowerUp
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BonusTimePowerUp" /> class.
        ///     Precondition: None
        ///     Postcondition: this.Sprite set to an BonusTimePowerUpSprite, and its location set to the defined coordinates in
        ///     GameSettings
        /// </summary>
        public BonusTimePowerUp(PowerUpOperation operate) : base(operate)
        {
            Sprite = new BonusTimePowerUpSprite();
            X = GameSettings.BonusTimePowerUpLocation.X;
            Y = GameSettings.BonusTimePowerUpLocation.Y;
        }

        #endregion
    }
}