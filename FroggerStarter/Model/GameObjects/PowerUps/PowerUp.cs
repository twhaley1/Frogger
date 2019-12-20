using Windows.UI.Xaml;

namespace FroggerStarter.Model.GameObjects.PowerUps
{
    /// <summary>
    ///     Defines the properties and behavior of a PowerUp.
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObjects.GameObject" />
    public abstract class PowerUp : GameObject
    {
        #region Types and Delegates

        /// <summary>
        ///     Defines behavior that will happen when a power-up is collided with.
        /// </summary>
        public delegate void PowerUpOperation();

        #endregion

        #region Data members

        private readonly PowerUpOperation operate;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a value indicating whether this instance has been collected.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has been collected; otherwise, <c>false</c>.
        /// </value>
        public bool HasBeenCollected { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PowerUp" /> class.
        ///     Precondition: None
        ///     Postcondition: this.operate == operate AND this.HasBeenCollected == false
        /// </summary>
        /// <param name="operate">The operate.</param>
        protected PowerUp(PowerUpOperation operate)
        {
            this.operate = operate;
            this.HasBeenCollected = false;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Determines whether [is colliding with] [the specified other]. The other must have its Visibility set to Visible.
        ///     Precondition: none
        ///     Postcondition: none
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>
        ///     <c>true</c> if [is colliding with] [the specified other]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsCollidingWith(GameObject other)
        {
            if (base.IsCollidingWith(other) && !this.HasBeenCollected && Sprite.Visibility == Visibility.Visible)
            {
                Sprite.Visibility = Visibility.Collapsed;
                this.HasBeenCollected = true;
                this.operate();
                return true;
            }

            return false;
        }

        #endregion
    }
}