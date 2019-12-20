using FroggerStarter.Enums;
using FroggerStarter.View.Sprites.Obstacle;

namespace FroggerStarter.Model.GameObjects
{
    /// <summary>
    ///     Models a Log Platform.
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObjects.Platform" />
    public class Log : Platform
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Log" /> class.
        ///     Precondition: None
        ///     Postcondition: this.Sprite == a LogSprite
        ///     this.Direction == direction
        /// </summary>
        /// <param name="direction">The direction of the vehicle.</param>
        public Log(DirectionType direction) : base(direction)
        {
            Sprite = new LogSprite();
            SetDirection();
        }

        #endregion
    }
}