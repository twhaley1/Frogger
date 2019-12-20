using FroggerStarter.Enums;
using FroggerStarter.View.Sprites.Obstacle;

namespace FroggerStarter.Model.GameObjects
{
    /// <summary>
    ///     Defines the state and behavior of a Limo obstacle.
    /// </summary>
    /// <seealso cref="Obstacle" />
    public class Limo : Obstacle
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Limo" /> class.
        ///     Precondition: None
        ///     Postcondition: this.Sprite == a LimoSprite
        ///     this.Direction == direction
        /// </summary>
        /// <param name="direction">The direction of the obstacle.</param>
        public Limo(DirectionType direction) : base(direction)
        {
            Sprite = new LimoSprite();
            SetDirection();
        }

        #endregion
    }
}