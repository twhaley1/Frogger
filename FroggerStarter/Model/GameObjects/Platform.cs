using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using FroggerStarter.Enums;
using FroggerStarter.EventArgs;

namespace FroggerStarter.Model.GameObjects
{
    /// <summary>
    ///     Manages behavior for all platform type game objects.
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObjects.Obstacle" />
    public abstract class Platform : Obstacle
    {
        #region Data members

        private readonly ICollection<GameObject> riders;

        private double playerDeltaPosition;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Platform" /> class.
        ///     Precondition: None
        ///     Postcondition: this.Direction == direction
        ///     this.riders == an empty List of type GameObject
        /// </summary>
        /// <param name="direction">The direction of the vehicle.</param>
        protected Platform(DirectionType direction) : base(direction)
        {
            this.riders = new List<GameObject>();
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
            var baseCollision = base.IsCollidingWith(other);
            if (baseCollision && other is Frog player)
            {
                this.AddRider(player);
            }

            return baseCollision;
        }

        /// <summary>
        ///     Renders the platform and it's riders.
        ///     Precondition: None
        ///     Postcondition: The platform is rendered, and each rider is rendered along with it.
        /// </summary>
        protected override void Render()
        {
            base.Render();
            this.riders.ToList().ForEach(gameObject =>
            {
                gameObject.X = gameObject is Frog ? X - this.playerDeltaPosition : X;
            });
        }

        /// <summary>
        ///     Adds the specified rider.
        ///     Precondition: rider != null
        ///     Postcondition: this.riders.Count == @prev + 1
        /// </summary>
        /// <param name="rider">The rider.</param>
        /// <exception cref="ArgumentNullException">rider</exception>
        public void AddRider(Frog rider)
        {
            if (rider == null)
            {
                throw new ArgumentNullException(nameof(rider));
            }

            this.riders.Add(rider);
            renderRiderAbove(rider);
            rider.Moved += this.riderOnMoved;
            this.playerDeltaPosition = X - rider.X;
        }

        private static void renderRiderAbove(Frog rider)
        {
            Canvas.SetZIndex(rider.Sprite, 100);
            rider.Animations[AnimationType.Movement].AllSprites.ToList()
                 .ForEach(sprite => Canvas.SetZIndex(sprite, 100));
        }

        private void riderOnMoved(object sender, MovableObjectEventArgs e)
        {
            e.MovingObject.Moved -= this.riderOnMoved;
            this.removeRider(e.MovingObject);
            Canvas.SetZIndex(e.MovingObject.Sprite, 1);
        }

        private void removeRider(GameObject rider)
        {
            if (rider == null)
            {
                throw new ArgumentNullException(nameof(rider));
            }

            this.riders.Remove(rider);
        }

        /// <summary>
        ///     Determines whether this instance contains player.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance contains player; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsPlayer()
        {
            var containsPlayer = false;
            foreach (var gameObject in this.riders)
            {
                if (gameObject is Frog)
                {
                    containsPlayer = true;
                }
            }

            return containsPlayer;
        }

        /// <summary>
        ///     Clears the platform of its riders.
        ///     Precondition: None
        ///     Postcondition: this.riders.Count == 0
        /// </summary>
        public void Clear()
        {
            this.riders.Clear();
        }

        #endregion
    }
}