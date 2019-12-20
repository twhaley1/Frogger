using FroggerStarter.Enums;
using FroggerStarter.View.Sprites.Obstacle;

namespace FroggerStarter.Model.GameObjects
{
    /// <summary>
    ///     Defines the state and behavior of a PodRacer obstacle.
    /// </summary>
    /// <seealso cref="Obstacle" />
    public class PodRacer : Obstacle
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PodRacer" /> class.
        ///     Precondition: None
        ///     Postcondition: this.Sprite = a PodRacerSprite
        ///     this.Direction == direction
        /// </summary>
        /// <param name="direction">The direction of the obstacle.</param>
        public PodRacer(DirectionType direction) : base(direction)
        {
            Sprite = new PodRacerSprite();
            SetDirection();
        }

        #endregion
    }
}