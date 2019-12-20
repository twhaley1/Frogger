using System;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using FroggerStarter.Controller;
using FroggerStarter.Enums;

namespace FroggerStarter.Model.GameObjects
{
    /// <summary>
    ///     Defines the properties and behavior of a Obstacle.
    /// </summary>
    /// <seealso cref="GameObject" />
    public abstract class Obstacle : GameObject
    {
        #region Properties

        /// <summary>
        ///     Gets the direction.
        /// </summary>
        /// <value>
        ///     The direction.
        /// </value>
        public DirectionType Direction { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Obstacle" /> class.
        ///     Precondition: none
        ///     Postcondition: this.SpeedX == 0
        ///     this.SpeedY == 0
        ///     this.Direction == direction
        /// </summary>
        /// <param name="direction">The direction of the vehicle.</param>
        protected Obstacle(DirectionType direction)
        {
            this.Direction = direction;
            SetSpeed(0, 0);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the direction.
        ///     Precondition: None
        ///     Postcondition: Rotates this.Sprite to face in this.Direction
        /// </summary>
        protected void SetDirection()
        {
            if (this.Direction == DirectionType.Right)
            {
                Sprite.RenderTransformOrigin = new Point(0.5, 0.5);
                Sprite.RenderTransform = new ScaleTransform {ScaleX = -1};
            }
        }

        /// <summary>
        ///     Sets the horizontal speed and disables vertical movement.
        ///     Precondition: none
        ///     Postcondition: this.SpeedX == speed
        ///     this.SpeedY == 0
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <exception cref="ArgumentOutOfRangeException">speed</exception>
        public virtual void SetSpeedX(double speed)
        {
            if (speed < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(speed));
            }

            SetSpeed(speed, 0);
        }

        /// <summary>
        ///     Moves forward, with forward being defined as the direction the vehicle is facing.
        ///     Precondition: none
        ///     Postcondition: X += Speed if Direction == Right
        ///     X -= Speed is Direction == Left
        ///     X == GameManager.BackgroundWidth if new X would put vehicle entirely offscreen to the left
        ///     X == -Width if new X would put vehicle entirely offscreen to the right
        /// </summary>
        public void MoveForward()
        {
            if (this.Direction == DirectionType.Left)
            {
                MoveLeft();
                this.checkLeftWrap();
            }
            else
            {
                MoveRight();
                this.checkRightWrap();
            }
        }

        private void checkLeftWrap()
        {
            if (X <= -Width)
            {
                X = GameManager.BackgroundWidth;
            }
        }

        private void checkRightWrap()
        {
            if (X >= GameManager.BackgroundWidth)
            {
                X = -Width;
            }
        }

        #endregion
    }
}