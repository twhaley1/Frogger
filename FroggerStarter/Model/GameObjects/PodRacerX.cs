using FroggerStarter.Enums;
using FroggerStarter.View.Sprites.Obstacle;

namespace FroggerStarter.Model.GameObjects
{
    /// <summary>
    ///     Defines the state and behavior of a PodRacerX obstacle.
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObjects.Obstacle" />
    public class PodRacerX : PodRacer
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PodRacerX" /> class.
        ///     Precondition: None
        /// </summary>
        /// <param name="direction">The direction of the obstacle.</param>
        public PodRacerX(DirectionType direction) : base(direction)
        {
            Sprite = new PodRacerXSprite();
            SetDirection();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the horizontal speed to double the specified speed and disables vertical movement.
        ///     Precondition: none
        ///     Postcondition: this.SpeedX == speed * 2
        ///     this.SpeedY == 0
        /// </summary>
        /// <param name="speed">The speed.</param>
        public override void SetSpeedX(double speed)
        {
            base.SetSpeedX(speed * 2);
        }

        #endregion
    }
}